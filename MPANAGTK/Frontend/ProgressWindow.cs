using System;
using System.IO;
using System.Timers;
using Gtk;
using GLib;
using MPANAGTK.Backend;

namespace MPANAGTK.Frontend
{
    public class ProgressWindow : Window
    {
        private TextView textView;
        private DrawingArea drawingArea;
        private Gdk.Pixbuf backgroundImage;
        private Timer animationTimer;
        private double opacity = 1.0;
        private bool fadingOut = true;

        public ProgressWindow() : base("Progress")
        {
            SetDefaultSize(500, 900);
            SetPosition(WindowPosition.Center);

            textView = new TextView
            {
                Editable = false,
                WrapMode = WrapMode.Word
            };
            var scroll = new ScrolledWindow();
            scroll.Add(textView);

            drawingArea = new DrawingArea();
            
            drawingArea.ExposeEvent += OnDrawingAreaExpose;

            VBox vbox = new VBox();
            vbox.PackStart(drawingArea, true, true, 10);
            vbox.PackStart(scroll, true, true, 0);
            Add(vbox);

            string imagePath = Config.Settings.AppAsserts.LogoPath; //"/Volumes/Secondary/Projects/MealPlannerAndNutritionAssistant/MPANAGTK/MPANAGTK/Backend/Graphics/MPANA logo.png";
            if (File.Exists(imagePath))
            {
                backgroundImage = new Gdk.Pixbuf(imagePath,500,500);
            }
            // Start the animation timer
            animationTimer = new Timer(100); // Update every 100ms
            animationTimer.Elapsed += OnAnimationTimerElapsed;
            animationTimer.Start();

            ShowAll();
        }

        private void OnDrawingAreaExpose(object sender, ExposeEventArgs args)
        {
            if (backgroundImage != null)
            {
                using (Cairo.Context cr = Gdk.CairoHelper.Create(drawingArea.GdkWindow))
                {
                    using (Cairo.ImageSurface surface = new Cairo.ImageSurface(Cairo.Format.ARGB32, backgroundImage.Width, backgroundImage.Height))
                    {
                        using (Cairo.Context surfaceContext = new Cairo.Context(surface))
                        {
                            Gdk.CairoHelper.SetSourcePixbuf(surfaceContext, backgroundImage, 0, 0);
                            surfaceContext.Paint();
                        }

                        cr.Save();
                        cr.SetSource(surface, 0, 0);
                        cr.PaintWithAlpha(opacity);
                        cr.Restore();
                    }
                }
            }
        }


        private void OnAnimationTimerElapsed(object sender, ElapsedEventArgs e)
        {
            if (fadingOut)
            {
                opacity -= 0.05;
                if (opacity <= 0)
                {
                    opacity = 0;
                    fadingOut = false; 
                }
            }
            else
            {
                opacity += 0.05;
                if (opacity >= 1)
                {
                    opacity = 1;
                    fadingOut = true; 
                }
            }

            drawingArea.QueueDraw();
        }

        public void UpdateProgress(string message)
        {
            string logPath = $"log {DateTime.Now.Day}_{DateTime.Now.Month}_{DateTime.Now.Year}.txt";

            Idle.Add(() =>
            {
                textView.Buffer.InsertAtCursor($"{DateTime.Now}: {message}\n");
                textView.ScrollToIter(textView.Buffer.EndIter, 0, false, 0, 0);
                WriteToLogFile(logPath, message);
                return false; 
            });

        }

        private static void WriteToLogFile(string logPath, string message)
        {
            if (!System.IO.File.Exists(logPath))
            {
                File.CreateText(logPath);
                LogWriter(logPath, message);
            }
            else
            {
                LogWriter(logPath, message);
            }
        }

        private static void LogWriter(string logPath,string message)
        {
            using (StreamWriter writer = new StreamWriter(logPath, true))
            {
                writer.WriteLine($"{DateTime.Now}\n\t: {message}\n");
                writer.Flush();
                writer.Close();
            }
        }

        protected override bool OnDeleteEvent(Gdk.Event evnt)
        {
            animationTimer.Stop();
            animationTimer.Dispose();
            return base.OnDeleteEvent(evnt);
        }
    }

}

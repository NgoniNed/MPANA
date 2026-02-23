using System;
using Gtk;

namespace MPANAGTK.Frontend
{
    public class BMI_RView : ScrolledWindow
    {
        public Entry WeightEntry { get; private set; }
        public Entry HeightEntry { get; private set; }
        public Entry AgeEntry { get; private set; }
        public ComboBox GenderComboBox { get; private set; }
        public ComboBox ActivityComboBox { get; private set; }

        public Label BMIResultLabel { get; private set; }
        public Label BMIStatusLabel { get; private set; }
        public Label BMRResultLabel { get; private set; }
        public Label MaintainBMRResultLabel { get; private set; }

        public BMI_RView()
        {
            SetPolicy(PolicyType.Automatic, PolicyType.Automatic);

            VBox mainLayout = new VBox(false, 20)
            {
                BorderWidth = 20
            };

            Frame inputCard = new Frame() { ShadowType = ShadowType.In };
            Label inputHeader = new Label() { Markup = "<span size='x-large' weight='bold' color='#2c3e50'> Physical Metrics</span>" };
            inputHeader.SetAlignment(0, 0.5f);
            inputCard.LabelWidget = inputHeader;

            VBox inputVBox = new VBox(false, 10) { BorderWidth = 15 };

            WeightEntry = new Entry { TooltipText = "Enter Your Weight (kg)" };
            HeightEntry = new Entry { TooltipText = "Enter Your Height (m)" };
            AgeEntry = new Entry { TooltipText = "Enter Your Age" };
            GenderComboBox = new ComboBox(Enum.GetNames(typeof(Models.Gender))) { Active = 0 };
            ActivityComboBox = new ComboBox(Enum.GetNames(typeof(Models.ActivityLevel))) { Active = 0 };

            inputVBox.PackStart(CreateInputRow("Weight (kg):", WeightEntry), false, false, 0);
            inputVBox.PackStart(CreateInputRow("Height (m):", HeightEntry), false, false, 0);
            inputVBox.PackStart(CreateInputRow("Age:", AgeEntry), false, false, 0);
            inputVBox.PackStart(CreateInputRow("Gender:", GenderComboBox), false, false, 0);
            inputVBox.PackStart(CreateInputRow("Activity Level:", ActivityComboBox), false, false, 0);

            inputCard.Add(inputVBox);

            Frame resultCard = new Frame() { ShadowType = ShadowType.In };
            Label resultHeader = new Label() { Markup = "<span size='x-large' weight='bold' color='#27ae60'> Health Results</span>" };
            resultHeader.SetAlignment(0, 0.5f);
            resultCard.LabelWidget = resultHeader;

            VBox resultVBox = new VBox(false, 10) { BorderWidth = 15 };

            BMIResultLabel = new Label { Markup = "<span size='xx-large' weight='heavy'>0.0</span>" };
            BMIStatusLabel = new Label { Markup = "<span weight='bold'>Unknown</span>" };
            BMRResultLabel = new Label("0.0");
            MaintainBMRResultLabel = new Label("0.0");

            resultVBox.PackStart(BMIResultLabel, false, false, 10);
            resultVBox.PackStart(CreateInputRow("BMI Status:", BMIStatusLabel), false, false, 0);
            resultVBox.PackStart(CreateInputRow("Base BMR:", BMRResultLabel), false, false, 0);
            resultVBox.PackStart(CreateInputRow("Maintain Weight BMR:", MaintainBMRResultLabel), false, false, 0);

            resultCard.Add(resultVBox);
            Table layoutTable = new Table(1, 2, true);
            layoutTable.Attach(inputCard, 0, 1, 0, 1);
            layoutTable.Attach(resultCard, 1, 2, 0, 1);
            //mainLayout.PackStart(inputCard, false, false, 0);
            //mainLayout.PackStart(resultCard, false, false, 0);
            mainLayout.PackStart(layoutTable, false, false, 0);

            this.AddWithViewport(mainLayout);
        }

        private HBox CreateInputRow(string labelText, Widget inputWidget)
        {
            HBox row = new HBox(false, 10);
            Label lbl = new Label { Markup = $"<span weight='semibold'>{labelText}</span>" };
            lbl.SetAlignment(0, 0.5f);

            row.PackStart(lbl, true, true, 0);
            row.PackEnd(inputWidget, false, false, 0);
            return row;
        }
    }
}

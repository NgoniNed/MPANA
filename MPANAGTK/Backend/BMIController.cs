using System;
using MPANAGTK.Models;
using MPANAGTK.Frontend;

namespace MPANAGTK.Backend
{
    public class BMIController
    {
        private readonly BMI _model;
        private readonly BMI_RView _view;

        public BMIController(BMI model, BMI_RView view) 
        {
            _model = model;
            _view = view;

            WireUpViewEvents();
            WireUpModelEvents();
        }

        private void WireUpViewEvents()
        {
            _view.WeightEntry.Changed += (s, e) => {
                if (double.TryParse(_view.WeightEntry.Text, out double w)) _model.Weight = w;
            };

            _view.HeightEntry.Changed += (s, e) => {
                if (double.TryParse(_view.HeightEntry.Text, out double h)) _model.Height = h;
            };

            _view.AgeEntry.Changed += (s, e) => {
                if (int.TryParse(_view.AgeEntry.Text, out int a)) _model.Age = a;
            };

            _view.GenderComboBox.Changed += (s, e) => {
                _model.Gender = (Gender)_view.GenderComboBox.Active;
            };

            _view.ActivityComboBox.Changed += (s, e) => {
                _model.ActivityLevel = (ActivityLevel)_view.ActivityComboBox.Active;
            };
        }

        private void WireUpModelEvents()
        {
            _model.PropertyChanged += (s, e) =>
            {
                switch (e.PropertyName)
                {
                    case nameof(_model.BodyBMI):
                        _view.BMIResultLabel.Markup = $"<span size='xx-large' weight='heavy' color='#2980b9'>{_model.BodyBMI:F2}</span>";
                        break;
                    case nameof(_model.CurrentStatus):
                        _view.BMIStatusLabel.Markup = $"<span weight='bold'>{_model.CurrentStatus}</span>";
                        break;
                    case nameof(_model.BodyBMR):
                        _view.BMRResultLabel.Text = $"{_model.BodyBMR:F2} kcal";
                        break;
                    case nameof(_model.MaintainWeightBMR):
                        _view.MaintainBMRResultLabel.Text = $"{_model.MaintainWeightBMR:F2} kcal";
                        break;
                }
            };
        }
    }
}
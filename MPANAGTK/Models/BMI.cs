using System;
using System.ComponentModel;

namespace MPANAGTK.Models
{
    public class BMI : INotifyPropertyChanged
    {
        private double _weight;
        private double _height;
        private int _age;
        private Gender _gender = Gender.Female;
        private ActivityLevel _activityLevel = ActivityLevel.Sedentary;

        public double Weight
        {
            get => _weight;
            set { _weight = value; NotifyCalculations(); }
        }

        public double Height
        {
            get => _height;
            set { _height = value; NotifyCalculations(); }
        }

        public int Age
        {
            get => _age;
            set { _age = value; OnPropertyChanged(nameof(BodyBMR)); OnPropertyChanged(nameof(MaintainWeightBMR)); }
        }

        public Gender Gender
        {
            get => _gender;
            set { _gender = value; OnPropertyChanged(nameof(BodyBMR)); OnPropertyChanged(nameof(MaintainWeightBMR)); }
        }

        public ActivityLevel ActivityLevel
        {
            get => _activityLevel;
            set { _activityLevel = value; OnPropertyChanged(nameof(MaintainWeightBMR)); }
        }

        public double BodyBMI => (Height > 0 && Weight > 0) ? (Weight / (Height * Height)) : 0;

        public double BodyBMR
        {
            get
            {
                if (Weight == 0 || Height == 0 || Age == 0) return 0;
                double bmr = (10 * Weight + 6.25 * Height - 5 * Age);
                return Gender == Gender.Female ? bmr - 161 : bmr + 5;
            }
        }

        public double MaintainWeightBMR
        {
            get
            {
                switch (ActivityLevel)
                {
                    case ActivityLevel.Sedentary: return BodyBMR * 1.2;
                    case ActivityLevel.LightlyActive: return BodyBMR * 1.375;
                    case ActivityLevel.ModeratelyActive: return BodyBMR * 1.550;
                    case ActivityLevel.VeryActive: return BodyBMR * 1.725;
                    case ActivityLevel.ExtraActive: return BodyBMR * 1.9;
                    default: return BodyBMR;
                }
            }
        }

        public BMIStatus CurrentStatus
        {
            get
            {
                if (BodyBMI < 18.5) return BMIStatus.Underweight;
                if (BodyBMI < 25.0) return BMIStatus.NormalWeight;
                if (BodyBMI < 30.0) return BMIStatus.PreObesity;
                if (BodyBMI < 35.0) return BMIStatus.ObesityClassI;
                if (BodyBMI < 40.0) return BMIStatus.ObesityClassII;
                return BMIStatus.ObesityClassIII;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));

        private void NotifyCalculations()
        {
            OnPropertyChanged(nameof(BodyBMI));
            OnPropertyChanged(nameof(CurrentStatus));
            OnPropertyChanged(nameof(BodyBMR));
            OnPropertyChanged(nameof(MaintainWeightBMR));
        }
    }
}

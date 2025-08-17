using System;
using System.Collections.Generic;
using Gtk;
using MPANAGTK;

internal class BMI_RViewPoint : ScrolledWindow
{

    public BMI_RViewPoint()
    {
        bMICalculator = new MPANAGTK.Backend.BMI();

        //ShadowType = ShadowType.In;
        SetPolicy(PolicyType.Automatic, PolicyType.Automatic);

        Table bmiCalcTable = new Table(2,1,true);

        VBox heightWeightVBox = new VBox(true,10);

        Label weightLabel = new Label();
        weightLabel.Text = "Weight";
        Entry weightEntry = new Entry();
        weightEntry.TooltipText = "Enter Your Weight";
        weightEntry.TextInserted += WeightEntry_TextInserted;
        weightEntry.TextDeleted += WeightEntry_TextDeleted;
        HBox weightHbox = new HBox();
        weightHbox.PackStart(weightLabel, true, false, 5);
        weightHbox.PackEnd(weightEntry, true, false, 5);

        heightWeightVBox.PackStart(weightHbox);

        Label heightLabel = new Label();
        heightLabel.Text = "Height";
        Entry heightEntry = new Entry();
        heightEntry.TextInserted += HeightEntry_TextInserted;
        heightEntry.TextDeleted += HeightEntry_TextDeleted;
        heightEntry.TooltipText = "Enter Your Height";
        HBox heightHbox = new HBox();
        heightHbox.PackStart(heightLabel, true, false, 5);
        heightHbox.PackEnd(heightEntry, true, false, 5);

        heightWeightVBox.PackEnd(heightHbox);

        userBMIValue = new Entry();
        userBMIValue.Sensitive = false;
        userBMIValue.Text = "BMI Value Shows Here";

        bmiStatusCombobox = new ComboBox(System.Enum.GetNames(typeof(MPANAGTK.Backend.BMIStatus)));
        bmiStatusCombobox.Sensitive = false;

        VBox bmiStatusValueVBox = new VBox();
        bmiStatusValueVBox.PackStart(userBMIValue);

        HBox BMRActivityHBox = new HBox();
        BMRMaintainEntry = new Entry();
        BMRMaintainEntry.Sensitive = false;
        BMRActivityHBox.PackEnd(BMRMaintainEntry);

        Label BMRMaintainLabel = new Label("Weight-BMR To Maintain");
        BMRActivityHBox.PackEnd(BMRMaintainLabel);

        BMREntry = new Entry();
        BMREntry.Sensitive = false;
        BMRActivityHBox.PackEnd(BMREntry);

        Label BMRLabel = new Label("Basic Metabolic Rate");
        BMRActivityHBox.PackEnd(BMRLabel);

        activityEntry = new Entry(bMICalculator.ActivityLevel.ToString());
        activityEntry.Sensitive = false;
        BMRActivityHBox.PackEnd(activityEntry);

        Label ActivityLevel = new Label("Activity Level");
        BMRActivityHBox.PackEnd(ActivityLevel);

        bmiStatusValueVBox.PackStart(BMRActivityHBox);
        bmiStatusValueVBox.PackEnd(bmiStatusCombobox);

        Label ageLabel = new Label();
        ageLabel.Text = "Age";
        ageEntry = new Entry();
        ageEntry.TextInserted += AgeEntry_TextInserted;
        ageEntry.TextDeleted += AgeEntry_TextDeleted;
        ageEntry.TooltipText = "Enter Your Age";

        HBox ageHbox = new HBox();
        ageHbox.PackStart(ageLabel, true, false, 5);
        ageHbox.PackEnd(ageEntry, true, false, 5);

        Label genderLabel = new Label();
        genderLabel.Text = "Gender";
        ComboBox genderComboBox = new ComboBox(Enum.GetNames(typeof(MPANAGTK.Backend.Gender)));
        genderComboBox.Active = (int)MPANAGTK.Backend.Gender.Female;
        genderComboBox.Changed += GenderComboBox_Changed;

        HBox genderHbox = new HBox();
        genderHbox.PackStart(genderLabel, true, false, 5);
        genderHbox.PackEnd(genderComboBox, true, false, 5);

        VBox ageGenderVBox = new VBox(true, 10);


        Label activityLabel = new Label();
        activityLabel.Text = "Activity Level";
        ComboBox activityComboBox = new ComboBox(Enum.GetNames(typeof(MPANAGTK.Backend.ActivityLevel)));
        activityComboBox.Active = (int)MPANAGTK.Backend.ActivityLevel.Sedentary;
        HBox activityHbox = new HBox();
        activityHbox.PackStart(activityLabel, true, false, 5);
        activityHbox.PackEnd(activityComboBox, true, false, 5);
        activityComboBox.Changed += ActivityComboBox_Changed;

        ageGenderVBox.PackStart(ageHbox, true, false, 5);
        ageGenderVBox.PackEnd(genderHbox, true, false, 5);
        ageGenderVBox.PackStart(activityHbox, true, false, 5);

        //heightWeightVBox.PackEnd(ageGenderVBox);

        HBox topEntryArea = new HBox();
        
        topEntryArea.PackStart(heightWeightVBox);
        topEntryArea.PackEnd(ageGenderVBox);

        bMICalculator.BodyBMIPropertyChanged += BMICalculator_BodyBMIPropertyChanged;
        bMICalculator.BodyBMRPropertyChanged += BMICalculator_BodyBMRPropertyChanged;
        bMICalculator.BodyActivityPropertyChanged += BMICalculator_BodyActivityPropertyChanged;

        bmiCalcTable.Attach(topEntryArea, 0, 1, 0, 1);
        bmiCalcTable.Attach(bmiStatusValueVBox, 0, 1, 1, 2);

        this.AddWithViewport(bmiCalcTable);
    }

    private void ActivityComboBox_Changed(object sender, EventArgs e)
    {
        ComboBox comboBox = (ComboBox)sender;
        bMICalculator.ActivityLevel = (MPANAGTK.Backend.ActivityLevel)comboBox.Active;
    }

    private void GenderComboBox_Changed(object sender, EventArgs e)
    {
        ComboBox comboBox = (ComboBox)sender;
        bMICalculator.Gender = (MPANAGTK.Backend.Gender)comboBox.Active;
    }
    private Entry BMREntry,activityEntry, BMRMaintainEntry;
    private void BMICalculator_BodyActivityPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        activityEntry.Text = string.Empty;
        activityEntry.Text = bMICalculator.ActivityLevel.ToString();
        BMICalculator_BodyBMRPropertyChanged(sender,e);
    }

    private void BMICalculator_BodyBMRPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        BMREntry.Text = string.Empty;
        BMREntry.Text = bMICalculator.BodyBMR.ToString();
        BMRMaintainEntry.Text = bMICalculator.MaintainWeightBMR.ToString();
        //System.Diagnostics.Debug.WriteLine($"BMR value=\t{bMICalculator.BodyBMR}\n Maintain Weight value is\t{bMICalculator.MaintainWeightBMR}");
    }

    private void AgeEntry_TextDeleted(object sender, TextDeletedArgs args)
    {
        GetUserAge(sender);
    }
    private Entry ageEntry;
    private void AgeEntry_TextInserted(object sender, TextInsertedArgs args)
    {
        GetUserAge(sender);
    }

    private void GetUserAge(object sender)
    {
        Entry senderEntry = (Entry)sender;
        int ageValue = -1;
        if (int.TryParse(senderEntry.Text, out ageValue))
        {
            bMICalculator.Age = ageValue;
        }
    }

    private void BMICalculator_BodyBMIPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        SetBMIValue();
    }

    private void WeightEntry_TextDeleted(object sender, TextDeletedArgs args)
    {
        GetUserWeight(sender);
    }

    private void WeightEntry_TextInserted(object sender, TextInsertedArgs args)
    {
        GetUserWeight(sender);
    }

    private void GetUserWeight(object sender)
    {
        Entry senderEntry = (Entry)sender;
        double weightValue = -1;
        if (double.TryParse(senderEntry.Text, out weightValue))
        {
            bMICalculator.Weight = weightValue;
        }
    }
    private MPANAGTK.Backend.BMI bMICalculator;
    private void HeightEntry_TextInserted(object sender, TextInsertedArgs args)
    {
        GetUserHeight(sender);
    }
    private void HeightEntry_TextDeleted(object sender, TextDeletedArgs args)
    {
        GetUserHeight(sender);

    }
    private void GetUserHeight(object sender)
    {
        Entry senderEntry = (Entry)sender;
        double heightValue = -1;
        if (double.TryParse(senderEntry.Text, out heightValue))
        {
            bMICalculator.Height = heightValue;
        }
    }

    private Entry userBMIValue;
    private ComboBox bmiStatusCombobox;
    private void SetBMIValue()
    {
        userBMIValue.Text = bMICalculator.BodyBMI.ToString();
        if(bMICalculator.BodyBMI<18.5)
        {
            bmiStatusCombobox.Active = (int)MPANAGTK.Backend.BMIStatus.Underweight;
        }
        if (bMICalculator.BodyBMI >= 18.5 && bMICalculator.BodyBMI <= 24.9)
        {
            bmiStatusCombobox.Active = (int)MPANAGTK.Backend.BMIStatus.NormalWeight;
        }
        if (bMICalculator.BodyBMI >= 25.0 && bMICalculator.BodyBMI <= 29.9)
        {
            bmiStatusCombobox.Active = (int)MPANAGTK.Backend.BMIStatus.PreObesity;
        }
        if (bMICalculator.BodyBMI >= 30.0 && bMICalculator.BodyBMI <= 34.9)
        {
            bmiStatusCombobox.Active = (int)MPANAGTK.Backend.BMIStatus.ObesityClassI;
        }
        if (bMICalculator.BodyBMI >= 35.0 && bMICalculator.BodyBMI >= 39.9)
        {
            bmiStatusCombobox.Active = (int)MPANAGTK.Backend.BMIStatus.ObesityClassII;
        }
        if (bMICalculator.BodyBMI >= 40)
        {
            bmiStatusCombobox.Active = (int)MPANAGTK.Backend.BMIStatus.ObesityClassIII;
        }
    }
    

}
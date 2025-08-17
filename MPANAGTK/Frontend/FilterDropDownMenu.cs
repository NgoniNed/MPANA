using Gtk;

namespace MPANAGTK.Frontend
{

    public class FilterDropDownMenu : VBox
    {
        ComboBox filterOptions;
        Entry filterApplied;
        public FilterDropDownMenu(FoodDBView foodDBView)
        {
            Label label = new Label("Select Filter Option");

            filterOptions = new ComboBox(foodDBView.FilterOptionsList());
            filterApplied = new Entry()
            {
                Sensitive = false
            };
            filterOptions.Changed += FilterOptions_Changed;
            filterApplied.Changed += foodDBView.AppliedFilter_Changed;
            this.PackStart(label, false, false, 5);
            this.PackStart(filterOptions, false, false, 5);
            this.PackStart(filterApplied, false, false, 5);

            this.ShowAll();
        }

        private void FilterOptions_Changed(object sender, System.EventArgs e)
        {
            string selectedFilter = filterOptions.ActiveText;
            filterApplied.Text = selectedFilter;
        }
    }
}
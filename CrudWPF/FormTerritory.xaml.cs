using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CrudWPF
{
    /// <summary>
    /// Lógica de interacción para FormTerritory.xaml
    /// </summary>
    public partial class FormTerritory : Page
    {
        public string IdTerritory { get; set; } = "";

        public FormTerritory()
        {
            InitializeComponent();
            FillRegions();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (!IdTerritory.Equals(""))
            {
                txtTerritoryId.IsReadOnly = true;

                using (var db = new Model.NorthwindDataContext())
                {
                    (from t in db.Territories
                     join r in db.Region on t.RegionID equals r.RegionID
                     where t.TerritoryID.Equals(IdTerritory)
                     select new
                     {
                         t.TerritoryID,
                         t.TerritoryDescription,
                         r.RegionDescription
                     })
                     .ToList()
                     .ForEach(x => 
                     {
                         txtTerritoryId.Text = x.TerritoryID;
                         txtDescription.Text = x.TerritoryDescription;
                         cmbRegion.Text = x.RegionDescription;
                     });
                }
            }
        }

        void FillRegions()
        {
            using (var db = new Model.NorthwindDataContext())
            {
                cmbRegion.ItemsSource = db.Region;               
                cmbRegion.SelectedValuePath = "RegionID";
                cmbRegion.DisplayMemberPath = "RegionDescription";
            }
        }

        void AddTerritory()
        {
            using(var db = new Model.NorthwindDataContext())
            {
                var oTerritory = new Model.Territories
                {
                    TerritoryID = txtTerritoryId.Text,
                    TerritoryDescription = txtDescription.Text,
                    RegionID = (int)cmbRegion.SelectedValue
                };

                db.Territories.InsertOnSubmit(oTerritory);

                try
                {
                    db.SubmitChanges();
                    MessageBox.Show("Successfully registered","Add", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("error: " + ex.Message);
                }
            }
        }

        void UpdateTerritory()
        {
            using (var db = new Model.NorthwindDataContext())
            {
                var query = db.Territories.Where(x => x.TerritoryID.Equals(IdTerritory));

                foreach (Model.Territories oTerritory in query)
                {
                    oTerritory.TerritoryDescription = txtDescription.Text;
                    oTerritory.RegionID = (int)cmbRegion.SelectedValue;
                }

                try
                {
                    db.SubmitChanges();
                    MessageBox.Show("Successfully updated", "Update", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {

                    MessageBox.Show("error: " + ex.Message);
                }
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if(txtTerritoryId.Text== string.Empty || txtDescription.Text == string.Empty || cmbRegion.SelectedIndex == -1)
            {
                MessageBox.Show("empty fields");
                return;
            }

            if (IdTerritory.Equals(""))
            {
                AddTerritory();
            }
            else
            {
                UpdateTerritory();
            }
        }
    }
}

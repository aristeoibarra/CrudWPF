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
    /// Lógica de interacción para MenuTerritories.xaml
    /// </summary>
    public partial class MenuTerritories : Page
    {
        public MenuTerritories()
        {
            InitializeComponent();
            Refresh();
        }

        void Refresh()
        {
            using(var db = new Model.NorthwindDataContext())
            {
                dgvData.ItemsSource = (from t in db.Territories
                                       join r in db.Region on t.RegionID equals r.RegionID
                                       where t.habilitado.Equals(true)
                                       select new
                                       {
                                           t.TerritoryID,
                                           t.TerritoryDescription,
                                           Region = r.RegionDescription
                                       }).ToList();
            }
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            string idTerritory = (string)((Button)sender).CommandParameter;

            if (MessageBox.Show("Are you sure to delete?", "Confirm delete", MessageBoxButton.YesNo, MessageBoxImage.Question)
                == MessageBoxResult.Yes)
            {
                using (var db = new Model.NorthwindDataContext())
                {
                    var query = db.Territories.Where(x => x.TerritoryID.Equals(idTerritory));

                    foreach(Model.Territories oTerritory in query)
                    {
                        oTerritory.habilitado = false;
                    }

                    try
                    {
                        db.SubmitChanges();
                        MessageBox.Show("Was successfully removed", "Delete", MessageBoxButton.OK, MessageBoxImage.Information);
                        Refresh();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("error: " + ex.Message);
                    }
                } 
            }

           
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.StaticMainFrame.Content = new FormTerritory();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace TeamWorkOPC
{
    /// <summary>
    /// Interaction logic for ManageFiles.xaml
    /// </summary>
    public partial class ManageFiles : UserControl
    {
        //xamlObs ViewModel { get; set; }
        public ManageFiles()
        {
            InitializeComponent();
            //explore_file(xml, username, password);
            //this.DataContext = ;
        }

        
        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {

        }
    }
}

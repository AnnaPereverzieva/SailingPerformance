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
using ClientService;

namespace Gui
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void BtnCheck_OnClick(object sender, RoutedEventArgs e) // sluzy tylko do sprawdzania serwisu User. TO do usunac
        {
            UserService user = new UserService();
            //  string time = "02-10-2015 00:05";
            label.Content = await user.GetGpsDataByDate(DateTime.Now, DateTime.Now);

        }
    }
}

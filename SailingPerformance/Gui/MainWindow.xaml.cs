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
using Gui.ViewModel;

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
            DataContext=new ChartViewModel();
        }

        private async void BtnCheck_OnClick(object sender, RoutedEventArgs e) // sluzy tylko do sprawdzania serwisu Gps. TO do usunac
        {
            //GpsService gps = new GpsService();
            //int id = 3;
            //var response = await gps.GetGpsDataByDate(DateTime.Now, DateTime.Now, id);
            //TxtBlock.Text = response.Date.Date + "\n";
            //var data = response.Longitude.Zip(response.Latitude, (n, l) => new { Longitude = n, Latitude = l }).Zip(response.Time, (k,s)=>new  {LongLatitude=k, Time=s});
            //foreach (var nw in data)
            //{
            //    TxtBlock.Text += nw.Time + " " + nw.LongLatitude + "\n";
            //}
        }
    }
}

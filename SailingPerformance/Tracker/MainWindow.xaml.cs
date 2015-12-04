using System.Windows;
using Tracker.BoatService;

namespace Tracker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnAddBoat_Click(object sender, RoutedEventArgs e)
        {
            var client=new BoatServiceClient();
            BaseResponse response=client.AddBoatResponse(new BoatRequest {Name = "Ola", Model = "hgj"});
            Txb.Text= response.ErrorMessage.ToString();

            client.Close();
        }
    }
}

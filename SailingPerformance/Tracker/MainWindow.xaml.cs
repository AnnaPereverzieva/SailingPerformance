using System;
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
            GetBoatResponse response=client.GetBoatId(new BoatRequest {Name = "Frosia", Model = "hgj"});
            Txb.Text = response.IsSuccess.ToString();
            Guid g = response.Id;
            BaseResponse response1 = client.DeleteBoat(new DeleteBoatRequest {Id=g});
            if (response1.ErrorMessage!=string.Empty)
            Txb.Text += response.ErrorMessage;
            Txb.Text += response1.IsSuccess.ToString();

            client.Close();
        }
    }
}

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
            BaseResponse response=client.AddBoat(new BoatRequest {Name = "Marina", Model = "Gjj6790"});
            Txb.Text = response.IsSuccess.ToString();
          //  Guid g = response.Id;
          //  BaseResponse response1 = client.DeleteBoat(new DeleteBoatRequest {Id=g});
            if (response.ErrorMessage!=string.Empty)
            Txb.Text += response.ErrorMessage;
            Txb.Text += response.IsSuccess.ToString();

            client.Close();
        }
    }
}

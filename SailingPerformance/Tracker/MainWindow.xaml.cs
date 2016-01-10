using System;
using System.Windows;
using Tracker.BoatService;
using Tracker.ServiceReference;

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
           // var client=new BoatServiceClient();
          //  BaseResponse response=client.AddBoat(new BoatRequest {Name = "Marina", Model = "Gjj6790"});
          var client=new SessionServiceClient();
            AddSessionResponse response =
                client.AddSession(new AddSessionRequest
                {
                    StartDate =DateTime.ParseExact("2009-05-08 14:40:52,531", "yyyy-MM-dd HH:mm:ss,fff",
                            System.Globalization.CultureInfo.InvariantCulture),StopDate = DateTime.Now, IdBoat =new Guid("1608fc44-abb7-e511-82af-acb57d99b460") 
                });
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

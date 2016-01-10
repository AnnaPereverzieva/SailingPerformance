using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using Tracker.BoatService;
using Tracker.GpsServiceReference;
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

        private void btnAddData_Click(object sender, RoutedEventArgs e)
        {
             var client=new GpsServiceClient();
            AddDataRequest request=new AddDataRequest();
            request.IdBoat = new Guid("5608fc44-abb7-e511-82af-acb57d99b460");

            string[] dataGridPagerStyle = File.ReadAllLines(@"C:\Users\hpereverzieva\Desktop\11.txt");
            request.GpsDataList= new GpsData[60];
            try
            {
                for (int i = 0; i < dataGridPagerStyle.Count(); i++)
                {
                    string entry = dataGridPagerStyle[i];
                    string item = entry.Substring(0, entry.IndexOf(";", StringComparison.Ordinal));
                    request.GpsDataList[i]=new GpsData();
                    request.GpsDataList[i].SecondsFromStart = Convert.ToDateTime(item);


                    entry = entry.Remove(0, entry.IndexOf(";", StringComparison.Ordinal) + 1);
                    item = entry.Substring(0, entry.IndexOf(";", StringComparison.Ordinal));
                    request.GpsDataList[i].GeoWidth =item;

                    entry = entry.Remove(0, entry.IndexOf(";", StringComparison.Ordinal) + 1);
                    item = entry.Substring(0, entry.IndexOf(";", StringComparison.Ordinal));
                    request.GpsDataList[i].GeoHeight = item;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            var response =client.AddData(request);
            Txb.Text += "koniec!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!";
            Txb.Text += response.ErrorMessage;
            Txb.Text += response.IsSuccess.ToString();

            client.Close();

        }
    }
}

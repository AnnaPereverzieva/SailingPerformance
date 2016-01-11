using System;
using System.Collections.ObjectModel;

using System.Windows.Input;
using ClientService.Model;
using ClientService.Services;
using Microsoft.Expression.Interactivity.Core;
using PropertyChanged;
using Microsoft.Win32;

namespace Gui.ViewModel
{
    [ImplementPropertyChanged]
    public class MainWindowViewModel
    {
        public ChartViewModel ChartViewModel { get; set; }
        public ICommand DrawAction { get; set; }

        public ICommand ImportExcelDataCommand { get; set; }
        public ICommand SaveToExcelCommand { get; set; }
        public MainWindowViewModel()
        {
            DrawAction=new ActionCommand(DrawChart);
            ImportExcelDataCommand = new ActionCommand(ImportExcel);
            SaveToExcelCommand = new ActionCommand(SaveExcel);
            DrawAction = new ActionCommand(DrawChart);
            GetBoatsCommand = new ActionCommand(GetBoats);
        }

        private void SaveExcel()
        {
            string filePath = string.Empty;
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Excel files (*.xls;*.xlsx)|*.xls;xlsx";
            if (saveFileDialog.ShowDialog() == true)
                filePath = saveFileDialog.FileName;
        }

        private void ImportExcel()
        {
            string filePath = string.Empty;
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Excel files (*.xls;*.xlsx)|*.xls;xlsx|All files (*.*)| *.*";
            if (openFileDialog.ShowDialog() == true)
                filePath = openFileDialog.FileName;
        }




        public ICommand GetBoatsCommand { get; set; }
        public ObservableCollection<BoatDto> BoatsCollection { get; set; }
        public ObservableCollection<SessionDto> SessionCollection { get; set; }
        public ObservableCollection<DataGps> DataCollection { get; set; }

    

        private void GetBoats()
        {
            var boatService = new BoatService();

            BoatsCollection = new ObservableCollection<BoatDto>(boatService.GetBoats());

            var sessionService = new SessionService();
            SessionCollection = new ObservableCollection<SessionDto>(sessionService.GetSessions(DateTime.ParseExact("2008-05-08 14:40:52,531", "yyyy-MM-dd HH:mm:ss,fff",
                            System.Globalization.CultureInfo.InvariantCulture), DateTime.Now, new Guid("5608FC44-ABB7-E511-82AF-ACB57D99B460")));
            var dataService = new GpsDataService();
            DataCollection = new ObservableCollection<DataGps>(dataService.GetSessions(new Guid("4ADAEDD9-DAB7-E511-82AF-ACB57D99B460")));

        }

        private void DrawChart()
        {
            var readExcel = new ReadExcelService();
            var list = readExcel.LoadData("");
            ChartViewModel = new ChartViewModel(list, 10, 2);
        }
    }
}

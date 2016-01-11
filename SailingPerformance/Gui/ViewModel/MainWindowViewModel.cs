using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using ClientService.Model;
using ClientService.Services;
using Microsoft.Expression.Interactivity.Core;
using PropertyChanged;

namespace Gui.ViewModel
{
    [ImplementPropertyChanged]
    public class MainWindowViewModel
    {
        public ChartViewModel ChartViewModel { get; set; }
        public ICommand DrawAction { get; set; }
        public ICommand GetBoatsCommand { get; set; }
        public ObservableCollection<BoatDto> BoatsCollection { get; set; }
        public ObservableCollection<SessionDto> SessionCollection { get; set; }

        public MainWindowViewModel()
        {
            DrawAction = new ActionCommand(DrawChart);
            GetBoatsCommand = new ActionCommand(GetBoats);
        }

        private void GetBoats()
        {
            var boatService = new BoatService();
            BoatsCollection=new ObservableCollection<BoatDto>(boatService.GetBoats());

            var sessionService=new SessionService();
            SessionCollection = new ObservableCollection<SessionDto>(sessionService.GetSessions(DateTime.ParseExact("2008-05-08 14:40:52,531", "yyyy-MM-dd HH:mm:ss,fff",
                            System.Globalization.CultureInfo.InvariantCulture), DateTime.Now, new Guid("5608FC44-ABB7-E511-82AF-ACB57D99B460")));
        }

        private void DrawChart()
        {
            var readExcel = new ReadExcelService();
            var list = readExcel.LoadData("");
            ChartViewModel = new ChartViewModel(list, 10, 2);
        }
    }
}

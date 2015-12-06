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

        public MainWindowViewModel()
        {
            DrawAction = new ActionCommand(DrawChart);
            GetBoatsCommand = new ActionCommand(GetBoats);
        }

        private void GetBoats()
        {
            var boatService = new BoatService();
            BoatsCollection=new ObservableCollection<BoatDto>(boatService.GetBoats());
        }

        private void DrawChart()
        {
            var readExcel = new ReadExcelService();
            var list = readExcel.LoadData("");
            ChartViewModel = new ChartViewModel(list, 10, 2);
        }
    }
}

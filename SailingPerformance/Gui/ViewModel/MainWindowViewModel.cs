using System;
using System.Windows.Input;
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
        public MainWindowViewModel()
        {
            DrawAction=new ActionCommand(DrawChart);
        }

        private void DrawChart()
        {
            var readExcel = new ReadExcelService();
            var list = readExcel.LoadData("");
            ChartViewModel = new ChartViewModel(list, 10, 2);
        }
    }
}

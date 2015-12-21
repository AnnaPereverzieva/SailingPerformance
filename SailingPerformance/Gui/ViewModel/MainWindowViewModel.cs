using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;
using ClientService.Model;
using ClientService.Services;
using Gui.Common;
using Microsoft.Expression.Interactivity.Core;
using PropertyChanged;
using Gui.View;

namespace Gui.ViewModel
{
    [ImplementPropertyChanged]
    public class MainWindowViewModel
    {
        public ChartViewModel ChartViewModel { get; set; }
        public ICommand DrawAction { get; set; }
        public ICommand ImportExcelDataCommand { get; set; }
        public MainWindowViewModel()
        {
            DrawAction=new ActionCommand(DrawChart);
            ImportExcelDataCommand = new ActionCommand(ImportExcel);
        }

        private void ImportExcel()
        {
            ImportExcelDataViewModel importExcelDataViewModel = new ImportExcelDataViewModel();
            var winImportData = new ImportExcelDataWindow(importExcelDataViewModel);
            winImportData.ShowDialog();
        }

        private void DrawChart()
        {
            var readExcel = new ReadExcelService();
            var list = readExcel.LoadData("");
            ChartViewModel = new ChartViewModel(list, 10, 2);
        }
    }
}

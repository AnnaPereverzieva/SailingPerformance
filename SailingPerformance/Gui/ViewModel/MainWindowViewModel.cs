using System;

using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using System.Collections.ObjectModel;

using System.Windows.Input;
using ClientService.Model;
using ClientService.Services;
using Gui.Common;
using Microsoft.Expression.Interactivity.Core;
using PropertyChanged;
using Gui.View;
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

using Gui.View;
using Microsoft.Expression.Interactivity.Core;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Gui.ViewModel
{
    [ImplementPropertyChanged]
    public class ImportExcelDataViewModel
    {
        public Action CloseAction { get; set; }
        public ImportExcelDataWindow Window { get; set; }
        public ICommand SearchCommand { get; set; }
        public string FilePath { get; set; }
        public ImportExcelDataViewModel()
        {
            SearchCommand = new ActionCommand(SearchForExcel);
        }

        private void SearchForExcel()
        {
            Gat.Controls.OpenDialogView openDialog = new Gat.Controls.OpenDialogView();
            Gat.Controls.OpenDialogViewModel viewModel = (Gat.Controls.OpenDialogViewModel)openDialog.DataContext;

            viewModel.AddFileFilterExtension(".xls");
            viewModel.AddFileFilterExtension(".txt");
            viewModel.AddFileFilterExtension(".doc");
            // Show dialog and take result into account
            bool? result = viewModel.Show();
            if (result == true)
            {
                // Get selected file path
                FilePath = viewModel.SelectedFilePath;
            }
            else
            {
                FilePath = string.Empty;
            }

        }
    }
}

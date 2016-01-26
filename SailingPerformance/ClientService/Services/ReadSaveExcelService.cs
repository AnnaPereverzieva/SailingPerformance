using System;
using System.Collections.Generic;
using System.Data;
using ClientService.Model;
using Spire.Xls;
using System.IO;
using System.Collections.ObjectModel;
using System.Linq;
using System.Globalization;

namespace ClientService.Services
{
    public class ReadSaveExcelService
    {
        /// <summary>
        /// pobieranie danych z excelu
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public List<DataGps> LoadData(string path)
        {
            Workbook workbook = new Workbook();
            List<DataGps> DataGpsList=new List<DataGps>();
            string fileExtension = Path.GetExtension(path).Replace(".", "");
            if (fileExtension == "xls")
                workbook.LoadFromFile(path, ExcelVersion.Version97to2003);
            else if (fileExtension == "xlsx")
                workbook.LoadFromFile(path, ExcelVersion.Version2010);


            Worksheet sheet = workbook.Worksheets[0];


            DataTable dataTable = sheet.ExportDataTable();
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                var dataGps=new DataGps();
                dataGps.BoatSpeed = Convert.ToDouble(dataTable.Rows[i]["predkosc"]);
                dataGps.BoatDirection = Convert.ToDouble(dataTable.Rows[i]["kurs"]);
                dataGps.WindDirection = Convert.ToDouble(dataTable.Rows[i]["kierunek_wiatru"]);
                dataGps.WindSpeed = Convert.ToDouble(dataTable.Rows[i]["sila_wiatru"]);
                dataGps.GeoWidth = dataTable.Rows[i]["szerokosc"].ToString();
                dataGps.GeoHeight = dataTable.Rows[i]["dlugosc"].ToString();
                DataGpsList.Add(dataGps);
            }
            return DataGpsList;
        }
		
        /// <summary>
        /// zapisywanie danych do excelu
        /// </summary>
        /// <param name="DataCollection">kolekcja danych gps </param>
        /// <param name="filePath">scierzka pliku</param>
        public void SaveData(ObservableCollection<DataGps> DataCollection, string filePath)
        {
            Workbook workbook = new Workbook();
            Worksheet sheet = workbook.Worksheets[0];
            int row = 2;
            var obj = DataCollection.FirstOrDefault();
            if (obj == null) return;
            sheet.Range["A" + 1].Text = nameof(obj.GeoHeight);
            sheet.Range["B" + 1].Text = nameof(obj.GeoWidth);
            sheet.Range["C" + 1].Text = nameof(obj.BoatDirection);
            sheet.Range["D" + 1].Text = nameof(obj.BoatSpeed);
            sheet.Range["E" + 1].Text = nameof(obj.WindDirection);
            sheet.Range["F" + 1].Text = nameof(obj.WindSpeed);
            sheet.Range["G" + 1].Text = nameof(obj.SecondsFromStart);

            foreach (var item in DataCollection)
            {
                sheet.Range["A" + row].Text = item.GeoHeight;
                sheet.Range["B" + row].Text = item.GeoWidth;
                sheet.Range["C" + row].Text = item.BoatDirection.ToString(CultureInfo.InvariantCulture);
                sheet.Range["D" + row].Text = item.BoatSpeed.ToString(CultureInfo.InvariantCulture);
                sheet.Range["E" + row].Text = item.WindDirection.ToString(CultureInfo.InvariantCulture);
                sheet.Range["F" + row].Text = item.WindDirection.ToString(CultureInfo.InvariantCulture);
                sheet.Range["G" + row].Text = item.SecondsFromStart.ToString(CultureInfo.InvariantCulture);
                row++;
            }
            workbook.SaveToFile(filePath);
            System.Diagnostics.Process.Start(workbook.FileName);
        }
    }
}

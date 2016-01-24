using System;
using System.Collections.Generic;
using System.Data;
using ClientService.Model;
using Spire.Xls;
using System.Windows.Forms;
using System.IO;

namespace ClientService.Services
{
    public class ReadExcelService
    {
        public List<DataGps> LoadData(string path)
        {
            Workbook workbook = new Workbook();
            List<DataGps> DataGpsList=new List<DataGps>();
            string fileExtension = Path.GetExtension(path).Replace(".", "");
            if (fileExtension == "xls")
                workbook.LoadFromFile(path, ExcelVersion.Version97to2003);
            else if (fileExtension == "xlsx")
                workbook.LoadFromFile(path, ExcelVersion.Version2013);


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
    }
}

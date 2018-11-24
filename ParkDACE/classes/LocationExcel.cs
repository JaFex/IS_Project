using System;
using System.Collections.Generic;

namespace ParkDACE.classes
{
    class LocationExcel
    {
        public string parkingID { get; set; }
        public string fileName { get; set; }
        public double numberOfSpots { get; set; }
        public DateTime updateDate { get; set; }
        public List<Location> locations { get; set; }

        
        public LocationExcel(string fileName)
        {
            this.fileName = fileName;
            locations = new List<Location>();
            readFile(fileName);
        }

        public Boolean update()
        {
            if (checkIfAlreadyUpdated(fileName))
            {
                Console.WriteLine("Already is updated!");
                return true;
            }
            return readFile(fileName);
        }

        private Boolean checkIfAlreadyUpdated(string fileName)
        {
            if (!FunctionHelper.checkIfFileExist(fileName))
            {
                Console.WriteLine("Fail to find the file(" + fileName + ")!");
                return false;
            }

            var excelAplication = new Microsoft.Office.Interop.Excel.Application();
            excelAplication.Visible = false;

            //Opens the excel file
            var excelWorkbook = excelAplication.Workbooks.Open(@"" + FunctionHelper.directory() + "" + fileName);
            var excelWorksheet = (Microsoft.Office.Interop.Excel.Worksheet)excelWorkbook.ActiveSheet;

            string parameter = excelWorksheet.Cells[3, 1].Value;
            DateTime value = excelWorksheet.Cells[3, 2].Value;
            if (!parameter.Equals("Updated Data:") || value == null)
            {
                Console.WriteLine("Fail to update because fail to read parameter 'Updated Data' on file(" + fileName + ")!");
                FunctionHelper.ReleaseAndCloseExcel(excelWorksheet, excelWorkbook, excelAplication);
                return true;
            }
            return value.Equals(this.updateDate);
        }

        private Boolean readFile(string fileName)
        {
            clean();
            if (!FunctionHelper.checkIfFileExist(fileName))
            {
                Console.WriteLine("Fail to find the file("+fileName+")!");
                return false;
            }
            var excelAplication = new Microsoft.Office.Interop.Excel.Application();
            excelAplication.Visible = false;

            //Opens the excel file
            var excelWorkbook = excelAplication.Workbooks.Open(@"" + FunctionHelper.directory() + "" + fileName);
            var excelWorksheet = (Microsoft.Office.Interop.Excel.Worksheet)excelWorkbook.ActiveSheet;

            string stringParkingID = excelWorksheet.Cells[1, 1].Value;
            string valueParkingID = excelWorksheet.Cells[1, 2].Value;
            string stringNumberOfSpots = excelWorksheet.Cells[2, 1].Value;
            double valueNumberOfSpots = excelWorksheet.Cells[2, 2].Value;
            string stringUpdateDate = excelWorksheet.Cells[3, 1].Value;
            DateTime valueUpdateDate = excelWorksheet.Cells[3, 2].Value;
            string stringSpotID = excelWorksheet.Cells[5, 1].Value;
            string valueSpotID = excelWorksheet.Cells[5, 2].Value;
            if (stringParkingID == null || !stringParkingID.Equals("Parking Id:") || stringNumberOfSpots == null || !stringNumberOfSpots.Equals("Number of spots:") || stringUpdateDate == null || !stringUpdateDate.Equals("Updated Data:") || stringSpotID == null || !stringSpotID.Equals("Spot Id:") || valueSpotID == null || !valueSpotID.Equals("Location (Geo):"))
            {
                Console.WriteLine("Fail to read parameters on file(" + fileName + ")!");
                FunctionHelper.ReleaseAndCloseExcel(excelWorksheet, excelWorkbook, excelAplication);
                clean();
                return true;
            }
            if (valueParkingID == null || valueParkingID == String.Empty || valueNumberOfSpots == 0|| valueUpdateDate == null || valueUpdateDate == null)
            {
                Console.WriteLine("Fail to read values on file(" + fileName + ")!");
                FunctionHelper.ReleaseAndCloseExcel(excelWorksheet, excelWorkbook, excelAplication);
                clean();
                return true;
            }

            this.parkingID = valueParkingID;
            this.numberOfSpots = valueNumberOfSpots;
            this.updateDate = valueUpdateDate;

            for (int y = 0; y < this.numberOfSpots; y++)
            {
                string parameter = excelWorksheet.Cells[y+6, 1].Value;
                string[] stringLocation = excelWorksheet.Cells[y+6, 2].Value.Split(',');

                if (parameter == null || parameter.Equals(String.Empty) || stringLocation == null || stringLocation.Length != 2) {
                    Console.WriteLine("Fail to read spots on the file(" + fileName + ")!");
                    FunctionHelper.ReleaseAndCloseExcel(excelWorksheet, excelWorkbook, excelAplication);
                    clean();
                    return false;
                }

                this.locations.Add(new Location(parameter, stringLocation[0], stringLocation[1]));
            }

            FunctionHelper.ReleaseAndCloseExcel(excelWorksheet, excelWorkbook, excelAplication);
            
            return true;
        }

        public Location giveLocation(string name)
        {
            if(locations.Count == 0)
            {
                Console.WriteLine("You don t have any location is "+ locations.Count + "!");
                return null;
            }
            foreach(Location location in locations)
            {
                if(location.name.Equals(name))
                {
                    return location;
                }
            }
            return null;
        }

        public Location giveLocation(int index)
        {
            if (locations.Count == 0)
            {
                Console.WriteLine("You don t have any location is " + locations.Count + "!");
                return null;
            }
            return locations[index];
        }

        private void clean()
        {
            parkingID = "";
            numberOfSpots = 0;
            updateDate = new DateTime();
            locations = new List<Location>();
        }
    }
}

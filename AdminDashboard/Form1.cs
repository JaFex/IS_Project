﻿using AdminDashboard.classes;
using SmartPark.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
namespace AdminDashboard
{
    public partial class Form1 : Form 
    {
        private static string format = "dd-MM-yyyy h_mm_ss tt";
        private static string url_api_parks = "http://localhost:51352/api/parks";
        private static string url_api_spots = "http://localhost:51352/api/spots";


        public Form1()
        {
            InitializeComponent();
            this.comboBoxInformation.SelectedIndex = 0;
            hideComponents(HideComponets.ALL, false);
            this.showText.ReadOnly = true;
        }


        private void buttonInformation_Click(object sender, EventArgs e)
        {
            
            /*showText.Text = dateTime.ToString();*/
            if (comboBoxInformation.SelectedItem != null)
            {
                string str_option = comboBoxInformation.SelectedItem.ToString();
                int int_option = Convert.ToInt32(str_option.Substring(0,1));
                //showText.Text = str_option.Substring(1, 1);
                if(int_option == 1)
                {
                    if (str_option.Substring(1, 1).Equals("0")) int_option = 10;
                }
                //showText.Text =int_option.ToString();

                
                string str_url_edit = "";
                
                DateTime inicialDateLocal = inicialDate.Value.Date + inicialTime.Value.TimeOfDay;
                DateTime finalDateLocal = finalDate.Value.Date + finalTime.Value.TimeOfDay;
                String inicialDateString = Convert.ToDateTime(inicialDateLocal).ToString(format, System.Globalization.CultureInfo.InvariantCulture);
                String finalDateString = Convert.ToDateTime(finalDateLocal).ToString(format, System.Globalization.CultureInfo.InvariantCulture);
                String parkIDString = parkIdTextBox.Text;
                String spotIDString = spotIdTextBox.Text;
                Park park;
                Spot spot;
                List<Park> parks = new List<Park>();
                List<Spot> spots = new List<Spot>();
                try
                {
                    switch (int_option){
                        case 1:
                            parks = getList<Park>(url_api_parks);
                            showText.Text = toStringParks(parks);
                            break;
                        case 2:
                            if (inicialDateLocal == null || parkIDString.Length == 0)
                            {
                                MessageBox.Show("Initial date and Park ID can't be empty, please fill.", "Empty fills", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                            str_url_edit = url_api_parks + "/" + parkIDString + "/date/" + inicialDateString;
                            park = getOne<Park>(str_url_edit);
                            showText.Text = toStringPark(park);
                            break;
                        case 3:
                            if (inicialDateLocal == null || finalDateLocal == null || parkIDString.Length == 0)
                            {
                                MessageBox.Show("Initial date, final date and Park ID can't be empty, please fill.", "Empty fills", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                            str_url_edit = url_api_parks + "/" + parkIDString + "/date/initial/" + inicialDateString + "/final/" + finalDateString;
                            park = getOne<Park>(str_url_edit);
                            showText.Text = toStringPark(park);
                            break;
                        case 4:
                            if (inicialDateLocal == null || parkIDString.Length == 0)
                            {
                                MessageBox.Show("Initial date and Park ID can't be empty, please fill.", "Empty fills", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                            str_url_edit = url_api_parks + "/" + parkIDString + "/date/" + inicialDateString + "/free";
                            park = getOne<Park>(str_url_edit);
                            showText.Text = toStringPark(park);
                            break;
                        case 5:
                            if (parkIDString.Length == 0)
                            {
                                MessageBox.Show("Park ID can't be empty, please fill.", "Empty fills", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                            str_url_edit = url_api_parks + "/" + parkIDString + "/spots";
                            park = getOne<Park>(str_url_edit);
                            showText.Text = toStringPark(park);
                            break;
                        case 6:
                            if (parkIDString.Length == 0)
                            {
                                MessageBox.Show("Park ID can't be empty, please fill.", "Empty fills", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                            str_url_edit = url_api_parks + "/" + parkIDString + "/details";
                            park = getOne<Park>(str_url_edit);
                            showText.Text = toStringPark(park);
                            break;
                        case 7:
                            if (inicialDateLocal == null || spotIDString.Length == 0)
                            {
                                MessageBox.Show("Initial date and Spot ID can't be empty, please fill.", "Empty fills", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                            str_url_edit = url_api_spots + "/" + spotIDString + "/details/date/" + inicialDateString;
                            spot = getOne<Spot>(str_url_edit);
                            showText.Text = toStringSpot(spot);
                            break;
                        case 8:
                            str_url_edit = url_api_spots + "/sensors/low";
                            spots = getList<Spot>(str_url_edit);
                            showText.Text = toStringSpots(spots, false);
                            break;
                        case 9:
                            if (parkIDString.Length == 0)
                            {
                                MessageBox.Show("Park ID can't be empty, please fill.", "Empty fills", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                            str_url_edit = url_api_parks + "/" + parkIDString + "/sensors/low";
                            spots = getList<Spot>(str_url_edit);
                            showText.Text = toStringSpots(spots, false);
                            break;
                        case 10:
                            if (parkIDString.Length == 0)
                            {
                                MessageBox.Show("Park ID can't be empty, please fill.", "Empty fills", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                            str_url_edit = url_api_parks + "/" + parkIDString + "/rate/occupied";
                            float rate = getOne<float>(str_url_edit);
                            showText.Text = string.Format("Park "+ parkIDString + " has [{0}%] occupancy rate.", rate) + "\n" ;
                            break;
                        default:
                            break;

                    }
                }
                 catch (Exception ex)
                {
                    showText.Text = ex.Message;
                    //MessageBox.Show("Error obtaining information, please try again.", "System Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private List<T> getList<T>(string url)
        {
            WebClient webClient;
            MemoryStream ms;
            DataContractSerializer ser;
            webClient = new WebClient();
            webClient.Headers["Content-type"] = "application/xml";
            byte[] data = webClient.DownloadData(url);
            ms = new MemoryStream(data);
            ser = new DataContractSerializer(typeof(List<T>));
            List<T> list = ser.ReadObject(ms) as List<T>;
            ms.Dispose();
            ms.Close();
            return list;
        }

        private T getOne<T>(String url)
        {
            WebClient webClient;
            MemoryStream ms;
            DataContractSerializer ser;
            webClient = new WebClient();
            webClient.Headers["Content-type"] = "application/xml";
            byte[] data = webClient.DownloadData(url);
            ms = new MemoryStream(data);
            ser = new DataContractSerializer(typeof(T));
            T obj = (T)ser.ReadObject(ms);
            ms.Dispose();
            ms.Close();
            return obj;
        }

        private string toStringParks(List<Park> parks)
        {
            string auxString = "";
            foreach (Park park in parks)
            {
                auxString += "\n------------------------------------------------------\n";
                auxString += toStringPark(park);
            }
            return auxString;
        }

        private string toStringPark(Park park)
        {
            string auxString = "";
            auxString += string.IsNullOrEmpty(park.id) ? "" : "Park Id: " + park.id;
            auxString += park.numberOfSpots == 0 ? "" : "\nNumber Of Spots: " + park.numberOfSpots;
            auxString += park.numberOfSpecialSpots == 0 ? "" : "\nNumber Of Special Spots: " + park.numberOfSpecialSpots;
            auxString += string.IsNullOrEmpty(park.operatingHours) ? "" : "\nOperating Hours: " + park.operatingHours;
            auxString += park.spots == null || park.spots.Count == 0 ? "" : "\nSpots: " + toStringSpots(park.spots, true);
            return auxString;
        }

        private string toStringSpots(List<Spot> spots, Boolean park_unique)
        {
            string last_park_id = "";
            string auxString = "";
            foreach (Spot spot in spots)
            {
                if(!park_unique && !last_park_id.Equals(spot.parkID))
                {
                    auxString += "\n------------------------------------------------------";
                    auxString += string.Format("\nPark [{0}] contains: \n", spot.parkID);
                    last_park_id = spot.parkID;
                }
                auxString += toStringSpot(spot);
            }
            return auxString;
        }

        private string toStringSpot(Spot spot)
        {
            string auxString = "";
            auxString += string.IsNullOrEmpty(spot.id) ? "" : "\n\t\tSpot Id: " + spot.id;
            auxString += string.IsNullOrEmpty(spot.latitude) ? "" : "\n\t\t\tLatitude: " + spot.latitude;
            auxString += string.IsNullOrEmpty(spot.longitude) ? "" : "\n\t\t\tLongitude: " + spot.longitude;
            auxString += string.IsNullOrEmpty(spot.batteryStatus) ? "" : "\n\t\t\tBattery Status: " + spot.batteryStatus;
            auxString += string.IsNullOrEmpty(spot.status) ? "" : "\n\t\t\tStatus: " + spot.status;
            auxString += !spot.timestamp.HasValue ? "" : "\n\t\t\tTimestamp: " + spot.timestamp.Value;
            return auxString;
        }
        

        private void comboBoxInformation_SelectedIndexChanged(object sender, EventArgs e)
        {
            string str_option = comboBoxInformation.SelectedItem.ToString();
            int int_option = Convert.ToInt32(str_option.Substring(0, 1));
            //showText.Text = str_option.Substring(1, 1);
            if (int_option == 1)
            {
                if (str_option.Substring(1, 1).Equals("0")) int_option = 10;
            }
            hideComponents(HideComponets.ALL, false);
            switch (int_option)
            {
                case 1:
                    break;
                case 2:
                    hideComponents(HideComponets.PARK, true);
                    hideComponents(HideComponets.INICIAL_DATE, true);
                    break;
                case 3:
                    hideComponents(HideComponets.PARK, true);
                    hideComponents(HideComponets.INICIAL_DATE, true);
                    hideComponents(HideComponets.FINAL_DATE, true);
                    break;
                case 4:
                    hideComponents(HideComponets.PARK, true);
                    hideComponents(HideComponets.INICIAL_DATE, true);
                    break;
                case 5:
                    hideComponents(HideComponets.PARK, true);
                    break;
                case 6:
                    hideComponents(HideComponets.PARK, true);
                    break;
                case 7:
                    hideComponents(HideComponets.SPOT, true);
                    hideComponents(HideComponets.INICIAL_DATE, true);
                    break;
                case 8:
                    break;
                case 9:
                    hideComponents(HideComponets.PARK, true);
                    break;
                case 10:
                    hideComponents(HideComponets.PARK, true);
                    break;
                default:
                    break;
            }
        }

        private void hideComponents(HideComponets component, Boolean enable)
        {
            switch(component)
            {
                case HideComponets.ALL:
                    parkLabel.Enabled = enable;
                    parkIdTextBox.Enabled = enable;
                    parkLabelFormat.Enabled = enable;
                    parkLabel.Enabled = enable;
                    parkIdTextBox.Enabled = enable;
                    parkLabelFormat.Enabled = enable;
                    spotLabel.Enabled = enable;
                    spotIdTextBox.Enabled = enable;
                    spotLabelFormat.Enabled = enable;
                    inicialDateGroupBox.Enabled = enable;
                    finalDateGroupBox.Enabled = enable;
                    break;
                case HideComponets.PARK:
                    parkLabel.Enabled = enable;
                    parkIdTextBox.Enabled = enable;
                    parkLabelFormat.Enabled = enable;
                    break;
                case HideComponets.SPOT:
                    spotLabel.Enabled = enable;
                    spotIdTextBox.Enabled = enable;
                    spotLabelFormat.Enabled = enable;
                    break;
                case HideComponets.INICIAL_DATE:
                    inicialDateGroupBox.Enabled = enable;
                    break;
                case HideComponets.FINAL_DATE:
                    finalDateGroupBox.Enabled = enable;
                    break;
            }
        }
    }
}
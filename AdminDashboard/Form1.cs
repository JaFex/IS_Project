using AdminDashboard.classes;
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
        private static string str_url = "http://localhost:51352/api/parks";
       

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
                            parks = getList<Park>(str_url);
                            showText.Text = toStringParks(parks);
                            break;
                        case 2:
                            if (inicialDateLocal == null || parkIDString.Length == 0)
                            {
                                MessageBox.Show("Initial date and Park ID can't be empty, please fill.", "Empty fills", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                            str_url_edit = str_url + "/" + parkIDString + "/date/" + inicialDateString;
                            park = getOne<Park>(str_url_edit);
                            showText.Text = toStringParkOfSpots(park);
                            break;
                        case 3:
                            if (inicialDateLocal == null || finalDateLocal == null || parkIDString.Length == 0)
                            {
                                MessageBox.Show("Initial date, final date and Park ID can't be empty, please fill.", "Empty fills", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                            str_url_edit = str_url + "/" + parkIDString + "/date1/" + inicialDateString + "/date2/" + finalDateString;
                            park = getOne<Park>(str_url_edit);
                            showText.Text = toStringParkOfSpots(park);
                            break;
                        case 4:
                            if (inicialDateLocal == null || parkIDString.Length == 0)
                            {
                                MessageBox.Show("Initial date and Park ID can't be empty, please fill.", "Empty fills", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                            str_url_edit = str_url + "/" + parkIDString + "/date/" + inicialDateString + "/free";
                            park = getOne<Park>(str_url_edit);
                            showText.Text = toStringParkOfSpots(park);
                            break;
                        case 5:
                            if (parkIDString.Length == 0)
                            {
                                MessageBox.Show("Park ID can't be empty, please fill.", "Empty fills", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                            str_url_edit = str_url + "/" + parkIDString + "/spots";
                            park = getOne<Park>(str_url_edit);
                            showText.Text = toStringParkOfSpots(park);
                            break;
                        case 6:
                            if (parkIDString.Length == 0)
                            {
                                MessageBox.Show("Park ID can't be empty, please fill.", "Empty fills", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                            str_url_edit = str_url + "/" + parkIDString + "/details";
                            park = getOne<Park>(str_url_edit);
                            showText.Text = toStringParkDetails(park);
                            break;
                        case 7:
                            if (inicialDateLocal == null || spotIDString.Length == 0)
                            {
                                MessageBox.Show("Initial date and Spot ID can't be empty, please fill.", "Empty fills", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                            str_url_edit = str_url + "/" + spotIDString + "/details/"+ inicialDateString + "/date";
                            spot = getOne<Spot>(str_url_edit);
                            showText.Text = toStringSpotDetails(spot);
                            break;
                        case 8:
                            str_url = str_url + "/sensors";
                            spots = getList<Spot>(str_url_edit);
                            showText.Text = toStringSpots(spots);
                            break;
                        case 9:
                            if (parkIDString.Length == 0)
                            {
                                MessageBox.Show("Park ID can't be empty, please fill.", "Empty fills", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                            str_url = str_url + "/" + parkIDString + "/sensors";
                            spots = getList<Spot>(str_url_edit);
                            showText.Text = toStringSpots(spots);
                            break;
                        case 10:
                            if (parkIDString.Length == 0)
                            {
                                MessageBox.Show("Park ID can't be empty, please fill.", "Empty fills", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                            str_url = str_url + "/" + parkIDString + "/rate";
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
                    MessageBox.Show("Error obtaining information, please try again.", "System Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private string toStringSpotDetails(Spot spot)
        {
            return string.Format("Spot Details: \n \tId: [{0}] \n \tLatitude: [{1}] \n " +
                                "\tLongitude: [{2}] \n \tStatus: [{3}] \n \tBattery Status: [{4}] \n \tTimestamp: [{5}] \n \tPark Id: [{6}]\n"
                                , spot.id, spot.latitude, spot.longitude, spot.status, spot.battery_status, spot.timestamp, spot.park_id);
        }

        private string toStringParkDetails(Park park)
        {
            return string.Format("Park Details: \n \tId: [{0}] \n \tNumber of Spots: [{1}] \n " +
                                "\tNumber of Special Spots: [{2}] \n \tDescription: [{3}] \n \tOperating Hours: [{4}]\n"
                                , park.id, park.number_spot, park.number_special_spot, park.description, park.operating_hours);
        }

        private string toStringSpots(List<Spot> spots)
        {
            string last_park_id = "";
            string auxString = "";
            foreach (Spot spot in spots)
            {
                if(!last_park_id.Equals(spots[0].park_id))
                {
                    auxString += string.Format("Park [{0}] contains: \n", spots[0].park_id);
                }
                auxString += string.Format("\tSpot Id: {0} has  {1} Battery.\n", spot.id, spot.battery_status);
            }
            return auxString;
        }

        private string toStringParkOfSpots(Park park)
        {
            string auxString = string.Format("Park [{0}] contains: \n", park.id);
            foreach (Spot spot in park.spots)
            {
                auxString += string.Format("\tSpot Id: {0} has  {1} Battery.\n", spot.id, spot.battery_status);
            }
            return auxString;
        }

        private string toStringParks(List<Park> parks)
        {
            string auxString = "";
            foreach (Park park in parks)
            {
                auxString += string.Format("Park [{0}] is available.\n", park.id);
            }
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
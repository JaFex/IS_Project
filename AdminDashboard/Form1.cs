using SmartPark.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
namespace AdminDashboard
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

       /* private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string str_url ="http://localhost:51352/api/parks";
            WebClient webClient = new WebClient();
            webClient.Headers["Content-type"] = "application/xml";

            byte[] data = webClient.DownloadData(str_url);
            MemoryStream ms = new MemoryStream(data);

            DataContractSerializer ser = new DataContractSerializer(typeof(List<Park>));
            List<Park> returnProds = ser.ReadObject(ms) as List<Park>;
            string output = "";
            foreach (Park park in returnProds)
            {
               output =output+ string.Format("Park Id: {0} is available", park.Id)+ "\n";
            }
            showText.Text = output;
        }*/


        private void buttonInformation_Click(object sender, EventArgs e)
        {
            if(comboBoxInformation.SelectedItem != null)
            {
                string str_option = comboBoxInformation.SelectedItem.ToString();
                int int_option = Convert.ToInt32(str_option.Substring(0,1));
                //showText.Text = str_option.Substring(1, 1);
                if(int_option == 1)
                {
                    if (str_option.Substring(1, 1).Equals("0")) int_option = 10;
                }
                //showText.Text =int_option.ToString();

                string str_url = "http://localhost:51352/api/parks";
                string str_output = "";
                WebClient webClient = new WebClient();
                webClient.Headers["Content-type"] = "application/xml";
                MemoryStream ms ;
                DataContractSerializer ser;
                switch (int_option){
                    case 1:
                        try {
                            byte[] data1 = webClient.DownloadData(str_url);
                            ms = new MemoryStream(data1);
                            ser = new DataContractSerializer(typeof(List<Park>));
                            List<Park> returnParks = ser.ReadObject(ms) as List<Park>;
                            foreach (Park park1 in returnParks)
                            {
                                str_output = str_output + string.Format("Park [{0}] is available.", park1.Id) + "\n";
                            }
                            showText.Text = str_output;
                        }catch (Exception ex){
                            showText.Text = ex.ToString();
                        }
                        break;
                    case 2://given moment pode precisar de mudar***************************************************************
                        try
                        {
                            str_url = str_url + "/" + textBoxParkId.Text + "/date/" + textBoxDate1.Text;
                            byte[] data2 = webClient.DownloadData(str_url);
                            ms = new MemoryStream(data2);
                            Park_ex2 park2;
                            ser = new DataContractSerializer(typeof(Park_ex2));
                            park2 = ser.ReadObject(ms) as Park_ex2;
                            foreach (Spot_ex2 spot2 in park2.spots)
                            {
                                str_output = str_output + string.Format("\tSpot Id: {0} is {1}.", spot2.Id, spot2.status) + "\n";
                            }
                            showText.Text = string.Format("Park [{0}] contains spots:", park2.Id) + "\n" + str_output;
                        }catch (Exception ex)
                        {
                            showText.Text = ex.ToString();
                        }
                        break;
                    case 3:
                        try { 
                            str_url = str_url + "/" + textBoxParkId.Text + "/date1/" + textBoxDate1.Text+ "/date2/" + textBoxDate2.Text;
                            /*showText.Text = str_url;
                            break;*/
                            byte[] data3 = webClient.DownloadData(str_url);
                            ms = new MemoryStream(data3);
                            Park_ex2 park3;
                            ser = new DataContractSerializer(typeof(Park_ex2));
                            park3 = ser.ReadObject(ms) as Park_ex2;
                            foreach (Spot_ex2 spot2 in park3.spots)
                            {
                                str_output = str_output + string.Format("\tSpot Id: {0} is {1} at the time [{2}]", spot2.Id,spot2.status,spot2.date) + "\n";
                            }
                            showText.Text = string.Format("Park [{0}] contains spots:", park3.Id) + "\n" + str_output;
                        }catch (Exception ex)
                        {
                            showText.Text = ex.ToString();
                        }
                        break;
                    case 4:
                        try { 
                            str_url = str_url + "/" + textBoxParkId.Text + "/date/" + textBoxDate1.Text +"/free";
                            byte[] data3 = webClient.DownloadData(str_url);
                            ms = new MemoryStream(data3);
                            Park_ex2 park3;
                            ser = new DataContractSerializer(typeof(Park_ex2));
                            park3 = ser.ReadObject(ms) as Park_ex2;
                            foreach (Spot_ex2 spot2 in park3.spots)
                            {
                                str_output = str_output + string.Format("Spot Id: {0} is {1} at the time [{2}]", spot2.Id, spot2.status, spot2.date) + "\n";
                            }
                            showText.Text = string.Format("Park [{0}] contains spots:", park3.Id) + "\n" + str_output;
                        }
                        catch (Exception ex)
                        {
                            showText.Text = ex.ToString();
                        }
                        break;
                    case 5:
                        try
                        {
                            str_url = str_url + "/" + textBoxParkId.Text + "/spots";
                            byte[] data3 = webClient.DownloadData(str_url);
                            ms = new MemoryStream(data3);
                            Park_ex5 park3;
                            ser = new DataContractSerializer(typeof(Park_ex5));
                            park3 = ser.ReadObject(ms) as Park_ex5;
                            foreach (Spot_ex5 spot2 in park3.spots)
                            {
                                str_output = str_output + string.Format("Spot Id: {0}.", spot2.Id) + "\n";
                            }
                            showText.Text = string.Format("Park [{0}] contains spots:", park3.Id) + "\n" + str_output;
                        }
                        catch (Exception ex)
                        {
                            showText.Text = ex.ToString();
                        }
                        break;
                    case 6:
                        try
                        {
                            str_url = str_url + "/" + textBoxParkId.Text + "/details";
                            byte[] data3 = webClient.DownloadData(str_url);
                            ms = new MemoryStream(data3);
                            Park park6;
                            ser = new DataContractSerializer(typeof(Park));
                            park6 = ser.ReadObject(ms) as Park;
                            
                            showText.Text = string.Format("Park Details: \n \tId: [{0}] \n \tNumber of Spots: [{1}] \n "+
                                "\tNumber of Special Spots: [{2}] \n \tDescription: [{3}] \n \tOperating Hours: [{4}]"+
                                "" , park6.Id, park6.number_spot, park6.number_special_spot, park6.description, park6.operating_hours) + "\n";
                        }
                        catch (Exception ex)
                        {
                            showText.Text = ex.ToString();
                        }
                        break;
                    case 7:
                        try
                        {
                            str_url = str_url + "/" + textBoxSpotId.Text + "/details/"+textBoxDate1.Text + "/date";
                            byte[] data3 = webClient.DownloadData(str_url);
                            ms = new MemoryStream(data3);
                            Spot_ex7 spot;
                            ser = new DataContractSerializer(typeof(Spot_ex7));
                            spot = ser.ReadObject(ms) as Spot_ex7;

                            showText.Text = string.Format("Spot Details: \n \tId: [{0}] \n \tLatitude: [{1}] \n " +
                                "\tLongitude: [{2}] \n \tStatus: [{3}] \n \tBattery Status: [{4}] \n \tTimestamp: [{5}] \n \tPark Id: [{6}]" +
                                "", spot.Id, spot.latitude, spot.longitude, spot.status, spot.battery_status, spot.timestamp, spot.park_id) + "\n";
                        }
                        catch (Exception ex)
                        {
                            showText.Text = ex.ToString();
                        }
                        break;
                    case 8:
                        try
                        {
                            str_url = str_url + "/sensors";
                            byte[] data3 = webClient.DownloadData(str_url);
                            ms = new MemoryStream(data3);
                            List<Spot_ex8> spots;
                            ser = new DataContractSerializer(typeof(List<Spot_ex8>));
                            spots = ser.ReadObject(ms) as List<Spot_ex8>;
                            foreach (Spot_ex8 spot in spots)
                            {
                                str_output = str_output + string.Format("Spot Id: {0}\n\tBattery Status: {1}\n\tPark Id: {2}.", spot.Id, spot.battery_status, spot.park_id) + "\n###########\n";
                            }
                            showText.Text = str_output;
                        }
                        catch (Exception ex)
                        {
                            showText.Text = ex.ToString();
                        }
                        break;
                    case 9:
                        try
                        {
                            str_url = str_url + "/" + textBoxParkId.Text +"/sensors";
                            byte[] data2 = webClient.DownloadData(str_url);
                            ms = new MemoryStream(data2);
                            List<Spot_ex8> spots;
                            ser = new DataContractSerializer(typeof(List<Spot_ex8>));
                            spots = ser.ReadObject(ms) as List<Spot_ex8>;
                            string aux=null;
                            foreach (Spot_ex8 spot in spots)
                            {
                                if(aux==null)aux = spot.park_id;
                                str_output = str_output + string.Format("\tSpot Id: {0} has  {1} Battery.", spot.Id, spot.battery_status) + "\n";
                            }
                            showText.Text = string.Format("Park [{0}] contains:", aux) + "\n" + str_output;
                        }
                        catch (Exception ex)
                        {
                            showText.Text = ex.ToString();
                        }
                        break;
                    case 10:
                        try
                        {
                            str_url = str_url + "/" + textBoxParkId.Text + "/rate";
                            byte[] data2 = webClient.DownloadData(str_url);
                            ms = new MemoryStream(data2);
                            float rate;
                            ser = new DataContractSerializer(typeof(float));
                            rate = (float)ser.ReadObject(ms);
                           
                            showText.Text = string.Format("Park "+ textBoxParkId.Text + " has [{0}%] occupancy rate.", rate) + "\n" ;
                        }
                        catch (Exception ex)
                        {
                            showText.Text = ex.ToString();
                        }
                        break;
                    default:
                        break;

                }
            }
        }

        private void showText_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBoxInformation_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
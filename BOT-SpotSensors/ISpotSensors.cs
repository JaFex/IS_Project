using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Xml;

namespace BOT_SpotSensors
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface SpotSensors
    {

        [OperationContract]
        string GetSensorDataString(string spotId);

        [OperationContract]
        ParkinSpot GetSensorDataUsingDataContract(string spotId);

        // TODO: Add your service operations here
    }


    // Use a data contract as illustrated in the sample below to add composite types to service operations.
    [DataContract]
    public class ParkinSpot
    {
        string id;
        string name;
        string statusValue;
        DateTime timestamp;
        int batteryStatus;


        [DataMember]
        public string Id
        {
            get { return id; }
            set { id = value; }
        }

        [DataMember]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        [DataMember]
        public string Value
        {
            get { return statusValue; }
            set { statusValue = value; }
        }

        [DataMember]
        public DateTime Timestamp
        {
            get { return timestamp; }
            set { timestamp = value; }
        } 

        [DataMember]
        public int BatteryStatus
        {
            get { return batteryStatus; }
            set { batteryStatus = value; }
        } 
    }
}

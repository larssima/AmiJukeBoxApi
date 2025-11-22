using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace AmiJukeBoxApi.MqttFolder
{
    public class Mqtt
    {
        private string _raspberryip = "192.168.68.134"; //ConfigurationManager.AppSettings["RaspberryPiAddress"];

        public void SendCancelToSubscriber()
        {
            var mqttClient = new MqttClient(IPAddress.Parse(_raspberryip));
            string clientId = Guid.NewGuid().ToString();
            mqttClient.Connect(clientId);
            mqttClient.Publish("amiJukebox", Encoding.UTF8.GetBytes("cancel record"),
                MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE,false);
        }

        public void PlaySelectionOnJukebox(string jbletter, string jbnumber)
        {
            var mqttClient = new MqttClient(IPAddress.Parse(_raspberryip));
            string clientId = Guid.NewGuid().ToString();
            mqttClient.Connect(clientId);
            mqttClient.Publish("amiJukebox", Encoding.UTF8.GetBytes(jbletter+jbnumber),
                MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
            //System.Threading.Thread.Sleep(3000);
        }

    }
}
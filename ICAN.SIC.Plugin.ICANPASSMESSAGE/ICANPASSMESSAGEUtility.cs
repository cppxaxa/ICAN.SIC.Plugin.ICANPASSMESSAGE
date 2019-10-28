using ICAN.SIC.Plugin.ICANPASSMESSAGE.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICAN.SIC.Plugin.ICANPASSMESSAGE
{
    class ICANPASSMESSAGEUtility
    {
        public Task<bool> PostMessageAsync(string ip, int port, string message)
        {
            HttpClient client = new HttpClient(ip, port);
            return new Task<bool>(() =>
            {
                try
                {
                    client.MakePostCall(message);
                    return true;
                }
                catch
                {
                    return false;
                }
            });
        }

        public string SanitizeIp(string ip)
        {
            string[] ipUnits = ip.Split('.');
            for (int i = 0; i < ipUnits.Length; i++)
                ipUnits[i] = int.Parse(ipUnits[i]).ToString();

            ip = String.Join(".", ipUnits);
            return ip;
        }

        public void SaveState(string configFilename, Dictionary<string, StateValue> subscribers)
        {
            string content = JsonConvert.SerializeObject(subscribers);
            File.WriteAllText(configFilename, content);
        }
    }
}

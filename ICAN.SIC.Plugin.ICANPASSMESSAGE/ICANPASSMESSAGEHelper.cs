using ICAN.SIC.Abstractions;
using ICAN.SIC.Abstractions.ConcreteClasses;
using ICAN.SIC.Abstractions.IMessageVariants;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICAN.SIC.Plugin.ICANPASSMESSAGE
{
    class StateValue
    {
        public int FailureCount = 0;
        public string ip;
        public int port;

        public StateValue(string ip, int port)
        {
            this.ip = ip;
            this.port = port;
        }

        public string GenerateId()
        {
            return ip + ":" + port;
        }
    }

    class ICANPASSMESSAGEHelper
    {
        private string configFilename;
        private Dictionary<string, StateValue> subscribers;
        private ICANPASSMESSAGEUtility utility;
        private IHub hub;

        public ICANPASSMESSAGEHelper(ICANPASSMESSAGEUtility utility, IHub hub, string configFilename = "ICANPASSMESSAGE_Subscribers.json")
        {
            this.utility = utility;
            this.configFilename = configFilename;
            this.hub = hub;

            if (File.Exists(this.configFilename))
            {
                subscribers = JsonConvert.DeserializeObject<Dictionary<string, StateValue>>(File.ReadAllText(this.configFilename));
                var list = subscribers.Keys.ToList();
                for (int i = 0; i < list.Count; i++)
                {
                    // Reset count
                    subscribers[list[i]].FailureCount = 0;
                }
            }
            else
            {
                subscribers = new Dictionary<string, StateValue>();
            }
        }

        public void PublishMessageToSubscribers(MessageType machineMessage, string content)
        {
            List<Task<bool>> taskList = new List<Task<bool>>();

            foreach (var subscriber in subscribers)
            {
                string formattedContent = machineMessage.Value + " " + content;
                var task = utility.PostMessageAsync(subscriber.Value.ip, subscriber.Value.port, formattedContent);
                task.Start();

                taskList.Add(task);
            }

            int i;

            //File.WriteAllText("hello.txt", "Hi");

            List<bool> taskSuccess = new List<bool>();
            for (i = 0; i < subscribers.Count; i++)
            {
                taskList[i].Wait();

                if (taskList[i].Result)
                    taskSuccess.Add(true);
                else
                    taskSuccess.Add(false);
            }

            List<string> purgeKeys = new List<string>();
            i = 0;
            foreach (var subscriber in subscribers)
            {
                if (!taskSuccess[i++])
                {
                    subscriber.Value.FailureCount++;
                    if (subscriber.Value.FailureCount > 3)
                    {
                        purgeKeys.Add(subscriber.Key);
                    }
                }
                else
                {
                    subscriber.Value.FailureCount = 0;
                }
            }

            foreach (var key in purgeKeys)
            {
                subscribers.Remove(key);

                Console.WriteLine("[ICANPASSMESSAGE] Deleted Subscriber " + key);
                IMachineMessage message = new MachineMessage("[ICANPASSMESSAGE] Deleted Subscriber " + key);
                hub.Publish(message);
            }

            if (purgeKeys.Count > 0)
                utility.SaveState(configFilename, subscribers);
        }

        public void AddSubscriber(string ip, string port)
        {
            var stateVal = new StateValue(ip, int.Parse(port));
            subscribers[stateVal.GenerateId()] = new StateValue(ip, int.Parse(port));

            utility.SaveState(configFilename, subscribers);

            Console.WriteLine("[ICANPASSMESSAGE] Added Subscriber " + stateVal.GenerateId());
            IMachineMessage message = new MachineMessage("[ICANPASSMESSAGE] Added Subscriber " + stateVal.GenerateId());
            hub.Publish(message);
        }
    }

    class MessageType
    {
        public static MessageType MachineMessage { get => new MessageType("[MachineMessage]"); }
        public static MessageType UserResponse { get => new MessageType("[UserResponse]"); }
        public static MessageType BotResult { get => new MessageType("[BotResult]"); }
        public static MessageType UserFriendlyMachineMessage { get => new MessageType("[UserFriendlyMachineMessage]"); }

        public string Value { get; }
        public MessageType(string value)
        {
            Value = value;
        }
    }
}

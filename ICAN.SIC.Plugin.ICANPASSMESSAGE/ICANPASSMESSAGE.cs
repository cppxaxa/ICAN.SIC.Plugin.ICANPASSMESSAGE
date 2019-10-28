using ICAN.SIC.Abstractions;
using ICAN.SIC.Abstractions.ConcreteClasses;
using ICAN.SIC.Abstractions.IMessageVariants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICAN.SIC.Plugin.ICANPASSMESSAGE
{
    public class ICANPASSMESSAGE : AbstractPlugin
    {
        ICANPASSMESSAGEUtility utility;
        ICANPASSMESSAGEHelper helper;

        public ICANPASSMESSAGE() : base("ICANPASSMESSAGEv1")
        {
            utility = new ICANPASSMESSAGEUtility();
            helper = new ICANPASSMESSAGEHelper(utility, Hub);

            Hub.Subscribe<IUserResponse>(this.ProcessUserResponse);
            Hub.Subscribe<IBotResult>(this.ProcessBotResult);
            Hub.Subscribe<IMachineMessage>(this.ProcessMachineMessage);
            Hub.Subscribe<IUserFriendlyMachineMessage>(this.ProcessUserFriendlyMachineMessage);

            PrintHelp();
        }

        private void PrintHelp()
        {
            Console.WriteLine("[ICANPASSMESSAGE] Post a Machine message to add client as follows: AddMachineMessageSubscriber,<ip>,<port>");
            Console.WriteLine("[ICANPASSMESSAGE] <" + MessageType.BotResult + "> <" + MessageType.MachineMessage + ">");
            Console.WriteLine("[ICANPASSMESSAGE] <" + MessageType.UserFriendlyMachineMessage + "> <" + MessageType.UserResponse + ">");
        }

        private void ProcessUserFriendlyMachineMessage(IUserFriendlyMachineMessage msg)
        {
            helper.PublishMessageToSubscribers(MessageType.UserFriendlyMachineMessage, msg.PrettyMessage);
        }

        private void ProcessMachineMessage(IMachineMessage msg)
        {
            string content = msg.Message;

            // Sample format "AddMachineMessageSubscriber,127.0.0.1,9000"
            if (content.StartsWith("AddMachineMessageSubscriber"))
            {
                string[] paramList = content.Split(',');
                string ip = paramList[1];
                string port = paramList[2];

                helper.AddSubscriber(ip, port);
            }
            else // Forward the message
            {
                helper.PublishMessageToSubscribers(MessageType.MachineMessage, content);
            }
        }

        private void ProcessUserResponse(IUserResponse msg)
        {
            helper.PublishMessageToSubscribers(MessageType.UserResponse, msg.Text);
        }

        private void ProcessBotResult(IBotResult botResult)
        {
            helper.PublishMessageToSubscribers(MessageType.BotResult, botResult.Text);
        }

        public override void Dispose()
        {
            utility = null;
            helper = null;
        }
    }
}

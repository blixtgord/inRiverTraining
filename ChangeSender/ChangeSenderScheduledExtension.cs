using inRiver.Remoting.Extension.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using inRiver.Remoting.Extension;
using IntegrationLib;
using inRiver.Remoting.Objects;
using inRiver.Remoting.Log;

namespace ChangeSender
{
    public class ChangeSenderScheduledExtension : IScheduledExtension
    {
        public inRiverContext Context { get ; set; }

        private string connectorStateIdSettingKey = "ConnectorStateId";
        public Dictionary<string, string> DefaultSettings => new Dictionary<string, string> { { connectorStateIdSettingKey, "AcademyConnectorStates" } };

        public void Execute(bool force)
        {
            if (force)
            {
                List<CustomerYXZConnectorMessage> messages = ConnectorStateHelper.Instance.PeakAllMessages(GetConnectorStateId(), Context);
                if (messages == null || messages.Count == 0)
                {
                    return;
                }

                Context.Log(LogLevel.Information, $"Found {messages.Count} changes, creating file");

                string dataToSend = GetDataToSend(messages);

                Entity resource = SaveDataToResource(dataToSend);
            }
        }

        private string GetDataToSend(List<CustomerYXZConnectorMessage> messages)
        {
            string body = "Updates since last file:";
            foreach (CustomerYXZConnectorMessage message in messages)
            {
                if (message.Action == MessageTypes.LinkCreated || message.Action == MessageTypes.LinkDeleted)
                {
                    // TODO: Add additional parameters in the CustomerYXZConnectorMessage class
                    //  body += $"{Environment.NewLine} [{message.User}] {message.Action} for Link: {message.Id} with Source: {message.SourceEntityId} and Target: {message.TargetEntityId}";
                }
                else
                {
                    body += $"{Environment.NewLine} [{message.User}] {message.Action} for entity: {message.Id}";
                }
            }
            return body;
        }

        private Entity SaveDataToResource(string body)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(body);
            string filename = $"Changes_{DateTime.Now.Day}_{DateTime.Now.Hour}_{DateTime.Now.Minute}_{DateTime.Now.Second}.txt";
            int fileID = Context.ExtensionManager.UtilityService.AddFile(filename, bytes);

            Entity resource = Entity.CreateEntity(Context.ExtensionManager.ModelService.GetEntityType("Resource"));

            resource.GetField("ResourceFilename").Data = filename;
            resource.GetField("ResourceMimeType").Data = "text/plain";
            resource.GetField("ResourceFileId").Data = fileID;

            resource.GetField("ResourceName").Data = filename;


            resource = Context.ExtensionManager.DataService.AddEntity(resource);

            Context.Log(LogLevel.Information, "File has been added");

            return resource;
        }

        public string GetConnectorStateId()
        {
            if (Context.Settings == null || !Context.Settings.ContainsKey(connectorStateIdSettingKey))
            {
                //No Settings defined, use default values
                Context.Log(LogLevel.Warning, $"No setting exists for {connectorStateIdSettingKey}, will use default value");

                return DefaultSettings[connectorStateIdSettingKey];
            }
            else
            {
                return Context.Settings[connectorStateIdSettingKey];
            }
        }

        public string Test()
        {
            return "Scheduled extension is longing to get started";
        }
    }
}

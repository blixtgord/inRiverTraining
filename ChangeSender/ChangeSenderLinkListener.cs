using inRiver.Remoting.Extension;
using inRiver.Remoting.Extension.Interface;
using IntegrationLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChangeSender
{
    public class ChangeSenderLinkListener : ILinkListener
    {
        public inRiverContext Context { get; set; }

        private string connectorStateIdSettingKey = "ConnectorStateId";

        public Dictionary<string, string> DefaultSettings => new Dictionary<string, string> { { connectorStateIdSettingKey, "AcademyConnectorStates" } };


        public void LinkActivated(int linkId, int sourceId, int targetId, string linkTypeId, int? linkEntityId)
        {
        }

        public void LinkCreated(int linkId, int sourceId, int targetId, string linkTypeId, int? linkEntityId)
        {
            ConnectorStateHelper.Instance.Save(GetConnectorStateId(), new CustomerYXZConnectorMessage
            {
                Action = MessageTypes.LinkCreated,
                Id = linkId,
                User = Context.Username,
                SourceEntityId = sourceId,
                TargetEntityId = targetId
            }, Context);

            Context.Log(inRiver.Remoting.Log.LogLevel.Information, $"Saved ConnectorState for LinkCreated for id {linkId}");
        }

        public void LinkDeleted(int linkId, int sourceId, int targetId, string linkTypeId, int? linkEntityId)
        {
            ConnectorStateHelper.Instance.Save(GetConnectorStateId(), new CustomerYXZConnectorMessage
            {
                Action = MessageTypes.LinkDeleted,
                Id = linkId,
                User = Context.Username,
                SourceEntityId = sourceId,
                TargetEntityId = targetId
            }, Context);

            Context.Log(inRiver.Remoting.Log.LogLevel.Information, $"Saved ConnectorState for LinkDeleted for id {linkId}");
        }

        public void LinkInactivated(int linkId, int sourceId, int targetId, string linkTypeId, int? linkEntityId)
        {
        }

        public void LinkUpdated(int linkId, int sourceId, int targetId, string linkTypeId, int? linkEntityId)
        {
        }

        public string Test()
        {
            return "I'm working as hell!";
        }

        public string GetConnectorStateId()
        {
            if (Context.Settings == null || !Context.Settings.ContainsKey(connectorStateIdSettingKey))
            {
                //No Settings defined, use default values
                Context.Log(inRiver.Remoting.Log.LogLevel.Warning, $"No setting exists for {connectorStateIdSettingKey}, will use default value");

                return DefaultSettings[connectorStateIdSettingKey];
            }
            else
            {
                return Context.Settings[connectorStateIdSettingKey];
            }
        }
    }
}

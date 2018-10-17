using inRiver.Remoting.Extension;
using inRiver.Remoting.Objects;
using IntegrationLib;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegrationLib
{
    public struct MessageTypes
    {
        public static string EntityCreated = "EntityCreated";
        public static string EntityUpdated = "EntityUpdated";
        public static string LinkCreated = "LinkCreated";
        public static string LinkDeleted = "LinkDeleted";
    }

    public class ConnectorStateHelper
    {
        private static ConnectorStateHelper instance;

        private ConnectorStateHelper()
        { }
        public static ConnectorStateHelper Instance => instance ?? (instance = new ConnectorStateHelper());

        public void Save(string connectorId, CustomerYXZConnectorMessage message, inRiverContext Context)
        {
            Context.ExtensionManager.UtilityService.AddConnectorState(new ConnectorState { ConnectorId = connectorId, Data = JsonConvert.SerializeObject(message) });
        }

        public void Clear(string connectorId, inRiverContext Context)
        {
            Context.ExtensionManager.UtilityService.DeleteConnectorStates(connectorId);
        }
        public List<CustomerYXZConnectorMessage> GetAllMessages(string connectorId, inRiverContext Context)
        {
            List<ConnectorState> states = Context.ExtensionManager.UtilityService.GetAllConnectorStatesForConnector(connectorId);

            if (!states.Any())
            {
                return new List<CustomerYXZConnectorMessage>();
            }

            List<CustomerYXZConnectorMessage> messages = states.Select(s => JsonConvert.DeserializeObject<CustomerYXZConnectorMessage>(s.Data)).ToList();

            Context.ExtensionManager.UtilityService.DeleteConnectorStates(states.Select(s => s.Id).ToList());

            return messages;
        }

        public List<CustomerYXZConnectorMessage> PeakAllMessages(string connectorId, inRiverContext Context)
        {
            List<ConnectorState> states = Context.ExtensionManager.UtilityService.GetAllConnectorStatesForConnector(connectorId);

            if (!states.Any())
            {
                return new List<CustomerYXZConnectorMessage>();
            }

            List<CustomerYXZConnectorMessage> messages = states.Select(s => JsonConvert.DeserializeObject<CustomerYXZConnectorMessage>(s.Data)).ToList();

            return messages;
        }
    }
}

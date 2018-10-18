using inRiver.Remoting.Extension.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using inRiver.Remoting.Extension;
using inRiver.Remoting.Objects;
using IntegrationLib;

namespace ChangeSender
{
    // Look at https://community.inriver.com/ for more information about the extensions
    public class ChangeSenderEntityListener : IEntityListener
    {
        public inRiverContext Context { get; set; }

        private string connectorStateIdSettingKey = "ConnectorStateId";

        public Dictionary<string, string> DefaultSettings => new Dictionary<string, string> { { connectorStateIdSettingKey, "AcademyConnectorStates" } };


        public void EntityCommentAdded(int entityId, int commentId)
        {
        }

        public void EntityCreated(int entityId)
        {
            var entity = Context.ExtensionManager.DataService.GetEntity(entityId, LoadLevel.DataAndLinks);

            Context.Log(inRiver.Remoting.Log.LogLevel.Information, $"This is the type: {entity.EntityType.Id}");

            if(entity.EntityType.Id == "Task" || 
                (entity.EntityType.Id == "Rescource" && entity.GetField("Name").Data.ToString().StartsWith("Changes_"))) {
                Context.Log(inRiver.Remoting.Log.LogLevel.Information, $"This should not be saved!!! XSX");

                return;
            }
            ConnectorStateHelper.Instance.Save(GetConnectorStateId(), new CustomerYXZConnectorMessage { Action = MessageTypes.EntityCreated, Id = entityId , User = Context.Username}, Context);
            Context.Log(inRiver.Remoting.Log.LogLevel.Information, $"Saved ConnectorState for EntityCreated for id {entityId}");
        }

        public void EntityDeleted(Entity deletedEntity)
        {
        }

        public void EntityFieldSetUpdated(int entityId, string fieldSetId)
        {
        }

        public void EntityLocked(int entityId)
        {
        }

        public void EntitySpecificationFieldAdded(int entityId, string fieldName)
        {
        }

        public void EntitySpecificationFieldUpdated(int entityId, string fieldName)
        {
        }

        public void EntityUnlocked(int entityId)
        {
        }

        public void EntityUpdated(int entityId, string[] fields)
        {
            var entity = Context.ExtensionManager.DataService.GetEntity(entityId, LoadLevel.DataAndLinks);
            if (entity.EntityType.Id == "Task" ||
                (entity.EntityType.Id == "Rescource" && entity.GetField("Name").Data.ToString().StartsWith("Changes_")))
            {
                Context.Log(inRiver.Remoting.Log.LogLevel.Information, $"This should not be saved!!! XSX");

                return;
            }
            ConnectorStateHelper.Instance.Save(GetConnectorStateId(), new CustomerYXZConnectorMessage { Action = MessageTypes.EntityUpdated, Id = entityId, User = Context.Username }, Context);

            Context.Log(inRiver.Remoting.Log.LogLevel.Information, $"Saved ConnectorState for EntityUpdated for id {entityId}");
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



        public string Test()
        {
            return "ChangeSenderEntityListener reporting for duty!";
        }
    }
}

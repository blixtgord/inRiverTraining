using inRiver.Remoting.Extension.Interface;
using System.Collections.Generic;
using System.Linq;
using inRiver.Remoting.Extension;
using inRiver.Remoting.Objects;

namespace ServerExtension
{
    public class BasicWorkFlow : IServerExtension
    {
        public inRiverContext Context {
            get;
            set;
        }

        public Dictionary<string, string> DefaultSettings => new Dictionary<string, string>();

        public void OnAdd(Entity entity, CancelUpdateArgument arg)
        {
        }

        public void OnCreateVersion(Entity entity, CancelUpdateArgument arg)
        {
        }

        public void OnDelete(Entity entity, CancelUpdateArgument arg)
        {
        }

        public void OnLink(Link link, CancelUpdateArgument arg)
        {
            if(!link.LinkType.Id.Equals("ProductItem"))
            {
                Context.Log(inRiver.Remoting.Log.LogLevel.Information, $"This is a productItem");

                return;
            }

            Entity item = Context.ExtensionManager.DataService.GetEntity(link.Target.Id, LoadLevel.DataOnly);
            Field itemStatusField = item.GetField("ItemStatus");
            if(itemStatusField == null || itemStatusField.IsEmpty() || itemStatusField.Data.ToString().Equals("new"))
            {
                Context.Log(inRiver.Remoting.Log.LogLevel.Information, $"This was is new. decline!");
                arg.Cancel = true;
                arg.Message = "No Link possible for status new";
            }
            Context.Log(inRiver.Remoting.Log.LogLevel.Information, $"This was not a new product.");
        }

        public void OnLinkUpdate(Link link, CancelUpdateArgument arg)
        {
        }

        public void OnLock(Entity entity, CancelUpdateArgument arg)
        {
        }

        public void OnUnlink(Link link, CancelUpdateArgument arg)
        {
        }

        public void OnUnlock(Entity entity, CancelUpdateArgument arg)
        {
        }

        public void OnUpdate(Entity entity, CancelUpdateArgument arg)
        {

        }

        public string Test()
        {
            return "This works! version 22";
        }
    }
}

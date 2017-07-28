using Potalk.Helpers;
using Potalk.Model;

namespace Potalk
{
    public class ChatConfig
    {
        public static void RegisterChannelAccess()
        {
            var chatManager = new ChatManager();
            chatManager.ForbidPublicAccessToChannel(ConfigurationHelper.ChatChannelName);
        }
    }
}
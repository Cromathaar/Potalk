using Potalk.DTO;
using Potalk.Helpers;
using Potalk.Model;
using System;
using System.Web.Mvc;

namespace Potalk.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Main(LoginDTO loginDTO)
        {
            String chatChannel = ConfigurationHelper.ChatChannel;
            String textToSpeechChannel = ConfigurationHelper.TextToSpeechChannel;
            String authKey = loginDTO.Username + DateTime.Now.Ticks.ToString();

            var chatManager = new ChatManager();
            
            if (loginDTO.ReadAccessOnly)
            {
                chatManager.GrantUserReadAccessToChannel(authKey, chatChannel);
            }
            else
            {
                chatManager.GrantUserReadWriteAccessToChannel(authKey, chatChannel);
            }

            chatManager.GrantUserReadWriteAccessToChannel(authKey, textToSpeechChannel);

            var authDTO = new AuthDTO()
            {
                PublishKey = ConfigurationHelper.PubNubPublishKey,
                SubscribeKey = ConfigurationHelper.PubNubSubscribeKey,
                AuthKey = authKey,
                Username = loginDTO.Username,
                ChatChannel = chatChannel,
                TextToSpeechChannel = textToSpeechChannel
            };

            return View(authDTO);
        }
    }
}
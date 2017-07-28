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
            String authKey = loginDTO.Username + DateTime.Now.Ticks.ToString();

            var chatManager = new ChatManager();
            
            if (loginDTO.ReadAccessOnly)
            {
                chatManager.GrantUserReadAccessToChannel(authKey, ConfigurationHelper.ChatChannelName);
            }
            else
            {
                chatManager.GrantUserReadWriteAccessToChannel(authKey, ConfigurationHelper.ChatChannelName);
            }

            var authDTO = new AuthDTO()
            {
                PublishKey = ConfigurationHelper.PubNubPublishKey,
                SubscribeKey = ConfigurationHelper.PubNubSubscribeKey,
                AuthKey = authKey,
                Username = loginDTO.Username
            };

            return View(authDTO);
        }
    }
}
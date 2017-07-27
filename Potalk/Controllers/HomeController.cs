using Potalk.Helpers;
using Potalk.Models;
using PubnubApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
        public ActionResult Main(LoginModel loginModel)
        {
            PNConfiguration pnConfiguration = new PNConfiguration();
            pnConfiguration.PublishKey = ConfigurationHelper.PubNubPublishKey;
            pnConfiguration.SubscribeKey = ConfigurationHelper.PubNubSubscribeKey;
            pnConfiguration.SecretKey = ConfigurationHelper.PubNubSecretKey;
            pnConfiguration.Secure = true;

            Pubnub pubnub = new Pubnub(pnConfiguration);

            pubnub.Grant()
                .Channels(new String[] { "chat" })
                .Read(false)
                .Write(false)
                .Async(new DemoGrantResult());

            String authKey = loginModel.Username + DateTime.Now.Ticks.ToString();

            pubnub.Grant()
                .Channels(new String[] { "chat" })
                .AuthKeys(new String[] { authKey})
                .Read(true)
                .Write(!loginModel.ReadOnly)
                .Async(new DemoGrantResult());

            var authModel = new AuthModel()
            {
                PublishKey = ConfigurationHelper.PubNubPublishKey,
                SubscribeKey = ConfigurationHelper.PubNubSubscribeKey,
                AuthKey = authKey,
                Username = loginModel.Username
            };

            return View(authModel);
        }

        public class DemoGrantResult : PNCallback<PNAccessManagerGrantResult>
        {
            public override void OnResponse(PNAccessManagerGrantResult result, PNStatus status)
            {
                ;
                // PNAccessManagerGrantResult is a parsed and abstracted response from server
            }
        };
    }
}
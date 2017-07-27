using Potalk.Helpers;
using PubnubApi;
using System;

namespace Potalk
{
    public class PubNubConfig
    {
        public static void RegisterChannelAccess()
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
        }

        public class DemoGrantResult : PNCallback<PNAccessManagerGrantResult>
        {
            public override void OnResponse(PNAccessManagerGrantResult result, PNStatus status)
            {
                // PNAccessManagerGrantResult is a parsed and abstracted response from server
            }
        };
    }
}
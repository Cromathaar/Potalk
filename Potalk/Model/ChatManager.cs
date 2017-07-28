using Potalk.Helpers;
using PubnubApi;
using System;

namespace Potalk.Model
{
    public class ChatManager
    {
        private const String PRESENCE_CHANNEL_SUFFIX = "-pnpres";

        private Pubnub pubnub;

        public ChatManager()
        {
            var pnConfiguration = new PNConfiguration();
            pnConfiguration.PublishKey = ConfigurationHelper.PubNubPublishKey;
            pnConfiguration.SubscribeKey = ConfigurationHelper.PubNubSubscribeKey;
            pnConfiguration.SecretKey = ConfigurationHelper.PubNubSecretKey;
            pnConfiguration.Secure = true;

            pubnub = new Pubnub(pnConfiguration);
        }

        public void ForbidPublicAccessToChannel(String channel)
        {
            pubnub.Grant()
                .Channels(new String[] { channel })
                .Read(false)
                .Write(false)
                .Async(new AccessGrantResult());
        }

        public void GrantUserReadAccessToChannel(String userAuthKey, String channel)
        {
            pubnub.Grant()
                .Channels(new String[] { channel, channel + PRESENCE_CHANNEL_SUFFIX })
                .AuthKeys(new String[] { userAuthKey })
                .Read(true)
                .Write(false)
                .Async(new AccessGrantResult());
        }

        public void GrantUserReadWriteAccessToChannel(String userAuthKey, String channel)
        {
            pubnub.Grant()
                .Channels(new String[] { channel, channel + PRESENCE_CHANNEL_SUFFIX })
                .AuthKeys(new String[] { userAuthKey })
                .Read(true)
                .Write(true)
                .Async(new AccessGrantResult());
        }

        private class AccessGrantResult : PNCallback<PNAccessManagerGrantResult>
        {
            public override void OnResponse(PNAccessManagerGrantResult result, PNStatus status)
            {
                // PNAccessManagerGrantResult is a parsed and abstracted response from server
            }
        }
    }
}
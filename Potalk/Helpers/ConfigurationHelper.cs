using System;

namespace Potalk.Helpers
{
    public static class ConfigurationHelper
    {
        public static String PubNubPublishKey {
            get {
                return Environment.GetEnvironmentVariable("PubNubPublishKey");
            }
        }

        public static String PubNubSubscribeKey {
            get {
                return Environment.GetEnvironmentVariable("PubNubSubscribeKey");
            }
        }

        public static String PubNubSecretKey {
            get {
                return Environment.GetEnvironmentVariable("PubNubSecretKey");
            }
        }
    }
}
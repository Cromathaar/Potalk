using System;

namespace Potalk.DTO
{
    public class AuthDTO
    {
        public String PublishKey { get; set; }
        public String SubscribeKey { get; set; }
        public String AuthKey { get; set; }
        public String Username { get; set; }
        public String ChatChannel { get; set; }
        public String TextToSpeechChannel { get; set; }
    }
}
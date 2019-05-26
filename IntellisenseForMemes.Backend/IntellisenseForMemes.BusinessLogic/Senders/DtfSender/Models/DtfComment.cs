using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace IntellisenseForMemes.BusinessLogic.Senders.DtfSender.Models
{
    public class DtfComment
    {
        public int Id { get; set; }

        public string Url { get; set; }

        public string Text { get; set; }

        public DtfUser Creator { get; set; } 

        [JsonProperty("replay_to")]
        public int ReplyTo { get; set; }
    }
}

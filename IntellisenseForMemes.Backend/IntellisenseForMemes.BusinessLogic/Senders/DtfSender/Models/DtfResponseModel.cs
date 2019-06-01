using System;
using System.Collections.Generic;
using System.Text;

namespace IntellisenseForMemes.BusinessLogic.Senders.DtfSender.Models
{
    public class DtfResponseModel<T>
    {
        public string Message { get; set; }

        public List<T> Result { get; set; }
    }
}

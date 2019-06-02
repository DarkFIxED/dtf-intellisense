namespace IntellisenseForMemes.BusinessLogic.Senders.DtfSender.Models
{
    public class DtfRequestModel<T>
    {
        public string Type { get; set; }

        public T Data { get; set; }
    }
}

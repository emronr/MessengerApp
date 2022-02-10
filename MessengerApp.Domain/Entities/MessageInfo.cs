using MessengerApp.Domain.Common;

namespace MessengerApp.Domain.Entities;

public class MessageInfo : BaseEntity
{
    public string Message { get; set; }
    public DateTime DeliveryDate{ get; set; }
    public string SenderPhoneNumber { get; set; }
    public string RecipientPhoneNumber { get; set; }
}
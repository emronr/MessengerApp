using MessengerApp.Domain.Common;

namespace MessengerApp.Domain.Entities;

public class Customer : BaseEntity
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string PhoneNumber { get; set; }
}
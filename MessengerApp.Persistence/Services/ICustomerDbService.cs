using MessengerApp.Application.Interfaces.Services;
using MessengerApp.Domain.Entities;
using MessengerApp.Persistence.Context;

namespace MessengerApp.Persistence.Services;

public class ICustomerDbService : DbService<Customer>, ICustomerServices
{
    public ICustomerDbService(ApplicationDbContext context) : base(context)
    {
    }
}
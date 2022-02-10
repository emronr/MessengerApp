using MessengerApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MessengerApp.Application.Interfaces.Context;

public interface IApplicationDbContext
{
    DbSet<MessageInfo> MessageInfos { get;}
    DbSet<Customer> Customers { get; set; }
}
using System.Net;
using MessengerApp.Application.Constants;
using MessengerApp.Application.Dtos;
using MessengerApp.Application.Exception;
using MessengerApp.Application.Interfaces.Services;
using MessengerApp.Application.RequestResponseModels;
using MessengerApp.Application.RequestResponseModels.RequestModels;
using MessengerApp.Application.RequestResponseModels.ResponseModels;
using MessengerApp.Domain.Entities;
using MessengerApp.Persistence.Context;

namespace MessengerApp.Persistence.Services;

public class MessageInfoDbService : DbService<MessageInfo>, IMessageInfoServices
{
    public MessageInfoDbService(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<BaseResponse<IPaginate<MessageInfo>>> GetMessagesAsync(
        QueryParameter<GetMessageInfoDto> queryParameter)
    {
        var datas = _DbSet.AsQueryable()
            .Where(x => x.SenderPhoneNumber == queryParameter.ModelDto.SenderPhoneNumber &&
                        x.RecipientPhoneNumber == queryParameter.ModelDto.RecipientPhoneNumber);
        if(datas == null)
            throw new UserFriendlyException(DbServiceErrorMessages.NotFoundDataByPhoneNumber, HttpStatusCode.NoContent);

        var paginatedDatas = new Paginate<MessageInfo>(datas, queryParameter);
        
        return new BaseResponse<IPaginate<MessageInfo>>(paginatedDatas);
    }
}
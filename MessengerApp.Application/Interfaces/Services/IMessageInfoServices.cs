using MessengerApp.Application.Dtos;
using MessengerApp.Application.RequestResponseModels;
using MessengerApp.Application.RequestResponseModels.RequestModels;
using MessengerApp.Application.RequestResponseModels.ResponseModels;
using MessengerApp.Domain.Entities;

namespace MessengerApp.Application.Interfaces.Services;

public interface IMessageInfoServices : IDbServices<MessageInfo>
{
    Task<BaseResponse<IPaginate<MessageInfo>>>
        GetMessagesAsync(QueryParameter<GetMessageInfoDto> queryParameter);
}
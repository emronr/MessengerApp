using MessengerApp.Application.RequestResponseModels;
using MessengerApp.Application.RequestResponseModels.RequestModels;
using MessengerApp.Application.RequestResponseModels.ResponseModels;

namespace MessengerApp.Application.Interfaces.Services;

public interface IDbServices<T> 
    where T : class
{
    
    Task<BaseResponse<T>> AddAsync(T entity);
    
    Task Delete(T entity);
    
    Task<BaseResponse<T>> UpdateAsync(T entity);

    Task<BaseResponse<T>> GetById(int id);

    Task<BaseResponse<IPaginate<T>>> GetAllAsync(QueryParameter queryParameter);

}
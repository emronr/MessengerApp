using System.Net;
using MessengerApp.Application.Constants;
using MessengerApp.Application.Errors;
using MessengerApp.Application.Exception;
using MessengerApp.Application.Interfaces.Services;
using MessengerApp.Application.RequestResponseModels;
using MessengerApp.Application.RequestResponseModels.RequestModels;
using MessengerApp.Application.RequestResponseModels.ResponseModels;
using MessengerApp.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace MessengerApp.Persistence.Services;

public class DbService<TEntity> : IDbServices<TEntity>
    where TEntity : class
{
    private readonly ApplicationDbContext _context;

    public virtual DbSet<TEntity> _DbSet => _context.Set<TEntity>();

    public DbService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<BaseResponse<TEntity>> AddAsync(TEntity entity)
    {
        await _context.AddAsync(entity);
        await _context.SaveChangesAsync();

        return new BaseResponse<TEntity>(entity);
    }

    public async Task Delete(TEntity entity)
    {
        _context.Entry(entity).State = EntityState.Deleted;
        _context.Remove(entity);

        await Task.CompletedTask;
    }

    public async Task<BaseResponse<TEntity>> UpdateAsync(TEntity entity)
    {
        _context.Entry(entity).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        
        return new BaseResponse<TEntity>(entity);
    }

    public async Task<BaseResponse<TEntity>> GetById(int id)
    {
        var data = await _DbSet.FindAsync(id);
        if (data == null)
            throw new UserFriendlyException(DbServiceErrorMessages.NotFoundDataById, HttpStatusCode.NoContent);
                
        return new BaseResponse<TEntity>(data);
    }

    public async Task<BaseResponse<IPaginate<TEntity>>> GetAllAsync(QueryParameter queryParameter)
    {
        var datas = _DbSet.AsQueryable();
        if(datas == null)
            throw new UserFriendlyException(DbServiceErrorMessages.NotFoundData, HttpStatusCode.NoContent);

        var paginatedDatas = new Paginate<TEntity>(datas, queryParameter);
        
        return new BaseResponse<IPaginate<TEntity>>(paginatedDatas);
    }
}
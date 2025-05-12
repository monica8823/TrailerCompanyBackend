using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrailerCompanyBackend.Models;
using TrailerCompanyBackend.Enums;


namespace TrailerCompanyBackend.Services
{
    public class TrailerService
    {
        private readonly TrailerCompanyDbContext _context;
        private readonly ILogger<TrailerService> _logger;

        public TrailerService(TrailerCompanyDbContext context, ILogger<TrailerService> logger)
        {
            _context = context;//是 DbContext 的instance，用于与数据库交互
            _logger = logger;
        }

        // 获取所有拖车
        public async Task<IEnumerable<Trailer>> GetAllTrailersAsync()//IEnumerable 是一个接口，表示一个可以被枚举（遍历）的集合
        {
            return await _context.Trailers//执行了一个数据库查询。Trailers 是 DbSet<Trailer>，表示数据库中 Trailers 表
                .Include(t => t.TrailerModel) // 加载主实体的数据（这里是 Trailers），不会自动加载与之关联的导航属性（如 TrailerModel）。如果不加 Include查询结果中的每个 Trailer 对象的 TrailerModel 属性会是 null，可能会导致多个小查询
                .ToListAsync();//将查询结果转化为 List 并异步加载所有数据，调用 .ToListAsync() 会触发查询立即执行，将查询结果加载到内存中，并以 List<T> 的形式返回。
        }



        // 根据 TrailerId 获取特定拖车
        public async Task<Trailer?> GetTrailerByIdAsync(int trailerId)
        {
            return await _context.Trailers
                .Include(t => t.TrailerModel)
                .FirstOrDefaultAsync(t => t.TrailerId == trailerId);//足条件的第一条数据（如果有）或返回 null
        }

        // 创建拖车
        public async Task<Trailer> CreateTrailerAsync(Trailer trailer)
        {
            if (string.IsNullOrWhiteSpace(trailer.CurrentStatus))
            {
                throw new ArgumentException("CurrentStatus is required.");
            }

            var trailerModelExists = await _context.TrailerModels.AnyAsync(tm => tm.TrailerModelId == trailer.TrailerModelId);
            if (!trailerModelExists)
            {
                throw new ArgumentException("Invalid TrailerModelId. TrailerModel does not exist.");
            }

            _context.Trailers.Add(trailer);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Trailer created successfully with ID: {TrailerId}", trailer.TrailerId);
            return trailer;
        }

        // 更新拖车
        public async Task<bool> UpdateTrailerAsync(int trailerId, Trailer updatedTrailer)
        {
            var existingTrailer = await _context.Trailers.FindAsync(trailerId);
            if (existingTrailer == null)
            {
                _logger.LogWarning("Trailer with ID {TrailerId} not found.", trailerId);
                return false;
            }

            existingTrailer.Vin = updatedTrailer.Vin;
            existingTrailer.CurrentStatus = updatedTrailer.CurrentStatus;
            existingTrailer.CustomFields = updatedTrailer.CustomFields;
            existingTrailer.TrailerModelId = updatedTrailer.TrailerModelId;

            await _context.SaveChangesAsync();
            _logger.LogInformation("Trailer with ID {TrailerId} updated successfully.", trailerId);
            return true;
        }

        // 删除拖车
        public async Task<bool> DeleteTrailerAsync(int trailerId)
        {
            var trailer = await _context.Trailers.FindAsync(trailerId);
            if (trailer == null)
            {
                _logger.LogWarning("Trailer with ID {TrailerId} not found.", trailerId);
                return false;
            }

            _context.Trailers.Remove(trailer);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Trailer with ID {TrailerId} deleted successfully.", trailerId);
            return true;
        }

        // 获取特定 TrailerModel 下的所有拖车
        public async Task<IEnumerable<Trailer>> GetTrailersByModelAsync(int trailerModelId)
        {
            return await _context.Trailers
                .Where(t => t.TrailerModelId == trailerModelId)
                .Include(t => t.TrailerModel)
                .ToListAsync();
        }

                    public async Task<bool> UpdateTrailerStatusAsync(int trailerId, string newStatus)
            {
                var trailer = await _context.Trailers.FindAsync(trailerId);
                if (trailer == null)
                {
                    _logger.LogWarning("Trailer with ID {TrailerId} not found.", trailerId);
                    return false;
                }

                // 校验状态是否有效
                if (!Enum.TryParse<TrailerStatus>(newStatus, out var parsedStatus))
                {
                    _logger.LogWarning("Invalid status value: {NewStatus}", newStatus);
                    return false;
                }

                trailer.CurrentStatus = parsedStatus.ToString();

                await _context.SaveChangesAsync();
                _logger.LogInformation("Trailer status updated successfully. ID: {TrailerId}, New Status: {NewStatus}", trailerId, newStatus);
                return true;
            }

             // 批量添加 VIN
        public async Task<bool> BatchAddVinsAsync(Dictionary<int, string> trailerIdVinMap)
        {
            foreach (var kvp in trailerIdVinMap)
            {
                var trailer = await _context.Trailers.FindAsync(kvp.Key);
                if (trailer == null)
                {
                    _logger.LogWarning("Trailer with ID {TrailerId} not found. Skipping.", kvp.Key);
                    continue;
                }

                trailer.Vin = kvp.Value;
            }

            await _context.SaveChangesAsync();
            _logger.LogInformation("Bulk VIN addition completed successfully.");
            return true;
        }

        // 批量删除 VIN
        public async Task<bool> BatchDeleteVinsAsync(IEnumerable<int> trailerIds)
        {
            foreach (var trailerId in trailerIds)
            {
                var trailer = await _context.Trailers.FindAsync(trailerId);
                if (trailer == null)
                {
                    _logger.LogWarning("Trailer with ID {TrailerId} not found. Skipping.", trailerId);
                    continue;
                }

                trailer.Vin = null; // 删除 VIN 的逻辑
            }

            await _context.SaveChangesAsync();
            _logger.LogInformation("Bulk VIN deletion completed successfully.");
            return true;
        }

        // 批量编辑 VIN
        public async Task<bool> BatchEditVinsAsync(Dictionary<int, string> trailerIdVinMap)
        {
            foreach (var kvp in trailerIdVinMap)
            {
                var trailer = await _context.Trailers.FindAsync(kvp.Key);
                if (trailer == null)
                {
                    _logger.LogWarning("Trailer with ID {TrailerId} not found. Skipping.", kvp.Key);
                    continue;
                }

                trailer.Vin = kvp.Value;
            }

            await _context.SaveChangesAsync();
            _logger.LogInformation("Bulk VIN editing completed successfully.");
            return true;
        }
    

    }
}

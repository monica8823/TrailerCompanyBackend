using Microsoft.EntityFrameworkCore;
using TrailerCompanyBackend.Models;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;


namespace TrailerCompanyBackend.Services
{
    public class TrailerModelManager
    {
        private readonly TrailerCompanyDbContext _context;
        private readonly ILogger<TrailerModelManager> _logger;
 


         public TrailerModelManager(
            TrailerCompanyDbContext context,
            ILogger<TrailerModelManager> logger
          )
        {
            _context = context;
            _logger = logger;
        }

        // 获取特定 Store 下的所有 TrailerModel
        public async Task<IEnumerable<TrailerModel>> GetAllTrailerModelsAsync(int storeId)
        {
            return await _context.TrailerModels
                .Where(tm => tm.StoreId == storeId)
                .Include(tm => tm.Trailers) // 确保加载 TrailerModel 关联的拖车
                .ToListAsync();
        }

        // 创建新的 TrailerModel，需要进行store Id检查，必须要有效id才能创建，防止错误。
              public async Task<TrailerModel> CreateTrailerModelAsync(TrailerModel trailerModel)
        {
            if (trailerModel.StoreId <= 0)
            {
                throw new ArgumentException("Invalid Store ID.");
            }

            if (string.IsNullOrWhiteSpace(trailerModel.ModelName))
            {
                throw new ArgumentException("Model name is required.");
            }

            // 添加 TrailerModel 并保存到数据库
            _context.TrailerModels.Add(trailerModel);
            await _context.SaveChangesAsync();

            _logger.LogInformation(
                "TrailerModel created successfully with ID: {TrailerModelId} for Store ID: {StoreId}",
                trailerModel.TrailerModelId,
                trailerModel.StoreId
            );

            return trailerModel;
        }


        // 获取特定 TrailerModel 通过其 ID
        public async Task<TrailerModel?> GetTrailerModelByIdAsync(int trailerModelId)
        {
            var trailerModel = await _context.TrailerModels
                .Include(tm => tm.Trailers) // 包含关联的拖车
                .FirstOrDefaultAsync(tm => tm.TrailerModelId == trailerModelId);

            if (trailerModel == null)
            {
                _logger.LogWarning("TrailerModel with ID {TrailerModelId} not found.", trailerModelId);
            }

            return trailerModel;
        }

        // 更新 TrailerModel
        public async Task<bool> UpdateTrailerModelAsync(int trailerModelId, TrailerModel updatedModel)
        {

            var storeExists = await _context.Stores.AnyAsync(s => s.StoreId == updatedModel.StoreId);
            if (!storeExists)
            {
                _logger.LogWarning("Update failed: Store ID {StoreId} does not exist.", updatedModel.StoreId);
                return false; 
            }
                    
            var existingModel = await _context.TrailerModels.FindAsync(trailerModelId);
            if (existingModel == null)
            {
                _logger.LogWarning("Failed to update: TrailerModel with ID {TrailerModelId} not found.", trailerModelId);
                return false;
            }

            existingModel.ModelName = updatedModel.ModelName;
            existingModel.StoreId = updatedModel.StoreId;

            await _context.SaveChangesAsync();
            _logger.LogInformation("TrailerModel with ID {TrailerModelId} successfully updated.", trailerModelId);
            return true;
        }

        // 删除 TrailerModel
        public async Task<bool> DeleteTrailerModelAsync(int trailerModelId)
        {
            var trailerModel = await _context.TrailerModels.FindAsync(trailerModelId);
            if (trailerModel == null)
            {
                _logger.LogWarning("Failed to delete: TrailerModel with ID {TrailerModelId} not found.", trailerModelId);
                return false;
            }

            _context.TrailerModels.Remove(trailerModel);
            await _context.SaveChangesAsync();
            _logger.LogInformation("TrailerModel with ID {TrailerModelId} successfully deleted.", trailerModelId);
            return true;
        }
    }
}

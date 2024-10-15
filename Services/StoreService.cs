using TrailerCompanyBackend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;  // 引入日志

namespace TrailerCompanyBackend.Services
{
    public class StoreService
    {
        private readonly TrailerCompanyDbContext _context;
        private readonly ILogger<StoreService> _logger;

        public StoreService(TrailerCompanyDbContext context, ILogger<StoreService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<Store>> GetAllStoresAsync()
        {
            try
            {
                var stores = await _context.Stores.ToListAsync();
                _logger.LogInformation("All stores retrieved successfully.");
                return stores;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving all stores.");
                throw;
            }
        }

        public async Task<Store?> GetStoreByIdAsync(int id)
        {
            try
            {
                var store = await _context.Stores.FindAsync(id);
                if (store == null)
                {
                    _logger.LogWarning("Attempt to retrieve non-existing store with ID: {StoreId}", id);
                    return null;
                }

                _logger.LogInformation("Store with ID: {StoreId} retrieved successfully.", id);
                return store;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving store with ID: {StoreId}", id);
                throw;
            }
        }

        public async Task<Store> CreateStoreAsync(Store store)
        {
            try
            {
                _context.Stores.Add(store);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Store created successfully with ID: {StoreId}", store.StoreId);
                return store;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating store.");
                throw;
            }
        }

        public async Task<bool> UpdateStoreAsync(Store store)
        {
            _context.Entry(store).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
                _logger.LogInformation("Store with ID: {StoreId} updated successfully.", store.StoreId);
                return true;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!StoreExists(store.StoreId))
                {
                    _logger.LogWarning("Attempt to update non-existing store with ID: {StoreId}", store.StoreId);
                    return false;
                }

                _logger.LogError(ex, "Concurrency error occurred while updating store with ID: {StoreId}", store.StoreId);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating store with ID: {StoreId}", store.StoreId);
                throw;
            }
        }

        public async Task<bool> DeleteStoreAsync(int id)
        {
            try
            {
                var store = await _context.Stores.FindAsync(id);
                if (store == null)
                {
                    _logger.LogWarning("Attempt to delete non-existing store with ID: {StoreId}", id);
                    return false;
                }

                _context.Stores.Remove(store);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Store with ID: {StoreId} deleted successfully.", id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting store with ID: {StoreId}", id);
                throw;
            }
        }

        private bool StoreExists(int id)
        {
            try
            {
                return _context.Stores.Any(e => e.StoreId == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while checking if store exists with ID: {StoreId}", id);
                throw;
            }
        }

        public async Task<bool> CopyStoreContentsAsync(int sourceStoreId, int targetStoreId)
        {
            try
            {
                var sourceStore = await _context.Stores
                    .Include(s => s.Trailers)
                    .Include(s => s.Accessories)
                    .FirstOrDefaultAsync(s => s.StoreId == sourceStoreId);

                var targetStore = await _context.Stores.FindAsync(targetStoreId);

                if (sourceStore == null || targetStore == null)
                {
                    _logger.LogWarning("Attempt to copy from source store or to target store where one or both do not exist. Source ID: {SourceStoreId}, Target ID: {TargetStoreId}", sourceStoreId, targetStoreId);
                    return false;
                }

                // Copy trailers
                foreach (var trailer in sourceStore.Trailers)
                {
                    var newTrailer = new Trailer
                    {
                        ModelName = trailer.ModelName,
                        RatedCapacity = trailer.RatedCapacity,
                        Size = trailer.Size,
                        CurrentStatus = trailer.CurrentStatus,
                        ThresholdQuantity = trailer.ThresholdQuantity,
                        StoreId = targetStoreId,  // Assign to the new store
                    };

                    _context.Trailers.Add(newTrailer);
                }

                // Copy accessories
                foreach (var accessory in sourceStore.Accessories)
                {
                    var newAccessory = new Accessory
                    {
                        AccessoryType = accessory.AccessoryType,
                        Description = accessory.Description,
                        StoreId = targetStoreId,  // Assign to the new store
                    };

                    _context.Accessories.Add(newAccessory);
                }

                await _context.SaveChangesAsync();
                _logger.LogInformation("Contents from store with ID: {SourceStoreId} copied to store with ID: {TargetStoreId} successfully.", sourceStoreId, targetStoreId);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while copying contents from store with ID: {SourceStoreId} to store with ID: {TargetStoreId}", sourceStoreId, targetStoreId);
                throw;
            }
        }
    }
}

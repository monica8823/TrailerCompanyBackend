using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TrailerCompanyBackend.Models;

namespace TrailerCompanyBackend.Services
{
    public class AccessoryService
    {
        private readonly TrailerCompanyDbContext _context;
        private readonly ILogger<AccessoryService> _logger;

        public AccessoryService(TrailerCompanyDbContext context, ILogger<AccessoryService> logger)
        {
            _context = context;
            _logger = logger;
        }

      
        public async Task<Accessory> CreateAccessoryAsync(Accessory accessory)
        {
            _context.Accessories.Add(accessory);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Accessory created successfully.");
            return accessory;
        }

   
        public async Task<List<Accessory>> GetAllAccessoriesAsync()
        {
            return await _context.Accessories
                .Include(a => a.AccessorySizes) // 加载配件的尺寸
                .Include(a => a.Store)         // 加载关联的 Store
                .ToListAsync();
        }

      
        public async Task<Accessory?> GetAccessoryByIdAsync(int accessoryId)
        {
            var accessory = await _context.Accessories
                .Include(a => a.AccessorySizes) 
                .Include(a => a.Store)         
                .FirstOrDefaultAsync(a => a.AccessoryId == accessoryId);

            if (accessory == null)
            {
                _logger.LogWarning("Accessory with ID {AccessoryId} not found.", accessoryId);
            }

            return accessory;
        }

  
        public async Task<bool> UpdateAccessoryAsync(int accessoryId, Accessory updatedAccessory)
        {
            var existingAccessory = await _context.Accessories.FindAsync(accessoryId);
            if (existingAccessory == null)
            {
                _logger.LogWarning("Accessory with ID {AccessoryId} not found.", accessoryId);
                return false;
            }

            existingAccessory.AccessoryType = updatedAccessory.AccessoryType;
            existingAccessory.Description = updatedAccessory.Description;
            existingAccessory.StoreId = updatedAccessory.StoreId;

            await _context.SaveChangesAsync();
            _logger.LogInformation("Accessory with ID {AccessoryId} updated successfully.", accessoryId);
            return true;
        }


        public async Task<bool> DeleteAccessoryAsync(int accessoryId)
        {
            var accessory = await _context.Accessories.FindAsync(accessoryId);
            if (accessory == null)
            {
                _logger.LogWarning("Accessory with ID {AccessoryId} not found.", accessoryId);
                return false;
            }

            _context.Accessories.Remove(accessory);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Accessory with ID {AccessoryId} deleted successfully.", accessoryId);
            return true;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TrailerCompanyBackend.Models;

namespace TrailerCompanyBackend.Services
{
    public class AccessorySizeService
    {
        private readonly TrailerCompanyDbContext _context;
        private readonly ILogger<AccessorySizeService> _logger;

        public AccessorySizeService(TrailerCompanyDbContext context, ILogger<AccessorySizeService> logger)
        {
            _context = context;
            _logger = logger;
        }

        // 创建配件尺寸
        public async Task<AccessorySize> CreateAccessorySizeAsync(AccessorySize accessorySize)
        {
            _context.AccessorySizes.Add(accessorySize);
            await _context.SaveChangesAsync();
            _logger.LogInformation("AccessorySize created successfully.");
            return accessorySize;
        }

        // 获取所有配件尺寸
        public async Task<List<AccessorySize>> GetAllAccessorySizesAsync()
        {
            return await _context.AccessorySizes
                .Include(asize => asize.Accessory) // 加载关联的配件
                .ToListAsync();
        }

        // 获取单个配件尺寸通过ID
        public async Task<AccessorySize?> GetAccessorySizeByIdAsync(int accessorySizeId)
        {
            var accessorySize = await _context.AccessorySizes
                .Include(asize => asize.Accessory) // 加载关联的配件
                .FirstOrDefaultAsync(asize => asize.SizeId == accessorySizeId);

            if (accessorySize == null)
            {
                _logger.LogWarning("AccessorySize with ID {AccessorySizeId} not found.", accessorySizeId);
            }

            return accessorySize;
        }

        // 更新配件尺寸
        public async Task<bool> UpdateAccessorySizeAsync(int accessorySizeId, AccessorySize updatedAccessorySize)
        {
            var existingAccessorySize = await _context.AccessorySizes.FindAsync(accessorySizeId);
            if (existingAccessorySize == null)
            {
                _logger.LogWarning("AccessorySize with ID {AccessorySizeId} not found.", accessorySizeId);
                return false;
            }

            existingAccessorySize.SizeName = updatedAccessorySize.SizeName;
            existingAccessorySize.DetailedSpecification = updatedAccessorySize.DetailedSpecification;
            existingAccessorySize.ThresholdQuantity = updatedAccessorySize.ThresholdQuantity;
            existingAccessorySize.AccessoryId = updatedAccessorySize.AccessoryId;

            await _context.SaveChangesAsync();
            _logger.LogInformation("AccessorySize with ID {AccessorySizeId} updated successfully.", accessorySizeId);
            return true;
        }

        // 删除配件尺寸
        public async Task<bool> DeleteAccessorySizeAsync(int accessorySizeId)
        {
            var accessorySize = await _context.AccessorySizes.FindAsync(accessorySizeId);
            if (accessorySize == null)
            {
                _logger.LogWarning("AccessorySize with ID {AccessorySizeId} not found.", accessorySizeId);
                return false;
            }

            _context.AccessorySizes.Remove(accessorySize);
            await _context.SaveChangesAsync();
            _logger.LogInformation("AccessorySize with ID {AccessorySizeId} deleted successfully.", accessorySizeId);
            return true;
        }

        // 获取所有配件尺寸
        public async Task<List<AccessorySize>> GetAllAccessorySizes()
        {
            return await _context.AccessorySizes.ToListAsync();
        }

        // 通过 ID 获取单个配件尺寸
        public async Task<AccessorySize?> GetAccessorySizeById(int sizeId)
        {
            return await _context.AccessorySizes
                .Include(a => a.Trailers) // 加载 Trailer 关联，若需要
                .FirstOrDefaultAsync(a => a.SizeId == sizeId);
        }
    }
}

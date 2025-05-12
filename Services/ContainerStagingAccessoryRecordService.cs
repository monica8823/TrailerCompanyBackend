using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrailerCompanyBackend.Models;

namespace TrailerCompanyBackend.Services
{
    public class ContainerStagingAccessoryRecordService
    {
        private readonly TrailerCompanyDbContext _context;
        private readonly ILogger<ContainerStagingAccessoryRecordService> _logger;

        public ContainerStagingAccessoryRecordService(TrailerCompanyDbContext context, ILogger<ContainerStagingAccessoryRecordService> logger)
        {
            _context = context;
            _logger = logger;
        }

        // 创建单个配件
        public async Task<ContainerStagingAccessoryRecord> CreateAsync(ContainerStagingAccessoryRecord accessory)
        {
            _context.ContainerStagingAccessoryRecords.Add(accessory);
            await _context.SaveChangesAsync();
            return accessory;
        }

        // 批量创建配件（支持复制粘贴）
        public async Task BulkCreateAsync(List<ContainerStagingAccessoryRecord> accessories)
        {
            _context.ContainerStagingAccessoryRecords.AddRange(accessories);
            await _context.SaveChangesAsync();
        }

        // 删除单个
        public async Task<bool> DeleteByIdAsync(int id)
        {
            var record = await _context.ContainerStagingAccessoryRecords.FindAsync(id);
            if (record == null)
                return false;

            _context.ContainerStagingAccessoryRecords.Remove(record);
            await _context.SaveChangesAsync();
            return true;
        }

        // 删除多个
        public async Task DeleteBatchAsync(List<int> ids)
        {
            var records = await _context.ContainerStagingAccessoryRecords.Where(x => ids.Contains(x.Id)).ToListAsync();
            _context.ContainerStagingAccessoryRecords.RemoveRange(records);
            await _context.SaveChangesAsync();
        }

        // 更新（型号+数量+备注）
        public async Task<bool> UpdateAsync(ContainerStagingAccessoryRecord updated)
        {
            var record = await _context.ContainerStagingAccessoryRecords.FindAsync(updated.Id);
            if (record == null)
                return false;

            record.AccessorySizeId = updated.AccessorySizeId;
            record.Quantity = updated.Quantity;
            record.Remarks = updated.Remarks;

            await _context.SaveChangesAsync();
            return true;
        }

        // 获取单个
        public async Task<ContainerStagingAccessoryRecord?> GetByIdAsync(int id)
        {
            return await _context.ContainerStagingAccessoryRecords.FindAsync(id);
        }

        // 获取某个集装箱下的所有配件
        public async Task<List<ContainerStagingAccessoryRecord>> GetAllByContainerIdAsync(int containerEntryId)
        {
            return await _context.ContainerStagingAccessoryRecords
                .Where(x => x.ContainerEntryId == containerEntryId)
                .ToListAsync();
        }
    }
}

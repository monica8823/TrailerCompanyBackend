using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrailerCompanyBackend.Models;

namespace TrailerCompanyBackend.Services
{
    public class ContainerStagingTrailerRecordService
    {
        private readonly TrailerCompanyDbContext _context;
        private readonly ILogger<ContainerStagingTrailerRecordService> _logger;

        public ContainerStagingTrailerRecordService(TrailerCompanyDbContext context, ILogger<ContainerStagingTrailerRecordService> logger)
        {
            _context = context;
            _logger = logger;
        }

        // 获取某个集装箱的所有预入库拖车
        public async Task<IEnumerable<ContainerStagingTrailerRecord>> GetAllByContainerIdAsync(int containerEntryId)
        {
            return await _context.ContainerStagingTrailerRecords
                                 .Where(t => t.ContainerEntryId == containerEntryId)
                                 .ToListAsync();
        }

        // 获取单个预入库拖车
        public async Task<ContainerStagingTrailerRecord?> GetByIdAsync(int id)
        {
            return await _context.ContainerStagingTrailerRecords.FindAsync(id);
        }

        // 创建单个预入库拖车（返回创建结果）
        public async Task<ContainerStagingTrailerRecord> CreateAsync(ContainerStagingTrailerRecord trailer)
        {
            _context.ContainerStagingTrailerRecords.Add(trailer);
            await _context.SaveChangesAsync();
            return trailer;
        }


        // 批量创建预入库拖车（支持 Excel 粘贴多行）
        public async Task BulkCreateAsync(List<ContainerStagingTrailerRecord> trailers)
        {
            _context.ContainerStagingTrailerRecords.AddRange(trailers);
            await _context.SaveChangesAsync();
        }

        // 删除单个预入库拖车
        public async Task DeleteByIdAsync(int id)
        {
            var record = await _context.ContainerStagingTrailerRecords.FindAsync(id);
            if (record != null)
            {
                _context.ContainerStagingTrailerRecords.Remove(record);
                await _context.SaveChangesAsync();
            }
        }

        // 删除多个预入库拖车
        public async Task DeleteBatchAsync(List<int> ids)
        {
            var records = await _context.ContainerStagingTrailerRecords
                                        .Where(t => ids.Contains(t.Id))
                                        .ToListAsync();
            _context.ContainerStagingTrailerRecords.RemoveRange(records);
            await _context.SaveChangesAsync();
        }

        // 更新 VIN（支持暂时没有 VIN，后续补录 VIN）
        public async Task<bool> UpdateVinAsync(int id, string vin)
        {
            var record = await _context.ContainerStagingTrailerRecords.FindAsync(id);
            if (record == null)
                return false;

            record.Vin = vin;
            await _context.SaveChangesAsync();
            return true;
        }

    }
}

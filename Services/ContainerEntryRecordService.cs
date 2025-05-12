using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrailerCompanyBackend.Models;

namespace TrailerCompanyBackend.Services
{
    public class ContainerEntryRecordService
    {
        private readonly TrailerCompanyDbContext _context;
        private readonly ILogger<ContainerEntryRecordService> _logger;

        public ContainerEntryRecordService(TrailerCompanyDbContext context, ILogger<ContainerEntryRecordService> logger)
        {
            _context = context;
            _logger = logger;
        }

        // ✅ 创建集装箱预入库记录
        public async Task<ContainerEntryRecord> CreateContainerEntryRecordAsync(ContainerEntryRecord containerEntry)
        {
            _context.ContainerEntryRecords.Add(containerEntry);
            await _context.SaveChangesAsync();
            return containerEntry;
        }

        // ✅ 修改集装箱记录（仅限“Pending”状态）
        public async Task<bool> UpdateContainerEntryRecordAsync(int containerEntryId, ContainerEntryRecord updatedContainerEntry)
        {
            var record = await _context.ContainerEntryRecords.FindAsync(containerEntryId);
            if (record == null || record.EntryStatus == "Confirmed") return false;

            record.ContainerNumber = updatedContainerEntry.ContainerNumber;
            record.EntryTime = updatedContainerEntry.EntryTime;
            record.StoreId = updatedContainerEntry.StoreId;
            record.Remarks = updatedContainerEntry.Remarks;
            await _context.SaveChangesAsync();
            return true;
        }

        // ✅ 删除未入库的集装箱
        public async Task<bool> DeleteContainerEntryRecordAsync(int containerEntryId)
        {
            var record = await _context.ContainerEntryRecords.FindAsync(containerEntryId);
            if (record == null || record.EntryStatus == "Confirmed") return false;
            
            _context.ContainerEntryRecords.Remove(record);
            await _context.SaveChangesAsync();
            return true;
        }

        // ✅ 确认入库（转换到 InventoryRecord）
        public async Task<bool> ConfirmContainerEntryAsync(int containerEntryId)
        {
            var record = await _context.ContainerEntryRecords.FindAsync(containerEntryId);
            if (record == null || record.EntryStatus == "Confirmed") return false;
            
            // 生成 InventoryRecord
            var transactions = await _context.TransactionAccessoryRecords
                .Where(t => t.TransactionType == "ContainerEntry" && t.TransactionId == containerEntryId)
                .ToListAsync();

            foreach (var transaction in transactions)
            {
                _context.InventoryRecords.Add(new InventoryRecord
                {
                    AccessorySizeId = transaction.AccessorySizeId,
                    Quantity = transaction.Quantity,
                    OperationType = "Stock In",
                    OperationTime = DateTime.UtcNow,
                    Operator = "System",
                    TargetStoreId = record.StoreId
                });
            }
            
            record.EntryStatus = "Confirmed";
            await _context.SaveChangesAsync();
            return true;
        }

        // ✅ 查询单个集装箱
        public async Task<ContainerEntryRecord?> GetContainerEntryRecordByIdAsync(int containerEntryId)
        {
            return await _context.ContainerEntryRecords.FirstOrDefaultAsync(c => c.ContainerEntryId == containerEntryId);
        }

        // ✅ 查询所有集装箱
        public async Task<IEnumerable<ContainerEntryRecord>> GetAllContainerEntryRecordsAsync()
        {
            return await _context.ContainerEntryRecords.ToListAsync();
        }

        // ✅ 查询未入库的集装箱
        public async Task<IEnumerable<ContainerEntryRecord>> GetPendingContainerEntriesAsync()
        {
            return await _context.ContainerEntryRecords.Where(c => c.EntryStatus == "Pending").ToListAsync();
        }

        // ✅ 查询已入库的集装箱
        public async Task<IEnumerable<ContainerEntryRecord>> GetConfirmedContainerEntriesAsync()
        {
            return await _context.ContainerEntryRecords.Where(c => c.EntryStatus == "Confirmed").ToListAsync();
        }

       public async Task<object?> GetStagingDetailsForContainerAsync(int containerEntryId)
        {
            var trailers = await _context.ContainerStagingTrailerRecords
                                        .Where(t => t.ContainerEntryId == containerEntryId)
                                        .ToListAsync();

            var accessories = await _context.ContainerStagingAccessoryRecords
                                            .Where(a => a.ContainerEntryId == containerEntryId)
                                            .ToListAsync();

            return new { trailers, accessories };
        }




        // ✅ 检查是否可以删除
        public async Task<bool> CheckIfContainerCanBeDeletedAsync(int containerEntryId)
        {
            var record = await _context.ContainerEntryRecords.FindAsync(containerEntryId);
            return record != null && record.EntryStatus == "Pending";
        }

        // ✅ 撤销入库（仅限管理员）
        public async Task<bool> RollbackContainerEntryAsync(int containerEntryId)
        {
            var record = await _context.ContainerEntryRecords.FindAsync(containerEntryId);
            if (record == null || record.EntryStatus != "Confirmed") return false;

            // 删除对应 InventoryRecord
            var inventoryRecords = await _context.InventoryRecords
                .Where(i => i.TargetStoreId == record.StoreId)
                .ToListAsync();
            _context.InventoryRecords.RemoveRange(inventoryRecords);

            // 恢复 ContainerEntryRecord 状态
            record.EntryStatus = "Pending";
            await _context.SaveChangesAsync();
            return true;
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TrailerCompanyBackend.Models;

namespace TrailerCompanyBackend.Services
{
    public class ArchiveService
    {
        private readonly TrailerCompanyDbContext _context;
        private readonly ILogger<ArchiveService> _logger;

        public ArchiveService(TrailerCompanyDbContext context, ILogger<ArchiveService> logger)
        {
            _context = context;
            _logger = logger;
        }

        // 以每年1月1号为归档点
        public async Task<bool> ArchiveDataAsync()
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // 确定归档的截止日期为上一年度的12月31日
                var archiveBeforeDate = new DateTime(DateTime.Now.Year - 1, 12, 31);

                // 查找需要归档的数据，并加载 TrailerModel
                var trailersToArchive = await _context.Trailers
                    .Include(t => t.TrailerModel)
                    .Include(t => t.ContainerEntryRecords)
                    .Include(t => t.SalesRecords)
                    .Include(t => t.TransferRecords)
                    .Where(t => t.ContainerEntryRecords.Any(r => r.EntryTime  <= archiveBeforeDate))
                    .ToListAsync();

                if (!trailersToArchive.Any())
                {
                    _logger.LogInformation("No trailers found for archiving before {ArchiveBeforeDate}", archiveBeforeDate);
                    return false;
                }

                // 将数据导出为 CSV 文件
                var fileName = $"Backup_{archiveBeforeDate:yyyy}_to_{DateTime.Now:yyyyMMddHHmmss}.csv";
                var success = await ExportDataToCsvAsync(trailersToArchive, fileName);

                if (!success)
                {
                    _logger.LogError("Failed to export trailer data to CSV.");
                    return false;
                }

                // 记录备份
                var backupRecord = new BackupRecord
                {
                    BackupTime = DateTime.Now,
                    FileName = fileName
                };
                _context.BackupRecords.Add(backupRecord);

                // 删除已归档的数据
                _context.Trailers.RemoveRange(trailersToArchive);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                _logger.LogInformation("Successfully archived trailer data and deleted from the database.");
                return true;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Error occurred during data archiving.");
                return false;
            }
        }

       // 导出数据到 CSV 文件的辅助方法
        private async Task<bool> ExportDataToCsvAsync(List<Trailer> trailers, string fileName)
        {
            try
            {
                var filePath = Path.Combine("Backups", fileName);

                using (var writer = new StreamWriter(filePath))
                {
                    // 写入表头
                    await writer.WriteLineAsync("TrailerId,VIN,ModelName,StoreId");

                    // 写入数据
                    foreach (var trailer in trailers)
                    {
                        // 从关联的 TrailerModel 获取 ModelName 和 StoreId
                        var modelName = trailer.TrailerModel?.ModelName ?? "Unknown Model"; // 如果 TrailerModel 为空，设置默认值
                        var storeId = trailer.TrailerModel?.StoreId ?? 0; // 如果 TrailerModel 为空，则为 0
                        
                        var line = $"{trailer.TrailerId},{trailer.Vin ?? "No VIN"},{modelName},{storeId}";
                        await writer.WriteLineAsync(line);
                    }
                }

                _logger.LogInformation("Trailer data exported to CSV: {FileName}", fileName);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while exporting data to CSV.");
                return false;
            }
        }


        // 确保数据已经备份后才允许删除
        public async Task<bool> ValidateBackupBeforeDeleteAsync()
        {
            var latestBackup = await _context.BackupRecords
                .OrderByDescending(b => b.BackupTime)
                .FirstOrDefaultAsync();

            if (latestBackup == null || (DateTime.Now - latestBackup.BackupTime).TotalDays > 365)
            {
                _logger.LogWarning("Cannot delete data. No recent backup found or backup is older than one year.");
                return false;  // 如果没有备份或者备份超过一年，则阻止删除
            }

            return true;
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization; // 添加此引用
using System.Linq;
using System.Threading.Tasks;
using TrailerCompanyBackend.Models;

namespace TrailerCompanyBackend.Services
{
    public class MonthService
    {
        private readonly TrailerCompanyDbContext _context;
        private readonly ILogger<MonthService> _logger;

        public MonthService(TrailerCompanyDbContext context, ILogger<MonthService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<Month>> GetMonthsAsync(int storeId)
        {
            return await _context.Months
                .Where(m => m.StoreId == storeId)
                .ToListAsync();
        }

        public async Task<Month> CreateMonthAsync(Month newMonth)
        {
            // 检查门店是否存在
            var storeExists = await _context.Stores.AnyAsync(s => s.StoreId == newMonth.StoreId);
            if (!storeExists)
            {
                throw new ArgumentException($"Store with ID {newMonth.StoreId} does not exist.");
            }

            // 标准化月份名称
            newMonth.MonthName = NormalizeMonthName(newMonth.MonthName);

            // 检查月份是否已经存在
            var monthExists = await _context.Months.AnyAsync(m =>
                m.StoreId == newMonth.StoreId && m.MonthName == newMonth.MonthName);
            if (monthExists)
            {
                throw new ArgumentException($"Month {newMonth.MonthName} already exists for Store ID {newMonth.StoreId}.");
            }

            // 创建月份
            _context.Months.Add(newMonth);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Month {MonthName} created successfully for Store ID {StoreId}.", newMonth.MonthName, newMonth.StoreId);

            return newMonth;
        }

        public async Task<bool> UpdateMonthAsync(int monthId, Month updatedMonth)
        {
            var existingMonth = await _context.Months.FindAsync(monthId);
            if (existingMonth == null)
            {
                _logger.LogWarning("Month with ID {MonthId} not found.", monthId);
                return false;
            }

            // 标准化月份名称
            updatedMonth.MonthName = NormalizeMonthName(updatedMonth.MonthName);

            // 检查新的月份名称是否已经存在
            var monthExists = await _context.Months.AnyAsync(m =>
                m.StoreId == updatedMonth.StoreId &&
                m.MonthName == updatedMonth.MonthName &&
                m.MonthId != monthId); // 排除当前月份
            if (monthExists)
            {
                throw new ArgumentException($"Month {updatedMonth.MonthName} already exists for Store ID {updatedMonth.StoreId}.");
            }

            // 更新月份名称
            existingMonth.MonthName = updatedMonth.MonthName;
            await _context.SaveChangesAsync();

            _logger.LogInformation("Month with ID {MonthId} updated successfully.", monthId);
            return true;
        }

        public async Task<bool> DeleteMonthAsync(int monthId)
        {
            var month = await _context.Months.FindAsync(monthId);
            if (month == null)
            {
                _logger.LogWarning("Month with ID {MonthId} not found.", monthId);
                return false;
            }

            _context.Months.Remove(month);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Month with ID {MonthId} deleted successfully.", monthId);
            return true;
        }

        public async Task<IEnumerable<InventoryRecord>> GetInventoryByMonthAsync(int storeId, string monthName)
        {
            // 标准化月份名称
            monthName = NormalizeMonthName(monthName);

            var monthStartDate = DateTime.ParseExact($"01/{monthName}", "dd/MM/yy", null);
            var monthEndDate = monthStartDate.AddMonths(1).AddDays(-1);

            return await _context.InventoryRecords
                .Where(ir =>
                    ir.TargetStoreId == storeId &&
                    ir.OperationTime >= monthStartDate &&
                    ir.OperationTime <= monthEndDate)
                .Include(ir => ir.Trailer)
                .Include(ir => ir.AccessorySize)
                .ToListAsync();
        }

        private string NormalizeMonthName(string monthName)
        {
            // 将月份名称转换为统一的格式（例如 MM/yy）
            if (DateTime.TryParseExact(monthName, "M/yy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date))
            {
                return date.ToString("MM/yy");
            }
            if (DateTime.TryParseExact(monthName, "MM/yy", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
            {
                return date.ToString("MM/yy");
            }
            throw new ArgumentException("Invalid month format. Please use MM/yy or M/yy.");
        }
    }
}
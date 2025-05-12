using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrailerCompanyBackend.Models;

namespace TrailerCompanyBackend.Services
{
    public class AccessorySizeRelationService
    {
        private readonly TrailerCompanyDbContext _context;
        private readonly ILogger<AccessorySizeRelationService> _logger;

        public AccessorySizeRelationService(TrailerCompanyDbContext context, ILogger<AccessorySizeRelationService> logger)
        {
            _context = context;
            _logger = logger;
        }

        // 创建关系（默认数量=1）
        public async Task<AccessorySizeRelation> CreateRelationAsync(int parentSizeId, int childSizeId)
        {
            var relation = new AccessorySizeRelation
            {
                ParentAccessorySizeId = parentSizeId,
                ChildAccessorySizeId = childSizeId,
                Quantity = 1  // 默认绑定1:1
            };

            _context.AccessorySizeRelations.Add(relation);
            await _context.SaveChangesAsync();

            _logger.LogInformation("AccessorySizeRelation created successfully.");
            return relation;
        }

        // 修改数量（比如1:4）
        public async Task<bool> UpdateRelationQuantityAsync(int relationId, int quantity)
        {
            var relation = await _context.AccessorySizeRelations.FindAsync(relationId);
            if (relation == null) return false;

            relation.Quantity = quantity;
            await _context.SaveChangesAsync();

            _logger.LogInformation("AccessorySizeRelation quantity updated successfully.");
            return true;
        }

        // 删除绑定
        public async Task<bool> DeleteRelationAsync(int relationId)
        {
            var relation = await _context.AccessorySizeRelations.FindAsync(relationId);
            if (relation == null) return false;

            _context.AccessorySizeRelations.Remove(relation);
            await _context.SaveChangesAsync();

            _logger.LogInformation("AccessorySizeRelation deleted successfully.");
            return true;
        }

        // 获取 某个 Parent Size 的所有 Child
        public async Task<List<AccessorySizeRelation>> GetRelationsByParentSizeIdAsync(int parentSizeId)
        {
            return await _context.AccessorySizeRelations
                                 .Where(r => r.ParentAccessorySizeId == parentSizeId)
                                 .Include(r => r.ChildAccessorySize)
                                 .ToListAsync();
        }

        // 获取 某个 Child Size 被哪些 Parent 绑定
        public async Task<List<AccessorySizeRelation>> GetRelationsByChildSizeIdAsync(int childSizeId)
        {
            return await _context.AccessorySizeRelations
                                 .Where(r => r.ChildAccessorySizeId == childSizeId)
                                 .Include(r => r.ParentAccessorySize)
                                 .ToListAsync();
        }
    }
}

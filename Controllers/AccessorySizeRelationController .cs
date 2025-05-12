using Microsoft.AspNetCore.Mvc;
using TrailerCompanyBackend.Models;
using TrailerCompanyBackend.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TrailerCompanyBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccessorySizeRelationController : ControllerBase
    {
        private readonly AccessorySizeRelationService _relationService;

        public AccessorySizeRelationController(AccessorySizeRelationService relationService)
        {
            _relationService = relationService;
        }

        // 创建关系（默认数量=1）
        [HttpPost("create")]
        public async Task<ActionResult<AccessorySizeRelation>> CreateRelation(int parentSizeId, int childSizeId)
        {
            var result = await _relationService.CreateRelationAsync(parentSizeId, childSizeId);
            return Ok(result);
        }

        // 修改关系数量
        [HttpPatch("{relationId}/quantity")]
        public async Task<IActionResult> UpdateRelationQuantity(int relationId, int quantity)
        {
            var success = await _relationService.UpdateRelationQuantityAsync(relationId, quantity);
            if (!success) return NotFound();
            return NoContent();
        }

        // 删除关系
        [HttpDelete("{relationId}")]
        public async Task<IActionResult> DeleteRelation(int relationId)
        {
            var success = await _relationService.DeleteRelationAsync(relationId);
            if (!success) return NotFound();
            return NoContent();
        }

        // 获取某个 ParentSize 的所有子尺寸
        [HttpGet("parent/{parentSizeId}")]
        public async Task<ActionResult<IEnumerable<AccessorySizeRelation>>> GetRelationsByParentSizeId(int parentSizeId)
        {
            var result = await _relationService.GetRelationsByParentSizeIdAsync(parentSizeId);
            return Ok(result);
        }

        // 获取某个 ChildSize 被哪些 Parent 绑定
        [HttpGet("child/{childSizeId}")]
        public async Task<ActionResult<IEnumerable<AccessorySizeRelation>>> GetRelationsByChildSizeId(int childSizeId)
        {
            var result = await _relationService.GetRelationsByChildSizeIdAsync(childSizeId);
            return Ok(result);
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrailerCompanyBackend.Models;
using TrailerCompanyBackend.Services;

namespace TrailerCompanyBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccessorySizeController : ControllerBase
    {
        private readonly AccessorySizeService _accessorySizeService;

        public AccessorySizeController(AccessorySizeService accessorySizeService)
        {
            _accessorySizeService = accessorySizeService;
        }

        // 获取所有配件尺寸
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AccessorySize>>> GetAllAccessorySizes()
        {
            var accessorySizes = await _accessorySizeService.GetAllAccessorySizesAsync();
            return Ok(accessorySizes);
        }

        // 根据 ID 获取单个配件尺寸
        [HttpGet("{id}")]
        public async Task<ActionResult<AccessorySize>> GetAccessorySizeById(int id)
        {
            var accessorySize = await _accessorySizeService.GetAccessorySizeByIdAsync(id);

            if (accessorySize == null)
            {
                return NotFound(new { message = $"AccessorySize with ID {id} not found." });
            }

            return Ok(accessorySize);
        }

        // 创建新的配件尺寸
        [HttpPost]
        public async Task<ActionResult<AccessorySize>> CreateAccessorySize(AccessorySize accessorySize)
        {
            var createdAccessorySize = await _accessorySizeService.CreateAccessorySizeAsync(accessorySize);
            return CreatedAtAction(nameof(GetAccessorySizeById), new { id = createdAccessorySize.SizeId }, createdAccessorySize);
        }

        // 更新配件尺寸信息
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAccessorySize(int id, AccessorySize accessorySize)
        {
            var updated = await _accessorySizeService.UpdateAccessorySizeAsync(id, accessorySize);

            if (!updated)
            {
                return NotFound(new { message = $"AccessorySize with ID {id} not found." });
            }

            return NoContent();
        }

        // 删除配件尺寸
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAccessorySize(int id)
        {
            var deleted = await _accessorySizeService.DeleteAccessorySizeAsync(id);

            if (!deleted)
            {
                return NotFound(new { message = $"AccessorySize with ID {id} not found." });
            }

            return NoContent();
        }
    }
}

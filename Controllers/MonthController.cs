using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrailerCompanyBackend.Models;
using TrailerCompanyBackend.Services;

namespace TrailerCompanyBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MonthController : ControllerBase
    {
        private readonly MonthService _monthService;

        public MonthController(MonthService monthService)
        {
            _monthService = monthService;
        }

        // 获取指定 Store 的所有月份
        [HttpGet("{storeId}")]
        public async Task<ActionResult<IEnumerable<Month>>> GetMonths(int storeId)
        {
            try
            {
                var months = await _monthService.GetMonthsAsync(storeId);
                return Ok(months);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while fetching months.", details = ex.Message });
            }
        }

        // 创建新月份
        [HttpPost]
        public async Task<ActionResult<Month>> CreateMonth([FromBody] Month newMonth)
        {
            try
            {
                var createdMonth = await _monthService.CreateMonthAsync(newMonth);
                return CreatedAtAction(nameof(GetMonths), new { storeId = newMonth.StoreId }, createdMonth);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while creating the month.", details = ex.Message });
            }
        }

        // 更新月份
        [HttpPut("{monthId}")]
        public async Task<IActionResult> UpdateMonth(int monthId, [FromBody] Month updatedMonth)
        {
            try
            {
                var success = await _monthService.UpdateMonthAsync(monthId, updatedMonth);
                if (!success)
                {
                    return NotFound(new { message = "Month not found." });
                }
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while updating the month.", details = ex.Message });
            }
        }

        // 删除月份
        [HttpDelete("{monthId}")]
        public async Task<IActionResult> DeleteMonth(int monthId)
        {
            try
            {
                var success = await _monthService.DeleteMonthAsync(monthId);
                if (!success)
                {
                    return NotFound(new { message = "Month not found." });
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while deleting the month.", details = ex.Message });
            }
        }

        // 按月份获取库存记录
        [HttpGet("{storeId}/inventory/{monthName}")]
        public async Task<ActionResult<IEnumerable<InventoryRecord>>> GetInventoryByMonth(int storeId, string monthName)
        {
            try
            {
                var inventoryRecords = await _monthService.GetInventoryByMonthAsync(storeId, monthName);
                return Ok(inventoryRecords);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while fetching inventory records.", details = ex.Message });
            }
        }
    }
}
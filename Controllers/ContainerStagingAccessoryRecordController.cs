using Microsoft.AspNetCore.Mvc;
using TrailerCompanyBackend.Models;
using TrailerCompanyBackend.Services;

namespace TrailerCompanyBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContainerStagingAccessoryRecordController : ControllerBase
    {
        private readonly ContainerStagingAccessoryRecordService _service;

        public ContainerStagingAccessoryRecordController(ContainerStagingAccessoryRecordService service)
        {
            _service = service;
        }

        // 获取某个集装箱的所有配件
        [HttpGet("container/{containerEntryId}")]
        public async Task<ActionResult<IEnumerable<ContainerStagingAccessoryRecord>>> GetAllByContainerId(int containerEntryId)
        {
            var records = await _service.GetAllByContainerIdAsync(containerEntryId);
            return Ok(records);
        }

        // 获取单个配件
        [HttpGet("{id}")]
        public async Task<ActionResult<ContainerStagingAccessoryRecord>> GetById(int id)
        {
            var record = await _service.GetByIdAsync(id);
            if (record == null)
                return NotFound();

            return Ok(record);
        }

        // 创建单个配件
        [HttpPost]
        public async Task<ActionResult<ContainerStagingAccessoryRecord>> Create(ContainerStagingAccessoryRecord accessory)
        {
            var created = await _service.CreateAsync(accessory);
            return Ok(created);
        }

        // 批量创建（Excel粘贴）
        [HttpPost("batch")]
        public async Task<IActionResult> BulkCreate(List<ContainerStagingAccessoryRecord> accessories)
        {
            await _service.BulkCreateAsync(accessories);
            return Ok("Batch Create Successfully");
        }

        // 更新（AccessorySizeId + Quantity + Remarks）
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, ContainerStagingAccessoryRecord updated)
        {
            if (id != updated.Id) return BadRequest("Id Mismatch");

            var success = await _service.UpdateAsync(updated);
            if (!success) return NotFound();

            return Ok("Updated Successfully");
        }

        // 删除单个
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _service.DeleteByIdAsync(id);
            if (!success) return NotFound();

            return Ok("Deleted Successfully");
        }

        // 删除多个
        [HttpPost("delete-batch")]
        public async Task<IActionResult> DeleteBatch(List<int> ids)
        {
            await _service.DeleteBatchAsync(ids);
            return Ok("Batch Deleted Successfully");
        }
    }
}

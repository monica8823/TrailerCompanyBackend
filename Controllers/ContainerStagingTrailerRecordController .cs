using Microsoft.AspNetCore.Mvc;
using TrailerCompanyBackend.Models;
using TrailerCompanyBackend.Services;

namespace TrailerCompanyBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContainerStagingTrailerRecordController : ControllerBase
    {
        private readonly ContainerStagingTrailerRecordService _service;

        public ContainerStagingTrailerRecordController(ContainerStagingTrailerRecordService service)
        {
            _service = service;
        }

        // 获取某个集装箱的全部预入库拖车
        [HttpGet("container/{containerEntryId}")]
        public async Task<ActionResult<IEnumerable<ContainerStagingTrailerRecord>>> GetAllByContainerId(int containerEntryId)
        {
            return Ok(await _service.GetAllByContainerIdAsync(containerEntryId));
        }

        // 获取单个
        [HttpGet("{id}")]
        public async Task<ActionResult<ContainerStagingTrailerRecord>> GetById(int id)
        {
            var record = await _service.GetByIdAsync(id);
            if (record == null) return NotFound();
            return Ok(record);
        }

        // 创建单个
        [HttpPost]
        public async Task<IActionResult> Create(ContainerStagingTrailerRecord trailer)
        {
            await _service.CreateAsync(trailer);
            return Ok("Created Successfully");
        }

        // 批量创建
        [HttpPost("bulk")]
        public async Task<IActionResult> BulkCreate(List<ContainerStagingTrailerRecord> trailers)
        {
            await _service.BulkCreateAsync(trailers);
            return Ok("Bulk Created Successfully");
        }

        // 删除单个
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteById(int id)
        {
            await _service.DeleteByIdAsync(id);
            return Ok("Deleted Successfully");
        }

        // 删除多个
        [HttpPost("delete-batch")]
        public async Task<IActionResult> DeleteBatch([FromBody] List<int> ids)
        {
            await _service.DeleteBatchAsync(ids);
            return Ok("Batch Deleted Successfully");
        }

        // 修改 VIN
        [HttpPatch("{id}/vin")]
        public async Task<IActionResult> UpdateVin(int id, [FromBody] string vin)
        {
            await _service.UpdateVinAsync(id, vin);
            return Ok("VIN Updated Successfully");
        }
    }
}

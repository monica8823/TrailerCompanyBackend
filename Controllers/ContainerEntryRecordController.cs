using Microsoft.AspNetCore.Mvc;
using TrailerCompanyBackend.Models;
using TrailerCompanyBackend.Services;

namespace TrailerCompanyBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContainerEntryRecordController : ControllerBase
    {
        private readonly ContainerEntryRecordService _service;

        public ContainerEntryRecordController(ContainerEntryRecordService service)
        {
            _service = service;
        }

        // 获取全部集装箱
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ContainerEntryRecord>>> GetAll()
        {
            return Ok(await _service.GetAllContainerEntryRecordsAsync());
        }

        // 获取单个集装箱
        [HttpGet("{id}")]
        public async Task<ActionResult<ContainerEntryRecord>> GetById(int id)
        {
            var record = await _service.GetContainerEntryRecordByIdAsync(id);
            if (record == null) return NotFound();
            return Ok(record);
        }

        // 创建集装箱
        [HttpPost]
        public async Task<IActionResult> Create(ContainerEntryRecord record)
        {
            var result = await _service.CreateContainerEntryRecordAsync(record);
            return Ok(result);
        }

        // 修改集装箱
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, ContainerEntryRecord record)
        {
            var success = await _service.UpdateContainerEntryRecordAsync(id, record);
            if (!success) return BadRequest("Record not found or already confirmed.");
            return Ok("Updated Successfully");
        }

        // 删除集装箱
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _service.DeleteContainerEntryRecordAsync(id);
            if (!success) return BadRequest("Record not found or already confirmed.");
            return Ok("Deleted Successfully");
        }

        // 获取 Pending 状态的
        [HttpGet("pending")]
        public async Task<ActionResult<IEnumerable<ContainerEntryRecord>>> GetPending()
        {
            return Ok(await _service.GetPendingContainerEntriesAsync());
        }

        // 获取 Confirmed 状态的
        [HttpGet("confirmed")]
        public async Task<ActionResult<IEnumerable<ContainerEntryRecord>>> GetConfirmed()
        {
            return Ok(await _service.GetConfirmedContainerEntriesAsync());
        }

        // 查询某个集装箱的所有预入库数据（拖车和配件）
        [HttpGet("{id}/staging-details")]
        public async Task<IActionResult> GetStagingDetails(int id)
        {
            var data = await _service.GetStagingDetailsForContainerAsync(id);
            return Ok(data);
        }

        // 确认入库
        [HttpPost("{id}/confirm")]
        public async Task<IActionResult> Confirm(int id)
        {
            var success = await _service.ConfirmContainerEntryAsync(id);
            if (!success) return BadRequest("Record not found or already confirmed.");
            return Ok("Confirmed Successfully");
        }

        // 撤销入库
        [HttpPost("{id}/rollback")]
        public async Task<IActionResult> Rollback(int id)
        {
            var success = await _service.RollbackContainerEntryAsync(id);
            if (!success) return BadRequest("Record not found or not confirmed.");
            return Ok("Rollback Successfully");
        }
    }
}

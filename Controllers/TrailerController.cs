using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrailerCompanyBackend.Models;
using TrailerCompanyBackend.Services;

namespace TrailerCompanyBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TrailerController : ControllerBase
    {
        private readonly TrailerService _trailerService;
        private readonly ILogger<TrailerController> _logger;

        public TrailerController(TrailerService trailerService, ILogger<TrailerController> logger)
        {
            _trailerService = trailerService;
            _logger = logger;
        }

        // 获取所有拖车
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Trailer>>> GetAllTrailers()
        {
            try
            {
                var trailers = await _trailerService.GetAllTrailersAsync();
                return Ok(trailers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching trailers.");
                return StatusCode(500, "Internal server error.");
            }
        }

        // 根据 ID 获取特定拖车
        [HttpGet("{trailerId}")]
        public async Task<ActionResult<Trailer>> GetTrailerById(int trailerId)
        {
            try
            {
                var trailer = await _trailerService.GetTrailerByIdAsync(trailerId);
                if (trailer == null)
                {
                    return NotFound($"Trailer with ID {trailerId} not found.");
                }
                return Ok(trailer);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching trailer by ID.");
                return StatusCode(500, "Internal server error.");
            }
        }

        // 创建拖车
        [HttpPost]
        public async Task<ActionResult<Trailer>> CreateTrailer([FromBody] Trailer trailer)
        {
            try
            {
                var createdTrailer = await _trailerService.CreateTrailerAsync(trailer);
                return CreatedAtAction(nameof(GetTrailerById), new { trailerId = createdTrailer.TrailerId }, createdTrailer);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Invalid input for creating trailer.");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating trailer.");
                return StatusCode(500, "Internal server error.");
            }
        }

        // 更新拖车
        [HttpPut("{trailerId}")]
        public async Task<IActionResult> UpdateTrailer(int trailerId, [FromBody] Trailer updatedTrailer)
        {
            try
            {
                var success = await _trailerService.UpdateTrailerAsync(trailerId, updatedTrailer);
                if (!success)
                {
                    return NotFound($"Trailer with ID {trailerId} not found.");
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating trailer.");
                return StatusCode(500, "Internal server error.");
            }
        }

        // 删除拖车
        [HttpDelete("{trailerId}")]
        public async Task<IActionResult> DeleteTrailer(int trailerId)
        {
            try
            {
                var success = await _trailerService.DeleteTrailerAsync(trailerId);
                if (!success)
                {
                    return NotFound($"Trailer with ID {trailerId} not found.");
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting trailer.");
                return StatusCode(500, "Internal server error.");
            }
        }

        // 更新拖车状态
        [HttpPut("{trailerId}/status")]
        public async Task<IActionResult> UpdateTrailerStatus(int trailerId, [FromBody] string newStatus)
        {
            try
            {
                var success = await _trailerService.UpdateTrailerStatusAsync(trailerId, newStatus);
                if (!success)
                {
                    return BadRequest($"Failed to update status for Trailer with ID {trailerId}. Invalid status or trailer not found.");
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating trailer status.");
                return StatusCode(500, "Internal server error.");
            }
        }

        // 获取特定 TrailerModel 下的所有拖车
        [HttpGet("model/{trailerModelId}")]
        public async Task<ActionResult<IEnumerable<Trailer>>> GetTrailersByModel(int trailerModelId)
        {
            try
            {
                var trailers = await _trailerService.GetTrailersByModelAsync(trailerModelId);
                return Ok(trailers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching trailers by model.");
                return StatusCode(500, "Internal server error.");
            }
        }

        // 批量添加 VIN
        [HttpPost("batch-add-vins")]
        public async Task<IActionResult> BatchAddVins([FromBody] Dictionary<int, string> vinMapping)
        {
            if (vinMapping == null || vinMapping.Count == 0)
            {
                return BadRequest("VIN mapping data cannot be null or empty.");
            }

            var result = await _trailerService.BatchAddVinsAsync(vinMapping);
            if (result)
            {
                return Ok("Batch VIN assignment completed successfully.");
            }

            return StatusCode(500, "An error occurred while processing VIN assignment.");
        }

        // 批量编辑 VIN
        [HttpPut("batch-edit-vins")]
        public async Task<IActionResult> BatchEditVins([FromBody] Dictionary<int, string> vinMapping)
        {
            if (vinMapping == null || vinMapping.Count == 0)
            {
                return BadRequest("VIN mapping data cannot be null or empty.");
            }

            var result = await _trailerService.BatchEditVinsAsync(vinMapping);
            if (result)
            {
                return Ok("Batch VIN editing completed successfully.");
            }

            return StatusCode(500, "An error occurred while processing VIN editing.");
        }

        // 批量删除 VIN
        [HttpDelete("batch-delete-vins")]
        public async Task<IActionResult> BatchDeleteVins([FromBody] List<int> trailerIds)
        {
            if (trailerIds == null || trailerIds.Count == 0)
            {
                return BadRequest("Trailer IDs cannot be null or empty.");
            }

            var result = await _trailerService.BatchDeleteVinsAsync(trailerIds);
            if (result)
            {
                return Ok("Batch VIN deletion completed successfully.");
            }

            return StatusCode(500, "An error occurred while processing VIN deletion.");
        }
    }
}

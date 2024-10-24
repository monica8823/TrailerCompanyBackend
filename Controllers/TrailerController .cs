using Microsoft.AspNetCore.Mvc;
using TrailerCompanyBackend.Models;
using TrailerCompanyBackend.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace TrailerCompanyBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrailersController : ControllerBase
    {
        private readonly TrailerService _trailerService;

        public TrailersController(TrailerService trailerService)
        {
            _trailerService = trailerService;
        }

        // GET: api/Trailers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Trailer>>> GetTrailers([FromQuery] int storeId, [FromQuery] string? modelName = null)
        {
            var trailers = await _trailerService.GetAllTrailersAsync(storeId, modelName);
            return Ok(trailers);
        }

        // GET: api/Trailers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Trailer>> GetTrailer(int id)
        {
            var trailer = await _trailerService.GetTrailerByIdAsync(id);

            if (trailer == null)
            {
                return NotFound();
            }

            return Ok(trailer);
        }

        // POST: api/Trailers
        [HttpPost]
        public async Task<IActionResult> PostTrailer([FromBody] CreateTrailerRequest request)
        {
            var result = await _trailerService.CreateTrailersAsync(request.ModelNames, request.StoreId, request.CustomFields);
            if (result)
            {
                return Ok("Trailers created successfully.");
            }

            return BadRequest("Error occurred while creating trailers.");
        }

        // PUT: api/Trailers/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTrailer(int id, [FromBody] Trailer trailer)
        {
            var result = await _trailerService.EditTrailerDetailsAsync(id, trailer.ModelName, trailer.Vin, trailer.Size, trailer.RatedCapacity, trailer.CurrentStatus, trailer.CustomFields);
            if (!result)
            {
                return NotFound($"Trailer with ID {id} not found.");
            }

            return NoContent();
        }

        // DELETE: api/Trailers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTrailer(int id)
        {
            var result = await _trailerService.DeleteTrailerAsync(id);
            if (!result)
            {
                return NotFound($"Trailer with ID {id} not found.");
            }

            return NoContent();
        }

        // POST: api/Trailers/batch-delete
        [HttpPost("batch-delete")]
        public async Task<IActionResult> BatchDeleteTrailers([FromBody] List<int> trailerIds)
        {
            var result = await _trailerService.BatchDeleteTrailersAsync(trailerIds);
            if (!result)
            {
                return BadRequest("Error occurred during batch deletion.");
            }

            return Ok("Batch deletion successful.");
        }

   
        // GET: api/Trailers/logs/5
        [HttpGet("{id}/logs")]
        public async Task<ActionResult<IEnumerable<OperationLog>>> GetTrailerLogs(int id)
        {
            var logs = await _trailerService.GetTrailerLogsAsync(id);
            return Ok(logs);
        }

        // GET: api/Trailers/search
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Trailer>>> GlobalSearch([FromQuery] string? vin, [FromQuery] string? invNumber)
        {
            var trailers = await _trailerService.GlobalSearchTrailersAsync(vin, invNumber);
            return Ok(trailers);
        }
    }

    // CreateTrailerRequest 类，用于封装传递到 PostTrailer 方法的请求体
    public class CreateTrailerRequest
    {
        [Required]
        public List<string> ModelNames { get; set; } = new List<string>();

        [Required]
        public int StoreId { get; set; }

        public string? CustomFields { get; set; }
    }
}

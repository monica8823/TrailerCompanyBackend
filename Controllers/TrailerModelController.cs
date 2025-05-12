using Microsoft.AspNetCore.Mvc;
using TrailerCompanyBackend.Models;
using TrailerCompanyBackend.Services;
using System.Threading.Tasks;
using System.Collections.Generic;



namespace TrailerCompanyBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TrailerModelController : ControllerBase
    {
        private readonly TrailerModelManager _trailerModelManager;

        public TrailerModelController(TrailerModelManager trailerModelManager)
        {
            _trailerModelManager = trailerModelManager;
        }

     
        [HttpGet("{storeId}")]
        public async Task<ActionResult<IEnumerable<TrailerModel>>> GetAllTrailerModels(int storeId)
        {
            var trailerModels = await _trailerModelManager.GetAllTrailerModelsAsync(storeId);
            if (trailerModels == null)
            {
                return NotFound();
            }
            return Ok(trailerModels);
        }

       
        [HttpGet("model/{trailerModelId}")]
        public async Task<ActionResult<TrailerModel>> GetTrailerModelById(int trailerModelId)
        {
            var trailerModel = await _trailerModelManager.GetTrailerModelByIdAsync(trailerModelId);
            if (trailerModel == null)
            {
                return NotFound();
            }
            return Ok(trailerModel);
        }

    
        [HttpPost("{storeId}")]
        public async Task<ActionResult<TrailerModel>> CreateTrailerModel(int storeId, [FromBody] TrailerModel trailerModel)
        {
            try
            {

                if (storeId <= 0)
                {
                    return BadRequest(new { error = "Invalid Store ID. Store ID must be greater than 0." });
                }

                if (trailerModel == null || string.IsNullOrWhiteSpace(trailerModel.ModelName))
                {
                    return BadRequest(new { error = "Model name is required." });
                }

                
                trailerModel.StoreId = storeId;


                var createdModel = await _trailerModelManager.CreateTrailerModelAsync(trailerModel);

   
                return CreatedAtAction(nameof(GetAllTrailerModels), new { storeId }, createdModel);
            }
            catch (Exception ex)
            {
    
                return StatusCode(500, new { error = "An unexpected error occurred.", details = ex.Message });
            }
        }



  
        [HttpPut("{trailerModelId}")]
        public async Task<IActionResult> UpdateTrailerModel(int trailerModelId, [FromBody]TrailerModel updatedModel)
        {
            var result = await _trailerModelManager.UpdateTrailerModelAsync(trailerModelId, updatedModel);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }

     
        [HttpDelete("{trailerModelId}")]
        public async Task<IActionResult> DeleteTrailerModel(int trailerModelId)
        {
            var result = await _trailerModelManager.DeleteTrailerModelAsync(trailerModelId);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}

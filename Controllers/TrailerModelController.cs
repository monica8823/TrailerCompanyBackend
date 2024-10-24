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

    
        [HttpPost]
        public async Task<ActionResult<TrailerModel>> CreateTrailerModel(TrailerModel trailerModel)
        {
            var createdTrailerModel = await _trailerModelManager.CreateTrailerModelAsync(trailerModel);
            return CreatedAtAction(nameof(GetTrailerModelById), new { trailerModelId = createdTrailerModel.TrailerModelId }, createdTrailerModel);
        }

  
        [HttpPut("{trailerModelId}")]
        public async Task<IActionResult> UpdateTrailerModel(int trailerModelId, TrailerModel updatedModel)
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

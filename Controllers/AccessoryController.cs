using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrailerCompanyBackend.Models;
using TrailerCompanyBackend.Services;

namespace TrailerCompanyBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccessoryController : ControllerBase
    {
        private readonly AccessoryService _accessoryService;

        public AccessoryController(AccessoryService accessoryService)
        {
            _accessoryService = accessoryService;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<Accessory>>> GetAllAccessories()
        {
            var accessories = await _accessoryService.GetAllAccessoriesAsync();
            return Ok(accessories);
        }

  
        [HttpGet("{id}")]
        public async Task<ActionResult<Accessory>> GetAccessoryById(int id)
        {
            var accessory = await _accessoryService.GetAccessoryByIdAsync(id);

            if (accessory == null)
            {
                return NotFound(new { message = $"Accessory with ID {id} not found." });
            }

            return Ok(accessory);
        }

    
        [HttpPost]
        public async Task<ActionResult<Accessory>> CreateAccessory(Accessory accessory)
        {
            var createdAccessory = await _accessoryService.CreateAccessoryAsync(accessory);
            return CreatedAtAction(nameof(GetAccessoryById), new { id = createdAccessory.AccessoryId }, createdAccessory);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAccessory(int id, Accessory accessory)
        {
            var updated = await _accessoryService.UpdateAccessoryAsync(id, accessory);

            if (!updated)
            {
                return NotFound(new { message = $"Accessory with ID {id} not found." });
            }

            return NoContent(); 
        }

  
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAccessory(int id)
        {
            var deleted = await _accessoryService.DeleteAccessoryAsync(id);

            if (!deleted)
            {
                return NotFound(new { message = $"Accessory with ID {id} not found." });
            }

            return NoContent(); 
        }
    }
}

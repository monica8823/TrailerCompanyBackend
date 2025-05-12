using Microsoft.AspNetCore.Mvc;
using TrailerCompanyBackend.Models;
using TrailerCompanyBackend.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TrailerCompanyBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoresController : ControllerBase
    {
        private readonly StoreService _storeService;

        public StoresController(StoreService storeService)
        {
            _storeService = storeService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Store>>> GetStores()
        {
            return await _storeService.GetAllStoresAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Store>> GetStore(int id)
        {
            var store = await _storeService.GetStoreByIdAsync(id);
            if (store == null)
            {
                return NotFound();
            }
            return store;
        }
        [HttpPost]
        public async Task<IActionResult> CreateStore([FromBody] Store store)
        {
            if (string.IsNullOrWhiteSpace(store.StoreName))
            {
                return BadRequest("Store name cannot be empty.");
            }

            try
            {
                var createdStore = await _storeService.CreateStoreAsync(store);
                return CreatedAtAction(nameof(GetStore), new { id = createdStore.StoreId }, createdStore);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }




        [HttpPatch("{id}/name")]
        public async Task<IActionResult> UpdateStoreName(int id, [FromBody] string newName)
        {
            
            if (string.IsNullOrWhiteSpace(newName))
            {
                return BadRequest("Store name cannot be empty.");
            }

            var updateResult = await _storeService.UpdateStoreNameAsync(id, newName);

            if (!updateResult)
            {
                return NotFound($"Store with ID {id} not found.");
            }

            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStore(int id)
        {
            var result = await _storeService.DeleteStoreAsync(id);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }



        [HttpPost("copy-store-contents")]
        public async Task<IActionResult> CopyStoreContents(int sourceStoreId, int targetStoreId)
        {
            var result = await _storeService.CopyStoreContentsAsync(sourceStoreId, targetStoreId);
            
            if (!result)
            {
                return NotFound("Source or target store not found.");
            }

            return Ok("Store contents copied successfully.");
        }

    }
}

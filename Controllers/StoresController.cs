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
        public async Task<ActionResult<Store>> PostStore(Store store)
        {
            await _storeService.CreateStoreAsync(store);
            return CreatedAtAction(nameof(GetStore), new { id = store.StoreId }, store);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutStore(int id, Store store)
        {
            if (id != store.StoreId)
            {
                return BadRequest();
            }

            var result = await _storeService.UpdateStoreAsync(store);
            if (!result)
            {
                return NotFound();
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

        [HttpPatch("{id}")]
        public async Task<IActionResult> EditStore(int id, [FromBody] JsonPatchDocument<Store> patchDoc)
        {
            var store = await _storeService.GetStoreByIdAsync(id);
            if (store == null)
            {
                return NotFound();
            }

            try
            {
                patchDoc.ApplyTo(store);
            }
            catch (Exception ex) 
            {
     
                return BadRequest($"Patch failed: {ex.Message}");
            }


            var updateResult = await _storeService.UpdateStoreAsync(store);
            if (!updateResult)
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

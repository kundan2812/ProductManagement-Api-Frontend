using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using ProductManagement_Api.Data;
using ProductManagement_Api.Models;

namespace ProductManagement_Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ItemsController : Controller
    {
        private readonly ItemRepository _repo;
        public ItemsController(ItemRepository repo)
        {
            _repo = repo;
        }
        [HttpGet("ListItems")]
        public async Task<IActionResult> ListItems()
        {
            var items = await _repo.GetAllItemsAsync();
            return Ok(items);
        }
        [HttpPost("AddItem")]
        public async Task<IActionResult> AddItem([FromBody] ItemModel model)
        {
            if (model == null) return BadRequest("Invalid payload");
            try
            {
                model.PunchDate = DateTime.Now;
                await _repo.AddItemAsync(model);
                return Ok(new { message = "Item added successfully" });
            }
            catch (SqlException ex)
            {
                return StatusCode(500, new { message = "Database error", detail = ex.Message });
            }
        }
        [HttpPut("EditItem")]
        public async Task<IActionResult> EditItem([FromBody] ItemModel model)
        {
            if (model == null) return BadRequest("Invalid payload");
            try
            {
                model.PunchDate = DateTime.Now;
                await _repo.EditItemAsync(model);
                return Ok(new { message = "Item updated successfully" });
            }
            catch (SqlException ex)
            {
                return StatusCode(500, new { message = "Database error", detail = ex.Message });
            }
        }
        [HttpDelete("DeleteItem/{itemCode}")]
        public async Task<IActionResult> DeleteItem(string itemCode)
        {
            if (string.IsNullOrEmpty(itemCode)) return BadRequest("ItemCode required");
            try
            {
                await _repo.DeleteItemAsync(itemCode);
                return Ok(new { message = "Item deleted successfully" });
            }
            catch (SqlException ex)
            {
                return StatusCode(500, new { message = "Database error", detail = ex.Message });
            }
        }
    }
}

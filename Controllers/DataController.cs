using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Auth.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class DataController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Upload([FromForm] IFormFile file)
        {
            if (file.ContentType.ToLower().IndexOf("image") == -1)
            {
                return BadRequest(new { Message = "The server accept only image files." });
            }
            string fName = file.FileName;
            string path = Path.Combine("wwwroot/Images/" + file.FileName);
            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return Ok(new { Name = file.FileName, Type = file.ContentType });
        }

        // public async Task<IActionResult> GetFilesList()
        // {

        // }
    }
}
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CustomLexer.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Consumes("multipart/form-data")]
    public class LexerController : ControllerBase
    {
        [HttpGet]
        public ActionResult<string> SayHi()
        {
            return "Hello world!";
        }

        [HttpPost]
        public async Task<ActionResult<string>> Analyze([FromForm] Model model)
        {
            var fileContent = await model.File.ReadAsStringAsync();

            if (model.GetResult)
            {
                
            }

            return "OK";
        }
    }

    public static class FormFileExtensions
    {
        public static async Task<string> ReadAsStringAsync(this IFormFile file)
        {
            var result = new StringBuilder();
            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                while (reader.Peek() >= 0)
                    result.AppendLine(await reader.ReadLineAsync()); 
            }
            return result.ToString();
        }
    }

    public class Model
    {
        public IFormFile File { get; set; }
        public int NumberOfWordsInGroup { get; set; }
        public bool GetResult { get; set; }
    }
}

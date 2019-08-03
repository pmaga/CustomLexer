﻿using System.IO;
using System.Text;
using System.Threading.Tasks;
using CustomLexer.Api.Configuration;
using CustomLexer.Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace CustomLexer.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Consumes("multipart/form-data")]
    public class LexerController : ControllerBase
    {
        private readonly IOptions<FileConfiguration> _config;
        private readonly ILexicalAnalysisService _lexicalAnalysisService;

        public LexerController(IOptions<FileConfiguration> config, ILexicalAnalysisService lexicalAnalysisService)
        {
            _config = config;
            _lexicalAnalysisService = lexicalAnalysisService;
        }

        [HttpGet]
        public ActionResult<string> SayHi()
        {
            return "Hello world!";
        }

        [HttpPost]
        public async Task<IActionResult> Analyze([FromForm] FormModel model)
        {
            if (model.NumberOfWordsInGroup < 1 || model.NumberOfWordsInGroup > 3)
            {
                return BadRequest("Number of words in one group must be between 1 and 3.");
            }

            if (model.File == null)
            {
                return BadRequest("Input cannot be empty.");
            }

            if (model.File.Length == 0 || model.File.Length > _config.Value.MaxSizeInBytes)
            {
                return BadRequest("File size cannot exceed 10MB");
            }

            var fileContent = await model.File.ReadAsStringAsync();

            if (model.GetResult)
            {
                var tsvFile = _lexicalAnalysisService.AnalyzeAndGetResultAsTsv(fileContent, model.NumberOfWordsInGroup);
                return Ok(tsvFile);
            }

            _lexicalAnalysisService.Analyze(fileContent, model.NumberOfWordsInGroup);
            return Ok();
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

    public class FormModel
    {
        public IFormFile File { get; set; }
        public int NumberOfWordsInGroup { get; set; }
        public bool GetResult { get; set; }
    }
}

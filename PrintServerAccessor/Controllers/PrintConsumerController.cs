using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;

namespace PrintServerAccessor.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class PrintConsumerController : ControllerBase
	{
		private readonly ILogger<PrintConsumerController> _logger;

		public PrintConsumerController(ILogger<PrintConsumerController> logger)
		{
			_logger = logger;
		}

		[HttpPost]
		public async Task<IActionResult> SendToPrint(IFormFile file, [FromQuery] string UserName, CancellationToken token = default)
		{
			// save uploaded file
			string path = System.IO.Path.GetTempFileName();
			using (var stream = System.IO.File.Create(path))
			{
				await file.CopyToAsync(stream, token);
			}

			// convert to base64
			Byte[] bytes = System.IO.File.ReadAllBytes(path);
			String fileBase64 = Convert.ToBase64String(bytes);

			// prepare payload
			var request = new printRequest();
			request.request = new requestContent()
			{
				Document = fileBase64,
				FileName = file.FileName,
				PrinterName = "SamplePrinter",
				UserName = UserName
			};

			// send to api
			HttpClient client = new HttpClient();
			var response = await client.PostAsJsonAsync("https://localhost:44302/api/v4/Print", request, token);
			response.EnsureSuccessStatusCode();

			return Ok();
		}
	}
}

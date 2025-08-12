using Microsoft.AspNetCore.Mvc;
using NZWalk.UI.Models.DTO;
using System.Text;
using System.Text.Json;

namespace NZWalk.UI.Controllers
{
    public class RegionController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public RegionController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var response = new List<RegionDTO>();

            try
            {
                var client = _httpClientFactory.CreateClient();

                var httpResponseMessage = await client.GetAsync("https://localhost:7136/api/Regions/GetAll");

                httpResponseMessage.EnsureSuccessStatusCode();

                //var msg = JsonSerializer.Deserialize<RegionDTO>();

                response.AddRange(await httpResponseMessage.Content.ReadFromJsonAsync<IEnumerable<RegionDTO>>());
            }
            catch (Exception ex)
            {

                // Log exceptions
            }
            return View(response);
        }
       
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddRegionVM regionDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest("Something went wrong while adding new region");

                var client = _httpClientFactory.CreateClient();

                var httpRequestMessage = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri("https://localhost:7136/api/Regions/Create"),
                    Content = new StringContent(JsonSerializer.Serialize(regionDTO), Encoding.UTF8, "application/json")
                };

                var httpResponseMessage = await client.SendAsync(httpRequestMessage);

                httpResponseMessage.EnsureSuccessStatusCode();

                var response = await httpResponseMessage.Content.ReadFromJsonAsync<RegionDTO>();

                if (response is null)
                    return View(Index);
            }
            catch (Exception ex)
            {

                // Log exception
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var client = _httpClientFactory.CreateClient();

            var response = await client.GetFromJsonAsync<RegionDTO>($"https://localhost:7136/api/Regions/GetById/{id.ToString()}");

            return View(response);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(RegionDTO regionDTO)
        {
            if (regionDTO is null)
                return BadRequest("Region data is null.");

            if (!ModelState.IsValid)
                return View(regionDTO);

            try
            {
                var client = _httpClientFactory.CreateClient();

                var httpRequestMessage = new HttpRequestMessage
                {
                    Method = HttpMethod.Put,
                    RequestUri = new Uri($"https://localhost:7136/api/Regions/Update/{regionDTO.Id}"),
                    Content = new StringContent(JsonSerializer.Serialize(regionDTO), Encoding.UTF8, "application/json")
                };

                var httpResponseMessage = await client.SendAsync(httpRequestMessage);

                httpResponseMessage.EnsureSuccessStatusCode();

                var response = await httpResponseMessage.Content.ReadFromJsonAsync<RegionDTO>();

                if (response is null)
                    return View(response);
            }
            catch (Exception ex)
            {

                return View(regionDTO);
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();

                var response = await client.DeleteAsync($"https://localhost:7136/api/Regions/Delete/{id.ToString()}");
               // var httpResponseMessage = await client.DeleteAsync($"https://localhost:7081/api/regions/{request.Id}");

                response.EnsureSuccessStatusCode();

            }
            catch (Exception ex)
            {
                // Log exception
                Console.WriteLine($"Exception during delete: {ex.Message}");
                return View("Error");
            }

            return RedirectToAction("Index");
        }

    }
}

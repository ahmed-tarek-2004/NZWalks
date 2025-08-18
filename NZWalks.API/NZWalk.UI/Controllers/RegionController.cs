using Microsoft.AspNetCore.Mvc;
using NZWalk.UI.Models.DTO;
using System.Net.Http.Json;
using NZWalk.UI.Models; // RegionDTO

public class RegionController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly string _baseUrl = "http://localhost:5146/api/v1/Regions";

    public RegionController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    // GET: List all regions
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var client = _httpClientFactory.CreateClient();
        var regions = await client.GetFromJsonAsync<List<RegionDTO>>($"{_baseUrl}/GetAll");
        return View(regions);
    }

    // GET: Create form
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    // POST: Create region
    [HttpPost]
    public async Task<IActionResult> Create(RegionDTO region)
    {
        if (!ModelState.IsValid)
            return View(region);

        var client = _httpClientFactory.CreateClient();

        var response = await client.PostAsJsonAsync($"{_baseUrl}/Create", region);

        if (response.IsSuccessStatusCode)
            return RedirectToAction(nameof(Index));

        ModelState.AddModelError("", "Error creating region");
        return View(region);
    }

    // GET: Edit form
    [HttpGet]
    public async Task<IActionResult> Edit(Guid id)
    {
        var client = _httpClientFactory.CreateClient();
        var region = await client.GetFromJsonAsync<RegionDTO>($"{_baseUrl}/GetById/{id}");
       
        return View(region);
    }

    // POST: Update region
    [HttpPost]
    public async Task<IActionResult> Edit(RegionDTO region)
    {
        if (!ModelState.IsValid)
            return View(region);

        var client = _httpClientFactory.CreateClient();
        var response = await client.PutAsJsonAsync($"{_baseUrl}/Update/{region.Id}", region);

        if (response.IsSuccessStatusCode)
            return RedirectToAction(nameof(Index));

        ModelState.AddModelError("", "Error updating region");
        return View(region);
    }

    [HttpGet]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            var client = _httpClientFactory.CreateClient();

            var response = await client.DeleteFromJsonAsync<RegionDTO>($"{_baseUrl}/Delete/{id}");

            if (response is null)
                return View(response);
        }
        catch (Exception)
        {

            // Log exception
        }

        return RedirectToAction("Index");
    }
}

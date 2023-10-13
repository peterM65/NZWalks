using Microsoft.AspNetCore.Mvc;
using NZWalks.UI.Models.DTO;
using NZWalks.UI.Models.VM;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;

namespace NZWalks.UI.Controllers
{
    public class RegionsController : Controller
    {
        private readonly IHttpClientFactory httpClientFactory;

        public RegionsController(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<RegionVM> response = new List<RegionVM>();

            try
            {
                // Get All Regions from Web API
                var client = httpClientFactory.CreateClient();

                var httpResponseMassage = await client.GetAsync("https://localhost:7037/api/regions");

                httpResponseMassage.EnsureSuccessStatusCode();

                response.AddRange(await httpResponseMassage.Content.ReadFromJsonAsync<IEnumerable<RegionVM>>());

            }
            catch (Exception ex)
            {
                // Log the exception

            }

            return View(response);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Add(AddRegionVM model)
        {
            var client = httpClientFactory.CreateClient();

            var httpRequestMassage = new HttpRequestMessage()
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("https://localhost:7037/api/regions"),
                Content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json")
            };

            var httpResponseMassage = await client.SendAsync(httpRequestMassage);

            httpResponseMassage.EnsureSuccessStatusCode();

            var response = await httpResponseMassage.Content.ReadFromJsonAsync<RegionVM>();
            if (response is not null)
            {
                return RedirectToAction("Index", "Regions");
            }

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var client = httpClientFactory.CreateClient();

            var response = await client.GetFromJsonAsync<RegionVM>($"https://localhost:7037/api/regions/{id.ToString()}");

            if (response is not null) 
            {
                return View(response);
            }


            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(RegionVM request)
        {
            var client = httpClientFactory.CreateClient();

            var httpRequestMassage = new HttpRequestMessage()
            {
                Method = HttpMethod.Put,
                RequestUri = new Uri($"https://localhost:7037/api/regions/{request.Id}"),
                Content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json")
            };

            var httpResponseMassage = await client.SendAsync(httpRequestMassage);
            //httpResponseMassage.EnsureSuccessStatusCode();

            var response = await httpResponseMassage.Content.ReadFromJsonAsync<RegionVM>();
            if (response is not null)
            {
                return RedirectToAction("Edit", "Regions");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Delete(RegionVM request)
        {
            try
            {
                var client = httpClientFactory.CreateClient();

                var httpResponseMassage = await client.DeleteAsync($"https://localhost:7037/api/regions/{request.Id}");
                httpResponseMassage.EnsureSuccessStatusCode();

                return RedirectToAction("Insex", "Regions");
            }
            catch (Exception)
            {

                throw;
            }

            return View("Edit");
        }

    }
}

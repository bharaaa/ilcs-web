using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

public class HomeController : Controller
{
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    private readonly HttpClient _httpClient;

    public HomeController(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    [HttpGet]
    public async Task<IActionResult> GetItemSuggestions(string term)
    {
        if (string.IsNullOrEmpty(term))
        {
            return BadRequest("Search term cannot be empty");
        }

        string apiUrl = $"https://insw-dev.ilcs.co.id/n/negara?ur_negara"; // Replace with your API URL

        try
        {
            HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);
            response.EnsureSuccessStatusCode(); // Throw exception for non-2xx status codes

            string responseString = await response.Content.ReadAsStringAsync();
            List<Item> suggestions = JsonConvert.DeserializeObject<List<Item>>(responseString);

            return Ok(suggestions); // Return list of item objects or appropriate data structure
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Error retrieving suggestions: " + ex.Message); // Handle API errors gracefully
        }
    }
}

// Defines the Users.cshtml class/logic for the Supply Chain system.
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace SupplyChain.Frontend.Pages
{
    public class UsersModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public List<UserDto> Users { get; set; } = new List<UserDto>();

        public UsersModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task OnGet()
        {
            try
            {
                HttpClient client = _httpClientFactory.CreateClient("BackendApi");
                HttpResponseMessage response = await client.GetAsync("api/Users");

                if (response.IsSuccessStatusCode)
                {
                    Users = await response.Content.ReadFromJsonAsync<List<UserDto>>() ?? new List<UserDto>();
                }
            }
            catch (Exception)
            {
                Users = new List<UserDto>();
            }
        }

        public async Task<IActionResult> OnPostDelete(int id)
        {
            try
            {
                HttpClient client = _httpClientFactory.CreateClient("BackendApi");
                HttpResponseMessage response = await client.DeleteAsync($"api/Users/{id}");

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToPage();
                }
            }
            catch (Exception)
            {

            }
            return RedirectToPage();
        }
    }

    public class UserDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int? RoleId { get; set; }
        public string? RoleName { get; set; }
    }
}

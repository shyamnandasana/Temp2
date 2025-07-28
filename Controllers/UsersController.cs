using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using BookStorewithMVC.Models;
using System.Text;

namespace BookStorewithMVC.Controllers
{
    public class UsersController : Controller
    {
        private readonly HttpClient _client;

        public UsersController(IHttpClientFactory httpClientFactory)
        {
            _client = httpClientFactory.CreateClient();
            _client.BaseAddress = new Uri("https://localhost:44318/api/");
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var response = await _client.GetAsync("UsersAPI");
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                var list = JsonConvert.DeserializeObject<List<UsersModel>>(json);
                return View(list);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Unable to load users: " + (ex.InnerException?.Message ?? ex.Message);
                return View(new List<UsersModel>());
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var response = await _client.DeleteAsync($"UsersAPI/{id}");
                response.EnsureSuccessStatusCode();
                TempData["Message"] = "User deleted successfully.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Unable to delete user: " + (ex.InnerException?.Message ?? ex.Message);
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> AddEditUser(int? id)
        {
            try
            {
                UsersModel user;

                if (id == null)
                {
                    user = new UsersModel();
                }
                else
                {
                    var response = await _client.GetAsync($"UsersAPI/{id}");
                    if (!response.IsSuccessStatusCode) return NotFound();

                    var json = await response.Content.ReadAsStringAsync();
                    user = JsonConvert.DeserializeObject<UsersModel>(json);
                }

                return View(user);
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddEditUser(UsersModel user)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(user);
                }

                var content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");

                if (user.UserId == 0)
                {
                    user.CreatedAt = DateTime.Now;
                    await _client.PostAsync("UsersAPI", content);
                }
                else
                {
                    user.ModifiedAt = DateTime.Now;
                    await _client.PutAsync($"UsersAPI/{user.UserId}", content);
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }
    }
}

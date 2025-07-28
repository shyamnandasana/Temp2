using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using BookStorewithMVC.Models;
using System.Text;

namespace BookStorewithMVC.Controllers
{
    public class AuthorController : Controller
    {
        private readonly HttpClient _client;

        public AuthorController(IHttpClientFactory httpClientFactory)
        {
            _client = httpClientFactory.CreateClient();
            _client.BaseAddress = new Uri("https://localhost:44318/api/");
        }

        #region AuthorList
        public async Task<IActionResult> Index()
        {
            try
            {
                var response = await _client.GetAsync("AuthorsAPI");
                var json = await response.Content.ReadAsStringAsync();
                var list = JsonConvert.DeserializeObject<List<AuthorModel>>(json);
                return View(list);
            }
            catch
            {
                return View(new List<AuthorModel>());
            }
        }
        #endregion

        #region AuthorDelete
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var response = await _client.DeleteAsync($"AuthorsAPI/{id}");
                if (response.IsSuccessStatusCode)
                {
                    TempData["Message"] = "Author deleted successfully.";
                }
                else
                {
                    TempData["Error"] = $"Failed to delete author with ID {id}.";
                }
            }
            catch (Exception)
            {
                TempData["Error"] = "An error occurred while deleting the author.";
            }

            return RedirectToAction("Index");
        }
        #endregion

        #region InsertAuthor
        public async Task<IActionResult> AddEdit(int? id)
        {
            try
            {
                var userResponse = await _client.GetAsync("AuthorsAPI/user-dropdown");
                var userJson = await userResponse.Content.ReadAsStringAsync();
                var users = JsonConvert.DeserializeObject<List<AuthorModel.UserDropDownModel>>(userJson) ?? new List<AuthorModel.UserDropDownModel>();

                AuthorModel author;

                if (id == null)
                {
                    author = new AuthorModel();
                }
                else
                {
                    var response = await _client.GetAsync($"AuthorsAPI/{id}");
                    if (!response.IsSuccessStatusCode)
                        return NotFound();

                    var json = await response.Content.ReadAsStringAsync();
                    author = JsonConvert.DeserializeObject<AuthorModel>(json);
                }

                author.UserList = users;
                return View(author);
            }
            catch
            {
                TempData["Message"] = "Failed to load Author form.";
                return View(new AuthorModel
                {
                    UserList = new List<AuthorModel.UserDropDownModel>()
                });
            }
        }
        #endregion

        #region UpdateAuthor
        [HttpPost]
        public async Task<IActionResult> AddEdit(AuthorModel author)
        {
            try
            {
                var userResponse = await _client.GetAsync("AuthorsAPI/user-dropdown");
                var userJson = await userResponse.Content.ReadAsStringAsync();
                author.UserList = JsonConvert.DeserializeObject<List<AuthorModel.UserDropDownModel>>(userJson) ?? new List<AuthorModel.UserDropDownModel>();

                if (!ModelState.IsValid)
                {
                    return View(author);
                }

                var content = new StringContent(JsonConvert.SerializeObject(author), Encoding.UTF8, "application/json");

                if (author.AuthorId == 0)
                {
                    author.CreatedAt = DateTime.Now;
                    await _client.PostAsync("AuthorsAPI", content);
                    TempData["Message"] = "Author added successfully.";
                }
                else
                {
                    author.ModifiedAt = DateTime.Now;
                    await _client.PutAsync($"AuthorsAPI/{author.AuthorId}", content);
                    TempData["Message"] = "Author updated successfully.";
                }

                return RedirectToAction("Index");
            }
            catch
            {
                TempData["Message"] = "Something went wrong while saving the author.";
                author.UserList = new List<AuthorModel.UserDropDownModel>();
                return RedirectToAction("Index");
            }
        }
        #endregion
    }
}

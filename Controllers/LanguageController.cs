using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using BookStorewithMVC.Models;
using System.Text;

namespace BookStorewithMVC.Controllers
{
    public class LanguageController : Controller
    {
        private readonly HttpClient _client;

        public LanguageController(IHttpClientFactory httpClientFactory)
        {
            _client = httpClientFactory.CreateClient();
            _client.BaseAddress = new Uri("https://localhost:44318/api/");
        }

        #region GetAll
        public async Task<IActionResult> Index()
        {
            try
            {
                var response = await _client.GetAsync("LanguagesAPI");
                var json = await response.Content.ReadAsStringAsync();
                var list = JsonConvert.DeserializeObject<List<LanguageModel>>(json);
                return View(list);
            }
            catch
            {
                return View(new List<LanguageModel>());
            }
        }
        #endregion

        #region Add Language
        public async Task<IActionResult> AddEdit(int? id)
        {
            try
            {
                var userResponse = await _client.GetAsync("LanguagesAPI/user-dropdown");
                var userJson = await userResponse.Content.ReadAsStringAsync();
                var users = JsonConvert.DeserializeObject<List<LanguageModel.UserDropDownModel>>(userJson);

                LanguageModel language;

                if (id == null)
                {
                    language = new LanguageModel();
                }
                else
                {
                    var response = await _client.GetAsync($"LanguagesAPI/{id}");
                    if (!response.IsSuccessStatusCode)
                    {
                        TempData["Message"] = "Language not found.";
                        return RedirectToAction("Index");
                    }

                    var json = await response.Content.ReadAsStringAsync();
                    language = JsonConvert.DeserializeObject<LanguageModel>(json);
                }

                language.UserList = users;
                return View(language);
            }
            catch
            {
                TempData["Message"] = "Failed to load Language form.";
                return RedirectToAction("Index");
            }
        }
        #endregion

        #region UpdateLanguage
        [HttpPost]
        public async Task<IActionResult> AddEdit(LanguageModel language)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var userResponse = await _client.GetAsync("LanguagesAPI/user-dropdown");
                    var userJson = await userResponse.Content.ReadAsStringAsync();
                    language.UserList = JsonConvert.DeserializeObject<List<LanguageModel.UserDropDownModel>>(userJson);

                    return View(language);
                }

                var content = new StringContent(JsonConvert.SerializeObject(language), Encoding.UTF8, "application/json");

                if (language.LanguageId == 0)
                {
                    language.CreatedAt = DateTime.Now;
                    await _client.PostAsync("LanguagesAPI", content);
                    TempData["Message"] = "Language added successfully.";
                }
                else
                {
                    language.ModifiedAt = DateTime.Now;
                    await _client.PutAsync($"LanguagesAPI/{language.LanguageId}", content);
                    TempData["Message"] = "Language updated successfully.";
                }

                return RedirectToAction("Index");
            }
            catch
            {
                TempData["Message"] = "Something went wrong while saving the language.";
                return RedirectToAction("Index");
            }
        }
        #endregion

        #region DeleteLanguage
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var response = await _client.DeleteAsync($"LanguagesAPI/{id}");

                if (response.IsSuccessStatusCode)
                {
                    TempData["Message"] = "Language deleted successfully.";
                }
                else
                {
                    TempData["Message"] = "Failed to delete language.";
                }

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                TempData["Message"] = "An error occurred while deleting the language.";
                return RedirectToAction(nameof(Index));
            }
        }
        #endregion
    }
} 
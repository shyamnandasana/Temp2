using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using BookStorewithMVC.Models;
using System.Text;

namespace BookStorewithMVC.Controllers
{
    public class CategoryController : Controller
    {
        private readonly HttpClient _client;

        public CategoryController(IHttpClientFactory httpClientFactory)
        {
            _client = httpClientFactory.CreateClient();
            _client.BaseAddress = new Uri("https://localhost:44318/api/");
        }

        #region GetAll
        public async Task<IActionResult> Index()
        {
            try
            {
                var response = await _client.GetAsync("CategoriesAPI");
                var json = await response.Content.ReadAsStringAsync();
                var list = JsonConvert.DeserializeObject<List<CategoryModel>>(json);
                return View(list);
            }
            catch
            {
                return View(new List<CategoryModel>());
            }
        }
        #endregion

        #region Add Category
        public async Task<IActionResult> AddEdit(int? id)
        {
            try
            {
                var userResponse = await _client.GetAsync("CategoriesAPI/user-dropdown");
                var userJson = await userResponse.Content.ReadAsStringAsync();
                var users = JsonConvert.DeserializeObject<List<CategoryModel.UserDropDownModel>>(userJson);

                CategoryModel category;

                if (id == null)
                {
                    category = new CategoryModel();
                }
                else
                {
                    var response = await _client.GetAsync($"CategoriesAPI/{id}");
                    if (!response.IsSuccessStatusCode)
                    {
                        TempData["Message"] = "Category not found.";
                        return RedirectToAction("Index");
                    }

                    var json = await response.Content.ReadAsStringAsync();
                    category = JsonConvert.DeserializeObject<CategoryModel>(json);
                }

                category.UserList = users;
                return View(category);
            }
            catch
            {
                TempData["Message"] = "Failed to load Category form.";
                return RedirectToAction("Index");
            }
        }
        #endregion

        #region UpdateCategory
        [HttpPost]
        public async Task<IActionResult> AddEdit(CategoryModel category)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var userResponse = await _client.GetAsync("CategoriesAPI/user-dropdown");
                    var userJson = await userResponse.Content.ReadAsStringAsync();
                    category.UserList = JsonConvert.DeserializeObject<List<CategoryModel.UserDropDownModel>>(userJson);

                    return View(category);
                }

                var content = new StringContent(JsonConvert.SerializeObject(category), Encoding.UTF8, "application/json");

                if (category.CategoryId == 0)
                {
                    category.CreatedAt = DateTime.Now;
                    await _client.PostAsync("CategoriesAPI", content);
                    TempData["Message"] = "Category added successfully.";
                }
                else
                {
                    category.ModifiedAt = DateTime.Now;
                    await _client.PutAsync($"CategoriesAPI/{category.CategoryId}", content);
                    TempData["Message"] = "Category updated successfully.";
                }

                return RedirectToAction("Index");
            }
            catch
            {
                TempData["Message"] = "Something went wrong while saving the category.";
                return RedirectToAction("Index");
            }
        }
        #endregion

        #region DeleteCategory
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var response = await _client.DeleteAsync($"CategoriesAPI/{id}");

                if (response.IsSuccessStatusCode)
                {
                    TempData["Message"] = "Category deleted successfully.";
                }
                else
                {
                    TempData["Message"] = "Failed to delete category.";
                }

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                TempData["Message"] = "An error occurred while deleting the category.";
                return RedirectToAction(nameof(Index));
            }
        }
        #endregion
    }
}
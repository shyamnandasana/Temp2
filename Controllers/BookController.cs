using BookStorewithMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using static BookStorewithMVC.Models.BookModel;

namespace BookStorewithMVC.Controllers
{
    public class BookController : Controller
    {
        private readonly HttpClient _client;

        public BookController(IHttpClientFactory httpClientFactory)
        {
            _client = httpClientFactory.CreateClient();
            _client.BaseAddress = new Uri("https://localhost:44318/api/");
        }

        #region GetAll

        public async Task<IActionResult> Index()
        {
            try
            {
                var response = await _client.GetAsync("BooksAPI");
                var json = await response.Content.ReadAsStringAsync();
                var books = JsonConvert.DeserializeObject<List<BookModel>>(json);
                return View(books);
            }
            catch
            {
                return View(new List<BookModel>());
            }
        }
        #endregion

        #region DeleteBook
        public async Task<IActionResult> DeleteBook(int id)
        {
            try
            {
                var response = await _client.DeleteAsync($"BooksAPI/{id}");

                if (response.IsSuccessStatusCode)
                {
                    TempData["Message"] = "Book deleted successfully.";
                }
                else
                {
                    TempData["Message"] = "Failed to delete book.";
                }

                return RedirectToAction("Index");
            }
            catch
            {
                TempData["Message"] = "An error occurred while deleting the book.";
                return RedirectToAction("Index");
            }
        }
        #endregion

        #region AddEdit
        public async Task<IActionResult> AddEdit(int? id)
        {
            try
            {
                // Fetch dropdown data
                var authorResponse = await _client.GetAsync("BooksAPI/dropdown/authors");
                var publisherResponse = await _client.GetAsync("BooksAPI/dropdown/publishers");
                var categoryResponse = await _client.GetAsync("BooksAPI/dropdown/categories");
                var languageResponse = await _client.GetAsync("BooksAPI/dropdown/languages");
                var userResponse = await _client.GetAsync("BooksAPI/user-dropdown");

                var authorJson = await authorResponse.Content.ReadAsStringAsync();
                var publisherJson = await publisherResponse.Content.ReadAsStringAsync();
                var categoryJson = await categoryResponse.Content.ReadAsStringAsync();
                var languageJson = await languageResponse.Content.ReadAsStringAsync();
                var userJson = await userResponse.Content.ReadAsStringAsync();

                var authors = JsonConvert.DeserializeObject<List<AuthorDropDownModel>>(authorJson);
                var publishers = JsonConvert.DeserializeObject<List<PublisherDropDownModel>>(publisherJson);
                var categories = JsonConvert.DeserializeObject<List<CategoryDropDownModel>>(categoryJson);
                var languages = JsonConvert.DeserializeObject<List<LanguageDropDownModel>>(languageJson);
                var users = JsonConvert.DeserializeObject<List<UserDropDownModel>>(userJson);

                BookModel book;

                if (id == null)
                {
                    book = new BookModel();
                }
                else
                {
                    var response = await _client.GetAsync($"BooksAPI/{id}");
                    if (!response.IsSuccessStatusCode)
                    {
                        TempData["Message"] = "Book not found.";
                        return RedirectToAction("Index");
                    }

                    var json = await response.Content.ReadAsStringAsync();
                    book = JsonConvert.DeserializeObject<BookModel>(json);
                }

                // Assign dropdowns with null checks
                book.AuthorList = authors ?? new List<AuthorDropDownModel>();
                book.PublisherList = publishers ?? new List<PublisherDropDownModel>();
                book.CategoryList = categories ?? new List<CategoryDropDownModel>();
                book.LanguageList = languages ?? new List<LanguageDropDownModel>();
                book.UserList = users ?? new List<UserDropDownModel>();

                return View(book);
            }
            catch
            {
                TempData["Message"] = "Failed to load Book form.";
                return RedirectToAction("Index");
            }
        }
        #endregion

        #region UpdateBook
        [HttpPost]
        public async Task<IActionResult> AddEdit(BookModel book)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    // Re-fetch dropdowns on validation error
                    var authorResponse = await _client.GetAsync("BooksAPI/dropdown/authors");
                    var publisherResponse = await _client.GetAsync("BooksAPI/dropdown/publishers");
                    var categoryResponse = await _client.GetAsync("BooksAPI/dropdown/categories");
                    var languageResponse = await _client.GetAsync("BooksAPI/dropdown/languages");
                    var userResponse = await _client.GetAsync("BooksAPI/user-dropdown");

                    var authorJson = await authorResponse.Content.ReadAsStringAsync();
                    var publisherJson = await publisherResponse.Content.ReadAsStringAsync();
                    var categoryJson = await categoryResponse.Content.ReadAsStringAsync();
                    var languageJson = await languageResponse.Content.ReadAsStringAsync();
                    var userJson = await userResponse.Content.ReadAsStringAsync();

                    book.AuthorList = JsonConvert.DeserializeObject<List<AuthorDropDownModel>>(authorJson) ?? new List<AuthorDropDownModel>();
                    book.PublisherList = JsonConvert.DeserializeObject<List<PublisherDropDownModel>>(publisherJson) ?? new List<PublisherDropDownModel>();
                    book.CategoryList = JsonConvert.DeserializeObject<List<CategoryDropDownModel>>(categoryJson) ?? new List<CategoryDropDownModel>();
                    book.LanguageList = JsonConvert.DeserializeObject<List<LanguageDropDownModel>>(languageJson) ?? new List<LanguageDropDownModel>();
                    book.UserList = JsonConvert.DeserializeObject<List<UserDropDownModel>>(userJson) ?? new List<UserDropDownModel>();

                    return View(book);
                }

                var content = new StringContent(JsonConvert.SerializeObject(book), Encoding.UTF8, "application/json");

                if (book.BookId == 0)
                {
                    book.CreatedAt = DateTime.Now;
                    await _client.PostAsync("BooksAPI", content);
                    TempData["Message"] = "Book added successfully.";
                }
                else
                {
                    book.ModifiedAt = DateTime.Now;
                    await _client.PutAsync($"BooksAPI/{book.BookId}", content);
                    TempData["Message"] = "Book updated successfully.";
                }

                return RedirectToAction("Index");
            }
            catch
            {
                TempData["Message"] = "Something went wrong while saving the book.";
                return RedirectToAction("Index");
            }
        }
        #endregion

        
    }
}
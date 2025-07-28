//using Microsoft.AspNetCore.Mvc;
//using Newtonsoft.Json;
//using BookStorewithMVC.Models;
//using System.Text;

//namespace BookStorewithMVC.Controllers
//{
//    public class PublishersController : Controller
//    {
//        private readonly HttpClient _client;

//        public PublishersController(IHttpClientFactory httpClientFactory)
//        {
//            _client = httpClientFactory.CreateClient();
//            _client.BaseAddress = new Uri("https://localhost:44318/api/");
//        }

//        #region GetAll
//        public async Task<IActionResult> Index()
//        {
//            try
//            {
//                var response = await _client.GetAsync("PublishersAPI");
//                var json = await response.Content.ReadAsStringAsync();
//                var list = JsonConvert.DeserializeObject<List<PublisherModel>>(json);
//                return View(list);
//            }
//            catch
//            {
//                return View(new List<PublisherModel>());
//            }
//        }
//        #endregion

//        #region Add Publisher
//        public async Task<IActionResult> AddEdit(int? id)
//        {
//            try
//            {
//                var userResponse = await _client.GetAsync("PublishersAPI/user-dropdown");
//                var userJson = await userResponse.Content.ReadAsStringAsync();
//                var users = JsonConvert.DeserializeObject<List<PublisherModel.UserDropDownModel>>(userJson);

//                PublisherModel publisher;

//                if (id == null)
//                {
//                    publisher = new PublisherModel();
//                }
//                else
//                {
//                    var response = await _client.GetAsync($"PublishersAPI/{id}");
//                    if (!response.IsSuccessStatusCode)
//                    {
//                        TempData["Message"] = "Publisher not found.";
//                        return RedirectToAction("Index");
//                    }

//                    var json = await response.Content.ReadAsStringAsync();
//                    publisher = JsonConvert.DeserializeObject<PublisherModel>(json);
//                }

//                publisher.UserList = users;
//                return View(publisher);
//            }
//            catch
//            {
//                TempData["Message"] = "Failed to load Publisher form.";
//                return RedirectToAction("Index");
//            }
//        }
//        #endregion

//        #region UpdatePublisher
//        [HttpPost]
//        public async Task<IActionResult> AddEdit(PublisherModel publisher)
//        {
//            try
//            {
//                if (!ModelState.IsValid)
//                {
//                    var userResponse = await _client.GetAsync("PublishersAPI/user-dropdown");
//                    var userJson = await userResponse.Content.ReadAsStringAsync();
//                    publisher.UserList = JsonConvert.DeserializeObject<List<PublisherModel.UserDropDownModel>>(userJson);

//                    return View(publisher);
//                }

//                var content = new StringContent(JsonConvert.SerializeObject(publisher), Encoding.UTF8, "application/json");

//                if (publisher.PublisherId == 0)
//                {
//                    publisher.CreatedAt = DateTime.Now;
//                    await _client.PostAsync("PublishersAPI", content);
//                    TempData["Message"] = "Author added successfully.";
//                }
//                else
//                {
//                    publisher.ModifiedAt = DateTime.Now;
//                    await _client.PutAsync($"PublishersAPI/{publisher.PublisherId}", content);
//                    TempData["Message"] = "Author updated successfully.";

//                }

//                return RedirectToAction("Index");
//            }
//            catch
//            {
//                TempData["Message"] = "Something went wrong while saving the author.";
//                return RedirectToAction("Index");
//            }
//        }
//        #endregion

//        #region DeletePublisher
//        public async Task<IActionResult> Delete(int id)
//        {
//            try
//            {
//                var response = await _client.DeleteAsync($"PublishersAPI/{id}");

//                if (response.IsSuccessStatusCode)
//                {
//                    TempData["Message"] = "Publisher deleted successfully.";
//                }
//                else
//                {
//                    TempData["Message"] = "Failed to delete publisher.";
//                }

//                return RedirectToAction(nameof(Index));
//            }
//            catch
//            {
//                TempData["Message"] = "An error occurred while deleting the publisher.";
//                return RedirectToAction(nameof(Index));
//            }
//        }
//        #endregion
//    }
//}
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using BookStorewithMVC.Models;
using System.Text;

namespace BookStorewithMVC.Controllers
{
    public class PublishersController : Controller
    {
        private readonly HttpClient _client;

        public PublishersController(IHttpClientFactory httpClientFactory)
        {
            _client = httpClientFactory.CreateClient();
            _client.BaseAddress = new Uri("https://localhost:44318/api/");
        }

        #region GetAll
        public async Task<IActionResult> Index()
        {
            try
            {
                var response = await _client.GetAsync("PublishersAPI");
                var json = await response.Content.ReadAsStringAsync();
                var list = JsonConvert.DeserializeObject<List<PublisherModel>>(json);
                return View(list);
            }
            catch
            {
                return View(new List<PublisherModel>());
            }
        }
        #endregion

        #region Add Publisher
        public async Task<IActionResult> AddEdit(int? id)
        {
            try
            {
                var userResponse = await _client.GetAsync("PublishersAPI/user-dropdown");
                var userJson = await userResponse.Content.ReadAsStringAsync();
                var users = JsonConvert.DeserializeObject<List<PublisherModel.UserDropDownModel>>(userJson);

                PublisherModel publisher;

                if (id == null)
                {
                    publisher = new PublisherModel();
                }
                else
                {
                    var response = await _client.GetAsync($"PublishersAPI/{id}");
                    if (!response.IsSuccessStatusCode)
                    {
                        TempData["Message"] = "Publisher not found.";
                        return RedirectToAction("Index");
                    }

                    var json = await response.Content.ReadAsStringAsync();
                    publisher = JsonConvert.DeserializeObject<PublisherModel>(json);
                }

                publisher.UserList = users;
                return View(publisher);
            }
            catch
            {
                TempData["Message"] = "Failed to load Publisher form.";
                return RedirectToAction("Index");
            }
        }
        #endregion

        #region UpdatePublisher
        [HttpPost]
        public async Task<IActionResult> AddEdit(PublisherModel publisher)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var userResponse = await _client.GetAsync("PublishersAPI/user-dropdown");
                    var userJson = await userResponse.Content.ReadAsStringAsync();
                    publisher.UserList = JsonConvert.DeserializeObject<List<PublisherModel.UserDropDownModel>>(userJson);

                    return View(publisher);
                }

                var content = new StringContent(JsonConvert.SerializeObject(publisher), Encoding.UTF8, "application/json");

                if (publisher.PublisherId == 0)
                {
                    publisher.CreatedAt = DateTime.Now;
                    await _client.PostAsync("PublishersAPI", content);
                    TempData["Message"] = "Publisher added successfully.";
                }
                else
                {
                    publisher.ModifiedAt = DateTime.Now;
                    await _client.PutAsync($"PublishersAPI/{publisher.PublisherId}", content);
                    TempData["Message"] = "Publisher updated successfully.";

                }

                return RedirectToAction("Index");
            }
            catch
            {
                TempData["Message"] = "Something went wrong while saving the Publisher.";
                return RedirectToAction("Index");
            }
        }
        #endregion

        #region DeletePublisher
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var response = await _client.DeleteAsync($"PublishersAPI/{id}");

                if (response.IsSuccessStatusCode)
                {
                    TempData["Message"] = "Publisher deleted successfully.";
                }
                else
                {
                    TempData["Message"] = "Failed to delete publisher.";
                }

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                TempData["Message"] = "An error occurred while deleting the publisher.";
                return RedirectToAction(nameof(Index));
            }
        }
        #endregion
    }
}



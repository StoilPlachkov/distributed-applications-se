using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using System.Text.Json.Serialization;
using WebClient.Models;

namespace WebClient.Controllers
{
    public class TrainController : Controller
    {
        private readonly string _apiKey = "?x-api-key=069D25CA8A5241688DBB7DA09FE68D75";
        Uri baseAddress = new Uri("https://localhost:7224/api");
        [HttpGet]
        public IActionResult Index(string sortOrder, string searchString)
        {
            List<TrainViewModel> trainLits = new List<TrainViewModel>();
            //Request
            using (var http = new HttpClient())
            {
                var response = http.GetAsync(baseAddress + "/Trains/GetTrains" + _apiKey).Result;
                if (response.IsSuccessStatusCode)
                {
                    var body = response.Content.ReadAsStringAsync().Result;
                    trainLits = JsonConvert.DeserializeObject<List<TrainViewModel>>(body);
                }

                ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name" : "";
                ViewData["DateSortParm"] = String.IsNullOrEmpty(sortOrder) ? "date" : "";
                ViewData["CapacitySortParm"] = String.IsNullOrEmpty(sortOrder) ? "capacity" : "";
                ViewData["TypeSortParm"] = String.IsNullOrEmpty(sortOrder) ? "type" : "";
                ViewData["SpeedSortParm"] = String.IsNullOrEmpty(sortOrder) ? "speed" : "";
                ViewData["IdSortParm"] = String.IsNullOrEmpty(sortOrder) ? "id" : "";
                ViewData["CurrentFilter"] = searchString;
                if (!String.IsNullOrEmpty(searchString))
                {
                    trainLits = trainLits.Where(t => t.Name.Contains(searchString)).ToList();
                }

                switch (sortOrder)
                {
                    case "name":
                        trainLits = trainLits.OrderBy(t => t.Name).ToList();
                        break;
                    case "capacity":
                        trainLits = trainLits.OrderBy(t => t.Capacity).ToList();
                        break;
                    case "date":
                        trainLits = trainLits.OrderBy(t => t.ManufactureDate).ToList();
                        break;
                    case "type":
                        trainLits = trainLits.OrderBy(t => t.Type).ToList();
                        break;
                    case "speed":
                        trainLits = trainLits.OrderBy(t => t.MaxSpeed).ToList();
                        break;
                    case "id":
                        trainLits = trainLits.OrderBy(t => t.Id).ToList();
                        break;
                    default:
                        trainLits = trainLits.OrderBy(t => t.Id).ToList();
                        break;
                }
                return View(trainLits);
            }
        }

        // https://localhost:7049/api/gettrain?id=2
        [HttpGet("Train/Get")]
        public IActionResult Get(int id)
        {
            using (var http = new HttpClient())
            {
                var response = http.GetAsync(baseAddress + "/Trains/GetTrain/" + id + _apiKey).Result;
                if (response.IsSuccessStatusCode)
                {
                    var body = response.Content.ReadAsStringAsync().Result;
                    return View(JsonConvert.DeserializeObject<TrainViewModel>(body));
                }
                return View();
            }
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(TrainViewModel train)
        {
            try
            {
                string data = JsonConvert.SerializeObject(train);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                using (var http = new HttpClient())
                {
                    HttpResponseMessage response = http.PostAsync(baseAddress + "/Trains/PostTrain" + _apiKey, content).Result;


                    if (response.IsSuccessStatusCode)
                    {
                        TempData["successMessage"] = "Train Added.";
                        return RedirectToAction("Index");
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View();
            }
            return View();
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            try
            {
                using (var http = new HttpClient())
                {
                    TrainViewModel train = new TrainViewModel();
                    HttpResponseMessage response = http.GetAsync(baseAddress + "/Trains/GetTrain/" + id + _apiKey).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        string data = response.Content.ReadAsStringAsync().Result;
                        train = JsonConvert.DeserializeObject<TrainViewModel>(data);
                    }
                    return View(train);
                }
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View();
            }
        }
        [HttpPost]
        public IActionResult Edit(TrainViewModel train)
        {
            try
            {
                using (var http = new HttpClient())
                {
                    string data = JsonConvert.SerializeObject(train);
                    StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = http.PutAsync(baseAddress + "/Trains/PutTrain/" + train.Id + _apiKey, content).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        TempData["successMessage"] = "Train updated successfully.";
                        return RedirectToAction("Index");
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View();
            }
            return View();
        }
        [HttpGet]
        public IActionResult Delete(int id)
        {
            try
            {
                TrainViewModel train = new TrainViewModel();
                using (var http = new HttpClient())
                {
                    HttpResponseMessage response = http.GetAsync(baseAddress + "/Trains/GetTrain/" + id + _apiKey).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        string data = response.Content.ReadAsStringAsync().Result;
                        train = JsonConvert.DeserializeObject<TrainViewModel>(data);
                    }
                    return View(train);
                }
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View();
            }
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirm(int id)
        {
            try
            {
                using (var http = new HttpClient())
                {
                    HttpResponseMessage response = http.DeleteAsync(baseAddress + "/Trains/DeleteTrain/" + id + _apiKey).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        TempData["successMessage"] = "Train deleted successfully.";
                        return RedirectToAction("Index");
                    }

                }
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View();
            }
            return View();
        }
    }
}

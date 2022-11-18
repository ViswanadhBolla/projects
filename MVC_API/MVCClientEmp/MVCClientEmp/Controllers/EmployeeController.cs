using System.Net.Http.Headers;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using MVCClientEmp.Models;
using Newtonsoft.Json;

namespace MVCClientEmp.Controllers
{
    public class EmployeeController : Controller
    {
        string Baseurl = "https://localhost:7141/";
        public async Task<ActionResult> Index()
        {
            List<Emp> EmpInfo = new List<Emp>();

            using (var client = new HttpClient())
            {
                //Passing service base url  
                client.BaseAddress = new Uri(Baseurl);

                client.DefaultRequestHeaders.Clear();
                //Define request data format  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //Sending request to find web api REST service resource GetAllEmployees using HttpClient  
                HttpResponseMessage Res = await client.GetAsync("api/Employees");

                //Checking the response is successful or not which is sent using HttpClient  
                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    var EmpResponse = Res.Content.ReadAsStringAsync().Result;

                    //Deserializing the response recieved from web api and storing into the Employee list  
                    EmpInfo = JsonConvert.DeserializeObject<List<Emp>>(EmpResponse);

                }
                //returning the employee list to view  
                return View(EmpInfo);
            }
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(Emp e)
        {
            Emp Emplobj = new Emp();
            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(e), Encoding.UTF8, "application/json");

                using (var response = await httpClient.PostAsync("https://localhost:7141/api/Employees", content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    Emplobj = JsonConvert.DeserializeObject<Emp>(apiResponse);
                }
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<ActionResult> Edit(int id)
        {
            Emp emp = new Emp();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("https://localhost:7141/api/Employees/" + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    emp = JsonConvert.DeserializeObject<Emp>(apiResponse);
                }
            }
            return View(emp);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(Emp e)
        {
            Emp receivedemp = new Emp();

            using (var httpClient = new HttpClient())
            {
                #region
                //var content = new MultipartFormDataContent();
                //content.Add(new StringContent(reservation.Empid.ToString()), "Empid");
                //content.Add(new StringContent(reservation.Name), "Name");
                //content.Add(new StringContent(reservation.Gender), "Gender");
                //content.Add(new StringContent(reservation.Newcity), "Newcity");
                //content.Add(new StringContent(reservation.Deptid.ToString()), "Deptid");
                #endregion
                int id = e.eid;
                StringContent content1 = new StringContent(JsonConvert.SerializeObject(e), Encoding.UTF8, "application/json");
                using (var response = await httpClient.PutAsync("https://localhost:7141/api/Employees/" + id, content1))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    ViewBag.Result = "Success";
                    receivedemp = JsonConvert.DeserializeObject<Emp>(apiResponse);
                }
            }
            return RedirectToAction("index");
        }

        [HttpGet]
        public async Task<ActionResult> Delete(int id)
        {
            TempData["empid"] = id;
            Emp e = new Emp();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("https://localhost:7141/api/Employees/" + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    e = JsonConvert.DeserializeObject<Emp>(apiResponse);
                }
            }
            return View(e);
        }


        [HttpPost]
        [ActionName("Delete")]
        public async Task<ActionResult> DeleteEmployee(Emp e)
        {
            int empid = Convert.ToInt32(TempData["empid"]);
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.DeleteAsync("https://localhost:7141/api/Employees/" + empid))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                }
            }

            return RedirectToAction("Index");
        }
    }
}

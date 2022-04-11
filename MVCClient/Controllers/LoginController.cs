using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MVCClient.Models;
using Newtonsoft.Json;

namespace MVCClient.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserViewModel user)
        {

            using (var client = new HttpClient())
            {
                var token = string.Empty;
                //Send HTTP requests from here. 
                client.BaseAddress = new Uri("http://localhost:51731/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.PostAsJsonAsync("api/Authentication", user);
                if (response.IsSuccessStatusCode)
                {
                    var stringJWT = response.Content.ReadAsStringAsync().Result;
                    JWT jwt = JsonConvert.DeserializeObject
      <JWT>(stringJWT);
                    HttpContext.Session.SetString("token", jwt.Token);

                    ViewBag.msg = "User logged in successfully!";
                    return RedirectToAction("Index", "Batches");
                }

                else
                {
                    ViewBag.msg = "Not a valid user";
                    return View();
                }
            }
        }
    }
    public class JWT
    {
        public string Token { get; set; }
    }

}

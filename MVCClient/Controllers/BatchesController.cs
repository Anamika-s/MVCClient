using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MVCClient.Models;

namespace MVCClient.Controllers
{
    public class BatchesController : Controller
    {
        public async Task<IActionResult >Index()
        {
            using (var client = new HttpClient())
            {
                //Send HTTP requests from here. 
                client.BaseAddress = new Uri("http://localhost:51731/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization =
new AuthenticationHeaderValue("Bearer",
HttpContext.Session.GetString("token"));


                HttpResponseMessage response = await client.GetAsync("api/Batch");
                if (response.IsSuccessStatusCode)
                {
                    List<BatchViewModel> batches = await response.Content.ReadAsAsync<List<BatchViewModel>>();
                    if (batches.Count == 0)
                        ViewBag.msg = "No Records";
                    else
                        return View(batches);
                }

            }
                return View();
        }

         public IActionResult Create()
        {
            return View(new BatchViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(BatchViewModel batch)
        {
            using (var client = new HttpClient())
            {
                //Send HTTP requests from here. 
                client.BaseAddress = new Uri("http://localhost:51731/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization =
new AuthenticationHeaderValue("Bearer",
HttpContext.Session.GetString("token"));

                HttpResponseMessage response = await client.PostAsJsonAsync("api/Batch", batch);
                if (response.IsSuccessStatusCode)
                     return RedirectToAction("Index");
                       
                    else
                        return View();
                }

            } 
        }
    }

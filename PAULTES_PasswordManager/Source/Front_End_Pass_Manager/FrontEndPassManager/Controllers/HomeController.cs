using FrontEndPassManager.Models;
using FrontEndPassManager.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;

namespace FrontEndPassManager.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private static readonly string _BaseUrl = $"https://localhost:5001/api/";
        private readonly string CleChiffreDechiffre = "lacledecryptageestassezsimpleokk";
        public IActionResult Index()
        {
            try
            {
                using (HttpClient client = new HttpClient
                {
                    BaseAddress = new Uri(_BaseUrl)
                })
                {
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage response = client.GetAsync($"Mdp/{User.Identity.Name}").Result;
                    if (response.IsSuccessStatusCode)
                    {
                        IEnumerable<MdpModels> LesMdp = JsonConvert.DeserializeObject<List<MdpModels>>(response.Content.ReadAsStringAsync().Result);
                        Crypto Moncrypt = new Crypto();
                        foreach (MdpModels x in LesMdp)
                        {
                            x.Mdp = Moncrypt.Decrypt(x.Mdp, CleChiffreDechiffre);
                        }
                        return View(LesMdp);
                    }
                }
            }
            catch
            {

            }
            return View();
        }

        // GET: User/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: User/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                using (HttpClient client = new HttpClient
                {
                    BaseAddress = new Uri(_BaseUrl)
                })
                {
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage response = client.GetAsync($"User/0&?User={User.Identity.Name}").Result;

                    if (response.IsSuccessStatusCode)
                    {
                        UserModels varia = JsonConvert.DeserializeObject<UserModels>(response.Content.ReadAsStringAsync().Result);
                        using (HttpClient client2 = new HttpClient
                        {
                            BaseAddress = new Uri(_BaseUrl)
                        })
                        {
                            Crypto moncrypt = new Crypto();
                            MdpModels motdepasse = new MdpModels
                            {

                                Id = varia.Id,
                                Mdp = moncrypt.Encrypt(collection["Mdp"], CleChiffreDechiffre),
                                ReferenceSite = collection["ReferenceSite"]
                            };
                            client2.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                            using (HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(motdepasse)))
                            {
                                httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                                HttpResponseMessage response2 = client2.PutAsync("Mdp/", httpContent).Result;
                            }
                        }
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: User/Edit/5
        public ActionResult Edit(long id)
        {
            using (HttpClient client = new HttpClient
            {
                BaseAddress = new Uri(_BaseUrl)
            })
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = client.GetAsync($"Mdp/{User.Identity.Name}").Result;
                if (response.IsSuccessStatusCode)
                {
                    IEnumerable<MdpModels> LesMdp = JsonConvert.DeserializeObject<List<MdpModels>>(response.Content.ReadAsStringAsync().Result);
                    MdpModels model = LesMdp.Where(x => x.Id == id).Select(x => x).FirstOrDefault();
                    Crypto Moncrypt = new Crypto();
                    model.Mdp = Moncrypt.Decrypt(model.Mdp, CleChiffreDechiffre);
                    return View(model);
                }
                return View();
            }
        }

        // POST: User/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collect)
        {
            try
            {
                Crypto moncrypt = new Crypto();
                MdpModels model = new MdpModels
                {
                    Id = id,
                    Mdp = moncrypt.Encrypt(collect["Mdp"], CleChiffreDechiffre),
                    ReferenceSite = collect["ReferenceSite"]
                };
                using (HttpClient client = new HttpClient
                {
                    BaseAddress = new Uri(_BaseUrl)
                })
                {
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    using (HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(model)))
                    {
                        httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                        HttpResponseMessage response = client.PostAsync("Mdp/", httpContent).Result;
                        return RedirectToAction(nameof(Index));
                    }
                }
            }
            catch
            {
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: User/Delete/5
        public ActionResult Delete(long id)
        {
            try
            {
                using (HttpClient client = new HttpClient
                {
                    BaseAddress = new Uri(_BaseUrl)
                })
                {
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage response = client.GetAsync($"Mdp/{User.Identity.Name}").Result;
                    if (response.IsSuccessStatusCode)
                    {
                        IEnumerable<MdpModels> LesMdp = JsonConvert.DeserializeObject<List<MdpModels>>(response.Content.ReadAsStringAsync().Result);
                        MdpModels model = LesMdp.Where(x => x.Id == id).Select(x => x).FirstOrDefault();
                        Crypto Moncrypt = new Crypto();
                        model.Mdp = Moncrypt.Decrypt(model.Mdp, CleChiffreDechiffre);
                        return View(model);
                    }
                    return View();
                }
            }
            catch
            {
                return View();
            }
        }

        // POST: User/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(long id, IFormCollection collection)
        {
            try
            {
                using (HttpClient client = new HttpClient
                {
                    BaseAddress = new Uri(_BaseUrl)
                })
                {
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage response = client.DeleteAsync("Mdp/" + id).Result;
                    return RedirectToAction(nameof(Index));
                }
            }
            catch
            {
                return View();
            }
        }
    }
}

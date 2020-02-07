using FrontEndPassManager.Models;
using FrontEndPassManager.Service;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Security.Cryptography;

namespace FrontEndPassManager.Controllers
{
    public class AccountController : Controller
    {
        private static readonly string _BaseUrl = $"https://localhost:5001/api/";
        private readonly string CleChiffreDechiffre = "lacledecryptageestassezsimpleokk";
        // GET: User
        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Inscript()
        {
            return View();
        }

        public ActionResult Inscription(IFormCollection collect)
        {
            if (collect["Mdp"] == collect["MdpConf"])
            {
                byte[] salt = new byte[128 / 8];
                using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
                {
                    rng.GetBytes(salt);
                }
                string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                    password: collect["Mdp"],
                    salt: salt,
                    prf: KeyDerivationPrf.HMACSHA1,
                    iterationCount: 10000,
                    numBytesRequested: 256 / 8)
                );
                UserModels model = new UserModels
                {
                    User = collect["User"],
                    Email = collect["Email"],
                    HA = hashed,
                    Salt = salt
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
                        HttpResponseMessage response = client.PostAsync("User/", httpContent).Result;
                        return RedirectToAction(nameof(Login));
                    }
                }
            }
            return RedirectToAction(nameof(Inscript));
        }


        public ActionResult Connect(IFormCollection collection)
        {
            try
            {
                using (HttpClient client = new HttpClient
                {
                    BaseAddress = new Uri(_BaseUrl)
                })
                {
                    string user = collection["User"];
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage response = client.GetAsync($"User/0&?User={user}").Result;

                    if (response.IsSuccessStatusCode)
                    {
                        UserModels UnModelUserForChallenge = new UserModels();
                        UserModels varia = JsonConvert.DeserializeObject<UserModels>(response.Content.ReadAsStringAsync().Result);
                        Crypto MonCrypt = new Crypto();
                        string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                            password: collection["Mdp"],
                            salt: varia.Salt,
                            prf: KeyDerivationPrf.HMACSHA1,
                            iterationCount: 10000,
                            numBytesRequested: 256 / 8)
                        );
                        string VariableChallenge = varia.ChallengeUser + hashed;
                        UnModelUserForChallenge.User = user;
                        UnModelUserForChallenge.ChallengeUser = MonCrypt.Encrypt(VariableChallenge, CleChiffreDechiffre);
                        using (HttpClient client2 = new HttpClient
                        {
                            BaseAddress = new Uri(_BaseUrl)
                        })
                        {
                            client2.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                            using (HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(UnModelUserForChallenge)))
                            {
                                httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                                HttpResponseMessage response2 = client.PutAsync("User/", httpContent).Result;
                                if (response2.IsSuccessStatusCode)
                                {
                                    List<Claim> claims = new List<Claim>
                                    {
                                        new Claim(ClaimTypes.Name, user)
                                    };
                                    ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                                    AuthenticationProperties authProperties = new AuthenticationProperties
                                    {
                                        ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
                                    };
                                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
                                }
                            }
                        }
                    }
                }
                return RedirectToAction("Index", "Home", null);
            }
            catch
            {
                return null;
            }
        }

        public ActionResult Deconnection()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home", null);
        }
    }
}
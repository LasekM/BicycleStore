using BicycleStore.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Text.Json;

namespace BicycleStore.Controllers
{
    public class AuthController : Controller
    {
        private readonly HttpClient _httpClient;

        public AuthController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var response = await _httpClient.PostAsJsonAsync("https://localhost:7137/api/Auth/Login", model);
            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();

                // Debugging: Print out the jsonResponse
                System.Diagnostics.Debug.WriteLine("API Response: " + jsonResponse);

                var tokenDict = JsonSerializer.Deserialize<Dictionary<string, string>>(jsonResponse);

                // Debugging: Print out the dictionary keys
                foreach (var key in tokenDict.Keys)
                {
                    System.Diagnostics.Debug.WriteLine("Key: " + key);
                }

                if (tokenDict.TryGetValue("token", out var token))
                {
                    // Print the JWT token to the console
                    System.Diagnostics.Debug.WriteLine("JWT Token: " + token);

                    HttpContext.Session.SetString("JWToken", token);

                    var handler = new JwtSecurityTokenHandler();

                    if (handler.CanReadToken(token))
                    {
                        var jsonToken = handler.ReadToken(token) as JwtSecurityToken;

                        // Debugging: Print out the claims in the token
                        foreach (var claim in jsonToken.Claims)
                        {
                            System.Diagnostics.Debug.WriteLine($"Claim Type: {claim.Type}, Value: {claim.Value}");
                        }

                        var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, model.Username)
                };

                        // Add role claims from the token
                        if (jsonToken != null)
                        {
                            foreach (var claim in jsonToken.Claims)
                            {
                                if (claim.Type == "role")
                                {
                                    claims.Add(new Claim(ClaimTypes.Role, claim.Value));
                                }
                            }
                        }

                        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                        return RedirectToAction("Index", "Home");
                    }
                }

                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return View(model);
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return View(model);
        }







        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var response = await _httpClient.PostAsJsonAsync("https://localhost:7137/api/Auth/Register", model);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Login");
            }

            ModelState.AddModelError(string.Empty, "Registration failed.");
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Remove("JWToken");
            return RedirectToAction("Index", "Home");
        }
    }
}
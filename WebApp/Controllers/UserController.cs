using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using WebApp.Models;
using WebApp.Services;

namespace WebApp.Controllers
{
    public class UserController : Controller
    {
        public IConfiguration Configuration;
        private string _endpointUrl;
        public ISessionService _sessionService;

        public UserController(IConfiguration configuration, ISessionService sessionService) : base()
        {
            Configuration = configuration;
            _endpointUrl = Configuration["RestApiUrl"] + "user";
            _sessionService = sessionService;
        }

        // GET: UserController
        public async Task<ActionResult> Index()
        {
            if (await _sessionService.Role(await _sessionService.UsernameLoggedIn(Request.Cookies["sessionid"])) != "admin")
                return RedirectToAction("Index", "Home");

            List<UserVM> userList;

            using (var response = await new HttpClient().GetAsync(_endpointUrl))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                userList = JsonConvert.DeserializeObject<List<UserVM>>(apiResponse);
            }

            string sessionId = Request.Cookies["sessionid"];
            ViewBag.SessionId = sessionId;
            return View(userList);
        }

        // GET: UserController/Details/5
        [Route("/[controller]/{id}")]
        public async Task<ActionResult> Details(int id)
        {
            if (await _sessionService.Role(await _sessionService.UsernameLoggedIn(Request.Cookies["sessionid"])) != "admin")
                return RedirectToAction("Index", "Home");
            string sessionId = Request.Cookies["sessionid"];
            ViewBag.SessionId = sessionId;
            return View();
        }

        // GET: UserController/Create
        public async Task<ActionResult> Create()
        {
            if (await _sessionService.Role(await _sessionService.UsernameLoggedIn(Request.Cookies["sessionid"])) != "admin")
                return RedirectToAction("Index", "Home");
            string sessionId = Request.Cookies["sessionid"];
            ViewBag.SessionId = sessionId;
            return View();
        }

        // POST: UserController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(UserPost u)
        {
            if (await _sessionService.Role(await _sessionService.UsernameLoggedIn(Request.Cookies["sessionid"])) != "admin")
                return RedirectToAction("Index", "Home");

            try
            {
                using (var HttpClient = new HttpClient())
                {
                    string userJson = System.Text.Json.JsonSerializer.Serialize(u);
                    var content = new StringContent(userJson, Encoding.UTF8, "application/json");

                    await HttpClient.PostAsync($"{_endpointUrl}", content);
                }
            }
            catch (Exception e)
            {
                return View("Error", e);
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: UserController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            if (await _sessionService.Role(await _sessionService.UsernameLoggedIn(Request.Cookies["sessionid"])) != "admin")
                return RedirectToAction("Index", "Home");

            string sessionId = Request.Cookies["sessionid"];
            ViewBag.SessionId = sessionId;
            return View();
        }

        // POST: UserController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, IFormCollection collection)
        {
            if (await _sessionService.Role(await _sessionService.UsernameLoggedIn(Request.Cookies["sessionid"])) != "admin")
                return RedirectToAction("Index", "Home");

            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                string sessionId = Request.Cookies["sessionid"];
                ViewBag.SessionId = sessionId;
                return View();
            }
        }

        // GET: UserController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            if (await _sessionService.Role(await _sessionService.UsernameLoggedIn(Request.Cookies["sessionid"])) != "admin")
                return RedirectToAction("Index", "Home");

            string sessionId = Request.Cookies["sessionid"];
            ViewBag.SessionId = sessionId;
            return View();
        }

        // POST: UserController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, IFormCollection collection)
        {
            if (await _sessionService.Role(await _sessionService.UsernameLoggedIn(Request.Cookies["sessionid"])) != "admin")
                return RedirectToAction("Index", "Home");

            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                string sessionId = Request.Cookies["sessionid"];
                ViewBag.SessionId = sessionId;
                return View();
            }
        }

        public async Task<ActionResult> Register()
        {
            string sessionId = Request.Cookies["sessionid"];
            ViewBag.SessionId = sessionId;
            return View(new RegisterVM());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterVM r)
        {
            if (!ModelState.IsValid)
                return View(r);

            try
            {
                using (var HttpClient = new HttpClient())
                {
                    string registerJson = System.Text.Json.JsonSerializer.Serialize(r.getUserData());
                    var content = new StringContent(registerJson, Encoding.UTF8, "application/json");

                    var response = await HttpClient.PostAsync($"{_endpointUrl}", content);
                    if (!response.IsSuccessStatusCode)
                    {
                        ModelState.AddModelError("", await response.Content.ReadAsStringAsync());
                        return View(r);
                    }
                }
            }
            catch (Exception e)
            {
                return View("Error", e);
            }

            string sessionId = Request.Cookies["sessionid"];
            ViewBag.SessionId = sessionId;
            return RedirectToAction("Index", "Home");
        }

        public async Task<ActionResult> Login()
        {
            string sessionId = Request.Cookies["sessionid"];
            ViewBag.SessionId = sessionId;
            return View(new LoginVM());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginVM l)
        {
            if (!ModelState.IsValid)
                return View(l);

            try
            {
                using (var HttpClient = new HttpClient())
                {
                    string registerJson = System.Text.Json.JsonSerializer.Serialize(l);
                    var content = new StringContent(registerJson, Encoding.UTF8, "application/json");

                    var response = await HttpClient.PostAsync($"{_endpointUrl}/login", content);
                    if (!response.IsSuccessStatusCode)
                    {
                        ModelState.AddModelError("", await response.Content.ReadAsStringAsync());
                        return View(l);
                    }
                    TokenBundle bundle = JsonConvert.DeserializeObject<TokenBundle>(await response.Content.ReadAsStringAsync());
                    string sessionId = bundle.SessionId;
                    string jwt = bundle.JsonWebToken;
                    Response.Cookies.Append("sessionid", sessionId, new CookieOptions()
                    {
                        HttpOnly = true,
                        Secure = true
                    });
                    ViewBag.SessionId = sessionId;
                    Response.Cookies.Append("jwt", jwt, new CookieOptions()
                    {
                        HttpOnly = true,
                        Secure = true
                    });
                }
            }
            catch (Exception e)
            {
                return View("Error", e);
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            string sessionId = Request.Cookies["sessionid"];
            await _sessionService.EndSession(sessionId);
            Response.Cookies.Delete("sessionid");
            Response.Cookies.Delete("jwt");
            return RedirectToAction("Index", "Home");
        }

        [Route("/[controller]/{uid}/{action}")]
        public async Task<IActionResult> ChangePassword(int uid)
        {
            if (uid != await _sessionService.UidLoggedIn(Request.Cookies["sessionid"]))
                return RedirectToAction("Index", "Home");

            string sessionId = Request.Cookies["sessionid"];
            ViewBag.SessionId = sessionId;
            return View(new ChangePasswordVM());
        }

        [HttpPost("/[controller]/{uid}/{action}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(int uid, ChangePasswordVM cp)
        {
            if (uid != await _sessionService.UidLoggedIn(Request.Cookies["sessionid"]))
                return RedirectToAction("Index", "Home");

            string sessionId;

            if (!ModelState.IsValid)
            {
                sessionId = Request.Cookies["sessionid"];
                ViewBag.SessionId = sessionId;
                return View(cp);
            }
                

            try
            {
                using (var HttpClient = new HttpClient())
                {
                    string registerJson = System.Text.Json.JsonSerializer.Serialize(cp);
                    var content = new StringContent(registerJson, Encoding.UTF8, "application/json");

                    var response = await HttpClient.PutAsync($"{_endpointUrl}/{uid}/changepassword", content);
                    if (!response.IsSuccessStatusCode)
                    {
                        ModelState.AddModelError("", await response.Content.ReadAsStringAsync());
                        sessionId = Request.Cookies["sessionid"];
                        ViewBag.SessionId = sessionId;
                        return View(cp);
                    }
                }
            }
            catch (Exception e)
            {
                return View("Error", e);
            }

            sessionId = Request.Cookies["sessionid"];
            ViewBag.SessionId = sessionId;
            return RedirectToAction("Index", "Home");
        }
    }
}

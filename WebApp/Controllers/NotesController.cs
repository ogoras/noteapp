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
    [Route("/user/{uid}/[controller]/{action=Index}")]
    public class NotesController : Controller
    {
        public IConfiguration Configuration;
        public ISessionService _sessionService;

        public NotesController(IConfiguration configuration, ISessionService sessionService) : base()
        {
            Configuration = configuration;
            _sessionService = sessionService;
        }

        private string getEndpointUrl (int uid)
        {
            return Configuration["RestApiUrl"] + $"user/{uid}/notes";
        }

        // GET: NotesController
        public async Task<ActionResult> Index(int uid)
        {
            if (uid != await _sessionService.UidLoggedIn(Request.Cookies["sessionid"]))
                return RedirectToAction("Index", "Home");

            List<NoteWithIDVM> notesList;

            using (var response = await new HttpClient().GetAsync(getEndpointUrl(uid)))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                notesList = JsonConvert.DeserializeObject<List<NoteWithIDVM>>(apiResponse);
            }

            string sessionId = Request.Cookies["sessionid"];
            ViewBag.SessionId = sessionId;
            return View(notesList);
        }

        // GET: NotesController/Details/5
        public ActionResult Details(int id)
        {
            string sessionId = Request.Cookies["sessionid"];
            ViewBag.SessionId = sessionId;
            return View();
        }

        // GET: NotesController/Create
        public ActionResult Create()
        {
            string sessionId = Request.Cookies["sessionid"];
            ViewBag.SessionId = sessionId;
            return View(new NoteVM());
        }

        // POST: NotesController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(int uid, NoteVM n)
        {
            try
            {
                using (var HttpClient = new HttpClient())
                {
                    string userJson = System.Text.Json.JsonSerializer.Serialize(n);
                    var content = new StringContent(userJson, Encoding.UTF8, "application/json");

                    await HttpClient.PostAsync(getEndpointUrl(uid), content);
                }
            }
            catch (Exception e)
            {
                return View("Error", e);
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: NotesController/Edit/5
        public ActionResult Edit(int id)
        {
            string sessionId = Request.Cookies["sessionid"];
            ViewBag.SessionId = sessionId;
            return View();
        }

        // POST: NotesController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
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

        // GET: NotesController/Delete/5
        public ActionResult Delete(int id)
        {
            string sessionId = Request.Cookies["sessionid"];
            ViewBag.SessionId = sessionId;
            return View();
        }

        // POST: NotesController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
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
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using WebApp.Models;

namespace WebApp.Controllers
{
    [Route("/user/{uid}/[controller]/{action=Index}")]
    public class NotesController : Controller
    {
        public IConfiguration Configuration;

        public NotesController(IConfiguration configuration) : base()
        {
            Configuration = configuration;
        }

        private string getEndpointUrl (int uid)
        {
            return Configuration["RestApiUrl"] + $"user/{uid}/notes";
        }

        // GET: NotesController
        public async Task<ActionResult> Index(int uid)
        {
            List<NoteVM> notesList;

            using (var response = await new HttpClient().GetAsync(getEndpointUrl(uid)))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                notesList = JsonConvert.DeserializeObject<List<NoteVM>>(apiResponse);
            }

            return View(notesList);
        }

        // GET: NotesController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: NotesController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: NotesController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: NotesController/Edit/5
        public ActionResult Edit(int id)
        {
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
                return View();
            }
        }

        // GET: NotesController/Delete/5
        public ActionResult Delete(int id)
        {
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
                return View();
            }
        }
    }
}
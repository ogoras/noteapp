﻿using System;
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

namespace WebApp.Controllers
{
    public class UserController : Controller
    {
        public IConfiguration Configuration;
        private string _endpointUrl;

        public UserController(IConfiguration configuration) : base()
        {
            Configuration = configuration;
            _endpointUrl = Configuration["RestApiUrl"] + "user";
        }

        // GET: UserController
        public async Task<ActionResult> Index()
        {
            List<UserVM> userList;

            using (var response = await new HttpClient().GetAsync(_endpointUrl))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                userList = JsonConvert.DeserializeObject<List<UserVM>>(apiResponse);
            }

            return View(userList);
        }

        // GET: UserController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: UserController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UserController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(UserPost u)
        {
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
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: UserController/Edit/5
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

        // GET: UserController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: UserController/Delete/5
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
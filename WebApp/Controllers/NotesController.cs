using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
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
        public async Task<ActionResult> Details(int uid, int id)
        {
            if (uid != await _sessionService.UidLoggedIn(Request.Cookies["sessionid"]))
                return RedirectToAction("Index", "Home");

            SimpleNoteVM note;

            using (var response = await new HttpClient().GetAsync($"{getEndpointUrl(uid)}/{id}"))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                note = JsonConvert.DeserializeObject<SimpleNoteVM>(apiResponse);
            }

            string sessionId = Request.Cookies["sessionid"];
            ViewBag.SessionId = sessionId;
            return View(note);
        }

        public async Task<ActionResult> Decrypt(int uid, int id)
        {
            if (uid != await _sessionService.UidLoggedIn(Request.Cookies["sessionid"]))
                return RedirectToAction("Index", "Home");

            DecryptVM note;

            using (var response = await new HttpClient().GetAsync($"{getEndpointUrl(uid)}/{id}"))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                note = JsonConvert.DeserializeObject<DecryptVM>(apiResponse);
            }

            string sessionId = Request.Cookies["sessionid"];
            ViewBag.SessionId = sessionId;
            return View(note);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Decrypt(DecryptVM v)
        {
            SimpleNoteVM decryptedNote = decryptText(v);

            return View("Details", decryptedNote);
        }

        // GET: NotesController/Create
        public async Task<ActionResult> Create(int uid)
        {
            if (uid != await _sessionService.UidLoggedIn(Request.Cookies["sessionid"]))
                return RedirectToAction("Index", "Home");
            string sessionId = Request.Cookies["sessionid"];
            ViewBag.SessionId = sessionId;
            return View(new NoteVM());
        }

        // POST: NotesController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(int uid, NoteVM n)
        {
            if (uid != await _sessionService.UidLoggedIn(Request.Cookies["sessionid"]))
                return RedirectToAction("Index", "Home");

            if (n.Encrypted)
                n = encryptText(n);

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

        private SimpleNoteVM decryptText(DecryptVM v)
        {
            var arr = v.Text.Split('$');
            var IV = Convert.FromBase64String(arr[0]);
            var encrypted = Convert.FromBase64String(arr[1]);

            string plaintext = null;

            // Create an Aes object
            // with the specified key and IV.
            using (Aes aesAlg = Aes.Create())
            {
                string keyString = v.Key;
                byte[] keyBytes = new byte[keyString.Length];

                for (int i = 0; i < keyString.Length; i++)
                {
                    keyBytes[i] = (byte)keyString[i];
                }

                HashAlgorithm algorithm = new SHA256Managed();
                aesAlg.Key = algorithm.ComputeHash(keyBytes);
                aesAlg.IV = IV;

                // Create a decryptor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for decryption.
                using (MemoryStream msDecrypt = new MemoryStream(encrypted))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {

                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }

            return new SimpleNoteVM
            {
                Id = v.Id,
                Text = plaintext
            };
        }

        private NoteVM encryptText(NoteVM n)
        {
            string keyString = n.Key;
            Aes myAes = Aes.Create();

            byte[] keyBytes =
            new byte[keyString.Length];

            for (int i = 0; i < keyString.Length; i++)
            {
                keyBytes[i] = (byte)keyString[i];
            }

            HashAlgorithm algorithm = new SHA256Managed();

            myAes.Key = algorithm.ComputeHash(keyBytes);

            ICryptoTransform encryptor = myAes.CreateEncryptor(myAes.Key, myAes.IV);

            byte[] encrypted;

            // Create the streams used for encryption.
            using (MemoryStream msEncrypt = new MemoryStream())
            {
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {
                        //Write all data to the stream.
                        swEncrypt.Write(n.Text);
                    }
                    encrypted = msEncrypt.ToArray();
                }
            }

            n.Text = $"{Convert.ToBase64String(myAes.IV)}${Convert.ToBase64String(encrypted)}";
            return n;
        }

        // GET: NotesController/Edit/5
        [Route("{id}")]
        public async Task<ActionResult> Edit(int uid, int id)
        {
            if (uid != await _sessionService.UidLoggedIn(Request.Cookies["sessionid"]))
                return RedirectToAction("Index", "Home");

            NoteWithIDVM note;

            try
            {
                using (var response = await new HttpClient().GetAsync($"{getEndpointUrl(uid)}/{id}"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    note = JsonConvert.DeserializeObject<NoteWithIDVM>(apiResponse);
                }
            }
            catch (Exception e)
            {
                return View("Error", e);
            }

            string sessionId = Request.Cookies["sessionid"];
            ViewBag.SessionId = sessionId;
            return View(note);
        }

        // POST: NotesController/Edit/5
        [HttpPost("{id}")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int uid, int id, NoteWithIDVM n)
        {
            if (uid != await _sessionService.UidLoggedIn(Request.Cookies["sessionid"]))
                return RedirectToAction("Index", "Home");
            try
            {
                using (var httpClient = new HttpClient())
                {
                    string noteString = System.Text.Json.JsonSerializer.Serialize(n);
                    var content = new StringContent(noteString, Encoding.UTF8, "application/json");

                    await httpClient.PutAsync($"{getEndpointUrl(uid)}/{id}", content);
                }
            }
            catch (Exception e)
            {
                return View("Error", e);
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: NotesController/Delete/5
        public async Task<ActionResult> Delete(int uid, int id)
        {
            if (uid != await _sessionService.UidLoggedIn(Request.Cookies["sessionid"]))
                return RedirectToAction("Index", "Home");
            string sessionId = Request.Cookies["sessionid"];
            ViewBag.SessionId = sessionId;
            return View();
        }

        // POST: NotesController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int uid, int id, IFormCollection collection)
        {
            if (uid != await _sessionService.UidLoggedIn(Request.Cookies["sessionid"]))
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

        [Route("/[controller]/{action}")]
        public async Task<ActionResult> Public()
        {
            List<PublicNoteVM> notesList;

            using (var response = await new HttpClient().GetAsync(Configuration["RestApiUrl"] + "notes/public"))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                notesList = JsonConvert.DeserializeObject<List<PublicNoteVM>>(apiResponse);
            }

            string sessionId = Request.Cookies["sessionid"];
            ViewBag.SessionId = sessionId;
            return View(notesList ?? new List<PublicNoteVM>());
        }

        public async Task<ActionResult> Encrypted(int uid)
        {
            if (uid != await _sessionService.UidLoggedIn(Request.Cookies["sessionid"]))
                return RedirectToAction("Index", "Home");

            List<SimpleNoteVM> notesList;

            using (var response = await new HttpClient().GetAsync($"{getEndpointUrl(uid)}/encrypted"))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                notesList = JsonConvert.DeserializeObject<List<SimpleNoteVM>>(apiResponse);
            }

            string sessionId = Request.Cookies["sessionid"];
            ViewBag.SessionId = sessionId;
            return View(notesList ?? new List<SimpleNoteVM>());
        }
    }
}

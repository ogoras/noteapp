using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.DTO;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPI.Controllers
{
    [Route("user/{uid}/[controller]")]
    [ApiController]
    public class NotesController : Controller
    {
        private readonly INoteService _noteService;
        public NotesController(INoteService noteService)
        {
            _noteService = noteService;
        }

        // GET: api/<NotesController>
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Get(int uid)
        {
            IEnumerable<NoteDTOwithID> notes = await _noteService.ReadAll(uid);
            return Json(notes);
        }

        [Authorize]
        // GET api/<NotesController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult> Get(int uid, int id)
        {
            NoteDTOwithID note = await _noteService.Read(id);
            return Json(note);
        }

        [Authorize]
        // POST api/<NotesController>
        [HttpPost]
        public async Task<IActionResult> Post(int uid, [FromBody] NoteDTO note)
        {
            if (note.Text == null)
                return BadRequest();
            try
            {
                await _noteService.Create(uid, note);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok();
        }

        [Authorize]
        // PUT api/<NotesController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int uid, int id, [FromBody] NoteDTO note)
        {
            if (note.Text == null)
                return BadRequest();
            try
            {
                await _noteService.Update(uid, id, note);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok();
        }

        [Authorize]
        // DELETE api/<NotesController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int uid, int id)
        {
            try
            {
                await _noteService.Delete(uid, id);
            }
            catch (NullReferenceException ex)
            {
                return NotFound();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok();
        }

        [HttpGet("/[controller]/public")]
        public async Task<IActionResult> GetPublic()
        {
            IEnumerable<NoteDTOwithID> notes = await _noteService.ReadPublic();
            return Json(notes);
        }

        [HttpGet("/[controller]/{id}")]
        public async Task<IActionResult> GetPublic(int id)
        {
            try
            {
                NoteDTOwithID notes = await _noteService.ReadPublic(id);
                return Json(notes);
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized("The note with specified ID is not public");
            }
        }

        [Authorize]
        [HttpGet("encrypted")]
        public async Task<IActionResult> GetEncrypted(int uid)
        {
            IEnumerable<NoteDTOwithID> notes = await _noteService.ReadEncrypted(uid);
            return Json(notes);
        }
    }
}

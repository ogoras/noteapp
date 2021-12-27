using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.DTO;
using Infrastructure.Services;
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
        [HttpGet]
        public async Task<IActionResult> Get(int uid)
        {
            IEnumerable<NoteDTOwithID> notes = await _noteService.ReadAll(uid);
            return Json(notes);
        }

        // GET api/<NotesController>/5
        [HttpGet("{id}")]
        public string Get(int uid, int id)
        {
            return "value";
        }

        // POST api/<NotesController>
        [HttpPost]
        public async Task<IActionResult> Post(int uid, [FromBody] NoteDTO note)
        {
            if (note.Text == null)
                return BadRequest();
            try
            {
                await _noteService.CreatePrivate(uid, note);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok();
        }

        // PUT api/<NotesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

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
    }
}

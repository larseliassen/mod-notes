using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ModNotes.DbModels;


namespace ModNotes.Controllers
{
    [Route("api/[controller]")]
    public class NotesController : Controller
    {

		readonly NotesDbContext _context;

        public NotesController(NotesDbContext context)
		{
			_context = context;
		}

		// GET api/values/
        [ProducesResponseType(typeof(Note[]), 200)]
		[HttpGet]
        public Note[] Get()
        {
            return _context.Notes.ToArray();				
        }

		// POST api/values
		[ProducesResponseType(typeof(Note), 201)]
        [HttpPost]
        public IActionResult Post([FromBody]Note note)           
        {
            _context.Notes.Add(note);
            _context.SaveChanges();
            return new CreatedResult(Url.RouteUrl("GetNote", new { id = note.Id }), note);
        }

		// GET api/values/
		[ProducesResponseType(typeof(Note), 200)]
		[ProducesResponseType(typeof(void), 404)]
        [HttpGet("{id}", Name = "GetNote")]
        public IActionResult GetNote(int id)
		{
            var note =_context.Notes.Find(id);
            if (note == null)
            {
                return new NotFoundResult();
            }
            else
            {
                return new JsonResult(note);
            }
		}

        // PUT api/values/5
        [ProducesResponseType(typeof(Note), 200)]
        [ProducesResponseType(typeof(void), 404)]
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]Note value)
        {
            var note = _context.Notes.Find(id);
            if (note == null)
            {
                return new NotFoundResult();
            }
            else
            {
                note.Content = value.Content;
                note.Title = value.Title;
                _context.SaveChanges();
                return new OkObjectResult(note);
            }		           
        }

		// DELETE api/values/5
		[ProducesResponseType(typeof(void), 200)]
		[ProducesResponseType(typeof(void), 404)]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var note = _context.Notes.Find(id);
			if (note == null)
			{
				return new NotFoundResult();
			}
			else
			{
				_context.Notes.Remove(note);
				_context.SaveChanges();
                return new OkResult();
			}
        }
    }
}

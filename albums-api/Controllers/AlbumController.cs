using Microsoft.AspNetCore.Mvc;
using albums_api.Models;

namespace albums_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AlbumController : ControllerBase
    {
        // GET: api/Album
        [HttpGet]
        public ActionResult<IEnumerable<Album>> Get()
        {
            var albums = Album.GetAll();
            return Ok(albums);
        }

        // GET: api/Album/5
        [HttpGet("{id}")]
        public ActionResult<Album> Get(int id)
        {
            var album = Album.GetById(id);
            if (album == null)
            {
                return NotFound($"Album with ID {id} not found");
            }
            return Ok(album);
        }

        // GET: api/Album/year/2023
        [HttpGet("year/{year}")]
        public ActionResult<IEnumerable<Album>> GetByYear(int year)
        {
            var albums = Album.GetByYear(year);
            return Ok(albums);
        }

        // POST: api/Album
        [HttpPost]
        public ActionResult<Album> Post([FromBody] Album album)
        {
            if (album == null)
            {
                return BadRequest("Album data is required");
            }

            if (string.IsNullOrWhiteSpace(album.Title) || string.IsNullOrWhiteSpace(album.Artist))
            {
                return BadRequest("Title and Artist are required fields");
            }

            var createdAlbum = Album.Create(album);
            return CreatedAtAction(nameof(Get), new { id = createdAlbum.Id }, createdAlbum);
        }

        // PUT: api/Album/5
        [HttpPut("{id}")]
        public ActionResult<Album> Put(int id, [FromBody] Album album)
        {
            if (album == null)
            {
                return BadRequest("Album data is required");
            }

            if (string.IsNullOrWhiteSpace(album.Title) || string.IsNullOrWhiteSpace(album.Artist))
            {
                return BadRequest("Title and Artist are required fields");
            }

            var updatedAlbum = Album.Update(id, album);
            if (updatedAlbum == null)
            {
                return NotFound($"Album with ID {id} not found");
            }

            return Ok(updatedAlbum);
        }

        // DELETE: api/Album/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var deleted = Album.Delete(id);
            if (!deleted)
            {
                return NotFound($"Album with ID {id} not found");
            }

            return NoContent();
        }
    }
}
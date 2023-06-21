using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Videoteca_API.Data;
using Videoteca_API.Models;

namespace Videoteca_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EpisodeServiceController : Controller
    {
        private VideotecaContext db = new VideotecaContext();
        
        // POSTMovie api/<EpisodeServiceController>
        [HttpPost]
        public ActionResult PostEpisode([FromBody] Episode episode)
        {

            try
            {

                db.Episodes.Add(episode);
                db.SaveChanges();

                var result = Task.Run(() => db.Database
                .ExecuteSqlRaw(@"exec dbo.updateNumEpisodes @id_serie", new SqlParameter("@id_serie", episode.movies_series_id)));

                result.Wait();


                return Ok(db.MoviesAndSeries);


            }
            catch (Exception ex)
            {
                return BadRequest("Error registering the episode: " + ex);
            }
        }
    }
}

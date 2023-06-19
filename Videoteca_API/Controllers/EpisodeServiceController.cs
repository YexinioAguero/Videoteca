using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Videoteca_API.Data;

namespace Videoteca_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EpisodeServiceController : Controller
    {
        private VideotecaContext db = new VideotecaContext();

        [HttpPost]
        public ActionResult PostEpisode([FromBody]  )
        {

            try
            {

                db.MoviesAndSeries.Add(movisData);
                db.SaveChanges();


                var movies = new List<MoviesAndSeries>();

                movies = db.MoviesAndSeries.FromSqlRaw(@"exec dbo.GetMovieDataForTitle @title", new SqlParameter("@title", movisData.title)).ToList();

                var movie = movies.FirstOrDefault();

                var idMovie = movie.id;


                return Ok(db.MoviesAndSeries);


            }
            catch (Exception ex)
            {
                return BadRequest("Error registering the movie: " + ex);
            }
        }


    }
}

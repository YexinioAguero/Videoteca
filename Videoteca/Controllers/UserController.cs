using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Videoteca.Data;
using Videoteca.Models;


namespace Videoteca.Controllers
{
    [Authorize(Roles = "user")]
    public class UserController : Controller
    {
        // GET: UserController
        private VideotecaContext vbd = new VideotecaContext();
        public ActionResult Index()
        {
            var list = new List<Movie_S_Genre>();
            var movie = new List<MoviesAndSeries>();
            var genres = new List<Genre>();

            genres = vbd.Genres.FromSqlRaw("exec dbo.GetGenre").ToList();

            foreach (var g in genres)
            {
                movie = vbd.MoviesAndSeries.FromSqlRaw("exec dbo.[Get" + g.genre_name + "]").ToList();
               list.Add(new Movie_S_Genre() { genre = g.genre_name, movies = movie });
                
            }

            return View(list);
        }

        public ActionResult InfoMovie(int id)
        {
            var list = new List<MovieInfo>();
            var movie = new List<MoviesAndSeries>();
            var genre =  new List<Genre>();
            var actor = new List<Actor>();
            var movieInfo = new MoviesAndSeries();

            movie = vbd.MoviesAndSeries.FromSqlRaw(@"exec dbo.GetMovie @id", new SqlParameter("@id", id)).ToList();
            actor = vbd.Actors.FromSqlRaw(@"exec dbo.GetActorsMovie @id", new SqlParameter("@id", id)).ToList();
            genre = vbd.Genres.FromSqlRaw(@"exec dbo.GetGenreMovie @id", new SqlParameter("@id", id)).ToList();

            foreach (var m in movie)
            {
                movieInfo = new MoviesAndSeries
                {
                    id = m.id,
                    title = m.title,
                    synopsis = m.synopsis,
                    release_year = m.release_year,
                    classification = m.classification,
                    director = m.director,
                    num_episodes = m.num_episodes,
                    num_seasons = m.num_seasons,
                    movie_url = m.movie_url,
                    date_added = m.date_added

                };
            }
            list.Add(new MovieInfo() { movie = movieInfo, actors = actor, genres = genre });

            return View(list);
        }


    }
}

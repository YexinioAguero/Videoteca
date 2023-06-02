using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Videoteca.Data;
using Videoteca.Models;
using Videoteca_Test_API_data.Models;

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

                if (g.genre_name != "Science Fiction")
                {
                    movie = vbd.MoviesAndSeries.FromSqlRaw("exec dbo.[Get" + g.genre_name + "]").ToList();

                    list.Add(new Movie_S_Genre() { genre = g.genre_name, movies = movie });
                }
            }

            return View(list);
        }




    }
}

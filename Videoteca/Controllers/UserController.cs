using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;
using System.Data;
using Videoteca.Data;
using Videoteca.Models;
using Videoteca.Models.DTO;
using static System.Net.Mime.MediaTypeNames;

namespace Videoteca.Controllers
{
    [Authorize(Roles = "user")]
    public class UserController : Controller
    {
        // GET: UserController
        private readonly UserManager<ApplicationUser> _userManager;

        public UserController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }


        private VideotecaContext vbd = new VideotecaContext();
        public ActionResult Index()
        {
            var list = new List<Movie_S_Genre>();
            var movie = new List<MoviesAndSeries>();
            var genres = new List<Genre>();

            genres = vbd.Genres.FromSqlRaw("exec dbo.GetGenre").ToList();

            foreach (var g in genres)
            {
                movie = vbd.MoviesAndSeries.FromSqlRaw("exec dbo.[GetMoviesForGenre] @id", new SqlParameter("@id", g.genre_id)).ToList();
                list.Add(new Movie_S_Genre() { genre = g.genre_name, movies = movie });

            }

            return View(list);
        }

        public ActionResult InfoMovie(int id)
        {
            var list = new List<MovieInfo>();
            var movie = new List<MoviesAndSeries>();
            var genre = new List<Genre>();
            var actor = new List<Actor>();
            var movieInfo = new MoviesAndSeries();
            var userInfo = new List<AspNetUser>();
            var user = new User();
            string userId = "";

            userId = _userManager.GetUserId(HttpContext.User);
            if (userId != null)
            {
                userInfo = vbd.AspNetUsers.FromSqlRaw(@"exec dbo.GetUser @id", new SqlParameter("@id", userId)).ToList();
                foreach (var u in userInfo)
                {
                    user = new User
                    {
                        user_id = userId,
                        username = u.UserName,
                        email = u.Email
                    };
                }
            }
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
                    duration = m.duration,
                    director = m.director,
                    num_episodes = m.num_episodes,
                    num_seasons = m.num_seasons,
                    movie_url = m.movie_url,
                    date_added = m.date_added

                };
            }
            list.Add(new MovieInfo() { user = user, movie = movieInfo, actors = actor, genres = genre });

            return View(list);
        }
        [HttpPost]
        public ActionResult SetRate(int value, int id)
        {
            var user = new User();
            string userId, userName = "";
            var userInfo = new List<AspNetUser>();

            userId = _userManager.GetUserId(HttpContext.User);
            if (userId != null)
            {
                userInfo = vbd.AspNetUsers.FromSqlRaw(@"exec dbo.GetUser @id", new SqlParameter("@id", userId)).ToList();
                foreach (var u in userInfo)
                {
                    userName = u.UserName;
                }
            }

            var parameter = new List<SqlParameter>();
            parameter.Add(new SqlParameter("@movies_series_id", id));
            parameter.Add(new SqlParameter("@userName", userName));
            parameter.Add(new SqlParameter("@rating", value));

            vbd.Database.ExecuteSqlRaw(@"exec dbo.RatingI @movies_series_id, @userName, @rating"
            , parameter.ToArray());
            vbd.SaveChanges();

            return Json(new { mensaje = value });
        }
        [HttpGet]
        public ActionResult GetRate(int id)
        {
            var ratings = new List<Rating>();
            int count = 0;
            string rate = null;

            ratings = vbd.Ratings.FromSqlRaw(@"exec dbo.GetRate @id", new SqlParameter("@id", id)).ToList();


            foreach(var r in ratings)
            {
                if(r.rating1 != null)
                {
                    count = (int)(count + r.rating1);
                }
                else
                {
                    count = 0;
                }
                
            }
            if (count != 0)
            {
                count = count / ratings.Count();
            }

            rate = count.ToString();


            return Json(rate.ToJson());
        }

        [HttpPost]
        public ActionResult SetComment(string text, int id)
        {
            var user = new User();
            string userId, userName = "";
            var userInfo = new List<AspNetUser>();

            userId = _userManager.GetUserId(HttpContext.User);
            if (userId != null)
            {
                userInfo = vbd.AspNetUsers.FromSqlRaw(@"exec dbo.GetUser @id", new SqlParameter("@id", userId)).ToList();
                foreach (var u in userInfo)
                {
                    userName = u.UserName;
                }
            }

            vbd.Add(new Comment() { userName = userName, comment1 = text, movies_series_id = id ,dateC = DateTime.Today});
            vbd.SaveChanges();

            return Json(new { mensaje = "Datos recibidos correctamente" });
        }

        public ActionResult GetComment(int id)
        {
            var comments = new List<Comment>();

            comments = vbd.Comments.FromSqlRaw(@"exec dbo.GetComments @id", new SqlParameter("@id", id)).ToList();

            return PartialView("ViewComment", comments);
       }

        public ActionResult GetEpisodes(int id)
        {
            var episodes = new List<Episode>();

            episodes = vbd.Episodes.FromSqlRaw(@"exec dbo.GetEpisodes @id", new SqlParameter("@id", id)).ToList();

            return PartialView("ViewEpisodes", episodes);
        }

      

    }
}

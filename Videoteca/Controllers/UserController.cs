using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;
using System.Collections.Generic;
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

        //GET: Search Movies
        public async Task<IActionResult> MovieSearch(string movieGenre, string searchString)
        {
          

            // Use LINQ to get list of genres.
            IQueryable<string> genres = from m in vbd.Genres
                                            orderby m.genre_id
                                            select m.genre_name;

            var movies = from m in vbd.MoviesAndSeries
                         select m;


            if (!string.IsNullOrEmpty(searchString))
            {
                movies = movies.Where(s => s.title!.Contains(searchString));
            }

            if (!string.IsNullOrEmpty(movieGenre))
            {
                movies = movies.Where(s => s.title!.Contains(searchString));
            }

            var movieGenreVM = new MovieGenreViewModel
            {
                Genres = new SelectList(await genres.Distinct().ToListAsync()),
                Movies = await movies.ToListAsync()
            };

            return View(movieGenreVM);


        }

        public ActionResult InfoMovie(int id)
        {
            var list = new List<MovieInfo>();
            var movie = new List<MoviesAndSeries>();
            var genre = new List<Genre>();
            string genresS = null;
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

            vbd.Add(new Comment() { userName = userName, comment1=text, movies_series_id = id, dateC = DateTime.UtcNow});
            vbd.SaveChanges();

            return Json(new { mensaje = "Datos recibidos correctamente" });
        }

        public ActionResult GetComment(int id)
        {
            var comments = new List<Comment>();
            var commentUser = new List<CommentUser>();
            var cUser = new CommentUser();
            string img,pImg = null;
            comments = vbd.Comments.FromSqlRaw(@"exec dbo.GetComments @id", new SqlParameter("@id", id)).ToList();

            foreach (var c in comments)
            {
                var user = 
                    vbd.AspNetUsers.FromSqlRaw(@"exec dbo.GetUserImg @username", new SqlParameter("@username", c.userName)).ToList();
                foreach(var u in user)
                {
                    pImg = u.ProfilePicture;
                }
                cUser = new CommentUser
                {
                    comment_id = c.comment_id,
                    comment1 = c.comment1,
                    dateC = c.dateC,
                    userName = c.userName,
                    movies_series_id = id,
                    image = pImg
                };
                commentUser.Add(cUser);
            }

            return PartialView("ViewComment", commentUser);
       }

        public ActionResult GetEpisodes(int id)
        {
            var episodes = new List<Episode>();

            episodes = vbd.Episodes.FromSqlRaw(@"exec dbo.GetEpisodes @id", new SqlParameter("@id", id)).ToList();

            return PartialView("ViewEpisodes", episodes);
        }

        //Edit profile
        // GET: UserController/Edit/5
        public ActionResult Edit(string id)
        {
            var person = vbd.AspNetUsers.Find(id);

            return View(person);
        }

        // POST: PersonController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(string id, AspNetUser person, IFormFile photo)
        {
            try
            {
                vbd.Update(person);
                if (photo != null && photo.Length > 0)
                {
                    // Leer los datos de la foto y convertirlos a un arreglo de bytes
                    using (var memoryStream = new MemoryStream())
                    {
                        photo.CopyTo(memoryStream);
                        byte[] photoData = memoryStream.ToArray();

                        // Guardar la foto en la tabla "Image"
                        var image = new profilePicture
                        {
                            image = photoData
                        };

                        vbd.profilePictures.Add(image);
                        vbd.SaveChanges();

                        // Actualizar el campo "ProfilePicture" en la tabla "AspNetUsers" con la ruta de la foto guardada en la tabla "Image"
                        person.ProfilePicture = image.id.ToString();
                    }
                }


                ViewBag.Message = "Se realizó de manera correcta";
                vbd.SaveChanges();
                ViewBag.Message = new MessagePack { Text = "Se realizo de manera correcta", Tipo = Tipo.message.success.ToString() };
                return RedirectToAction("Edit");
            }
            catch
            {
                ViewBag.Message = new MessagePack { Text = "No se realizo de manera correcta", Tipo = Tipo.message.danger.ToString() };

                return View();
            }
        }


        public ActionResult GetProfilePicture(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var image = vbd.profilePictures.Find(int.Parse(id));
            if (image == null)
            {
                return NotFound();
            }

            return File(image.image, "image/jpeg"); // Devuelve la imagen como un archivo de tipo "image/jpeg"
        }



    }
}

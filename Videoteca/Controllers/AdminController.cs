using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data;
using Videoteca.Data;
using Videoteca.Models;

namespace Videoteca.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        private VideotecaContext db = new VideotecaContext();

        //Get: AdminController/Index
        public IActionResult Index()
        {
            var MoviesList = new List<MoviesAndSeries>();

            MoviesList = db.MoviesAndSeries.FromSqlRaw("exec dbo.GetMoviesAndSerie").ToList();

            
                return View(MoviesList);
            

            
        }

        public IActionResult Details_MoviesAndSeries(int id) {

            var MovieSearch = new List<MoviesAndSeries>();
            var parameter = new List<SqlParameter>();
            parameter.Add(new SqlParameter("@id", id));


            MovieSearch = db.MoviesAndSeries.FromSqlRaw("exec dbo.SearchMoviesAndSerie @id", parameter.ToArray()).ToList();


            return View(MovieSearch.FirstOrDefault());
        }
        // GET: AdminController/Create
        public ActionResult Create_MovieAndSerie()
        {

            return View();
        }

        //POST_AdminController/Create_MovieAndSerie
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create_MovieAndSerie(MoviesAndSeries movisData) 
        {
            try
            {

                var parameter = new List<SqlParameter>();
                parameter.Add(new SqlParameter("@title", movisData.title));
                parameter.Add(new SqlParameter("@synopsis", movisData.synopsis));
                parameter.Add(new SqlParameter("@releaseYear", movisData.release_year));
                parameter.Add(new SqlParameter("@duration", movisData.duration));
                parameter.Add(new SqlParameter("@classification", movisData.classification));
                parameter.Add(new SqlParameter("@director", movisData.director));
                parameter.Add(new SqlParameter("@num_seasons", movisData.num_seasons));
                parameter.Add(new SqlParameter("@num_episodes", movisData.num_episodes));
                parameter.Add(new SqlParameter("@episode_duration", movisData.episode_duration));
                parameter.Add(new SqlParameter("@movie_url", movisData.movie_url));
                parameter.Add(new SqlParameter("@date_added", movisData.date_added));


                var result = Task.Run(() => db.Database
                .ExecuteSqlRaw(@"exec dbo.InsertMovieAndSerie @title, @synopsis, @releaseYear, @duration,
                @classification, @director, @num_seasons, @num_episodes, @episode_duration, @movie_url, @date_added",
                parameter.ToArray()));

                result.Wait();

                db.SaveChanges();
                //db.MoviesAndSeries.Add(movisData);



                ViewBag.Message = new Models.MessagePack()
                {
                    Text = "The Movie Or Serie Was Register",
                    Tipo = message.success.ToString()
                };

                return RedirectToAction("Index");

                
            }
            catch (Exception ex)
            {
                ViewBag.Message = new Models.MessagePack()
                {
                    Text = "The Movie Or Serie Can't Was Register",
                    Tipo = message.danger.ToString()
                };

                return RedirectToAction("Create_MovieAndSerie");
            }
        }


        //Get: AdminController/Edit_MovieAndSerie
        public ActionResult Edit_MovieAndSerie(int id) 
        {
            var MovieSearch = new List<MoviesAndSeries>();
            var parameter = new List<SqlParameter>();
            parameter.Add(new SqlParameter("@id", id));


            MovieSearch = db.MoviesAndSeries.FromSqlRaw("exec dbo.SearchMoviesAndSerie @id", parameter.ToArray()).ToList();


            return View(MovieSearch.FirstOrDefault());
        }

        //POST_AdminController/Edit_MovieAndSerie
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit_MovieAndSerie(int id, MoviesAndSeries movisData)
        {

            try
            {
                var MovieEdit = new List<MoviesAndSeries>();
                var parameter = new List<SqlParameter>();
                parameter.Add(new SqlParameter("@id", id));
                parameter.Add(new SqlParameter("@title", movisData.title));
                parameter.Add(new SqlParameter("@synopsis", movisData.synopsis));
                parameter.Add(new SqlParameter("@releaseYear", movisData.release_year));
                parameter.Add(new SqlParameter("@duration", movisData.duration));
                parameter.Add(new SqlParameter("@classification", movisData.classification));
                parameter.Add(new SqlParameter("@director", movisData.director));
                parameter.Add(new SqlParameter("@num_seasons", movisData.num_seasons));
                parameter.Add(new SqlParameter("@num_episodes", movisData.num_episodes));
                parameter.Add(new SqlParameter("@episode_duration", movisData.episode_duration));
                parameter.Add(new SqlParameter("@movie_url", movisData.movie_url));
                parameter.Add(new SqlParameter("@date_added", movisData.date_added));


                var result = Task.Run(() => db.Database
                .ExecuteSqlRaw(@"exec dbo.EditMoviesAndSerie @id, @title, @synopsis, @releaseYear, @duration,
            @classification, @director, @num_seasons, @num_episodes, @episode_duration, @movie_url, @date_added",
                parameter.ToArray()));

                result.Wait();

                db.SaveChanges();

                ViewBag.Message = new Models.MessagePack()
                {
                    Text = "The Movie Or Serie Was Register",
                    Tipo = message.success.ToString()
                };

                return RedirectToAction("index");
            }
            catch (Exception ex) 
            {
                ViewBag.Message = new Models.MessagePack()
                {
                    Text = "The Movie Or Serie Can't Was Register",
                    Tipo = message.danger.ToString()
                };

                return View();
            }
            
        }

        //Get: AdminController/Delete_MovieAndSerie
        public ActionResult Delete_MovieAndSerie(int id) 
        {
            var MovieSearch = new List<MoviesAndSeries>();
            var parameter = new List<SqlParameter>();
            parameter.Add(new SqlParameter("@id", id));


            MovieSearch = db.MoviesAndSeries.FromSqlRaw("exec dbo.SearchMoviesAndSerie @id", parameter.ToArray()).ToList();


            return View(MovieSearch.FirstOrDefault());

        }

        //POST_AdminController/Edit_MovieAndSerie
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete_MovieAndSerie(int id, MoviesAndSeries MovieData) 
        {
            try
            {
                var parameter = new List<SqlParameter>();
                parameter.Add(new SqlParameter("@Id", id));

                var MovieList = new List<MoviesAndSeries>();


                var result = Task.Run(() => db.Database
                .ExecuteSqlRaw(@"exec dbo.DeleteMoviesAndSerie @Id", parameter.ToArray()));
                result.Wait();

                db.SaveChanges();

                return RedirectToAction("Index");

            }
            catch (Exception ex) 
            {
                ViewBag.Message = new Models.MessagePack()
                {
                    Text = "The Movie Or Serie Can't Was Register",
                    Tipo = message.danger.ToString()
                };

                return View();
            }

            

           
        }
    }
}

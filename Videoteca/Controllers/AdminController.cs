using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;
using System;
using System.Data;
using Videoteca.Data;
using Videoteca.Models;

namespace Videoteca.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        //Llamamos al contexto
        private VideotecaContext db = new VideotecaContext();

        //Get: AdminController/View_MoviesAndSeries
        public IActionResult View_MoviesAndSeries()
        {
            //Instanciamos los modelos que usaremos 
            var list = new List<Movie_S_Genre>();
            var movie = new List<MoviesAndSeries>();
            var genres = new List<Genre>();
            //Optenemos los generos de la base de datos con un procedimiento almacenado
            genres = db.Genres.FromSqlRaw("exec dbo.GetGenre").ToList();

            //Optenemos las p
            foreach (var g in genres)
            {
                movie = db.MoviesAndSeries.FromSqlRaw("exec dbo.[GetMoviesForGenre] @id", new SqlParameter("@id", g.genre_id)).ToList();
                list.Add(new Movie_S_Genre() { genre = g.genre_name, movies = movie });

            }

            return View(list);



        }

        public IActionResult Details_MoviesAndSeries(int id)
        {

            var list = new List<MovieInfo>();
            var movie = new List<MoviesAndSeries>();
            var genre = new List<Genre>();
            var actor = new List<Actor>();
            var movieInfo = new MoviesAndSeries();
            var userInfo = new List<AspNetUser>();
            var comments = new List<Comment>();

            
            movie = db.MoviesAndSeries.FromSqlRaw(@"exec dbo.GetMovie @id", new SqlParameter("@id", id)).ToList();
            actor = db.Actors.FromSqlRaw(@"exec dbo.GetActorsMovie @id", new SqlParameter("@id", id)).ToList();
            genre = db.Genres.FromSqlRaw(@"exec dbo.GetGenreMovie @id", new SqlParameter("@id", id)).ToList();
            comments = db.Comments.FromSqlRaw(@"exec dbo.GetComments @id", new SqlParameter("@id", id)).ToList();

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
            list.Add(new MovieInfo() {movie = movieInfo, actors = actor, genres = genre});

            return View(list);
        }
        // GET: AdminController/Create
        public ActionResult Create_Movie()
        {
            return View();
        }

        //POST_AdminController/Create_Movie
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create_Movie(MoviesAndSeries movisData)
        {
            try
            {

                var moviesSearch = new List<MoviesAndSeries>();

                moviesSearch = db.MoviesAndSeries.FromSqlRaw(@"exec dbo.GetMovieDataForTitle @title", new SqlParameter("@title", movisData.title)).ToList();

                if (moviesSearch.Count > 0)
                {
                    ViewBag.Message = new Models.MessagePack()
                    {
                        Text = "The Movie Title Was Exist",
                        Tipo = message.danger.ToString()
                    };

                    return View();
                }
                else
                {


                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri("https://localhost:7220/api/");
                        //HTTP GET
                        var PostTask = client.PostAsJsonAsync("MovieService/", movisData);
                        PostTask.Wait();

                        var result = PostTask.Result;

                        if (result.IsSuccessStatusCode)
                        {
                            ViewBag.Message = new Models.MessagePack()
                            {
                                Text = "The Movie Was Register",
                                Tipo = message.success.ToString()
                            };

                            var movies = new List<MoviesAndSeries>();

                            movies = db.MoviesAndSeries.FromSqlRaw(@"exec dbo.GetMovieDataForTitle @title", new SqlParameter("@title", movisData.title)).ToList();

                            var movie = movies.FirstOrDefault();

                            var idMovie = movie.id;

                            return RedirectToAction("Details_MoviesAndSeries", new { id = idMovie });

                        }
                        else
                        {
                            ViewBag.Message = new Models.MessagePack()
                            {
                                Text = "Problem was register Movie",
                                Tipo = message.danger.ToString()
                            };

                            return View();
                        }
                    }


                }


            }
            catch (Exception ex)
            {
                ViewBag.Message = new Models.MessagePack()
                {
                    Text = "Error registering the movie: " + ex,
                    Tipo = message.danger.ToString()
                };

                return View();
            }
        }

        // GET: AdminController/Create
        public ActionResult Create_Serie()
        {
            return View();
        }

        //POST_AdminController/Create_Serie
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create_Serie(MoviesAndSeries movisData)
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
                parameter.Add(new SqlParameter("@movie_url", movisData.movie_url));
                parameter.Add(new SqlParameter("@date_added", movisData.date_added));


                var moviesSearch = new List<MoviesAndSeries>();

                moviesSearch = db.MoviesAndSeries.FromSqlRaw(@"exec dbo.GetMovieDataForTitle @title", new SqlParameter("@title", movisData.title)).ToList();

                if (moviesSearch.Count > 0)
                {
                    ViewBag.Message = new Models.MessagePack()
                    {
                        Text = "The Series Title Was Exist",
                        Tipo = message.danger.ToString()
                    };

                    return View();
                }
                else
                {

                    db.MoviesAndSeries.Add(movisData);
                    db.SaveChanges();

                    ViewBag.Message = new Models.MessagePack()
                    {
                        Text = "The Series Was Register",
                        Tipo = message.success.ToString()
                    };


                    var movies = new List<MoviesAndSeries>();

                    movies = db.MoviesAndSeries.FromSqlRaw(@"exec dbo.GetMovieDataForTitle @title", new SqlParameter("@title", movisData.title)).ToList();

                    var movie = movies.FirstOrDefault();

                    var idMovie = movie.id;

                    return RedirectToAction("Details_MoviesAndSeries", new { id = idMovie });
                }


            }
            catch (Exception ex)
            {
                ViewBag.Message = new Models.MessagePack()
                {
                    Text = "Error registering the series: " + ex,
                    Tipo = message.danger.ToString()
                };

                return View();
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
        public IActionResult Edit_MovieAndSerie(MoviesAndSeries movisData)
        {

            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://localhost:7220/api/");
                    //HTTP GET
                    var responseTask = client.PutAsJsonAsync("MovieService/" + movisData.id, movisData);
                    responseTask.Wait();

                    var result = responseTask.Result;

                    if (result.IsSuccessStatusCode)
                    {
                        ViewBag.Message = new Models.MessagePack()
                        {
                            Text = "The movie or series was update correctly.",
                            Tipo = message.success.ToString()
                        };
                    }
                    else
                    {
                        ViewBag.Message = new Models.MessagePack()
                        {
                            Text = "The series or movie was not modified correctly:",
                            Tipo = message.danger.ToString()
                        };

                        return View(result);
                    }


                    return RedirectToAction("Details_MoviesAndSeries", new { id = movisData.id });
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = new Models.MessagePack()
                {
                    Text = "Error The series or movie was not modified correctly: " + ex,
                    Tipo = message.danger.ToString()
                };

                return View(movisData.id);
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

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://localhost:7220/api/");
                    //HTTP GET
                    var responseTask = client.DeleteAsync("MovieService/" + id);
                    responseTask.Wait();

                    var result = responseTask.Result;

                    if (result.IsSuccessStatusCode)
                    {
                        ViewBag.Message = new Models.MessagePack()
                        {
                            Text = "The movie or series was deleted correctly.",
                            Tipo = message.danger.ToString()
                        };
                    }
                    else
                    {
                        ViewBag.Message = new Models.MessagePack()
                        {
                            Text = "The movie or series was not deleted.",
                            Tipo = message.danger.ToString()
                        };

                        return View(id);

                    }
                }


                //var parameter = new List<SqlParameter>();
                //parameter.Add(new SqlParameter("@Id", id));

                //var MovieList = new List<MoviesAndSeries>();


                //var result = Task.Run(() => db.Database
                //.ExecuteSqlRaw(@"exec dbo.DeleteMoviesAndSerie @Id", parameter.ToArray()));
                
                //result.Wait();

                //db.SaveChanges();

                //ViewBag.Message = new Models.MessagePack()
                //{
                //    Text = "The movie or series was deleted correctly.",
                //    Tipo = message.danger.ToString()
                //};

            
                return RedirectToAction("View_MoviesAndSeries");

            }
            catch (Exception ex)
            {
                ViewBag.Message = new Models.MessagePack()
                {
                    Text = "Error The series or movie was not Delete correctly: " + ex,
                    Tipo = message.danger.ToString()
                };

                return View(id);
            }

        }
        
        //Get: AdminController/CreateActor
        public IActionResult CreateActors(int id_pelicula) {
            var serieData = db.MoviesAndSeries.FromSqlRaw(@"exec dbo.GetMovie @id", new SqlParameter("@id", id_pelicula)).ToList();
           

            var data = new CreateActorMcs();

            data.movie_id = id_pelicula;
            data.title = serieData.FirstOrDefault().title;
            data.url = serieData.FirstOrDefault().movie_url;
            return View(data);
        }

        //Post: AdminController/CreateActors
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateActors(CreateActorMcs Data) 
        {
            try
            {

                var parameter = new List<SqlParameter>();
                parameter.Add(new SqlParameter("@first_name", Data.actor_first_name));
                parameter.Add(new SqlParameter("@last_name", Data.actor_last_name));



                var actorSearch = new List<Actor>();

                actorSearch = db.Actors.FromSqlRaw(@"exec dbo.GetActorData @first_name, @last_name", parameter.ToArray()).ToList();

                if (actorSearch.Count > 0)
                {
                    ViewBag.Message = new Models.MessagePack()
                    {
                        Text = "The Actor Was Exist",
                        Tipo = message.danger.ToString()
                    };

                    return View(Data.movie_id);
                }
                else
                {
                    var actorData = new Actor();
                    actorData.actor_first_name = Data.actor_first_name;
                    actorData.actor_last_name = Data.actor_last_name;
                    actorData.actor_url = Data.actor_url;


                    db.Actors.Add(actorData);
                    db.SaveChanges();


                    var actorSearch2 = new List<Actor>();
                    actorSearch2 = db.Actors.FromSqlRaw(@"exec dbo.GetActorData @first_name, @last_name", parameter.ToArray()).ToList();

                    var actor_id = actorSearch2.FirstOrDefault().actor_id;

                    var movie_id = (int) (Data.movie_id);

                    var MovieActor = new MoviesAndSeriesActor();

                    MovieActor.movies_series_id = movie_id;
                    MovieActor.actor_id = actor_id;

                    db.MoviesAndSeriesActors.Add(MovieActor);
                    db.SaveChanges();


                    ViewBag.Message = new Models.MessagePack()
                    {
                        Text = "The actor registered successfully",
                        Tipo = message.success.ToString()
                    };

                    return RedirectToAction("Details_MoviesAndSeries", new { id = movie_id });
                }


            }
            catch (Exception ex)
            {
                ViewBag.Message = new Models.MessagePack()
                {
                    Text = "Error The actor did not register correctly: " + ex,
                    Tipo = message.danger.ToString()
                };

                return View(Data.movie_id);
            }
        }

        //Get: Admin/AsignedActors
        public IActionResult AsignedActors(int id_pelicula)
        {


            var actor = new List<Actor>();
            var movieData = new List<MoviesAndSeries>();
            var data = new ActorsAndMovieData();


            movieData = db.MoviesAndSeries.FromSqlRaw(@"exec dbo.GetMovie @id", new SqlParameter("@id", id_pelicula)).ToList();
            actor = db.Actors.FromSqlRaw(@"exec dbo.GetActorsList").ToList();

            data.Actors = actor.ToArray();
            data.MoviesAndSeries= movieData.ToArray();

            return View(data);

        }

        //Post: AdminController/AsignedActor
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AsignedActors(int id_actor, int id_pelicula)
        {
            var movieData = new List<MoviesAndSeriesActor>();
            var parameter = new List<SqlParameter>();
            parameter.Add(new SqlParameter("@id_pelicula", id_pelicula));
            parameter.Add(new SqlParameter("@Id_actor", id_actor));

            movieData = db.MoviesAndSeriesActors.FromSqlRaw(@"exec dbo.ComparMovieActor @id_pelicula, @Id_actor", parameter.ToArray()).ToList();

            if (movieData.Count > 0)
            {

                ViewBag.Message = new Models.MessagePack()
                {
                    Text = "The actor is already registered in this movie or series",
                    Tipo = message.danger.ToString()
                };

                return View(id_pelicula);
            }
            else
            {
                var Datos_MSA = new MoviesAndSeriesActor();
                Datos_MSA.actor_id = id_actor;
                Datos_MSA.movies_series_id = id_pelicula;
                db.MoviesAndSeriesActors.Add(Datos_MSA);
                db.SaveChanges();

                ViewBag.Message = new Models.MessagePack()
                {
                    Text = "The actor entered the movie correctly or",
                    Tipo = message.success.ToString()
                };

                return RedirectToAction("Details_MoviesAndSeries", new { id = id_pelicula });
            }

        }

        //Get: Admin/CreateGenres
        public IActionResult CreateGenres(int id_pelicula)
        {
            var serieData = db.MoviesAndSeries.FromSqlRaw(@"exec dbo.GetMovie @id", new SqlParameter("@id", id_pelicula)).ToList();


            var data = new CreateGenresMcs();

            data.movie_id = id_pelicula;
            data.title = serieData.FirstOrDefault().title;
            data.url = serieData.FirstOrDefault().movie_url;

            return View(data);
        }

        //Post: AdminController/CreateGenres
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateGenres(CreateGenresMcs Data)
        {
            try
            {
                var parameter = new List<SqlParameter>();
                parameter.Add(new SqlParameter("@genre_name", Data.genre_name));

                var genreSearch = new List<Genre>();

                genreSearch = db.Genres.FromSqlRaw(@"exec dbo.GetGenreData @genre_name", parameter.ToArray()).ToList();

                if (genreSearch.Count > 0)
                {
                    ViewBag.Message = new Models.MessagePack()
                    {
                        Text = "The Genre Was Exist",
                        Tipo = message.danger.ToString()
                    };

                    return View(Data.movie_id);
                }
                else
                {
                    var genreData = new Genre();
                    genreData.genre_name = Data.genre_name;

                    db.Genres.Add(genreData);
                    db.SaveChanges();

                    
                    var genreSearch2 = new List<Genre>();
                    genreSearch2 = db.Genres.FromSqlRaw(@"exec dbo.GetGenreData @genre_name", parameter.ToArray()).ToList();

                    var actor_id = genreSearch2.FirstOrDefault().genre_id;

                    var movie_id = Data.movie_id;

                    var MovieGenre = new MoviesAndSeriesGenre();

                    MovieGenre.movies_series_id = (int)(movie_id);
                    MovieGenre.genre_id = actor_id;

                    db.MoviesAndSeriesGenres.Add(MovieGenre);
                    db.SaveChanges();

                    ViewBag.Message = new Models.MessagePack()
                    {
                        Text = "The genre was inserted correctly in the movie or series",
                        Tipo = message.success.ToString()
                    };

                    return RedirectToAction("Details_MoviesAndSeries", new { id = movie_id });
                }


            }
            catch (Exception ex)
            {
                ViewBag.Message = new Models.MessagePack()
                {
                    Text = "Error registering gender: " + ex,
                    Tipo = message.danger.ToString()
                };

                return View(Data.movie_id);
            }
        }

        //Get: Admin/AsignedGenres
        public IActionResult AsignedGenres(int id_pelicula) {

            var genres = new List<Genre>();
            var movieData = new List<MoviesAndSeries>();
            var data = new GenresAndMovieData();

            
            movieData = db.MoviesAndSeries.FromSqlRaw(@"exec dbo.GetMovie @id", new SqlParameter("@id", id_pelicula)).ToList();
            genres = db.Genres.FromSqlRaw(@"exec dbo.GetGenre").ToList();
            data.Genre = genres.ToArray();
            data.MoviesAndSeries = movieData.ToArray();

            return View(data);
        }

        //Post: AdminController/AsignedGenre
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AsignedGenres(int genre_id, int id_pelicula) 
        {
            var movieData = new List<MoviesAndSeriesGenre>();
            var parameter = new List<SqlParameter>();
            parameter.Add(new SqlParameter("@id_pelicula", id_pelicula));
            parameter.Add(new SqlParameter("@Id_Genre", genre_id));

            movieData = db.MoviesAndSeriesGenres.FromSqlRaw(@"exec dbo.ComparMovieGenre @id_pelicula, @Id_Genre", parameter.ToArray()).ToList();

            if (movieData.Count > 0)
            {

                ViewBag.Message = new Models.MessagePack()
                {
                    Text = "The movie or series already has this genre assigned",
                    Tipo = message.danger.ToString()
                };

                return View(id_pelicula);
            }
            else {
                var Datos_MSG = new MoviesAndSeriesGenre();
                Datos_MSG.genre_id = genre_id;
                Datos_MSG.movies_series_id = id_pelicula;
                db.MoviesAndSeriesGenres.Add(Datos_MSG);
                db.SaveChanges();

                ViewBag.Message = new Models.MessagePack()
                {
                    Text = "The genre was registered correctly in the movie or it will be",
                    Tipo = message.success.ToString()
                };

                return RedirectToAction("Details_MoviesAndSeries", new { id = id_pelicula });
            }

            
        }

        [HttpGet]
        public ActionResult GetRate(int id)
        {
            var ratings = new List<Rating>();
            int count = 0;
            string rate = null;

            ratings = db.Ratings.FromSqlRaw(@"exec dbo.GetRate @id", new SqlParameter("@id", id)).ToList();


            foreach (var r in ratings)
            {
                if (r.rating1 != null)
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

        //Get: AdminController/GetEpisodes
        public ActionResult GetEpisodes(int id)
        {
            var episodes = new List<Episode>();

            var datos = new EpisodeSeries();

            episodes = db.Episodes.FromSqlRaw(@"exec dbo.GetEpisodes @id", new SqlParameter("@id", id)).ToList();

            datos.id_serie = id;
            datos.episodes = episodes.ToArray();

            return View(datos);
        }


        //Get: AdminController/InserEpisode
        public IActionResult InsertEpisodes(int id_pelicula)
        {
            var serieData = db.MoviesAndSeries.FromSqlRaw(@"exec dbo.GetMovie @id", new SqlParameter("@id", id_pelicula)).ToList();
            var episode = new EpisodeSeries();
            episode.id_serie = id_pelicula;
            episode.title = serieData.FirstOrDefault().title;
            episode.url = serieData.FirstOrDefault().movie_url;

            

            return View(episode);
        }

        //Post: AdminController/InsertEpisode
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult InsertEpisodes(Episode episode) 
        {
            try
            {
                var parameter = new List<SqlParameter>();
                parameter.Add(new SqlParameter("@title", episode.title));
                parameter.Add(new SqlParameter("@num_episode", episode.episode_number));
                parameter.Add(new SqlParameter("@id_serie", episode.movies_series_id));

                var episodeSearch = new List<Episode>();

                episodeSearch = db.Episodes.FromSqlRaw(@"exec dbo.searchEpisode @title, @num_episode, @id_serie",
                    parameter.ToArray()).ToList();

                if (episodeSearch.Count > 0)
                {
                    ViewBag.Message = new Models.MessagePack()
                    {
                        Text = "The episode already exists for this series",
                        Tipo = message.danger.ToString()
                    };


                    return View(episode.movies_series_id);
                }
                else
                {

                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri("https://localhost:7220/api/");
                        //HTTP GET
                        var PostTask = client.PostAsJsonAsync("EpisodeService/", episode);
                        PostTask.Wait();

                        var result = PostTask.Result;

                        if (result.IsSuccessStatusCode)
                        {
                            ViewBag.Message = new Models.MessagePack()
                            {
                                Text = "The Episode Was Register",
                                Tipo = message.success.ToString()
                            };

                           
                            return RedirectToAction("GetEpisodes", new { id = episode.movies_series_id });

                        }
                        else
                        {
                            ViewBag.Message = new Models.MessagePack()
                            {
                                Text = "Problem was register Movie",
                                Tipo = message.success.ToString()
                            };

                            return View(episode.movies_series_id);
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                ViewBag.Message = new Models.MessagePack()
                {
                    Text = "Error registering the episode: " + ex,
                    Tipo = message.danger.ToString()
                };

                return View(episode.movies_series_id);
            }
        }


        public async Task<IActionResult> MovieSearch(string movieGenre, string searchString, string movieActor)
        {
            // Use LINQ to get list of genres.
            IQueryable<string> genres = from m in db.Genres
                                        orderby m.genre_id
                                        select m.genre_name;
            IQueryable<string> actors = from m in db.Actors
                                        orderby m.actor_last_name
                                        select m.actor_first_name;

            var movies = from m in db.MoviesAndSeries
                         select m;


            if (!string.IsNullOrEmpty(searchString))
            {
                movies = movies.Where(s => s.title!.Contains(searchString));
            }

            if (!string.IsNullOrEmpty(movieGenre))
            {
                movies = from m in movies
                         join mg in db.MoviesAndSeriesGenres on m.id equals mg.movies_series_id
                         join g in db.Genres on mg.genre_id equals g.genre_id
                         where g.genre_name == movieGenre
                         select m;
            }
            if (!string.IsNullOrEmpty(movieActor))
            {
                movies = from m in movies
                         join mg in db.MoviesAndSeriesActors on m.id equals mg.movies_series_id
                         join g in db.Actors on mg.actor_id equals g.actor_id
                         where g.actor_first_name == movieActor
                         select m;
            }

            var movieGenreVM = new MovieGenreViewModel
            {
                Genres = new SelectList(await genres.Distinct().ToListAsync()),
                Movies = movies.ToList(),
                Actors = new SelectList(await actors.Distinct().ToListAsync())
            };

            return View(movieGenreVM);


        }

    }
}

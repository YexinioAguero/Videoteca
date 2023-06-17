using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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

        //Get: AdminController/View_MoviesAndSeries
        public IActionResult View_MoviesAndSeries()
        {

            var list = new List<Movie_S_Genre>();
            var movie = new List<MoviesAndSeries>();
            var genres = new List<Genre>();

            genres = db.Genres.FromSqlRaw("exec dbo.GetGenre").ToList();

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
                        Text = "The Movie Title Was Exist",
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
                        Text = "The Movie Was Register",
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
                var MovieEdit = new List<MoviesAndSeries>();
                var parameter = new List<SqlParameter>();
                parameter.Add(new SqlParameter("@Id_Movie", movisData.id));
                parameter.Add(new SqlParameter("@title", movisData.title));
                parameter.Add(new SqlParameter("@synopsis", movisData.synopsis));
                parameter.Add(new SqlParameter("@releaseYear", movisData.release_year));
                parameter.Add(new SqlParameter("@duration", movisData.duration));
                parameter.Add(new SqlParameter("@classification", movisData.classification));
                parameter.Add(new SqlParameter("@director", movisData.director));
                parameter.Add(new SqlParameter("@movie_url", movisData.movie_url));


                var result = Task.Run(() => db.Database
                .ExecuteSqlRaw(@"exec dbo.EditMoviesAndSerie @Id_Movie, @title, @synopsis, @releaseYear, @duration,
                @classification, @director, @movie_url",
                parameter.ToArray()));

                result.Wait();

                db.SaveChanges();

                ViewBag.Message = new Models.MessagePack()
                {
                    Text = "The series or movie was modified correctly",
                    Tipo = message.success.ToString()
                };

                return RedirectToAction("Details_MoviesAndSeries", new { id = movisData.id});
            }
            catch (Exception ex)
            {
                ViewBag.Message = new Models.MessagePack()
                {
                    Text = "Error The series or movie was not modified correctly: " + ex,
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

                ViewBag.Message = new Models.MessagePack()
                {
                    Text = "The movie or series was deleted correctly.",
                    Tipo = message.danger.ToString()
                };


                return RedirectToAction("View_MoviesAndSeries");

            }
            catch (Exception ex)
            {
                ViewBag.Message = new Models.MessagePack()
                {
                    Text = "Error The series or movie was not Delete correctly: " + ex,
                    Tipo = message.danger.ToString()
                };

                return View();
            }

        }
        
        //Get: AdminController/CreateActor
        public IActionResult CreateActors(int id_pelicula) {

            
            var data = new CreateActorMcs();

            data.movie_id = id_pelicula;
            
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

                    return View();
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

                    var movie_id = Data.movie_id;

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

                return View();
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

                return View();
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

            var data = new CreateGenresMcs();

            data.movie_id = id_pelicula;

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

                    return View();
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

                    MovieGenre.movies_series_id = movie_id;
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

                return View();
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

                return View();
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
        
        //Get: AdminController/InserEpisode
        public IActionResult InsertEpisodes(int id_pelicula)
        {

            var episode = new Episode();
            episode.movies_series_id = id_pelicula;

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


                    return RedirectToAction("Details_MoviesAndSeries", new { id = episode.movies_series_id });
                }
                else
                {
                    
                    db.Episodes.Add(episode);
                    db.SaveChanges();

                    var result = Task.Run(() => db.Database
                    .ExecuteSqlRaw(@"exec dbo.updateNumEpisodes @id_serie", new SqlParameter("@id_serie", episode.movies_series_id)));

                    result.Wait();

                    ViewBag.Message = new Models.MessagePack()
                    {
                        Text = "Episode registered successfully",
                        Tipo = message.success.ToString()
                    };


                   
                    return RedirectToAction("Details_MoviesAndSeries", new { id = episode.movies_series_id });
                }


            }
            catch (Exception ex)
            {
                ViewBag.Message = new Models.MessagePack()
                {
                    Text = "Error registering the episode: " + ex,
                    Tipo = message.danger.ToString()
                };

                return View();
            }
        }

    }
}

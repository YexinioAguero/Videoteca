namespace Videoteca.Models
{
    public class MovieInfo
    {
        public User user { get; set; }
        public MoviesAndSeries movie { get; set; }
        public List<Actor> actors { get; set; }
        public List<Genre> genres { get; set; }

        public List<Episode>? episodes { get; set; }
    }
}

namespace Videoteca.Models
{
    public class MovieInfo
    {
        public MoviesAndSeries movie { get; set; }
        public List<Actor> actors { get; set; }
        public List<Genre> genres { get; set; }
    }
}

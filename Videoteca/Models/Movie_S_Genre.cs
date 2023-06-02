using Videoteca.Models;

namespace Videoteca_Test_API_data.Models
{
    public class Movie_S_Genre
    {
       public string? genre { get; set; }
       public List<MoviesAndSeries>? movies { get; set; }
    }
}

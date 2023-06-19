
using Microsoft.AspNetCore.Mvc.Rendering;
using Videoteca.Models;

namespace Videoteca.Models;

public class MovieGenreViewModel
{
    public List<MoviesAndSeries>? Movies { get; set; }
    public SelectList? Genres { get; set; }
    public SelectList? Actors { get; set; }
    public string? MovieGenre { get; set; }
    public string? MovieActor { get; set; }
    public string? SearchString { get; set; }

}
using System;
using System.Collections.Generic;

namespace Videoteca_API.Models;

public partial class MoviesAndSeries
{
    public int id { get; set; }

    public string? title { get; set; }

    public string? synopsis { get; set; }

    public string? release_year { get; set; }

    public string? duration { get; set; }

    public string? classification { get; set; }

    public string? director { get; set; }

    public int? num_seasons { get; set; }

    public int? num_episodes { get; set; }

    public string? movie_url { get; set; }

    public DateTime? date_added { get; set; }
}

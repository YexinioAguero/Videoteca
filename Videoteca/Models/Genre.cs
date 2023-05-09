using System;
using System.Collections.Generic;

namespace Videoteca.Models;

public partial class Genre
{
    public int genre_id { get; set; }

    public int movies_series_id { get; set; }

    public string? genre_name { get; set; }
}

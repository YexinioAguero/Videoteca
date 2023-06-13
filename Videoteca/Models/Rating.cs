using System;
using System.Collections.Generic;

namespace Videoteca.Models;

public partial class Rating
{
    public int rating_id { get; set; }

    public int movies_series_id { get; set; }

    public string userName { get; set; } = null!;

    public int? rating1 { get; set; }
}

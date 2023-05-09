using System;
using System.Collections.Generic;

namespace Videoteca.Models;

public partial class Rating
{
    public int rating_id { get; set; }

    public int movies_series_id { get; set; }

    public int user_id { get; set; }

    public int? rating1 { get; set; }
}

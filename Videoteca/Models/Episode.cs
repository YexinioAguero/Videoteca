using System;
using System.Collections.Generic;

namespace Videoteca.Models;

public partial class Episode
{
    public int episodes_id { get; set; }

    public string? title { get; set; }

    public string? duration { get; set; }

    public int? episode_number { get; set; }

    public int movies_series_id { get; set; }

    public string? url { get; set; }

    public string? descripction { get; set; }
}

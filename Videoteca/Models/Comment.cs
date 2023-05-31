using System;
using System.Collections.Generic;

namespace Videoteca.Models;

public partial class Comment
{
    public int comment_id { get; set; }

    public int user_id { get; set; }

    public string? comment1 { get; set; }

    public int movies_series_id { get; set; }
}

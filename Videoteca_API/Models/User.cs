using System;
using System.Collections.Generic;

namespace Videoteca_API.Models;

public partial class User
{
    public string user_id { get; set; } = null!;

    public string? username { get; set; }

    public string? email { get; set; }
}

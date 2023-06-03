using System;
using System.Collections.Generic;

namespace Videoteca.Models;

public partial class User
{
    public string user_id { get; set; } = null!;

    public string? username { get; set; }

    public string? email { get; set; }
}

using System;
using System.Collections.Generic;

namespace Videoteca.Models;

public partial class User
{
    public int user_id { get; set; }

    public string? username { get; set; }

    public string? email { get; set; }
}

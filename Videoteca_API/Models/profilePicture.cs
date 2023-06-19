using System;
using System.Collections.Generic;

namespace Videoteca_API.Models;

public partial class profilePicture
{
    public int id { get; set; }

    public byte[]? image { get; set; }
}

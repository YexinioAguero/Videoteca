using System;
using System.Collections.Generic;

namespace Videoteca.Models;

public partial class profilePicture
{
    public int id { get; set; }

    public byte[]? image { get; set; }
}

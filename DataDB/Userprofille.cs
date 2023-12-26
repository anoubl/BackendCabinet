using System;
using System.Collections.Generic;

namespace BackendCabinet.DataDB;

public partial class Userprofille
{
    public int Id { get; set; }

    public int? Userid { get; set; }

    public string? Imag { get; set; }

    public virtual User? User { get; set; }
}

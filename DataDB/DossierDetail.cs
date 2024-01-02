using System;
using System.Collections.Generic;

namespace BackendCabinet.DataDB;

public partial class DossierDetail
{
    public int Id { get; set; }

    public string? Image { get; set; }

    public int? DossierId { get; set; }

}

using System;
using System.Collections.Generic;

namespace BackendCabinet.DataDB;

public partial class DossierDetail
{
    public int Id { get; set; }
    public int? DossierId { get; set; }
    public string? Description { get; set; }

    public Double? Total { get; set; }

}

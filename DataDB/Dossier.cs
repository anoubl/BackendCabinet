using System;
using System.Collections.Generic;

namespace BackendCabinet.DataDB;

public partial class Dossier
{
    public int DossierId { get; set; }

    public int? PatientId { get; set; }

    public DateTime? DateCreation { get; set; }

    public string? PatDescription { get; set; }

}

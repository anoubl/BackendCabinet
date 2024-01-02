using System;
using System.Collections.Generic;

namespace BackendCabinet.DataDB;

public partial class RendezVou
{
    public int Id { get; set; }

    public DateTime? Daterendezvous { get; set; }

    public string? Description { get; set; }

    public string? Plage { get; set; }

    public string? Patientemail { get; set; }

    public int? Etat { get; set; }

}

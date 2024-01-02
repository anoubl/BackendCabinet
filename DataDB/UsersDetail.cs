using System;
using System.Collections.Generic;

namespace BackendCabinet.DataDB;

public partial class UsersDetail
{
    public int Id { get; set; }

    public string? Specialite { get; set; }

    public string? Universite { get; set; }

    public int? Experience { get; set; }

    public int? Userid { get; set; }

    public int? Heures { get; set; }

    public double? Salaire { get; set; }

}

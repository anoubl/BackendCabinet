using System;
using System.Collections.Generic;

namespace BackendCabinet.DataDB;

public partial class User
{
    public int Id { get; set; }

    public string? Prenom { get; set; }

    public string? Nom { get; set; }

    public string? Email { get; set; }

    public string? Password { get; set; }

    public string? Telephone { get; set; }

    public int? Rôle { get; set; }
}

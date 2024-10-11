using System;
using System.Collections.Generic;

namespace TrailerCompanyBackend.Models;

public partial class User
{
    public int UserId { get; set; }

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Role { get; set; } = null!;

    public DateTime RegistrationDate { get; set; }

    public string Status { get; set; } = null!;
}

﻿using System;
using System.Collections.Generic;

namespace Task.Connector.ConnectorDatabase;

public partial class Passwords
{
    public int Id { get; set; }

    public string UserId { get; set; } = null!;

    public string Password { get; set; } = null!;
}
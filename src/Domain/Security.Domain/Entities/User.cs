﻿using Common.Domain.Entities;

namespace Security.Domain.Entities
{ 
  public class User : TenantEntity
  {
    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string UserName { get; set; }

    public string Email { get; set; }
  }
}

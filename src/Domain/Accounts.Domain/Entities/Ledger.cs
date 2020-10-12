using System;
using Common.Domain.Entities;

namespace Accounts.Domain.Entities
{
  public class Ledger : TenantEntity
  {
    public string AccountName { get; set; }

    public Guid LedgerGroupId { get; set; }

    public string AddressLine1 { get; set; }

    public string AddressLine2 { get; set; }

    public string AddressLine3 { get; set; }

    public Guid? CityId { get; set; }

    public Guid? StateId { get; set; }

    public Guid? CountryId { get; set; }

    public string Email { get; set; }

    public string PhoneNo { get; set; }

    public string FaxNo { get; set; }

    public string ContactPerson { get; set; }

    public string BankAccountNo { get; set; }

    public string IFSCCode { get; set; }
  }
}
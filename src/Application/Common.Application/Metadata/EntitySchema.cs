using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Common.Domain.Entities;

namespace Common.Application.Metadata
{
  public class EntitySchema
  {

    public Type EntityType { get; private set; }

    public bool IsCatalog { get; private set; }

    public bool IsGeneric { get; private set; }

    public bool IsTenantSpecific { get; private set; }

    public CatalogInfo CatalogInfo { get; private set; }

    public Dictionary<MemberInfo, CatalogInfo> References { get; private set; }

    public List<MemberInfo> SearchColumn { get; private set; }

    public EntitySchema(Type entityType)
    {
      EntityType = entityType;
      References = new Dictionary<MemberInfo, CatalogInfo>();
      SearchColumn = new List<MemberInfo>();
    }

    public static EntitySchema Create<T>()
    {
      return new EntitySchema(typeof(T));
    }

    public EntitySchema IsGenericEntity() {
      IsGeneric = true;
      IsTenantSpecific = typeof(TenantEntity).IsAssignableFrom(EntityType);
      return this;
    }

    public EntitySchema HasCatalogInfo(string codeColumn = "Id", string descriptionColumn = "Name")
    {
      IsCatalog = true;
      CatalogInfo = new CatalogInfo(codeColumn, descriptionColumn);
      return this;
    }

    public EntitySchema HasReference<TReference>(string memberName, string codeColumn = "Id", string descriptionColumn = "Name")
    {
      References.Add(GetMemberInfo(memberName), new CatalogInfo(codeColumn, descriptionColumn, typeof(TReference).Name));
      return this;
    }

    public EntitySchema HasSearch(string memberName)
    {
      SearchColumn.Add(GetMemberInfo(memberName));
      return this;
    }

    public EntitySchema Build()
    {
      return this;
    }

    private MemberInfo GetMemberInfo(string memberName)
    {
      return EntityType
                    .GetMembers()
                    .First(memberInfo => memberInfo.Name == memberName);
    }
  }
}
﻿using Osekai.Octon.Permissions;

namespace Osekai.Octon.Persistence.EntityFramework.MySql.Entities;

internal sealed class UserPermissionsOverride
{
    public UserPermissionsOverride()
    {
        Permissions = new Dictionary<string, PermissionActionType>();
    }
    
    public int Id { get; set; }
    public int UserId { get; set; }
    public IDictionary<string, PermissionActionType> Permissions { get; set; }

    public Domain.AggregateRoots.UserPermissionsOverride ToEntity()
    {
        return new Domain.AggregateRoots.UserPermissionsOverride(UserId, Permissions);
    }
}
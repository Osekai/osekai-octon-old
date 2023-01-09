﻿using Osekai.Octon.Database.Dtos;
using Osekai.Octon.Database.HelperTypes;

namespace Osekai.Octon.Database.EntityFramework.MySql.Models;

public class UserPermissionsOverride
{
    public UserPermissionsOverride()
    {
        Permissions = new Dictionary<string, PermissionActionType>();
    }
    
    public int Id { get; set; }
    public int UserId { get; set; }
    public IDictionary<string, PermissionActionType> Permissions { get; set; }

    public UserPermissionsOverrideDto ToDto()
    {
        return new UserPermissionsOverrideDto(UserId, Permissions);
    }
}
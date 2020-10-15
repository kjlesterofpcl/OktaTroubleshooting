using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BizerbaOktaSample.Utilities
{
    public interface IAuthorizationGateway
    {
        Task<IEnumerable<IUserGroup>> GetGroupNames(ClaimsPrincipal principal);
        Task<Boolean> IsUserInGroup(ClaimsPrincipal principal, String groupName);
        Task<Boolean> IsUserInAnyGroup(ClaimsPrincipal principal, params String[] groupNames);
        Task<IEnumerable<String>> FindUserAssignedGroups(ClaimsPrincipal principal, params String[] groupNames);
    }
}
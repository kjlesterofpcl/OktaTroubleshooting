using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Options;
using Okta.Sdk;
using Okta.Sdk.Configuration;

namespace BizerbaOktaSample.Utilities.Okta
{
    public class OktaGateway : IAuthorizationGateway
    {
        public OktaGateway(IOptions<OktaSettings> options, IMapper mapper)
        {
            this._options = options;
            this._mapper = mapper;

            this._client = new OktaClient(new OktaClientConfiguration
            {
                OktaDomain = this._options.Value.OktaDomain,
                Token = this._options.Value.AccessToken
            });
        }

        async public Task<IEnumerable<IUserGroup>> GetGroupNames(ClaimsPrincipal principal)
        {
            String userId = GetUserIdFromPrincipal(principal);
            List<IApplicationGroupAssignment> groupAssignments = await this._client.Applications.ListApplicationGroupAssignments(this._options.Value.ClientId).ToListAsync();
            ICollectionClient<IGroup> allUserGroups = this._client.Users.ListUserGroups(userId);
            IList<IUserGroup> bizerbaAppGroups = new List<IUserGroup>();

            foreach (IApplicationGroupAssignment assignment in groupAssignments)
            {
                IGroup oktaGroup = await allUserGroups.FirstOrDefaultAsync(x => x.Id == assignment.Id);

                if (!ReferenceEquals(oktaGroup, null))
                {
                    bizerbaAppGroups.Add(this._mapper.Map<OktaUserGroup>(oktaGroup));
                }
            }

            return bizerbaAppGroups;
        }

        async public Task<Boolean> IsUserInGroup(ClaimsPrincipal principal, String groupName)
        {
            String userId = GetUserIdFromPrincipal(principal);
            return await this._client.Users.ListUserGroups(userId).AnyAsync(x => String.Equals(x.Profile.Name, groupName, StringComparison.InvariantCultureIgnoreCase));
        }

        async public Task<Boolean> IsUserInAnyGroup(ClaimsPrincipal principal, params String[] groupNames)
        {
            String userId = GetUserIdFromPrincipal(principal);
            ICollectionClient<IGroup> userGroups = this._client.Users.ListUserGroups(userId);

            foreach (String name in groupNames)
            {
                if (!await userGroups.AnyAsync(x => String.Equals(x.Profile.Name, name, StringComparison.InvariantCultureIgnoreCase)))
                {
                    return false;
                }
            }

            return true;
        }

        async public Task<IEnumerable<String>> FindUserAssignedGroups(ClaimsPrincipal principal, params String[] groupNames)
        {
            String userId = GetUserIdFromPrincipal(principal);
            ICollectionClient<IGroup> userGroups = this._client.Users.ListUserGroups(userId);
            IList<String> allowedGroups = new List<String>();

            foreach (String name in groupNames)
            {
                if (await userGroups.AnyAsync(x => String.Equals(x.Profile.Name, name, StringComparison.InvariantCultureIgnoreCase)))
                {
                    allowedGroups.Add(name);
                }
            }

            return allowedGroups;
        }

        private static String GetUserIdFromPrincipal(ClaimsPrincipal principal)
        {
            return principal.FindFirst("sub").Value;
        }

        private readonly OktaClient _client;
        private readonly IMapper _mapper;
        private readonly IOptions<OktaSettings> _options;
    }
}
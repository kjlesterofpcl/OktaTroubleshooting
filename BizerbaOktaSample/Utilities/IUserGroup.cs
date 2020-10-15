using System;

namespace BizerbaOktaSample.Utilities
{
    public interface IUserGroup
    {
        String Id { get; }
        String FriendlyName { get; }
        String Description { get; }
    }
}
using Microsoft.AspNetCore.Authorization;

namespace ClinicManagementSystem.Authorization
{
    public class UserOwnerOrAdminRequirement : IAuthorizationRequirement
    {
    }
}

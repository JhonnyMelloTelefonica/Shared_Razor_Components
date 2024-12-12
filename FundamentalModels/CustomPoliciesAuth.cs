using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Shared_Razor_Components.FundamentalModels;
using System.Security.Claims;

namespace Shared_Razor_Components.FundamentalModels;

public class CustomPoliciesAuth : IAuthorizationRequirement
{
    public UserService Acesso { get; set; }
}

public class GenericPolicyRequirement : IAuthorizationRequirement
{
    public UserService Acesso { get; set; }
    public Func<CustomPoliciesAuth, ClaimsPrincipal, bool> RequirementFunc { get; }

    public GenericPolicyRequirement(Func<CustomPoliciesAuth, ClaimsPrincipal, bool> requirementFunc)
    {
        RequirementFunc = requirementFunc ?? throw new ArgumentNullException(nameof(requirementFunc));
    }
}

public class GenericPolicyHandler : AuthorizationHandler<GenericPolicyRequirement>
{
    private readonly UserService _userService;

    public GenericPolicyHandler(UserService userService)
    {
        _userService = userService ?? throw new ArgumentNullException(nameof(userService));
    }


    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, GenericPolicyRequirement requirement)
    {
        // Recupera a instância do requisito
        var requirementInstance = Activator.CreateInstance(typeof(CustomPoliciesAuth));
        var saida = (CustomPoliciesAuth)requirementInstance;
        if (requirement.Acesso == null)
        {
            saida.Acesso = _userService;
            //saida.Acesso
        }
        // Verifica se o usuário atende aos requisitos
        if (requirement.RequirementFunc(saida, context.User))
        {
            context.Succeed(requirement);
        }
        else
        {
            context.Fail();
        }

        return Task.CompletedTask;
    }

}

public static class AuthorizationPolicyExtensions
{
    public static void AddGenericPolicy(this AuthorizationOptions options, string policyName, Func<CustomPoliciesAuth, ClaimsPrincipal, bool> requirementFunc)
    {
        options.AddPolicy(policyName, policy =>
            policy.Requirements.Add(new GenericPolicyRequirement(requirementFunc))
        );
    }
}
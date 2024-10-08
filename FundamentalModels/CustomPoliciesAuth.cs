using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Shared_Razor_Components.FundamentalModels;
using System.Security.Claims;

namespace Controle_Demandas
{
    public static class AuthorizationUtils
    {
        private static AuthenticationStateProvider AuthenticationStateProvider;

        public static void Initialize(AuthenticationStateProvider authenticationStateProvider)
        {
            AuthenticationStateProvider = authenticationStateProvider;
        }
            
        public static bool UserMeetsAnyPolicy(List<string> policies)
        {
            if (AuthenticationStateProvider == null)
            {
                throw new InvalidOperationException("AuthenticationStateProvider não foi inicializado. Execute AuthorizationUtils.Initialize() primeiro.");
            }

            var authenticationState = AuthenticationStateProvider.GetAuthenticationStateAsync().Result;
            var user = authenticationState.User;

            // Check if the user meets any of the required policies
            return policies.Any(policy => user.HasClaim(ClaimTypes.Role, policy));
        }
    }

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

        public GenericPolicyRequirement()
        {
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

}
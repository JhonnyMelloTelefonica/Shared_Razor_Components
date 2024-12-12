using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Shared_Razor_Components.Services;
using Shared_Razor_Components.ViewModels;
using Shared_Razor_Components.Layout;
using Shared_Razor_Components.Shared;
using Shared_Static_Class.Model;
using Shared_Razor_Components.FundamentalModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared_Razor_Components.Shared.BasicForApplication;
using Microsoft.AspNetCore.Server.IISIntegration;
using System.Security.Claims;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Shared_Razor_Components.ViewModel;

namespace Shared_Razor_Components
{
    public static class ServiceCollectionExtensions
    {
        public static void AddSharedServices(this IServiceCollection services)
        {
            services.AddCascadingAuthenticationState();
            services.AddAuthorizationCore(options =>
            {
                #region ADM

                options.AddGenericPolicy("Adm", (userRequirement, user) => userRequirement.Acesso.User.Perfil.Any(x => x.Perfil_Plataforma.ID_PERFIL == 1));
                options.AddGenericPolicy("All", (userRequirement, user) => user.Claims.Any(x => x.Type == ClaimTypes.Name || x.Type == ClaimTypes.Email || x.Type == ClaimTypes.HomePhone));

                #endregion

                #region Vivo X

                options.AddGenericPolicy("JornadaRTCZ", (userRequirement, user) =>
                    userRequirement.Acesso.User.Perfil.Any(x => new[] { 1, 3, 4, 5, 6 }.Contains(x.Perfil_Plataforma.ID_PERFIL)));

                options.AddGenericPolicy("JornadaRTCZUser", (userRequirement, user) =>
                    userRequirement.Acesso.User.Perfil.Any(x => new[] { 1, 4 }.Contains(x.Perfil_Plataforma.ID_PERFIL)));

                options.AddGenericPolicy("JornadaRTCZCreator", (userRequirement, user) =>
                    userRequirement.Acesso.User.Perfil.Any(x => new[] { 1, 2, 5, 6 }.Contains(x.Perfil_Plataforma.ID_PERFIL)));

                options.AddGenericPolicy("JornadaRTCZCreatorMaster", (userRequirement, user) =>
                    userRequirement.Acesso.User.Perfil.Any(x => new[] { 1, 6 }.Contains(x.Perfil_Plataforma.ID_PERFIL)));

                #endregion

                #region Controle Usuarios

                options.AddGenericPolicy("AccessControleAcessos", (userRequirement, user) =>
                    userRequirement.Acesso.User.Perfil.Any(x => new[] { 1, 2, 10,18,19 }.Contains(x.Perfil_Plataforma.ID_PERFIL)));

                options.AddGenericPolicy("ControleAcessos", (userRequirement, user) =>
                    userRequirement.Acesso.User.Perfil.Any(x => new[] { 1, 2, 10 }.Contains(x.Perfil_Plataforma.ID_PERFIL)));

                options.AddGenericPolicy("ControleAcessosMaster", (userRequirement, user) =>
                    userRequirement.Acesso.User.Perfil.Any(x => new[] { 1, 10 }.Contains(x.Perfil_Plataforma.ID_PERFIL)));

                options.AddGenericPolicy("ControleHierarquiaRTCZ", (userRequirement, user) =>
                    userRequirement.Acesso.User.Perfil.Any(x => new[] { 1, 18, 19 }.Contains(x.Perfil_Plataforma.ID_PERFIL)));

                #endregion

                #region Controle de Demandas

                options.AddGenericPolicy("GenericUserOrDemandaAdm", (userRequirement, user) =>
                userRequirement.Acesso.User.Perfil.Any(x => new[] { 1, 13, 14, 15, 20, 16, 17 }.Contains(x.Perfil_Plataforma.ID_PERFIL)));

                options.AddGenericPolicy("ControleFilaDemandas", (userRequirement, user) =>
                userRequirement.Acesso.User.Perfil.Any(x => new[] { 1, 16, 17 }.Contains(x.Perfil_Plataforma.ID_PERFIL)));

                options.AddGenericPolicy("ControleCriarFilaDemandas", (userRequirement, user) =>
                userRequirement.Acesso.User.Perfil.Any(x => new[] { 1, 16, 17 }.Contains(x.Perfil_Plataforma.ID_PERFIL)));

                options.AddGenericPolicy("GenericUser", (userRequirement, user) =>
                userRequirement.Acesso.User.Perfil.Any(x => new[] { 13 }.Contains(x.Perfil_Plataforma.ID_PERFIL)));

                options.AddGenericPolicy("ControleDemandasAdm", (userRequirement, user) =>
                userRequirement.Acesso.User.Perfil.Any(x => new[] { 1, 14, 15 }.Contains(x.Perfil_Plataforma.ID_PERFIL)));

                options.AddGenericPolicy("ControleDemandasLogico", (userRequirement, user) =>
                userRequirement.Acesso.User.Perfil.Any(x => new[] { 1, 15 }.Contains(x.Perfil_Plataforma.ID_PERFIL)));

                options.AddGenericPolicy("ControleDemandasGerente", (userRequirement, user) =>
                userRequirement.Acesso.User.Perfil.Any(x => new[] { 20, 1 }.Contains(x.Perfil_Plataforma.ID_PERFIL)));

                options.AddGenericPolicy("ControleDemandasManagement", (userRequirement, user) =>
                userRequirement.Acesso.User.Perfil.Any(x => new[] { 14, 13, 15, 20, 1 }.Contains(x.Perfil_Plataforma.ID_PERFIL)));

                options.AddGenericPolicy("Suporte", (userRequirement, user) =>
                    userRequirement.Acesso.User.IsSuporte());

                options.AddGenericPolicy("ControleAberturaDemandas", (userRequirement, user) =>

                    userRequirement.Acesso.User.Perfil.Any(x => new[] { 1, 14, 15 }.Contains(x.Perfil_Plataforma.ID_PERFIL)));

                #endregion

                #region Cardapio Digital

                options.AddGenericPolicy("CanSeeCardapio", (userRequirement, user) =>
                    userRequirement.Acesso.User.Perfil.Any(x => x.Perfil_Plataforma.ID_PERFIL == 1 || x.Perfil_Plataforma.ID_PERFIL == 21 || x.Perfil_Plataforma.ID_PERFIL == 22) && userRequirement.Acesso.User.REGIONAL == "NE");

                options.AddGenericPolicy("Default", (userRequirement, user) =>
                userRequirement.Acesso.User.Perfil.Any(x => x.Perfil_Plataforma.ID_PERFIL == 21) && userRequirement.Acesso.User.REGIONAL == "NE");

                options.AddGenericPolicy("VIVOX_CARDAPIO_DIGITAL_ADM", (userRequirement, user) =>
                userRequirement.Acesso.User.Perfil.Any(x => x.Perfil_Plataforma.ID_PERFIL == 1 || x.Perfil_Plataforma.ID_PERFIL == 22) && userRequirement.Acesso.User.REGIONAL == "NE");

                #endregion

                #region Fórum Giro V

                options.AddGenericPolicy("CanSeeForumGiroV", (userRequirement, user) =>
                    userRequirement.Acesso.User.Perfil.Any(x => x.Perfil_Plataforma.ID_PERFIL == 1 || x.Perfil_Plataforma.ID_PERFIL == 23 || x.Perfil_Plataforma.ID_PERFIL == 24) && userRequirement.Acesso.User.REGIONAL == "NE");

                options.AddGenericPolicy("VIVOX_FORUM_GIROV_USER", (userRequirement, user) =>
                userRequirement.Acesso.User.Perfil.Any(x => x.Perfil_Plataforma.ID_PERFIL == 23) && userRequirement.Acesso.User.REGIONAL == "NE");

                options.AddGenericPolicy("VIVOX_FORUM_GIROV_ADM", (userRequirement, user) =>
                userRequirement.Acesso.User.Perfil.Any(x => x.Perfil_Plataforma.ID_PERFIL == 1 || x.Perfil_Plataforma.ID_PERFIL == 24) && userRequirement.Acesso.User.REGIONAL == "NE");

                #endregion

            });
            services.AddSingleton<StaticUserRedecorp>(); // necessariamente Singleton pois guardam valores que são comuns a todos
            services.AddSingleton<GetUser_REDECORP>(); // necessariamente Singleton pois guardam valores que são comuns a todos
            services.AddScoped<Radzen.DialogService>();
            services.AddScoped<ViewOptionService>();
            services.AddScoped<UserCard>();
            services.AddTransient<ControleUserModalBody>();
            //services.AddScoped<SetHeader>();
            //services.AddScoped<SetFooter>();
            services.AddScoped<ControleUsuariosAppViewModel>();
            services.AddScoped<RegisterViewModel>();
            services.AddScoped<AcessosPendentesByIdViewModel>();
            services.AddSingleton<IPainelUsuariosService, PainelUsuariosService>();
            services.AddSingleton<IPrincipalService, PrincipalService>();
            services.AddSingleton<IControleUsuariosAppService, ControleUsuariosAppService>();
            services.AddSingleton<ICreateQuestionService, CreateQuestionService>();
            services.AddSingleton<ICreateFormService, CreateFormService>();
            services.AddSingleton<IListaFormService, ListaFormService>();
            services.AddSingleton<IAnswerFormService, AnswerFormService>();
            services.AddSingleton<IEditQuestionService, EditQuestionService>();
            services.AddSingleton<IEditSingleQuestionService, EditSingleQuestionService>();
            services.AddSingleton<IResultadosProvaService, ResultadosProvaService>();
            services.AddSingleton<IPainelProvasRealizadasService, PainelProvasRealizadasService>();
            services.AddSingleton<IAcessoPendenteByIdService, AcessoPendenteByIdService>();
            services.AddSingleton<IConsultarDemandasService, ConsultarDemandasService>();
            services.AddSingleton<IControleDemandaService, ControleDemandaService>();
            services.AddSingleton<IRegisterService, RegisterService>();
            services.AddSingleton<IEditUserService, EditUserService>();
            services.AddSingleton<IAcessoPendenteByIdService, AcessoPendenteByIdService>();
            services.AddSingleton<IAcessoTerceirosService, AcessoTerceirosService>();
            services.AddSingleton<IAnswerFormService, AnswerFormService>();
            services.AddSingleton<ICardapioDigitalService, CardapioDigitalService>();
            services.AddSingleton<IConsultarDemandasService, ConsultarDemandasService>();
            services.AddSingleton<IControleDemandaService, ControleDemandaService>();
            services.AddSingleton<IControleUsuariosAppService, ControleUsuariosAppService>();
            services.AddSingleton<ICreateFormService, CreateFormService>();
            services.AddSingleton<ICreateQuestionService, CreateQuestionService>();
            services.AddSingleton<IDesligamentosService, DesligamentosService>();
            services.AddSingleton<IEditQuestionService, EditQuestionService>();
            services.AddSingleton<IEditSingleQuestionService, EditSingleQuestionService>();
            services.AddSingleton<IEditUserService, EditUserService>();
            services.AddSingleton<IJornadaHierarquiaService, JornadaHierarquiaService>();
            services.AddSingleton<IListaFormService, ListaFormService>();
            services.AddSingleton<IPainelProvasRealizadasService, PainelProvasRealizadasService>();
            services.AddSingleton<IPainelUsuariosService, PainelUsuariosService>();
            services.AddSingleton<IPrincipalService, PrincipalService>();
            services.AddSingleton<IPWService, PWService>();
            //services.AddSingleton<IQualidadeService,QualidadeService>();
            services.AddSingleton<IRegisterService, RegisterService>();
            services.AddSingleton<IResultadosProvaService, ResultadosProvaService>();
            services.AddSingleton<IForumRTCZService, ForumRTCZService>();
            services.AddSingleton<UsersAtivos>();// necessariamente Singleton pois guardam valores que são comuns a todos
            services.AddScoped<UserService>(); // necessariamente Scoped pois são valores que só seram alterados ao recarregarem a página
            services.AddTransient<IAuthorizationHandler, GenericPolicyHandler>();
            services.AddAuthenticationCore();
            services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
            services.AddFluentUIComponents();
            FluentWizard.LabelButtonPrevious = "Anterior";
            FluentWizard.LabelButtonNext = "Próximo";
            FluentWizard.LabelButtonDone = "Finalizar";
        }
    }
}

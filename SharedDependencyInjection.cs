using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Shared_Class_Vivo_Apps.Services;
using Shared_Class_Vivo_Apps.ViewModels;
using Shared_Razor_Components.Shared;
using Shared_Static_Class.Model;
using Shared_Static_Class.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
    
namespace Shared_Razor_Components
{
    public static class ServiceCollectionExtensions
    {
        public static void AddSharedServices(this IServiceCollection services)
        {
            services.AddSingleton<StaticUserRedecorp>(); // necessariamente Singleton pois guardam valores que são comuns a todos
            services.AddSingleton<GetUser_REDECORP>(); // necessariamente Singleton pois guardam valores que são comuns a todos
            services.AddScoped<Radzen.DialogService>();
            services.AddScoped<ViewOptionService>();
            services.AddScoped<UserCard>();

            services.AddScoped<ControleUsuariosAppViewModel>();

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
            services.AddSingleton<UsersAtivos>();// necessariamente Singleton pois guardam valores que são comuns a todos
            services.AddScoped<UserService>(); // necessariamente Scoped pois são valores que só seram alterados ao recarregarem a página
        }
    }
}

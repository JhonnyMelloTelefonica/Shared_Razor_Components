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
            services.AddTransient<ControleUserModalBody>();
            //services.AddScoped<SetHeader>();
            //services.AddScoped<SetFooter>();
            services.AddScoped<ControleUsuariosAppViewModel>();
            services.AddScoped<RegisterViewModel>();
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
            services.AddSingleton<UsersAtivos>();// necessariamente Singleton pois guardam valores que são comuns a todos
            services.AddScoped<UserService>(); // necessariamente Scoped pois são valores que só seram alterados ao recarregarem a página
        }
    }
}

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Shared_Static_Class.Model;
using System.Text.RegularExpressions;


namespace Shared_Razor_Components.FundamentalModels
{
    public class GetUser_REDECORP
    {
        public IHttpContextAccessor _contextAccessor;
        public IWebHostEnvironment _Networkacessor;
        public StaticUserRedecorp custom_mat;
        public string? matricula;
        public GetUser_REDECORP(IHttpContextAccessor contextAccessor, IWebHostEnvironment networkacessor, StaticUserRedecorp _custom_mat)
        {
            custom_mat = _custom_mat;
            _contextAccessor = contextAccessor;
            _Networkacessor = networkacessor;
        }


        public int GetMatricula()
        {
            var matricula = "";
            if (_Networkacessor.IsDevelopment()) // Caso o sistema esteja rodando em ambiente de testes
            {
                if (custom_mat.matricula != 0)
                {
                    //this.matricula = this.matricula.Replace("https://k3s82191-4881.brs.devtunnels.ms/", "");
                    matricula = custom_mat.matricula.ToString();
                }
                else
                {
                    // Para testes com várias pessoas conectadas dentro da rede 
                    //matricula = ApenasNumeros(RegrasRedecorp(_contextAccessor.HttpContext.User.Identity.Name.Split(new[] { "\\" }, StringSplitOptions.None)[1].ToLower())).TrimStart('0');

                    //matricula = "40416870"; //
                    //matricula = "64654"; // ROGERIO LESTE
                    //matricula = "14824"; // victor divisão
                    //matricula = "61662"; // 
                    //matricula = "44374"; // 
                    //matricula = "3556940"; // Herbert
                    //matricula = "3521592"; //
                    //matricula = "46800"; //
                    //matricula = "157601"; //
                    //matricula = "111251"; //1133
                    //matricula = "72215"; //
                    //matricula = "3556940"; //
                    //matricula = "70365"; //1223123123
                    //matricula = "148676"; //Karla Boleta
                    //matricula = "109598"; //Karla Boleta
                    //matricula = "89987"; //Raquel Boleta
                    //matricula = "129294"; //Rodrigo Lessa
                    //matricula = "133819"; //Herbert
                    //matricula = "73203"; //mesmo PDV que eu 
                    //matricula = "28050"; //Divisáo que reponse ao  meu PDV
                    //matricula = "137940"; //
                    //matricula = "974950"; //
                    //matricula = "3551449"; //Random solicitante
                    //matricula = "30722"; //Chiquito!
                    //matricula = "64960"; //Isis Mary 💗💗
                    //matricula = "159209"; //
                    //matricula = "151191"; //Jhonny
                    //matricula = "158125"; //
                    matricula = "3511507"; //
                    //matricula = "16279"; //Carol cood. suporte
                    //matricula = "80702130"; //kefson
                    //matricula = "80926613"; //Erro Controle Usuarios
                    //matricula = "30722"; //Chico
                    //matricula = "39013"; //Flavinha
                    //matricula = "163864"; //Jhonny
                    //matricula = "1638642"; //Jhonny
                    //matricula = "325324"; //Jhonny
                    //matricula = "44374"; //
                    //matricula = "126218"; //
                    //matricula = "22385"; //
                    //matricula = "99420"; //Hugo
                    //matricula = "46800"; // RTCZ erro
                    //matricula = "30836"; //Washington
                    //matricula = "3521592"; //Washington
                    //matricula = "152027"; //Jhonny
                    //matricula = "151191"; //Eu
                    //matricula = "12194"; //Eu -- RJ LESTE
                    //matricula = "3511507"; //MELO - VItor
                    //matricula = "152664";
                    //matricula = "163637";

                    //matricula = "478312473892"; //Pessoa Inexistente

                    // --------------
                    // REGIONAL MINAS
                    // --------------
                    //matricula = "155251"; //Minas
                    //matricula = "427700"; //Minas Suporte

                    // --------------
                    // REGIONAL LESTE
                    // --------------
                    //matricula = "64654"; //Rogério
                    //matricula = "80866380"; //CN

                    // --------------
                    // INEXISTENTE
                    // --------------
                    //matricula = "6465472";
                }
            }
            else // Caso o sistema esteja rodando em ambiente de produção
            {
                matricula = ApenasNumeros(RegrasRedecorp(_contextAccessor.HttpContext.User.Identity.Name.Split(new[] { "\\" }, StringSplitOptions.None)[1].ToLower())).TrimStart('0');
            }



            return int.Parse(matricula);
        }

        private string ApenasNumeros(string str)
        {
            //se só tiver apenas letras
            if (Regex.IsMatch(str, @"^[a-zA-Z]+$"))
            {
                return str;
            }
            var apenasDigitos = new Regex(@"[^\d]");
            return apenasDigitos.Replace(str, "");
        }
        

        private string RegrasRedecorp(string str)
        {
            if (str.StartsWith("g00"))
            {
                str = str.Replace("g00", "35");
            }
            else if (str.StartsWith("g0"))
            {
                str = str.Replace("g0", "35");
            }
            return str;
        }
    }
}

using ClientesGFT.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ClientesGFT.Domain.Util
{
    public static class EnumHelper
    {
        #region Status

        public static EStatus StatusIdParaStatus(int statusId)
        {
            return statusId switch
            {
                6 => EStatus.CORRECAO_PERFIL,
                5 => EStatus.REPROVADO,
                4 => EStatus.APROVADO,
                3 => EStatus.AGUARDANDO_CONTROLE_DE_RISCO,
                2 => EStatus.AGUARDANDO_GERENCIA,
                1 => EStatus.EM_CADASTRO,
                _ => throw new ArgumentException("Status inválido")
            };
        }

        public static int StatusParaStatusId(EStatus status)
        {
            return status switch
            {
                EStatus.CORRECAO_PERFIL => 6,
                EStatus.REPROVADO => 5,
                EStatus.APROVADO => 4,
                EStatus.AGUARDANDO_CONTROLE_DE_RISCO => 3,
                EStatus.AGUARDANDO_GERENCIA => 2,
                EStatus.EM_CADASTRO => 1,
                EStatus.TODOS => 0,
                _ => throw new ArgumentException("Status inválido")
            };
        }
        public static EStatus StatusIdParaStatus(string statusId)
        {
            return StatusIdParaStatus(Convert.ToInt32(statusId));
        }

        #endregion

        #region Perfil
        public static ERoles PerfilIdParaPerfil(int perfilId)
        {
            return perfilId switch
            {
                4 => ERoles.CONTROLE_DE_RISCO,
                3 => ERoles.GERENCIA,
                2 => ERoles.OPERACAO,
                1 => ERoles.ADMINISTRACAO,
                _ => throw new ArgumentException("Perfil inválido")
            };
        }

        public static int PerfilParaPerfilId(ERoles perfil)
        {
            return perfil switch
            {
                ERoles.CONTROLE_DE_RISCO => 4,
                ERoles.GERENCIA => 3,
                ERoles.OPERACAO => 2,
                ERoles.ADMINISTRACAO => 1,
                _ => throw new ArgumentException("Perfil inválido")
            };
        }

        public static ERoles PerfilIdParaPerfil(string perfilId)
        {
            return PerfilIdParaPerfil(Convert.ToInt32(perfilId));
        }

        public static IList<ERoles> PerfilStringParaList(string perfilString)
        {
            if (string.IsNullOrEmpty(perfilString)) return new List<ERoles>();

            return perfilString
                               .Split(",")
                               .Select(x => Enum.Parse<ERoles>(x))
                               .Cast<ERoles>()
                               .ToList();
        }

        public static string PerfilListParaString(IList<ERoles> perfils)
        {
            return string.Join(",", perfils.Select(v => v.ToString()));
        }
        #endregion



        private static string lookupResource(Type resourceManagerProvider, string resourceKey)
        {
            var resourceKeyProperty = resourceManagerProvider.GetProperty(resourceKey,
                BindingFlags.Static | BindingFlags.Public, null, typeof(string),
                new Type[0], null);
            if (resourceKeyProperty != null)
            {
                return (string)resourceKeyProperty.GetMethod.Invoke(null, null);
            }

            return resourceKey;
        }

        public static string GetDisplayName<T>(this T value)
        {
            var fieldInfo = value.GetType().GetField(value.ToString());

            var descriptionAttributes = fieldInfo.GetCustomAttributes(
                typeof(DisplayAttribute), false) as DisplayAttribute[];

            if (descriptionAttributes[0].ResourceType != null)
                return lookupResource(descriptionAttributes[0].ResourceType, descriptionAttributes[0].Name);

            if (descriptionAttributes == null) return string.Empty;
            return (descriptionAttributes.Length > 0) ? descriptionAttributes[0].Name : value.ToString();
        }
    }
}

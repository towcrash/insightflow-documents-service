using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DocumentService.src.helper
{
    /// <summary>
    /// Helper para validar contenido JSON de documentos
    /// </summary>
    public static class JsonValidator
    {
        /// <summary>
        /// Valida si una cadena es JSON válido
        /// </summary>
        /// <param name="jsonString">Cadena JSON a validar</param>
        /// <returns>True si es JSON válido, False en caso contrario</returns>
        public static bool IsValidJson(string jsonString)
        {
            if (string.IsNullOrWhiteSpace(jsonString))
            {
                return false;
            }

            try
            {
                var obj = JToken.Parse(jsonString);
                return true;
            }
            catch (JsonReaderException)
            {
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Valida y formatea una cadena JSON
        /// </summary>
        /// <param name="jsonString">Cadena JSON a formatear</param>
        /// <returns>JSON formateado o cadena vacía si es inválido</returns>
        public static string FormatJson(string jsonString)
        {
            if (!IsValidJson(jsonString))
            {
                return "{}";
            }

            try
            {
                var obj = JToken.Parse(jsonString);
                return obj.ToString(Formatting.None);
            }
            catch
            {
                return "{}";
            }
        }

        /// <summary>
        /// Obtiene un JSON vacío con estructura de bloques
        /// </summary>
        /// <returns>JSON con estructura inicial</returns>
        public static string GetEmptyDocumentContent()
        {
            return JsonConvert.SerializeObject(new
            {
                blocks = new object[] { }
            });
        }
    }
}
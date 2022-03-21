using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;
using System.Linq;

namespace Flock.API.Models
{
    public class ErrorModel
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public IEnumerable<ValidationErrorModel> ValidationErrors { get; set; }

        public override string ToString()
        {
            var settings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                NullValueHandling = NullValueHandling.Ignore,
            };
            return JsonConvert.SerializeObject(this, Formatting.None, settings);
        }

        public static ErrorModel FromStatusCode(int statusCode)
        {
            var message = GetDefaultMessage(statusCode);
            return new ErrorModel { StatusCode = statusCode, Message = message };
        }

        private static string GetDefaultMessage(int statusCode)
        {
            switch (statusCode)
            {
                case 401: // Unauthorized
                    {
                        return "Acceso no autorizado";
                    }
                case 403: // Forbidden
                    {
                        return "Acceso denegado";
                    }
                case 404: // Not Found
                    {
                        return "Recurso no encontrado";
                    }
                case 504: // Gateway Timeout
                case 408: // Request Timeout
                    {
                        return "Tiempo de espera agotado";
                    }
            }
            return "Ocurrió un error en la aplicación";
        }

        public static ErrorModel FromModelState(ModelStateDictionary modelState)
        {
            var validationErrors = modelState.Select(x => new ValidationErrorModel()
            {
                FieldName = x.Key,
                Errors = x.Value.Errors.Select(e => e.ErrorMessage)
            });

            return new ErrorModel
            {
                StatusCode = 400,
                Message = "Existen campos con información incorrecta",
                ValidationErrors = validationErrors,
            };
        }
    }
}

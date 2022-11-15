using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.Helper.Comunes
{
    public class ExcepcionValidacion : Exception
    {
        public IDictionary<string, string[]> Errors { get; }

        public ExcepcionValidacion()
            : base("Uno o más errores de validación han ocurrido.")
        {
            Errors = new Dictionary<string, string[]>();
        }

        public ExcepcionValidacion(string mensaje)
            : base(mensaje)
        {
            Errors = new Dictionary<string, string[]>();
        }

        public ExcepcionValidacion(IEnumerable<ValidationFailure> failures)
            : this()
        {
            Errors = (from e in failures
                      group e.ErrorMessage by e.PropertyName).ToDictionary((IGrouping<string, string> failureGroup) => failureGroup.Key, (IGrouping<string, string> failureGroup) => failureGroup.ToArray());
            
        }
    }
}

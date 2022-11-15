using FluentValidation.Results;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Throw;
using Aplicacion.Helper.Comunes;

namespace Aplicacion.Helper.Comportamientos
{
    public class Validacion<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> validadores;

        public Validacion(IEnumerable<IValidator<TRequest>> validadores)
        {
            this.validadores = validadores;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (validadores.Any())
            {
                ValidationContext<TRequest> contexto = new ValidationContext<TRequest>(request);
                List<ValidationFailure> errores = (from f in (await Task.WhenAll(validadores.Select((IValidator<TRequest> v) => v.ValidateAsync(contexto, cancellationToken)))).SelectMany((ValidationResult r) => r.Errors)
                                                   where f != null
                                                   select f).ToList();
                Validatable<int> validatable = errores.Count.Throw((string x) => new ExcepcionValidacion(errores), "errores.Count");
                _ = ref validatable.IfNotEquals(0);
            }

            return await next();
        }
    }
}

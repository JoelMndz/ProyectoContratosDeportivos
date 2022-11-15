using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cliente.Shared
{
    public record InsertarCategoriaDTO(string Nombre,string Descripcion) : IRequest<CategoriaDTO>;
}

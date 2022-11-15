using Cliente.Shared;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;

namespace Cliente.Client.Shared.Componentes
{
    public partial class TablaUsuarios
    {
        public bool loading = false;
        protected override async Task OnInitializedAsync()
        {
            loading = true;
            await appState.UsuarioState.ObtenerTodos(Http);
            loading = false;
        }

        
    }
}

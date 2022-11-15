namespace Cliente.Client.Store
{
    public class AppState
    {
        public JugadorState JugadoreState { get; set; } = new();
        public UsuarioState UsuarioState { get; set; } = new();
        public CategoriaState CategoriaState { get; set; } = new();
        public EquipoState EquipoState { get; set; } = new();
        public ContratosState ContratosState { get; set; } = new();
        public ErrorState ErrorState { get; set; } = new();
    }
}

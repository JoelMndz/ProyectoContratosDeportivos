using MudBlazor;

namespace Cliente.Client.Shared;

public partial class MainLayout
{
    private readonly MudTheme temarsonalizado = new()
    {
        Palette = new Palette
        {
            Primary = Colors.Blue.Default,
            Secondary = Colors.Amber.Accent4,
            AppbarBackground = Colors.Blue.Default,
        }

    };

    private bool drawerAbierto = true;

    private void AlternarDrawer() => this.drawerAbierto = !this.drawerAbierto;
}
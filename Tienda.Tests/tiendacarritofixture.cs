namespace Tienda.Tests;

public class TiendaCarritoFixture
{
    private readonly TiendaFixture _tiendaFixtureBase;

    public Tienda Tienda => _tiendaFixtureBase.Tienda;
    public Producto ProductoRaton => _tiendaFixtureBase.ProductoRaton;
    public Producto ProductoTeclado => _tiendaFixtureBase.ProductoTeclado;
    public Producto ProductoMonitor { get; }
    public Producto ProductoAuriculares { get; }

    public TiendaCarritoFixture()
    {
        _tiendaFixtureBase = new TiendaFixture();

        ProductoMonitor = new Producto("Monitor", 50000m, "Tecnologia");
        ProductoAuriculares = new Producto("Auriculares", 20000m, "Accesorios");

        Tienda.AgregarProducto(ProductoMonitor);
        Tienda.AgregarProducto(ProductoAuriculares);
    }
}

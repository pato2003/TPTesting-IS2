namespace Tienda.Tests;

public class TiendaFixture
{
    public Tienda Tienda { get; }
    public Producto ProductoRaton { get; }
    public Producto ProductoTeclado { get; }

    public TiendaFixture()
    {
        Tienda = new Tienda();
        ProductoRaton = new Producto("Raton", 10000m, "Accesorios");
        ProductoTeclado = new Producto("Teclado", 5000m, "Accesorios");
        Tienda.AgregarProducto(ProductoRaton);
        Tienda.AgregarProducto(ProductoTeclado);
    }
}

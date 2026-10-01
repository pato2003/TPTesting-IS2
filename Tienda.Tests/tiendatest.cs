using Xunit;

namespace Tienda.Tests;

public class TiendaTests
{
    [Fact]
    public void AgregarProducto_DebeAgregarloAlInventario()
    {
        Tienda tienda = new Tienda();

        Producto producto = new Producto(
            "Mouse",
            10000,
            "Accesorios"
        );

        tienda.AgregarProducto(producto);

        Assert.Contains(producto, tienda.Inventario);
    }

    [Fact]
    public void BuscarProducto_ProductoExistente_DebeEncontrarlo()
    {
        Tienda tienda = new Tienda();

        Producto producto = new Producto(
            "Mouse",
            10000,
            "Accesorios"
        );

        tienda.AgregarProducto(producto);

        Producto? resultado = tienda.BuscarProducto("Mouse");

        Assert.NotNull(resultado);
        Assert.Equal("Mouse", resultado.Nombre);
    }

    [Fact]
    public void BuscarProducto_ProductoInexistente_DebeRetornarNull()
    {
        Tienda tienda = new Tienda();

        Producto? resultado = tienda.BuscarProducto("Teclado");

        Assert.Null(resultado);
    }

    [Fact]
    public void EliminarProducto_DebeEliminarloDelInventario()
    {
        Tienda tienda = new Tienda();

        Producto producto = new Producto(
            "Mouse",
            10000,
            "Accesorios"
        );

        tienda.AgregarProducto(producto);

        bool resultado = tienda.EliminarProducto("Mouse");

        Assert.True(resultado);
        Assert.DoesNotContain(producto, tienda.Inventario);
    }
}
using System;
using System.Collections.Generic;
using Xunit;

namespace Tienda.Tests;

public class CarritoIntegracionTests : IDisposable
{
    private readonly TiendaCarritoFixture _entornoPrueba;
    private readonly Tienda _tienda;

    public CarritoIntegracionTests()
    {
        // Setup: xUnit crea una instancia nueva de la clase por cada test, garantizando aislamiento
        _entornoPrueba = new TiendaCarritoFixture();
        _tienda = _entornoPrueba.Tienda;
    }

    public void Dispose()
    {
        // Teardown: se ejecuta tras cada test para asegurar limpieza
        _tienda.Inventario.Clear();
    }

    [Fact]
    public void CalcularTotalCarrito_ProductosSinDescuento_DebeRetornarSumaDePreciosOriginales()
    {
        // Arrange
        var carrito = new List<string> { "Raton", "Teclado" };

        // Act
        decimal total = _tienda.CalcularTotalCarrito(carrito);

        // Assert
        // Raton (10000) + Teclado (5000) = 15000
        Assert.Equal(15000m, total);
    }

    [Fact]
    public void CalcularTotalCarrito_FlujoConDescuentoEnUnProducto_DebeReflejarPrecioConDescuento()
    {
        // Arrange
        _tienda.AplicarDescuento("Raton", 20m); // Raton queda en 8000
        var carrito = new List<string> { "Raton", "Teclado" };

        // Act
        decimal total = _tienda.CalcularTotalCarrito(carrito);

        // Assert
        // Raton (8000) + Teclado (5000) = 13000
        Assert.Equal(13000m, total);
    }

    [Fact]
    public void CalcularTotalCarrito_DescuentosEnMultiplesProductos_DebeSumarPreciosActualizados()
    {
        // Arrange
        _tienda.AplicarDescuento("Monitor", 10m);     // Monitor: 50000 - 5000 = 45000
        _tienda.AplicarDescuento("Auriculares", 50m); // Auriculares: 20000 - 10000 = 10000
        var carrito = new List<string> { "Monitor", "Auriculares", "Teclado" };

        // Act
        decimal total = _tienda.CalcularTotalCarrito(carrito);

        // Assert
        // Monitor (45000) + Auriculares (10000) + Teclado (5000) = 60000
        Assert.Equal(60000m, total);
    }

    [Fact]
    public void CalcularTotalCarrito_ProductoRepetidoSinDescuento_DebeSumarCadaUnidad()
    {
        // Arrange
        var carrito = new List<string> { "Raton", "Raton" };

        // Act
        decimal total = _tienda.CalcularTotalCarrito(carrito);

        // Assert
        // Raton (10000) + Raton (10000) = 20000
        Assert.Equal(20000m, total);
    }

    [Fact]
    public void CalcularTotalCarrito_ProductoRepetidoConDescuento_DebeSumarCadaUnidadConDescuento()
    {
        // Arrange
        _tienda.AplicarDescuento("Raton", 20m); // Raton queda en 8000
        var carrito = new List<string> { "Raton", "Raton" };

        // Act
        decimal total = _tienda.CalcularTotalCarrito(carrito);

        // Assert
        // Raton (8000) + Raton (8000) = 16000
        Assert.Equal(16000m, total);
    }

    [Fact]
    public void CalcularTotalCarrito_DescuentoCienPorCiento_ProductoAportaCero()
    {
        // Arrange
        _tienda.AplicarDescuento("Teclado", 100m); // Teclado queda en 0
        var carrito = new List<string> { "Raton", "Teclado" };

        // Act
        decimal total = _tienda.CalcularTotalCarrito(carrito);

        // Assert
        // Raton (10000) + Teclado (0) = 10000
        Assert.Equal(10000m, total);
    }

    [Fact]
    public void CalcularTotalCarrito_DescuentoCeroPorCiento_NoAlteraElTotal()
    {
        // Arrange
        _tienda.AplicarDescuento("Raton", 0m); // Raton queda en 10000
        var carrito = new List<string> { "Raton" };

        // Act
        decimal total = _tienda.CalcularTotalCarrito(carrito);

        // Assert
        Assert.Equal(10000m, total);
    }

    [Fact]
    public void CalcularTotalCarrito_DescuentosConsecutivosEnMismoProducto_DebenAcumularse()
    {
        // Arrange
        _tienda.AplicarDescuento("Monitor", 20m); // Monitor: 50000 - 10000 = 40000
        _tienda.AplicarDescuento("Monitor", 50m); // Monitor: 40000 - 20000 = 20000
        var carrito = new List<string> { "Monitor" };

        // Act
        decimal total = _tienda.CalcularTotalCarrito(carrito);

        // Assert
        Assert.Equal(20000m, total);
    }

    [Fact]
    public void CalcularTotalCarrito_FlujoCompletoAgregarDescontarYCalcular_DebeCalcularTotalCorrecto()
    {
        // Arrange
        var nuevoProducto = new Producto("Notebook", 100000m, "Tecnologia");
        _tienda.AgregarProducto(nuevoProducto);
        _tienda.AplicarDescuento("Notebook", 10m); // Notebook: 100000 - 10000 = 90000
        var carrito = new List<string> { "Notebook", "Raton" };

        // Act
        decimal total = _tienda.CalcularTotalCarrito(carrito);

        // Assert
        // Notebook (90000) + Raton (10000) = 100000
        Assert.Equal(100000m, total);
    }

    [Fact]
    public void CalcularTotalCarrito_FlujoConEliminacion_DebeCalcularRestantesYLanzarSiSePideEliminado()
    {
        // Arrange
        _tienda.EliminarProducto("Teclado");
        var carritoValido = new List<string> { "Raton", "Monitor" };
        var carritoConEliminado = new List<string> { "Teclado" };

        // Act & Assert
        // Raton (10000) + Monitor (50000) = 60000
        decimal total = _tienda.CalcularTotalCarrito(carritoValido);
        Assert.Equal(60000m, total);

        Assert.Throws<KeyNotFoundException>(() => _tienda.CalcularTotalCarrito(carritoConEliminado));
    }

    [Fact]
    public void CalcularTotalCarrito_CarritoVacio_DebeRetornarCero()
    {
        // Arrange
        var carrito = new List<string>();

        // Act
        decimal total = _tienda.CalcularTotalCarrito(carrito);

        // Assert
        Assert.Equal(0m, total);
    }

    [Fact]
    public void CalcularTotalCarrito_ProductoInexistente_DebeLanzarKeyNotFoundException()
    {
        // Arrange
        var carrito = new List<string> { "Raton", "Inexistente" };

        // Act & Assert
        Assert.Throws<KeyNotFoundException>(() => _tienda.CalcularTotalCarrito(carrito));
    }

    [Fact]
    public void CalcularTotalCarrito_CarritoNulo_DebeLanzarArgumentNullException()
    {
        // Arrange
        List<string>? carrito = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => _tienda.CalcularTotalCarrito(carrito!));
    }

    [Fact]
    public void CalcularTotalCarrito_DescuentoInvalidoDuranteFlujo_LanzaExcepcionYNoAlteraElTotal()
    {
        // Arrange
        Assert.Throws<ArgumentException>(() => _tienda.AplicarDescuento("Raton", 150m));
        var carrito = new List<string> { "Raton", "Teclado" };

        // Act
        decimal total = _tienda.CalcularTotalCarrito(carrito);

        // Assert
        Assert.Equal(10000m, _entornoPrueba.ProductoRaton.Precio);
        Assert.Equal(15000m, total);
    }

    [Fact]
    public void CalcularTotalCarrito_DescuentoSeReflejaTantoEnInventarioComoEnTotal_SonCoherentes()
    {
        // Arrange
        _tienda.AplicarDescuento("Raton", 30m); // Raton: 10000 - 3000 = 7000
        var carrito = new List<string> { "Raton" };

        // Act
        Producto productoEnInventario = _tienda.BuscarProducto("Raton");
        decimal total = _tienda.CalcularTotalCarrito(carrito);

        // Assert
        Assert.Equal(7000m, productoEnInventario.Precio);
        Assert.Equal(productoEnInventario.Precio, total);
    }
}

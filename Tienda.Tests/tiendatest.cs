using System;
using System.Collections.Generic;
using Xunit;
using Moq;

namespace Tienda.Tests;

public class TiendaTests : IDisposable
{
    private readonly TiendaFixture _entornoPrueba;
    private readonly Tienda _tienda;

    public TiendaTests()
    {
        // Setup: xUnit crea una instancia nueva de esta clase por cada test
        _entornoPrueba = new TiendaFixture();
        _tienda = _entornoPrueba.Tienda;
    }

    public void Dispose()
    {
        // Teardown: se ejecuta después de cada test
        _tienda.Inventario.Clear();
    }

    [Fact]
    public void AgregarProducto_DebeAgregarloAlInventario()
    {
        Producto nuevoProducto = new Producto("Monitor", 50000m, "Tecnologia");

        _tienda.AgregarProducto(nuevoProducto);

        Assert.Contains(nuevoProducto, _tienda.Inventario);
        Assert.Contains(_entornoPrueba.ProductoRaton, _tienda.Inventario);
        Assert.Contains(_entornoPrueba.ProductoTeclado, _tienda.Inventario);
        Assert.Equal(3, _tienda.Inventario.Count);
    }

    [Fact]
    public void BuscarProducto_ProductoExistente_DebeEncontrarlo()
    {
        Producto resultado = _tienda.BuscarProducto("Raton");
        
        Assert.NotNull(resultado);
        Assert.Same(_entornoPrueba.ProductoRaton, resultado);
        Assert.Equal("Raton", resultado.Nombre);
        Assert.Equal(10000m, resultado.Precio);
    }

    [Fact]
    public void BuscarProducto_ProductoInexistente_DebeLanzarExcepcion()
    {
        Assert.Throws<KeyNotFoundException>(() => _tienda.BuscarProducto("Monitor"));
    }

    [Fact]
    public void EliminarProducto_DebeEliminarloDelInventario()
    {
        _tienda.EliminarProducto("Raton");
        
        Assert.DoesNotContain(_entornoPrueba.ProductoRaton, _tienda.Inventario);
        Assert.Contains(_entornoPrueba.ProductoTeclado, _tienda.Inventario);
        Assert.Single(_tienda.Inventario);
    }

    [Fact]
    public void EliminarProducto_ProductoInexistente_DebeLanzarExcepcion()
    {
        Assert.Throws<KeyNotFoundException>(() => _tienda.EliminarProducto("Monitor"));
    }

    [Fact]
    public void AplicarDescuento_ConDatosIniciales_DebeActualizarPrecioCorrectamente()
    {
        _tienda.AplicarDescuento("Raton", 20m);

        Assert.Equal(8000m, _entornoPrueba.ProductoRaton.Precio);
    }

    [Fact]
    public void AplicarDescuento_UsaDobleDePruebaYActualizaPrecio()
    {
        Tienda tienda = new Tienda();
        var productoSimulado = new Mock<Producto>("Raton", 10000m, "Accesorios");
        productoSimulado.SetupGet(p => p.Nombre).Returns("Raton");
        productoSimulado.SetupGet(p => p.Precio).Returns(10000m);
        tienda.AgregarProducto(productoSimulado.Object);

        tienda.AplicarDescuento("Raton", 20m); 

        productoSimulado.Verify(p => p.ActualizarPrecio(8000m), Times.Once);
    }

    [Fact]
    public void AplicarDescuento_ProductoInexistente_DebeLanzarExcepcion()
    {
        Assert.Throws<KeyNotFoundException>(() => _tienda.AplicarDescuento("Monitor", 20m));
    }

    [Theory]
    [InlineData(-0.01)]
    [InlineData(100.01)]
    public void AplicarDescuento_PorcentajeInvalido_DebeLanzarExcepcion(double porcentajeInvalidoValor)
    {
        decimal porcentajeInvalido = (decimal)porcentajeInvalidoValor;
        Tienda tienda = new Tienda();
        var productoSimulado = new Mock<Producto>("Raton", 10000m, "Accesorios");
        productoSimulado.SetupGet(p => p.Nombre).Returns("Raton");
        tienda.AgregarProducto(productoSimulado.Object);
        
        Assert.Throws<ArgumentException>(() => tienda.AplicarDescuento("Raton", porcentajeInvalido));
        
        productoSimulado.Verify(p => p.ActualizarPrecio(It.IsAny<decimal>()), Times.Never);
    }

    [Fact]
    public void AplicarDescuento_CeroPorciento_DebeDejarMismoPrecio()
    {
        Tienda tienda = new Tienda();
        var productoSimulado = new Mock<Producto>("Raton", 10000m, "Accesorios");
        productoSimulado.SetupGet(p => p.Nombre).Returns("Raton");
        productoSimulado.SetupGet(p => p.Precio).Returns(10000m);
        tienda.AgregarProducto(productoSimulado.Object);

        tienda.AplicarDescuento("Raton", 0m);

        productoSimulado.Verify(p => p.ActualizarPrecio(10000m), Times.Once);
    }

    [Fact]
    public void AplicarDescuento_CienPorciento_DebeQuedarEnCero()
    {
        Tienda tienda = new Tienda();
        var productoSimulado = new Mock<Producto>("Raton", 10000m, "Accesorios");
        productoSimulado.SetupGet(p => p.Nombre).Returns("Raton");
        productoSimulado.SetupGet(p => p.Precio).Returns(10000m);
        tienda.AgregarProducto(productoSimulado.Object);

        tienda.AplicarDescuento("Raton", 100m);

        productoSimulado.Verify(p => p.ActualizarPrecio(0m), Times.Once);
    }
}
using System;
using System.Collections.Generic;
using Xunit;
using Moq;

namespace Tienda.Tests;

public class TiendaTests
{
    [Fact]
    public void AgregarProducto_DebeAgregarloAlInventario()
    {
        Tienda tienda = new Tienda();
        Producto producto = new Producto("Mouse", 10000m, "Accesorios");
        tienda.AgregarProducto(producto);
        Assert.Contains(producto, tienda.Inventario);
    }

    [Fact]
    public void BuscarProducto_ProductoExistente_DebeEncontrarlo()
    {
        Tienda tienda = new Tienda();
        Producto producto = new Producto("Mouse", 10000m, "Accesorios");
        tienda.AgregarProducto(producto);
        
        Producto resultado = tienda.BuscarProducto("Mouse");
        
        Assert.Same(producto, resultado);
    }

    [Fact]
    public void BuscarProducto_ProductoInexistente_DebeLanzarExcepcion()
    {
        Tienda tienda = new Tienda();
        Assert.Throws<KeyNotFoundException>(() => tienda.BuscarProducto("Teclado"));
    }

    [Fact]
    public void EliminarProducto_DebeEliminarloDelInventario()
    {
        Tienda tienda = new Tienda();
        Producto producto1 = new Producto("Mouse", 10000m, "Accesorios");
        Producto producto2 = new Producto("Teclado", 5000m, "Accesorios");
        tienda.AgregarProducto(producto1);
        tienda.AgregarProducto(producto2);
        
        tienda.EliminarProducto("Mouse");
        
        Assert.DoesNotContain(producto1, tienda.Inventario);
        Assert.Contains(producto2, tienda.Inventario); // Verifica que el otro producto sigue
    }

    [Fact]
    public void EliminarProducto_ProductoInexistente_DebeLanzarExcepcion()
    {
        Tienda tienda = new Tienda();
        Assert.Throws<KeyNotFoundException>(() => tienda.EliminarProducto("Teclado"));
    }

    [Fact]
    public void AplicarDescuento_UsaMockYActualizaPrecio()
    {
        Tienda tienda = new Tienda();
        var mockProducto = new Mock<Producto>("Mouse", 10000m, "Accesorios");
        mockProducto.SetupGet(p => p.Nombre).Returns("Mouse");
        mockProducto.SetupGet(p => p.Precio).Returns(10000m);
        tienda.AgregarProducto(mockProducto.Object);

        tienda.AplicarDescuento("Mouse", 20m); 

        mockProducto.Verify(p => p.ActualizarPrecio(8000m), Times.Once);
    }

    [Fact]
    public void AplicarDescuento_ProductoInexistente_DebeLanzarExcepcion()
    {
        Tienda tienda = new Tienda();
        Assert.Throws<KeyNotFoundException>(() => tienda.AplicarDescuento("Teclado", 20m));
    }

    [Theory]
    [InlineData(-0.01)]
    [InlineData(100.01)]
    public void AplicarDescuento_PorcentajeInvalido_DebeLanzarExcepcion(double porcentajeInvalidoDouble)
    {
        decimal porcentajeInvalido = (decimal)porcentajeInvalidoDouble;
        Tienda tienda = new Tienda();
        var mockProducto = new Mock<Producto>("Mouse", 10000m, "Accesorios");
        mockProducto.SetupGet(p => p.Nombre).Returns("Mouse");
        tienda.AgregarProducto(mockProducto.Object);
        
        Assert.Throws<ArgumentException>(() => tienda.AplicarDescuento("Mouse", porcentajeInvalido));
        
        mockProducto.Verify(p => p.ActualizarPrecio(It.IsAny<decimal>()), Times.Never);
    }

    [Fact]
    public void AplicarDescuento_CeroPorciento_DebeDejarMismoPrecio()
    {
        Tienda tienda = new Tienda();
        var mockProducto = new Mock<Producto>("Mouse", 10000m, "Accesorios");
        mockProducto.SetupGet(p => p.Nombre).Returns("Mouse");
        mockProducto.SetupGet(p => p.Precio).Returns(10000m);
        tienda.AgregarProducto(mockProducto.Object);

        tienda.AplicarDescuento("Mouse", 0m);

        mockProducto.Verify(p => p.ActualizarPrecio(10000m), Times.Once);
    }

    [Fact]
    public void AplicarDescuento_CienPorciento_DebeQuedarEnCero()
    {
        Tienda tienda = new Tienda();
        var mockProducto = new Mock<Producto>("Mouse", 10000m, "Accesorios");
        mockProducto.SetupGet(p => p.Nombre).Returns("Mouse");
        mockProducto.SetupGet(p => p.Precio).Returns(10000m);
        tienda.AgregarProducto(mockProducto.Object);

        tienda.AplicarDescuento("Mouse", 100m);

        mockProducto.Verify(p => p.ActualizarPrecio(0m), Times.Once);
    }
}
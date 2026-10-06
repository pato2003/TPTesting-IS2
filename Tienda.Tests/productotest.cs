using System;
using Xunit;

namespace Tienda.Tests;

public class ProductoTests
{
    [Fact]
    public void ActualizarPrecio_PrecioValido_DebeCambiarPrecio()
    {
        Producto producto = new Producto("Mouse", 10000m, "Accesorios");
        producto.ActualizarPrecio(15000m);
        Assert.Equal(15000m, producto.Precio);
    }

    [Fact]
    public void ActualizarPrecio_PrecioCero_DebePermitirse()
    {
        Producto producto = new Producto("Mouse", 10000m, "Accesorios");
        producto.ActualizarPrecio(0m);
        Assert.Equal(0m, producto.Precio);
    }

    [Fact]
    public void ActualizarPrecio_PrecioNegativo_DebeLanzarExcepcionYNoCambiarPrecio()
    {
        Producto producto = new Producto("Mouse", 10000m, "Accesorios");
        
        Assert.Throws<ArgumentException>(() => producto.ActualizarPrecio(-500m));
        Assert.Equal(10000m, producto.Precio);
    }
}

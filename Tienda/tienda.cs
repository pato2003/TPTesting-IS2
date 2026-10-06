using System;
using System.Collections.Generic;

namespace Tienda;

public class Tienda
{
    public List<Producto> Inventario { get; set; }

    public Tienda()
    {
        Inventario = new List<Producto>();
    }

    public void AgregarProducto(Producto producto)
    {
        Inventario.Add(producto);
    }

    public Producto BuscarProducto(string nombre)
    {
        foreach (Producto producto in Inventario)
        {
            if (producto.Nombre == nombre)
            {
                return producto;
            }
        }

        throw new KeyNotFoundException($"Producto '{nombre}' no encontrado.");
    }

    public void EliminarProducto(string nombre)
    {
        Producto producto = BuscarProducto(nombre);
        Inventario.Remove(producto);
    }

    public void AplicarDescuento(string nombre, decimal porcentaje)
    {
        if (porcentaje < 0 || porcentaje > 100)
        {
            throw new ArgumentException("El porcentaje de descuento debe estar entre 0 y 100.");
        }

        Producto producto = BuscarProducto(nombre);
        decimal descuento = producto.Precio * (porcentaje / 100m);
        producto.ActualizarPrecio(producto.Precio - descuento);
    }
}
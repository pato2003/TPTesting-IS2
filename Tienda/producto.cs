namespace Tienda;

public class Producto
{
    public string Nombre { get; set; }
    public double Precio { get; set; }
    public string Categoria { get; set; }

    public Producto(string nombre, double precio, string categoria)
    {
        Nombre = nombre;
        Precio = precio;
        Categoria = categoria;
    }
}
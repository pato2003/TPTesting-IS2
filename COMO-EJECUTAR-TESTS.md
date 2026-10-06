# Instructivo: Cómo ejecutar las pruebas de cada punto

Abrí la terminal en la raíz del proyecto (donde está el archivo `TPTestingIS2.sln`) y usá los siguientes comandos para comprobar que cada punto del TP funcione correctamente.

---

## Ejecutar todos los tests juntos

Para verificar que todo el proyecto compile y pase las 30 pruebas:

```bash
dotnet test
```

*(Resultado esperado: **Total: 30, Superado: 30, Con error: 0**).*

Si querés ver el nombre de cada test en pantalla mientras corre:

```bash
dotnet test --logger "console;verbosity=normal"
```

---

## Punto 1: Pruebas Básicas

Comprueba el funcionamiento básico de agregar, buscar y eliminar un producto del inventario:

```bash
dotnet test --filter "FullyQualifiedName~TiendaTests.AgregarProducto|FullyQualifiedName~TiendaTests.BuscarProducto_ProductoExistente|FullyQualifiedName~TiendaTests.EliminarProducto_DebeEliminarloDelInventario"
```

- **Resultado esperado:** Pasan **3 tests**.
- **Qué valida:** Que el producto se agregue al inventario, que buscar por nombre retorne el producto existente y que eliminarlo lo quite de la tienda.

---

## Punto 2: Pruebas con Excepciones

Comprueba que el sistema lance las excepciones correspondientes ante casos inválidos o productos inexistentes:

```bash
dotnet test --filter "FullyQualifiedName~ProductoTests|FullyQualifiedName~TiendaTests.BuscarProducto_ProductoInexistente|FullyQualifiedName~TiendaTests.EliminarProducto_ProductoInexistente"
```

- **Resultado esperado:** Pasan **5 tests**.
- **Qué valida:** Que `Producto.ActualizarPrecio` lance `ArgumentException` ante un precio negativo, y que `BuscarProducto` y `EliminarProducto` en `Tienda` lancen `KeyNotFoundException` si el producto no existe.

---

## Punto 3: Dobles de Prueba (Mocks con Moq)

Comprueba el método `AplicarDescuento` aislando a la clase `Producto` mediante un objeto simulado (Mock):

```bash
dotnet test --filter "FullyQualifiedName~AplicarDescuento_UsaDobleDePrueba|FullyQualifiedName~AplicarDescuento_PorcentajeInvalido|FullyQualifiedName~AplicarDescuento_CeroPorciento|FullyQualifiedName~AplicarDescuento_CienPorciento"
```

- **Resultado esperado:** Pasan **5 tests** (incluye las 2 variantes de porcentaje inválido).
- **Qué valida:** Que `Tienda.AplicarDescuento` calcule bien el nuevo precio y llame a `ActualizarPrecio` sobre el mock, sin usar un objeto real.

---

## Punto 4: Uso de Fixtures

Comprueba que las pruebas de la tienda utilicen el `TiendaFixture` para precargar datos de ejemplo antes de cada test:

```bash
dotnet test --filter "FullyQualifiedName~Tienda.Tests.TiendaTests"
```

- **Resultado esperado:** Pasan **12 tests**.
- **Qué valida:** Que la clase `TiendaTests` inicialice un fixture fresco con productos de prueba (`Raton` y `Teclado`) en cada ejecución, sin repetir código de creación manual.
- *(Opcional, solo agregar y buscar del enunciado):*
  ```bash
  dotnet test --filter "FullyQualifiedName~TiendaTests.AgregarProducto|FullyQualifiedName~TiendaTests.BuscarProducto"
  ```
  *(Pasan **3 tests**).*

---

## Punto 5: Pruebas de Integración y Cobertura

### 1. Pruebas de integración del carrito
Comprueba el flujo completo del sistema: agregar productos, aplicar descuentos y calcular el total del carrito con objetos reales integrados:

```bash
dotnet test --filter "FullyQualifiedName~CarritoIntegracionTests"
```

- **Resultado esperado:** Pasan **15 tests**.
- **Qué valida:** Que `CalcularTotalCarrito` sume los precios reales actualizados con descuentos, maneje productos repetidos, carritos vacíos, productos eliminados y excepciones de nulidad o producto inexistente.

### 2. Medir cobertura de código
Para verificar la cobertura de sentencias y ramas con Coverlet:

```bash
dotnet test --collect:"XPlat Code Coverage"
```

- **Resultado esperado:** Pasan los **30 tests** y genera el reporte `coverage.cobertura.xml` dentro de `Tienda.Tests/TestResults/`.
- **Cobertura obtenida:** **100% de líneas** (56/56) y **100% de ramas** (14/14).

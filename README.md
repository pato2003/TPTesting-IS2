# Trabajo Práctico 1 - Pruebas del software


### Punto 1
**¿Puedes identificar pruebas de unidad y de integración en la práctica que se realizó?**
Sí. En la práctica, los tests que le hicimos a la clase `Producto` (como probar `ActualizarPrecio`) son puramente **pruebas de unidad**, porque estamos probando el comportamiento aislado de esa clase. 
Por otro lado, cuando probamos los métodos de `Tienda` (como agregar o buscar) y le pasamos objetos reales de la clase `Producto`, técnicamente ya estamos haciendo **pruebas de integración**. Esto pasa porque estamos viendo cómo interactúan dos clases reales juntas.

### Punto 2
**¿Podría haber escrito las pruebas primero antes de modificar el código de la aplicación? ¿Cómo sería el proceso de escribir primero los tests? Describe el proceso con tus palabras.**
Totalmente, se podría hacer al revés. Ese enfoque es el famoso TDD (Test-Driven Development).
Si lo hiciéramos así, el ciclo sería:
1. Pienso qué funcionalidad quiero y escribo el test primero. Obvio que si lo ejecuto va a tirar error (es lo que se llama la **fase roja**), porque la lógica todavía no existe o ni siquiera compila.
2. Después me voy al código fuente y escribo el código mínimo e indispensable para que ese test pase. Vuelvo a correr el test y, si pasa, estamos en la **fase verde**.
3. Por último, sabiendo que el test me cubre las espaldas, refactorizo el código para dejarlo prolijo, optimizado y sin duplicaciones.

### Punto 3
**En lo que va del trabajo práctico, ¿puedes identificar 'Controladores' y 'Resguardos'?**
Los **Controladores** son básicamente nuestros propios tests , ya que se encargan de instanciar, invocar y "manejar" a la unidad que queremos probar. 
Los **Resguardos** (o stubs) son los objetos "truchos" que le inyectamos a la unidad para aislarla y no depender de módulos reales. En nuestro caso, el mock de `Producto` que armamos con la librería Moq para aislar el descuento cumple justamente el rol de resguardo/stub.

**¿Qué es un "test double"? ¿Hay otros nombres para los objetos/funciones simulados?**
Un "test double"  es un término general que usamos en testing (como los dobles de riesgo de las películas) para referirnos a un objeto falso que reemplaza a un componente real, con el objetivo de aislar lo que queremos probar.
Dependiendo de qué tan complejos sean o para qué los usemos en el test, a los dobles de prueba también se los llama:
- **Mock**: Verifica interacciones (ej: "fijate si te llamaron al método X tantas veces").
- **Stub**: Solo devuelve datos hardcodeados para que el test fluya.
- **Fake**: Es una implementación de verdad pero simplificada para testear rápido (por ejemplo, una base de datos "trucha" en memoria).
- **Dummy**: Un objeto de relleno que se pasa como parámetro porque es obligatorio, pero que nunca se usa realmente.
- **Spy**: Un envoltorio que guarda registro de cómo y cuándo llaman a la clase para después poder inspeccionarlo.

### Punto 4
**Defina usando palabras propias y según la práctica realizada qué es un fixture.**
Un fixture es un conjunto de datos o un estado base conocido y predeterminado que preparamos para que los tests se ejecuten de manera consistente, repetible y controlada.
En nuestra práctica con C# y xUnit, el fixture es la clase `TiendaFixture`, donde instanciamos la `Tienda` y le precargamos productos de prueba (`Raton`, `Teclado`). Al instanciar este fixture en el constructor de la clase de pruebas antes de cada test, evitamos repetir código de inicialización y aseguramos que cada prueba inicie desde un estado limpio y predecible.

**¿Qué ventajas ve en el uso de fixtures? ¿Qué enfoque de diseño de pruebas estaríamos aplicando (caja negra/blanca)?**
- **Ventajas:**
  - **Evita la repetición de código (DRY)**: Centraliza la creación y configuración inicial de los objetos de prueba.
  - **Mantenibilidad**: Si el día de mañana cambia el constructor de `Producto` o de `Tienda`, solo modificamos el fixture en un único lugar en vez de arreglar decenas de tests individuales.
  - **Aislamiento e independencia entre tests**: Al instanciarse un fixture nuevo antes de cada prueba, aseguramos que ningún test sufra efectos secundarios o datos alterados por una prueba previa.
  - **Claridad**: Los métodos de prueba quedan mucho más concisos, enfocándose directo en la acción (`Act`) y la aserción (`Assert`).
- **Enfoque de diseño (caja negra / blanca):**
  - Al diseñar las pruebas que consumen el fixture, nos basamos principalmente en la especificación funcional y en el contrato público de `Tienda` (ej. "si busco un producto existente por su nombre me debe retornar el producto", "si intento eliminar uno que no está debe lanzar una excepción"). Esto responde a un enfoque de **caja negra**, ya que evaluamos las entradas y salidas esperadas sin depender de la estructura interna.
  - Si bien tenemos en cuenta las bifurcaciones y validaciones del código (como el control de nombres o el lanzamiento de excepciones) para diseñar casos que pasen por esos caminos, el enfoque aplicado es predominantemente de caja negra (la medición formal y sistemática de cobertura de código por caja blanca se abordará en el Punto 5).

**Explique los conceptos de Setup y Teardown en testing.**
- **Setup:** Es la fase de preparación previa a la ejecución de las pruebas para dejar todo en un estado limpio y predecible. En xUnit, a diferencia de otros frameworks que usan atributos como `[SetUp]`, el Setup por prueba se implementa de forma idiomática mediante el **constructor** de la clase de prueba, ya que xUnit crea una instancia nueva de la clase por cada test que ejecuta. En nuestro código, usamos el constructor como Setup para instanciar un `TiendaFixture` nuevo por cada prueba. La alternativa `IClassFixture` se reserva para recursos costosos compartidos por toda la clase de prueba (como una conexión a base de datos o un servidor web), pero no para fixtures de datos que deben aislarse por test.
- **Teardown:** Es la fase de desmontaje o limpieza posterior a la ejecución de las pruebas para no dejar efectos residuales. En xUnit y .NET, el Teardown por prueba se realiza implementando la interfaz **`IDisposable`** y su método **`Dispose()`** (en nuestro caso, limpiando el inventario de la tienda al finalizar cada test).

### Punto 5
**¿Realizó una prueba de cobertura completa? ¿Qué tipo de cobertura utilizó?**
Sí, realizamos una medición real de cobertura automatizada utilizando la herramienta `coverlet` (`dotnet test --collect:"XPlat Code Coverage"`). Evaluamos tanto la cobertura de **sentencias (líneas de código)** como la cobertura de **decisión o ramas (branches)**:

- **Cobertura de sentencias / líneas**: Obtuvimos un **100%** de cobertura en el código de producción (`56/56` líneas ejecutadas). Tanto en la clase `Producto` (`15/15` líneas, 100%) como en `Tienda` (`41/41` líneas, 100%), cada una de las líneas ejecutables fue transitada al menos una vez por la suite de pruebas.
- **Cobertura de ramas / decisiones**: También alcanzamos un **100%** de cobertura de ramas (`14/14` ramas cubiertas). Se ejercitaron todas las bifurcaciones lógicas del sistema: la validación de precio negativo en `Producto.ActualizarPrecio` (2/2 ramas), el bucle y la condición de búsqueda en `BuscarProducto` (4/4 ramas), el rango permitido de porcentaje (0 a 100) en `AplicarDescuento` (4/4 ramas) y la verificación de nulo junto al bucle en `CalcularTotalCarrito` (4/4 ramas).

Es importante señalar las diferencias conceptuales entre los tipos de cobertura:
- **Cobertura de sentencias (Line/Statement Coverage)**: Mide qué porcentaje de líneas de código se ejecutaron al menos una vez. Es la métrica más básica, ya que una línea puede ejecutarse sin haber probado todos los caminos lógicos posibles (por ejemplo, evaluar un `if` solo cuando es verdadero sin verificar el camino alternativo).
- **Cobertura de decisión o ramas (Branch Coverage)**: Exige que cada estructura de control condicional tome tanto el valor verdadero como el falso. Es más estricta que la de sentencias porque obliga a transitar todas las bifurcaciones del flujo de control.
- **Cobertura de condición (Condition Coverage)**: Requiere evaluar de forma independiente cada sub-condición booleana elemental dentro de una expresión lógica compuesta (por ejemplo, en `porcentaje < 0 || porcentaje > 100`, probar casos donde la primera sea verdadera, donde la segunda sea verdadera y donde ambas sean falsas).

Por último, es fundamental destacar que **alcanzar el 100% de cobertura no garantiza la ausencia total de defectos (bugs)**. La cobertura solo nos indica qué partes del código existente fueron transitadas por los tests, pero no puede detectar requerimientos omitidos, especificaciones incompletas, problemas de concurrencia o errores en las aserciones de prueba si estas no verifican adecuadamente el comportamiento esperado.

**¿Puede describir una situación de desarrollo para este caso en donde se plantee pruebas de integración ascendente? Describa la situación.**
La **integración ascendente (bottom-up)** es una estrategia de pruebas de integración donde se integran y prueban primero los componentes de más bajo nivel en la jerarquía del sistema (aquellos que no tienen dependencias hacia otros módulos de la aplicación). A medida que estos módulos base se verifican y estabilizan, se avanza progresivamente hacia los módulos de niveles superiores que dependen de ellos. En este enfoque se utilizan **controladores de prueba (drivers)** —que son las propias clases y métodos de test— para invocar, enviar datos y coordinar a los módulos bajo prueba en ausencia de módulos superiores reales. Su principal ventaja es que no requiere construir **resguardos (stubs)** para los niveles inferiores (ya que estos ya existen y están probados), permitiendo detectar fallos en los cimientos del sistema de forma temprana.

En el contexto de nuestra tienda, una situación concreta de desarrollo ascendente se estructuró en cuatro niveles sucesivos:
1. **Nivel 1 (Módulo base independiente - `Producto`)**: Se desarrolló primero la clase `Producto` con su constructor y su método `ActualizarPrecio`. Al no depender de ninguna otra clase del dominio, se probó de forma unitaria con un controlador (`ProductoTests`), verificando que los atributos se asignaran correctamente y que se lanzara una excepción ante precios negativos.
2. **Nivel 2 (Gestión básica de inventario - `Tienda`)**: Con `Producto` probado y estable, se construyó la clase `Tienda` con sus métodos de inventario (`AgregarProducto`, `BuscarProducto`, `EliminarProducto`), integrándola directamente con objetos reales de `Producto`. La clase `TiendaTests` actuó como controlador para validar que los productos se agregaran, buscaran y eliminaran con éxito dentro de la colección.
3. **Nivel 3 (Lógica de negocio intermedia - `AplicarDescuento`)**: Sobre `Tienda`, se agregó la lógica para modificar precios porcentualmente (`AplicarDescuento`). Esta función integra la búsqueda dentro del inventario de `Tienda` con la mutación del estado interno a través del método `ActualizarPrecio` de `Producto`.
4. **Nivel 4 (Nivel superior / Flujo completo - `CalcularTotalCarrito`)**: Finalmente, se implementó la función de más alto nivel, `CalcularTotalCarrito`. Este método depende de todo lo anterior: recibe una lista de nombres, busca cada producto en el inventario mediante `BuscarProducto`, recupera el precio actual (pudiendo haber recibido descuentos previos) y calcula el importe total. A través de `CarritoIntegracionTests`, el controlador puso a prueba el flujo completo de punta a punta, validando que todas las piezas ensambladas trabajaran de forma armónica sobre una base ya testeada y libre de fallos básicos.



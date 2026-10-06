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


# Guía y resumen de materiales – Unidad 4 C#

## 1. Documentos teóricos

### 1.1 Introducción al lenguaje C# (1.1 Introduccion al lenguaje CSharp.pdf)
- Presenta la motivación de C# como lenguaje nativo de .NET con foco en sencillez, modernidad, POO y orientación a componentes, destacando soporte para propiedades, eventos, atributos y recolección de basura.
- Resume garantías de seguridad de tipos (conversiones controladas, variables inicializadas, verificación de índices) y compatibilidad con punteros en bloques `unsafe` cuando se necesita rendimiento.
- Incluye primer programa "Hola, mundo" y repaso de aspectos léxicos: comentarios, identificadores sensibles a mayúsculas, lista de palabras reservadas, literales numéricos/booleanos/caracter y operadores ordenados por precedencia.
- Describe reglas de ámbito, necesidad de inicialización previa y E/S básica en consola (`Console.WriteLine`, `Console.ReadLine`).
- Explica el sistema unificado de tipos de .NET (tipos por valor vs referencia, uso de `new`, conversiones implícitas/explicitas, polimorfismo mediante `object` y delegados) y detalla boxing/unboxing para tratar valores como objetos.

### 1.2 Tipos de datos en .NET (1.2 Tipos de datos en .net.pdf)
- Clasifica los tipos en por valor, por referencia y `string`, describiendo características de almacenamiento, copia, necesidad de `new` y soporte para `null`.
- Enumera los tipos primitivos por valor (enteros con/sin signo, `float`, `double`, `decimal`, `bool`, `char`) con rangos y precisión, resaltando ventajas de `decimal` para cálculos financieros.
- Muestra cómo definir tipos por valor personalizados con `enum` y `struct`, resaltando diferencias frente a clases (sealed, sin herencia, `this` asignable).
- Recorre tipos por referencia (`object`, arreglos, clases, interfaces, delegados) con ejemplos completos de sintaxis e implementación.
- Dedica una sección a `string` como tipo inmutable con semántica de copia, soporte para Unicode y hasta 1 GB de longitud.
- Segunda parte profundiza en implementación interna de tipos por valor y referencia, justifica el mecanismo de boxing/unboxing y ejemplifica uso de contenedores como `ArrayList` para almacenar valores heterogéneos.

### 1.3 Conversiones de datos (1.3 Conversiones de datos.pdf)
- Define formalmente conversiones boxing (valor→referencia) a `object`, `System.ValueType`, interfaces implementadas o `System.Enum`, ilustrando el modelo conceptual con clases `T_Box`.
- Explica unboxing (referencia→valor) con validaciones de tiempo de ejecución y excepciones `NullReferenceException` o `InvalidCastException` ante incompatibilidades.
- Introduce conversiones definidas por el usuario en clases/struct (`operator implicit/explicit`), detallando restricciones: tipos distintos, uno perteneciente al tipo que declara el operador, exclusión de `object`/interfaces y reglas de jerarquía.
- Describe el proceso de selección de operadores de conversión cuando existen múltiples candidatos, considerando clases base y accesibilidad.

### 1.4 Excepciones (1.4 Excepciones.pdf)
- Repasa el modelo de excepciones de .NET (similar a C++/Java): `throw`, bloques `try/catch/finally`, y jerarquía basada en `System.Exception` con propiedades `Message`, `StackTrace`, `HelpLink`, `InnerException`.
- Incluye ejemplos de múltiples `catch` específicos (`ArithmeticException`, `ArgumentNullException`) y genérico, mostrando cómo manejar división por cero, entradas vacías y otras fallas.
- Detalla comportamiento del CLR al recorrer la pila en busca de manejadores, la importancia del orden de `catch`, y escenarios donde finaliza la aplicación si la excepción no se controla.
- Resume características recomendadas para tipos de excepción personalizados y buenas prácticas (no abusar de excepciones, usar `finally` para liberar recursos).

### 3.1 Entity Framework Core introducción (3.1 Entity Framework Core introducción.pdf)
- Introduce EF como ORM de Microsoft que abstrae ADO.NET, describiendo escenarios Database First, Code First y Model First; aclara que en .NET Core se utilizan Code First y Database First.
- Explica qué es un ORM, componentes (clases de dominio, objetos de BD, metadatos de mapeo) y ventajas en mantenibilidad y automatización CRUD.
- Documenta prerequisitos y paquetes NuGet necesarios (`Microsoft.EntityFrameworkCore.SqlServer`, `Microsoft.EntityFrameworkCore.Tools`) y muestra flujos de instalación desde Visual Studio o consola.
- Para Database First, muestra uso de `Scaffold-DbContext 
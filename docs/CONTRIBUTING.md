# English ([Español](#español))

## Contributing Guidelines

This is an open source project, and if you believe you can contribute to it, please, by all means read this document and start contributing.

### Contributing in Source Code

Any and all contributions must have a related issue in order to justify the code change.  It may be a bug report (and therefore you are contributing with a fix), or it could be a feature request (and therefore you are contributing with new code).

#### Source Code Guidelines

All code submitted must be C#, except for usage examples; usage examples can be in any .Net language.  Pascal case convention is applied to class, property, event, method, and delegate names, and camel case convention is used for fields and variables.  As much as possible, all fields must be either `private` or `protected`.  If any is needed as `internal` or `public`, make them a property.

**IMPORTANT**:  Do not use the underscore character in any identifier (name).  There is just one exception to this rule in the following paragraph.

Whenever a property must be backed with a private field, prepend `m_` to the field name (written in camel case).  I know this is an old convention from the VB4 (maybe earlier) days, but it is useful to quickly access backing fields via Intellisense.

Namespaces should follow Pascal casing as well, but if it is all an acronym (like WJ, which stands for webJose), declare it all lowercase.  If the namespace is the union of two acronyms, use pascal case.  As an example, the resulting namespace of joining **SQL** (Structured Query Language) with **DAL** (Data Access Layer) must be declared as **SqlDal** as opposed to **sqldal**.  Minimize as much as possible the namespace depth.

Always, *always* avoid the use of `var`.  It is my strongest belief that explicit data types help coders understand code.  Only use `var` whenever anonymous types are involved.

Avoid over-using LINQ.  I know, it is super simple to use and super nice, but it can be abused and performance can go down, especially with `IEnumerable`-only extensions.  Always code with the following in mind, which are very few but important to-do's:

1. **What I code must be easy to use and difficult to misuse.**
2. All code that I write focuses as much as possible in performance, without compromising #1 above.
3. I must never copy/paste code.  If I find myself doing this more than twice for a piece of code, I must and will refactor it into a reusable form.
4. I will never add `try..catch` statements with empty `catch` blocks or that simply re-throw.  If I do not have useful code for the `catch` block, then I will use no `try..catch` at all.
5. I will always use the `using` statement with objects that implement `IDisposable`, and I will always implement `IDisposable` in classes with properties or fields of data types that implement `IDisposable` or if the class handles unmanaged resources.

There are many, many more, but I think the above may be the most important ones.

Lines in code files should not exceed 100 characters whenever possible, to be considerate with users with screens with lower resolutions or that must use large fonts or zooming to read comfortably.  The character limit per line is therefore meant to minimize horizontal scrolling.

If a class must use nested classes, make the parent class a `partial` class and write each nested class in its own code file.  The code file must be named **ParentClass-NestedClass.cs**.

Apply a single attribute per line.  Avoid using a single line syntax.  In an example:

```c#
//DO NOT!!
[Browsable(false), DefaultValue(0)]
public int ReferenceId { get; set; }

//OK.
[Browsable(false)]
[DefaultValue(0)]
public int ReferenceId { get; set; }
```

This is so it is trivial to remove a single attribute cleanly, should the need arises.

##### XML Comments

All source code must have [XML comments][1] in well-written English, meaning full sentences and proper punctuation.  Classes, properties, methods, delegates, events, fields, operators, and constructors must all have XML comments (and probably other elements I may be forgetting).

The summary of properties must start with `Gets or sets` if both get and set accessors are `public`.  If only one of them is public, then it must start with `Gets` or `Sets`, matching the public accessor.  If no accessor is public, re-apply the rule replacing `public` with `protected`, and if not, `internal`.  If both accessors are `private` then start the summary with `Gets or sets`.  Needless to say, if one of the accessors is missing then all of the above is irrelevant and simply start with the appropiate word for the only accessor that exists.

Whenever a property is backed with a private field, it is sufficient to write in the summary of this field `Backing field for the <code>PropertyName</code> property.`.

All XML comments for methods must include `<exception>` elements after the `<returns>` element (if the method returns something) for every exception type that is explicitly thrown from code.  Include exception types from other routines called as much as possible.  For best results and to minimize redoing this step, document exceptions at the very end of the development, just before committing.

Start constructor summaries with `Creates a new instance of this class` and continue describing the conditions under which the instance is created.  For example, if the constructor takes a Boolean value that is meant to enable functionality, the comment summary could read `Creates a new instance of this class enabling or disabling XXX functionality as requested via the <paramref name="paramName"/> parameter.`.

##### Regions

Group all class functionality into [#regions][2].  There must be one blank line after the opening `#region` statement, and no blank line before the closing `#endregion` statement.

In any class, the static members go first, and all are enclosed into a region that must be named **Static Section**.  Inside this region, create a region for properties named **Properties**, and a region for methods named **Methods**.  If there is a static constructor, since there cannot be more than one, do not enclose with a region but place it at the beginning of the static section.  The region for operators must be named **Operators**.

**NOTE**:  If it is a static class, having the **Static Section** region is unnecessary.

Now instance members come next.  A **Properties** region is the first of these.  After this one, all constructors, the destructor and `IDisposable` implementation comes next.  The region must be named **Constructors** if it is a class that does not implement `IDisposable`; if it implements it, the region must be named **Constructors, Destructor & IDisposable**.  Then comes the **Methods** region and encloses all methods.

Interface implementations are grouped together, effectively pulling properties, methods and events out of their usual regions.  The region name must be equal to the interface name, optionally with its full namespace, and go after the **Methods** region.

If a class is highly specialized, like a MVC controller where there are very specific methods referred to as *actions*, or a unit test class where there are specific methods referred to as *tests*, it is desirable to create a region just for them.  A suitable name for the actions in an MVC controller could be **Action Methods**; a suitable name for unit test methods could be **Test Methods**.  These special regions go after the **Methods** region.

***

# Español

## Guía para Contribuciones

Este proyecto es de código abierto, y si usted cree que puede contribuir a él, por favor siéntase en libertad de leer este documento y contribuir.

### Contribuyendo con Código Fuente

Toda contribución debe tener un reporte relacionado que justifique el cambio del código.  Puede ser un reporte de defecto (y por lo tanto usted estaría contribuyendo con un arreglo), o puede ser un reporte de funcionalidad nueva (y por lo tanto usted estaría contribuyendo con código nuevo).

#### Guía para Código Fuente

Todo el código confirmado debe ser C#, excepto por ejemplos de uso; ejemplos de uso pueden estar en cualquier lenguaje de .Net.  La convención de nombres Pascal se utiliza para nombres de clases, propiedades, eventos, métodos y delegados, y la convención de nombres camello se utiliza para nombres de campos y variables.  Tanto como fuere posible, todos los campos deben ser `private` o `protected`.  Si se necesitara alguno `internal` o `public`, hágalos una propiedad.

**IMPORTANTE**:  No utilice guión bajo (subrayado) en ningún identificador (nombre).  Solamente hay una excepción a esta regla descrita en el párrafo siguiente.

Cuando una propiedad debe soportarse con un campo privado, prefije el nombre del campo (escrito con la convención camello) con `m_`.  Yo sé que es una convención de los días de VB4 (o tal vez antes), pero es útil para acceder rápidamente a los campos de soporte vía Intellisense.

Los espacios de nombre también deben usar la convención Pascal, pero si es completamente un acrónimo (como WJ, que significa **webJose**), declárelo todo en minúsculas.  Si el espacio de nombres es la unión de dos acrónimos entonces utilice la convención Pascal.  Por ejemplo, el nombre resultante de unir **SQL** (Structured Query Language) con **DAL** (Data Access Layer) debe declararse como **SqlDal** y no como **sqldal**.  Minimize tanto como sea posible los niveles de los espacios de nombres.

Siempre, *siempre* evite el uso de `var`.  Creo profundamente que los tipos de datos explícitos ayudan a los desarrolladores a entender código.  Solamente utilice `var`cuando hay tipos anónimos involucrados.

Evite sobreutilizar LINQ.  Yo sé, es súper simple de usar y súper bonito, pero puede abusarse y el desempeño puede disminuir, especialmente con las extensiones para `IEnumerable`.  Siempre programe con lo siguiente en mente, que son unas pocas pero muy importantes normativas:

1. **Lo que yo programo debe ser fácil de usar y difícil de abusar.**
2. Todo código que escribo se enfoca tanto como es posible en desempeño, sin comprometer el punto #1 arriba.
3. Nunca debo copiar/pegar código.  Si me encuentro haciendo esto más de dos veces para una pieza de código, debo refactorizarlo en una forma reutilizable.
4. Nunca agregaré sentencias `try..catch` con bloques `catch` vacíos o que simplemente arrojan la excepción capturada.  Si no tengo código útil para el bloque `catch`entonces eliminaré del todo la sentencia `try..catch`.
5. Siempre usaré la sentencia `using` con objetos que implementan `IDisposable`, y siempre implementaré `IDisposable` en clases con propiedades o campos cuyo tipo de dato implemente `IDisposable` or si la clase manipula recursos no administrados por .Net.

Existen muchas, muchas más, pero creo que las normativas listadas son las más importantes.

Las líneas en archivos de código no deberían exceder los 100 carácteres siempre que sea posible, esto para ser considerados con los usuarios cuyas pantallas son de baja resolución o que deben utiliar tamaños de fuente grandes o aumento para leer confortablemente.  El límite de carácteres existe, por lo tanto, para minimizar el desplazamiento horizontal.

Si una clase necesitara clases anidadas, la clase padre debe declararse `partial` y cada clase anidada se escribe en su propio archivo de código.  El nombre de dicho archivo de código debe ser de la forma **ClasePadre-ClaseAnidada.cs**.

Aplique un único atributo por línea.  Evite el uso de sintaxis de una línea.  En ejemplo:

```c#
//¡¡NO HACER!!
[Browsable(false), DefaultValue(0)]
public int ReferenceId { get; set; }

//OK.
[Browsable(false)]
[DefaultValue(0)]
public int ReferenceId { get; set; }
```

Esto se hace así para que sea trivial eliminar un atributo a la vez limpiamente, en caso de que resultara necesario.

##### Comentarios XML

Todo código fuente debe tener [comentarios XML][1] en inglés correctamente escrito; entiéndase oraciones completas y puntuación.  Clases, propiedades, métodos, delegados, eventos, campos, operadores y constructores, todos ellos deben tener comentarios XML (y probablemente otros elementos que escapan mi memoria).

El resumen de las propiedades debe empezar con `Gets or sets` si ambos accesores son `public`.  Si solamente uno de ellos lo es, entonces debe iniciar con `Gets` o `Sets` según el accesor que sea público.  Si no hay accesores públicos, aplique estsa regla para accesores `protected`, y si no, `internal`.  Si ambos accesores son `private` entonces inicie el resumen con `Gets or sets`.  Va sin decir que si uno de los accesores no existe entonces todo lo anterior es irrelevante y simplemente inicie con la palabra apropiada para el único accesor que existe.

Cuando una propiedad se soporta con un campo privado, es suficiente escribir en el resumen de este campo `Backing field for the <code>PropertyName</code> property.`.

Todo comentario XML para métodos debe incluir elementos `<exception>` después del elemento `<returns>` (si el método retorna algo) para cada tipo de excepción que se arroje explícitamente en código.  Incluya los tipos de excepción de otras rutinas utilizadas tanto como sea posible.  Para obtener los mejores resultados y para minimizar la necesidad de hacer esto repetidamente, documente estas excepciones justo al final del desarrollo, justo antes de confirmar cambios en GIT.

Inicie los resúmenes de constructores con `Creates a new instance of this class` y continúe describiendo las condiciones bajo las cuales se crea dicha instancia.  Por ejemplo, si el constructor admite un valor Booleano que se usa para habilitar alguna función, el resumen podría leer algo como `Creates a new instance of this class enabling or disabling XXX functionality as requested via the <paramref name="paramName"/> parameter.`.

##### Regiones

Agrupe toda la funcionalidad de la clase en [#regions][2].  Debe dejarse una línea en blanco después de la sentencia de apertura `#region` y ninguna línea en blanco antes de la sentencia de cierre `#endregion`.

En toda clase, los miembros estáticos van primero, y todos deben encerrarse en una región llamada **Static Section**.  Dentro de esta región, cree una región para propiedades llamada **Properties** y una región para métodos llamada **Methods**.  Si hay un constructor estático, puesto que no puede haber más de uno, no se encierra en una región pero se coloca al inicio de la sección estática.  La región para operadores debe llamarse **Operators**.

**NOTA**:  Si es una clase estática, es innecesario tener la región **Static Section**.

Luego siguen los miembros de instancia.  La región **Properties** va primero.  Después van los constructores, el destructor y la implementación de `IDisposable`.  La región debe llamarse **Constructors** si la clase no implementa `IDisposable`, pero si lo hace entonces la región debe llamarse **Constructors, Destructor & IDisposable**.  Luego viene la región **Methods** que encierra todos los métodos.

Las implementaciones de interfaz se agrupan, efectivamente sacando las propiedades, métodos y eventos de la interfaz fuera de sus regions usuales.  El nombre de región debe ser igual al nombre de la interfaz, incluyendo opcionalmente el nombre completo de su espacio de nombres, y va después de la región **Methods**.

Si una clase es altamente especializada como un controlador MVC donde hay métodos muy específicos referidos como *acciones*, o una clase de pruebas unitarias donde  hay métodos referidos como *pruebas*, es deseable crear una región solamente para ellos.  Un nombre apropiado para las acciones en un controlador MVC podría ser **Action Methods**; uno para los métodos de pruebas podría ser **Test Methods**.  Estas regiones especiales van después de la región **Methods**.

[1]: https://docs.microsoft.com/en-us/dotnet/csharp/codedoc
[2]: https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/preprocessor-directives/preprocessor-region

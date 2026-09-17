# 🌳 ArbolVisual

> Aplicación interactiva en C# y Windows Forms para crear, modificar y
> visualizar un árbol binario de búsqueda (BST) paso a paso: pensada para
> aprender de forma visual cómo insertar, eliminar, buscar y recorrer un BST,
> con el dibujo del árbol recalculado en cada operación.

## Características

- **Árbol binario de búsqueda real**: inserta, elimina y busca valores
  enteros (1 a 99) manteniendo la propiedad de orden del BST.
- **Eliminación con dos hijos correctamente implementada**: al eliminar un
  nodo con dos hijos se promueve su predecesor in-order (el mayor valor de
  su subárbol izquierdo), reconectando los enlaces restantes. Cubierto por
  pruebas automatizadas que verifican que el árbol resultante siga siendo
  un BST válido (recorrido en-orden ordenado, sin nodos perdidos ni
  duplicados), incluyendo el caso en que el predecesor tiene a su vez un
  hijo izquierdo que debe promoverse.
- **Recorridos en-orden, pre-orden y post-orden**, mostrados como texto y
  animados visualmente (cada nodo se resalta en rojo un momento, en el
  orden del recorrido elegido).
- **Estadísticas del árbol**: altura, suma de los valores, cantidad de
  nodos y profundidad de un valor específico.
- **Operaciones seguras en casos límite**: insertar un valor duplicado o
  eliminar/buscar un valor inexistente no corrompen el árbol ni bloquean la
  aplicación; el árbol vacío se maneja sin errores (ver nota más abajo).
- **El dibujo del árbol se recalcula por completo en cada operación**
  (`Arbol_Binario.DibujarArbol` vuelve a calcular la posición de cada nodo
  desde la raíz), por lo que lo que se ve en pantalla nunca queda
  desincronizado de la estructura real tras insertar o eliminar.

### Nota sobre un bug corregido

Antes, llamar a **Eliminar** con el árbol vacío insertaba silenciosamente
ese valor en vez de no hacer nada (un copiar-y-pegar del código de
Insertar). Ya está corregido: eliminar sobre un árbol vacío simplemente no
hace nada.

### Limitación conocida

Al buscar un valor, la posición del nodo encontrado se informa por texto
(coordenadas X/Y) en un cuadro de diálogo, pero el nodo no se resalta
visualmente sobre el dibujo del árbol (a diferencia de los recorridos, que
sí animan cada nodo). Es una función incompleta heredada del código
original, no un objetivo de esta ronda de correcciones.

## Cómo usar

1. Abre `ArbolBinario.sln` en Visual Studio (con la carga de trabajo *.NET
   desktop development*) y presiona **F5**, o compílalo desde la línea de
   comandos (ver más abajo) y ejecuta el `.exe` generado.
2. Escribe un valor entre 1 y 99 y usa **Insertar**, **Eliminar** o
   **Buscar**.
3. Usa los botones de recorrido (**En-Orden**, **Pre-Orden**, **Post-Orden**)
   para ver la animación del recorrido elegido, o los botones de
   **Altura**, **Suma de nodos**, **Contar nodos** y **Profundidad** para
   ver estadísticas del árbol actual.

## Instalación y uso local

Requiere Windows y el SDK de .NET 8 (el proyecto compila sobre
`net8.0-windows`).

```bash
git clone https://github.com/Luiss2080/ArbolVisual.git
cd ArbolVisual

# Compilar la app y el proyecto de pruebas
dotnet build ArbolBinario.sln

# Ejecutar la app (WinForms; requiere Windows)
dotnet run --project ArbolBinario/ArbolBinario.csproj
```

## Tecnologías

- **C#** sobre **.NET 8** (`net8.0-windows`, `ArbolBinario.csproj`)
- **Windows Forms**, con dibujo personalizado del árbol mediante
  `System.Drawing`/GDI+
- **xUnit** para las pruebas unitarias del BST
- **GitHub Actions** para build y pruebas automáticas en cada cambio
  (`.github/workflows/build-and-test.yml`)

## Tests

El BST (`Arbol_Binario`/`Nodo_Arbol`) está cubierto por 24 pruebas
unitarias que no dependen de la interfaz gráfica: inserción, eliminación
(incluyendo el caso de dos hijos, en varias formas de árbol), búsqueda,
recorridos en árboles balanceados y degenerados, y casos límite (árbol
vacío, un solo nodo, eliminar la raíz repetidamente, duplicados, valores
inexistentes).

```bash
dotnet test ArbolBinario.Tests/ArbolBinario.Tests.csproj
```

## Licencia

MIT. Consulta el archivo [`LICENSE`](LICENSE).

using System.Runtime.CompilerServices;

// Nodo_Arbol y Arbol_Binario son internal (visibilidad por defecto): el resto de
// la aplicacion nunca necesito que fueran publicas. El proyecto de pruebas
// ArbolBinario.Tests si necesita acceder a ellas directamente, ya que las pruebas
// ejercitan el modelo del arbol (Insertar/Eliminar/Buscar/recorridos) sin pasar
// por la interfaz grafica.
[assembly: InternalsVisibleTo("ArbolBinario.Tests")]

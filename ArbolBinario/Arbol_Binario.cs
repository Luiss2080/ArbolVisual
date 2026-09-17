using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Diagnostics;
using System.Threading;

namespace ArbolBinario
{
    class Arbol_Binario
    {
        // Añadir constante Radio para coincidir con Nodo_Arbol
        private const int Radio = 30;

        public Nodo_Arbol Raiz;
        public Nodo_Arbol aux;

        public Arbol_Binario()
        {
            aux = new Nodo_Arbol();
        }

        public Arbol_Binario(Nodo_Arbol nueva_raiz)
        {
            Raiz = nueva_raiz;
        }

        public void Insertar(int x)
        {
            // Si el árbol está vacío, se crea la raíz con el valor dado.
            if (Raiz == null)
            {
                Raiz = new Nodo_Arbol(x, null, null, null);
                Raiz.nivel = 0; // Nivel de la raíz es 0.
            }
            else
                // Si el árbol no está vacío, se inserta el valor en la posición correcta.
                Raiz = Raiz.Insertar(x, Raiz, Raiz.nivel);
        }

        public void Eliminar(int x)
        {
            // Si el árbol está vacío no hay nada que eliminar.
            // (Antes, por un copiar-y-pegar de Insertar, esta rama creaba un nodo
            // nuevo con el valor `x`: llamar a Eliminar sobre un árbol vacío
            // insertaba un dato en vez de no hacer nada.)
            if (Raiz != null)
                Raiz.Eliminar(x, ref Raiz);
        }

        public void Buscar(int x)
        {
            // Si el árbol no está vacío, se llama al método buscar del nodo raíz.
            if (Raiz != null)
            {
                Raiz.buscar(x, Raiz);
            }
        }

        public void DibujarArbol(Graphics grafo, Font fuente, Brush Relleno, Brush RellenoFuente, Pen Lapiz, Brush encuentro, int anchoFormulario = 800)
        {
            if (Raiz == null)
                return;

            // Iniciar posicionamiento desde un valor base
            int x = 100; // Valor inicial bajo
            int y = 100; // Posición vertical inicial

            // Primera pasada: calcular posiciones
            Raiz.PosicionNodo(ref x, y);

            // Ahora calcular los límites reales del árbol
            int minX = int.MaxValue;
            int maxX = int.MinValue;
            ObtenerLimitesX(Raiz, ref minX, ref maxX);

            // Calcular el factor de desplazamiento para centrar con ajuste hacia la izquierda
            int anchoArbol = maxX - minX;
            // Usamos un factor de 0.45 en lugar de 0.5 para desplazar ligeramente a la izquierda
            int desplazamiento = (int)(anchoFormulario * 0.45) - minX - anchoArbol / 2;

            // Aplicar desplazamiento a todos los nodos
            AplicarDesplazamiento(Raiz, desplazamiento);

            // Dibujar el árbol con las posiciones ajustadas
            Raiz.DibujarRamas(grafo, Lapiz);
            Raiz.DibujarNodo(grafo, fuente, Relleno, RellenoFuente, Lapiz, encuentro);
        }



        // Método para obtener los límites del árbol
        private void ObtenerLimitesX(Nodo_Arbol nodo, ref int minX, ref int maxX)
        {
            if (nodo != null)
            {
                // Considerar la posición de este nodo
                minX = Math.Min(minX, nodo.CoordenadaX - Radio / 2);
                maxX = Math.Max(maxX, nodo.CoordenadaX + Radio / 2);

                // Recursivamente verificar los subárboles
                ObtenerLimitesX(nodo.Izquierdo, ref minX, ref maxX);
                ObtenerLimitesX(nodo.Derecho, ref minX, ref maxX);
            }
        }

        private void AplicarDesplazamiento(Nodo_Arbol nodo, int desplazamiento)
        {
            if (nodo != null)
            {
                // Desplazar este nodo
                nodo.CoordenadaX += desplazamiento;

                // Recursivamente desplazar los subárboles
                AplicarDesplazamiento(nodo.Izquierdo, desplazamiento);
                AplicarDesplazamiento(nodo.Derecho, desplazamiento);
            }
        }



        // Posiciones iniciales de la raíz del árbol
        public int x1 = 400;
        public int y2 = 75;



        // Función para Colorear los nodos
        public void colorear(Graphics grafo, Font fuente, Brush Relleno, Brush RellenoFuente, Pen Lapiz, Nodo_Arbol Raiz, bool post, bool inor, bool preor)
        {
            // Antes de empezar a colorear, limpiamos el área de dibujo
            if (Raiz == this.Raiz) // Solo si estamos en el nodo raíz principal
            {
                grafo.Clear(Color.FromArgb(240, 240, 240)); // Color de fondo del formulario
                                                            // Dibujamos las ramas del árbol
                this.Raiz.DibujarRamas(grafo, Lapiz);
                // Dibujamos todos los nodos inicialmente en azul
                this.Raiz.DibujarNodo(grafo, fuente, Relleno, RellenoFuente, Lapiz, Brushes.White);
            }

            // Pincel para resaltar el nodo actual
            Brush entorno = Brushes.Red;

            // Verificar si el recorrido es en orden (in-order)
            if (inor == true)
            {
                if (Raiz != null)
                {
                    // Recorrer el subárbol izquierdo
                    colorear(grafo, fuente, Relleno, RellenoFuente, Lapiz, Raiz.Izquierdo, post, inor, preor);

                    // Resaltar el nodo actual
                    Raiz.colorear(grafo, fuente, entorno, RellenoFuente, Lapiz);
                    Thread.Sleep(1000); // Pausar la ejecución por 1 segundo

                    // Restaurar el color original del nodo
                    Raiz.colorear(grafo, fuente, Relleno, RellenoFuente, Lapiz);

                    // Recorrer el subárbol derecho
                    colorear(grafo, fuente, Relleno, RellenoFuente, Lapiz, Raiz.Derecho, post, inor, preor);
                }
            }
            // Verificar si el recorrido es preorden (pre-order)
            else if (preor == true)
            {
                if (Raiz != null)
                {
                    // Resaltar el nodo actual
                    Raiz.colorear(grafo, fuente, entorno, RellenoFuente, Lapiz);
                    Thread.Sleep(1000); // Pausar la ejecución por 1 segundo

                    // Restaurar el color original del nodo
                    Raiz.colorear(grafo, fuente, Relleno, RellenoFuente, Lapiz);

                    // Recorrer el subárbol izquierdo
                    colorear(grafo, fuente, Relleno, RellenoFuente, Lapiz, Raiz.Izquierdo, post, inor, preor);

                    // Recorrer el subárbol derecho
                    colorear(grafo, fuente, Relleno, RellenoFuente, Lapiz, Raiz.Derecho, post, inor, preor);
                }
            }
            // Verificar si el recorrido es postorden (post-order)
            else if (post == true)
            {
                if (Raiz != null)
                {
                    // Recorrer el subárbol izquierdo
                    colorear(grafo, fuente, Relleno, RellenoFuente, Lapiz, Raiz.Izquierdo, post, inor, preor);

                    // Recorrer el subárbol derecho
                    colorear(grafo, fuente, Relleno, RellenoFuente, Lapiz, Raiz.Derecho, post, inor, preor);

                    // Resaltar el nodo actual
                    Raiz.colorear(grafo, fuente, entorno, RellenoFuente, Lapiz);
                    Thread.Sleep(1000); // Pausar la ejecución por 1 segundo

                    // Restaurar el color original del nodo
                    Raiz.colorear(grafo, fuente, Relleno, RellenoFuente, Lapiz);
                }
            }
        }






        // Recorrido en orden (I-R-D)
        public string RecorridoEnOrden()
        {
            return RecorridoEnOrden(Raiz); // Llama al método privado con la raíz del árbol
        }

        private string RecorridoEnOrden(Nodo_Arbol r)
        {
            if (r != null) // Si el nodo no es nulo
            {
                // Recorrer el subárbol izquierdo, procesar el nodo actual y luego el subárbol derecho
                return RecorridoEnOrden(r.Izquierdo) + r.info.ToString() + " " + RecorridoEnOrden(r.Derecho);
            }
            else
                return ""; // Si el nodo es nulo, retornar cadena vacía
        }





        // Recorrido pre-orden (R-I-D)
        public string RecorridoPreOrden()
        {
            return RecorridoPreOrden(Raiz); // Llama al método privado con la raíz del árbol
        }

        private string RecorridoPreOrden(Nodo_Arbol r)
        {
            if (r != null) // Si el nodo no es nulo
            {
                // Procesar el nodo actual, luego recorrer el subárbol izquierdo y derecho
                return r.info.ToString() + " " + RecorridoPreOrden(r.Izquierdo) + RecorridoPreOrden(r.Derecho);
            }
            else
                return ""; // Si el nodo es nulo, retornar cadena vacía
        }






        // Recorrido post-orden (I-D-R)
        public string RecorridoPostOrden()
        {
            return RecorridoPostOrden(Raiz); // Llama al método privado con la raíz del árbol
        }

        private string RecorridoPostOrden(Nodo_Arbol r)
        {
            if (r != null) // Si el nodo no es nulo
            {
                // Recorrer el subárbol izquierdo, luego el derecho y finalmente procesar el nodo actual
                return RecorridoPostOrden(r.Izquierdo) + RecorridoPostOrden(r.Derecho) + r.info.ToString() + " ";
            }
            else
                return ""; // Si el nodo es nulo, retornar cadena vacía
        }






        // Método público que inicia el cálculo de altura desde la raíz
        public int ObtenerAltura()
        {
            return ObtenerAltura(Raiz);
        }

        private int ObtenerAltura(Nodo_Arbol nodo)
        {
            // Caso base: si llegamos a un nodo nulo, su altura es -1
            if (nodo == null)
                return -1;
            else
            {
                // Calculamos recursivamente la altura de cada subárbol
                int alturaIzquierda = ObtenerAltura(nodo.Izquierdo);
                int alturaDerecha = ObtenerAltura(nodo.Derecho);

                // La altura del nodo actual es 1 + la mayor altura de sus subárboles
                if (alturaIzquierda > alturaDerecha)
                    return alturaIzquierda + 1;
                else
                    return alturaDerecha + 1;
            }
        }




        // Método público que inicia la suma de valores desde la raíz
        public int SumarNodos()
        {
            return SumarNodos(Raiz);
        }

        private int SumarNodos(Nodo_Arbol nodo)
        {
            // Caso base: nodo nulo no aporta valor a la suma
            if (nodo == null)
                return 0;
            else
                // Suma el valor del nodo actual con la suma de sus subárboles
                return nodo.info + SumarNodos(nodo.Izquierdo) + SumarNodos(nodo.Derecho);
        }





        // Método público que inicia el conteo de nodos desde la raíz
        public int ContarNodos()
        {
            return ContarNodos(Raiz);
        }

        private int ContarNodos(Nodo_Arbol nodo)
        {
            // Caso base: nodo nulo no cuenta como nodo
            if (nodo == null)
                return 0;
            else
                // Suma 1 (por el nodo actual) más los nodos de cada subárbol
                return 1 + ContarNodos(nodo.Izquierdo) + ContarNodos(nodo.Derecho);
        }






        // Método público que inicia la búsqueda de profundidad para un valor específico
        public int ObtenerProfundidad(int valor)
        {
            return ObtenerProfundidad(Raiz, valor, 0);
        }

        private int ObtenerProfundidad(Nodo_Arbol nodo, int valor, int nivel)
        {
            // Caso base 1: nodo nulo significa que el valor no existe en este camino
            if (nodo == null)
                return -1; 

            // Caso base 2: encontramos el nodo con el valor buscado
            if (nodo.info == valor)
                return nivel; // Devolvemos el nivel actual como profundidad

            // Primero buscamos en el subárbol izquierdo
            int profundidad = ObtenerProfundidad(nodo.Izquierdo, valor, nivel + 1);

            // Si lo encontramos en el subárbol izquierdo, propagamos el resultado
            if (profundidad != -1)
                return profundidad;

            // Si no está en el izquierdo, buscamos en el subárbol derecho
            return ObtenerProfundidad(nodo.Derecho, valor, nivel + 1);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using ArbolBinario;
using Xunit;

namespace ArbolBinario.Tests
{
    /// <summary>
    /// Pruebas del BST (Arbol_Binario / Nodo_Arbol), completamente separadas de la
    /// interfaz grafica: solo se ejercitan Insertar/Eliminar/Buscar y los
    /// recorridos, que ya no muestran ningun MessageBox (ver
    /// fix/separate-bst-model-from-messagebox-ui), asi que estas pruebas corren
    /// sin bloquearse en ningun dialogo.
    /// </summary>
    public class ArbolBinarioTests
    {
        private static Arbol_Binario NuevoArbol(params int[] valores)
        {
            var arbol = new Arbol_Binario();
            foreach (var v in valores)
                arbol.Insertar(v);
            return arbol;
        }

        private static List<int> EnOrdenComoLista(Arbol_Binario arbol)
        {
            var texto = arbol.RecorridoEnOrden();
            return texto
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse)
                .ToList();
        }

        // ---------------------------------------------------------------
        // Caso central: eliminacion de un nodo con dos hijos.
        // ---------------------------------------------------------------

        [Fact]
        public void Eliminar_NodoConDosHijos_MantieneElArbolOrdenadoYSinPerderNodos()
        {
            //              50
            //          /       \
            //        30          70
            //       /  \        /  \
            //     20    40     60   80
            //          /  \
            //        35    45
            var arbol = NuevoArbol(50, 30, 70, 20, 40, 60, 80, 35, 45);
            int cantidadAntes = arbol.ContarNodos();

            bool eliminado = arbol.Eliminar(30); // nodo con dos hijos (20 y 40)

            Assert.True(eliminado);
            Assert.Equal(cantidadAntes - 1, arbol.ContarNodos());

            var enOrden = EnOrdenComoLista(arbol);

            // El arbol resultante sigue siendo un BST valido: el recorrido en-orden
            // debe seguir estrictamente ordenado (sin duplicados, ya que Insertar no
            // permite valores repetidos) y contener exactamente los nodos que quedan.
            Assert.Equal(enOrden.OrderBy(v => v).ToList(), enOrden);
            Assert.Equal(new List<int> { 20, 35, 40, 45, 50, 60, 70, 80 }, enOrden);

            // El valor eliminado ya no debe encontrarse.
            Assert.Null(arbol.Buscar(30));
            // El resto de los valores originales debe seguir presente.
            foreach (var v in new[] { 20, 35, 40, 45, 50, 60, 70, 80 })
                Assert.NotNull(arbol.Buscar(v));
        }

        [Fact]
        public void Eliminar_LaRaizConDosHijos_PromueveElPredecesorCorrectamente()
        {
            var arbol = NuevoArbol(50, 30, 70, 20, 40, 60, 80, 35, 45);

            bool eliminado = arbol.Eliminar(50); // la raiz, con dos hijos

            Assert.True(eliminado);
            var enOrden = EnOrdenComoLista(arbol);

            Assert.Equal(8, enOrden.Count);
            Assert.Equal(enOrden.OrderBy(v => v).ToList(), enOrden);
            Assert.DoesNotContain(50, enOrden);
            // La nueva raiz debe ser el predecesor in-order del valor eliminado (45:
            // el mayor del subarbol izquierdo de 50).
            Assert.Equal(45, arbol.Raiz.info);
        }

        [Fact]
        public void Eliminar_NodoConDosHijosDondeElPredecesorTieneHijoIzquierdo_PromueveEseHijo()
        {
            // Construye un arbol donde, al eliminar 60 (dos hijos: 40 y 80), el
            // predecesor in-order (48) tiene a su vez un hijo izquierdo (47) que debe
            // promoverse a la posicion del predecesor.
            var arbol = NuevoArbol(60, 40, 80, 70, 90, 45, 48, 47);
            //        60
            //      /    \
            //    40      80
            //      \    /  \
            //      45  70   90
            //        \
            //        48
            //       /
            //      47

            bool eliminado = arbol.Eliminar(60);

            Assert.True(eliminado);
            var enOrden = EnOrdenComoLista(arbol);
            Assert.Equal(new List<int> { 40, 45, 47, 48, 70, 80, 90 }, enOrden);
            Assert.Equal(enOrden.OrderBy(v => v).ToList(), enOrden);
        }

        // ---------------------------------------------------------------
        // Casos limite
        // ---------------------------------------------------------------

        [Fact]
        public void ArbolVacio_Eliminar_NoInsertaUnNodoYDevuelveFalse()
        {
            var arbol = new Arbol_Binario();

            bool eliminado = arbol.Eliminar(10);

            Assert.False(eliminado);
            Assert.Null(arbol.Raiz); // el arbol sigue vacio (antes, este era el bug)
        }

        [Fact]
        public void ArbolVacio_Buscar_DevuelveNullSinLanzarExcepcion()
        {
            var arbol = new Arbol_Binario();

            var resultado = arbol.Buscar(10);

            Assert.Null(resultado);
        }

        [Fact]
        public void ArbolDeUnSoloNodo_EliminarEseNodo_DejaElArbolVacio()
        {
            var arbol = NuevoArbol(42);

            bool eliminado = arbol.Eliminar(42);

            Assert.True(eliminado);
            Assert.Null(arbol.Raiz);
            Assert.Equal(0, arbol.ContarNodos());
        }

        [Fact]
        public void EliminarLaRaizRepetidamente_HastaVaciarElArbol_NuncaDejaNodosColgados()
        {
            var arbol = NuevoArbol(50, 30, 70, 20, 40, 60, 80);
            int total = arbol.ContarNodos();

            for (int i = 0; i < total; i++)
            {
                Assert.NotNull(arbol.Raiz);
                int raizActual = arbol.Raiz.info;
                bool eliminado = arbol.Eliminar(raizActual);
                Assert.True(eliminado);

                var enOrden = EnOrdenComoLista(arbol);
                Assert.Equal(enOrden.OrderBy(v => v).ToList(), enOrden); // sigue siendo BST valido
                Assert.Equal(total - i - 1, arbol.ContarNodos());
            }

            Assert.Null(arbol.Raiz);
        }

        [Fact]
        public void InsertarValorDuplicado_EsRechazadoYNoModificaElArbol()
        {
            var arbol = NuevoArbol(10, 5, 15);

            bool insertado = arbol.Insertar(5); // duplicado

            Assert.False(insertado);
            Assert.Equal(3, arbol.ContarNodos());
            Assert.Equal(new List<int> { 5, 10, 15 }, EnOrdenComoLista(arbol));
        }

        [Fact]
        public void EliminarValorInexistente_DevuelveFalseYNoModificaElArbol()
        {
            var arbol = NuevoArbol(10, 5, 15);

            bool eliminado = arbol.Eliminar(999);

            Assert.False(eliminado);
            Assert.Equal(3, arbol.ContarNodos());
            Assert.Equal(new List<int> { 5, 10, 15 }, EnOrdenComoLista(arbol));
        }

        [Fact]
        public void BuscarValorInexistente_DevuelveNull()
        {
            var arbol = NuevoArbol(10, 5, 15);

            Assert.Null(arbol.Buscar(999));
        }

        [Fact]
        public void BuscarValorExistente_DevuelveElNodoConEseValor()
        {
            var arbol = NuevoArbol(10, 5, 15);

            var nodo = arbol.Buscar(15);

            Assert.NotNull(nodo);
            Assert.Equal(15, nodo!.info);
        }

        // ---------------------------------------------------------------
        // Correccion de los recorridos, en varias formas de arbol.
        // ---------------------------------------------------------------

        [Fact]
        public void RecorridoEnOrden_ArbolBalanceado_DevuelveTodosLosValoresOrdenados()
        {
            var arbol = NuevoArbol(50, 30, 70, 20, 40, 60, 80);

            var enOrden = EnOrdenComoLista(arbol);

            Assert.Equal(new List<int> { 20, 30, 40, 50, 60, 70, 80 }, enOrden);
        }

        [Fact]
        public void RecorridoEnOrden_ArbolDegeneradoHaciaLaDerecha_DevuelveTodosLosValoresOrdenados()
        {
            // Insertar en orden creciente crea una "lista enlazada" hacia la derecha.
            var arbol = NuevoArbol(1, 2, 3, 4, 5);

            Assert.Equal(new List<int> { 1, 2, 3, 4, 5 }, EnOrdenComoLista(arbol));
        }

        [Fact]
        public void RecorridoEnOrden_ArbolDegeneradoHaciaLaIzquierda_DevuelveTodosLosValoresOrdenados()
        {
            // Insertar en orden decreciente crea una "lista enlazada" hacia la izquierda.
            var arbol = NuevoArbol(5, 4, 3, 2, 1);

            Assert.Equal(new List<int> { 1, 2, 3, 4, 5 }, EnOrdenComoLista(arbol));
        }

        [Fact]
        public void RecorridoPreOrden_VisitaRaizAntesQueSusHijos_SinOmitirNiDuplicarNodos()
        {
            //     10
            //    /  \
            //   5    15
            var arbol = NuevoArbol(10, 5, 15);

            var preOrden = arbol.RecorridoPreOrden()
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse)
                .ToList();

            Assert.Equal(new List<int> { 10, 5, 15 }, preOrden);
        }

        [Fact]
        public void RecorridoPostOrden_VisitaLaRaizAlFinal_SinOmitirNiDuplicarNodos()
        {
            var arbol = NuevoArbol(10, 5, 15);

            var postOrden = arbol.RecorridoPostOrden()
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse)
                .ToList();

            Assert.Equal(new List<int> { 5, 15, 10 }, postOrden);
        }

        [Fact]
        public void Recorridos_TodosCubrenLaMismaCantidadDeNodosQueContarNodos()
        {
            var arbol = NuevoArbol(50, 30, 70, 20, 40, 60, 80, 35, 45, 65, 75, 90);
            int cantidad = arbol.ContarNodos();

            int enOrdenCount = arbol.RecorridoEnOrden().Split(' ', StringSplitOptions.RemoveEmptyEntries).Length;
            int preOrdenCount = arbol.RecorridoPreOrden().Split(' ', StringSplitOptions.RemoveEmptyEntries).Length;
            int postOrdenCount = arbol.RecorridoPostOrden().Split(' ', StringSplitOptions.RemoveEmptyEntries).Length;

            Assert.Equal(cantidad, enOrdenCount);
            Assert.Equal(cantidad, preOrdenCount);
            Assert.Equal(cantidad, postOrdenCount);
        }

        // ---------------------------------------------------------------
        // Metodos auxiliares (altura, suma, conteo, profundidad): pruebas de
        // regresion sobre logica que ya existia y que la UI expone directamente.
        // ---------------------------------------------------------------

        [Fact]
        public void ObtenerAltura_ArbolBalanceadoDeTresNiveles_Devuelve2()
        {
            var arbol = NuevoArbol(50, 30, 70, 20, 40, 60, 80);

            Assert.Equal(2, arbol.ObtenerAltura());
        }

        [Fact]
        public void ObtenerAltura_ArbolVacio_DevuelveMenosUno()
        {
            var arbol = new Arbol_Binario();

            Assert.Equal(-1, arbol.ObtenerAltura());
        }

        [Fact]
        public void ObtenerAltura_UnSoloNodo_Devuelve0()
        {
            var arbol = NuevoArbol(1);

            Assert.Equal(0, arbol.ObtenerAltura());
        }

        [Fact]
        public void SumarNodos_SumaTodosLosValoresInsertados()
        {
            var arbol = NuevoArbol(10, 5, 15, 3, 7);

            Assert.Equal(10 + 5 + 15 + 3 + 7, arbol.SumarNodos());
        }

        [Fact]
        public void ContarNodos_CuentaExactamenteLosNodosInsertados()
        {
            var arbol = NuevoArbol(10, 5, 15, 3, 7);

            Assert.Equal(5, arbol.ContarNodos());
        }

        [Fact]
        public void ObtenerProfundidad_DevuelveElNivelCorrectoParaCadaValor()
        {
            var arbol = NuevoArbol(50, 30, 70, 20, 40);

            Assert.Equal(0, arbol.ObtenerProfundidad(50));
            Assert.Equal(1, arbol.ObtenerProfundidad(30));
            Assert.Equal(2, arbol.ObtenerProfundidad(20));
            Assert.Equal(2, arbol.ObtenerProfundidad(40));
        }

        [Fact]
        public void ObtenerProfundidad_ValorInexistente_DevuelveMenosUno()
        {
            var arbol = NuevoArbol(50, 30, 70);

            Assert.Equal(-1, arbol.ObtenerProfundidad(999));
        }
    }
}

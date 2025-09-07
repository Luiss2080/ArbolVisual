using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing; 
using System.Linq;
using System.Text;
using System.Threading; 
using System.Windows.Forms;

namespace ArbolBinario
{
    class Nodo_Arbol
    {
        public int info; // Dato a almacenar en el nodo
        public Nodo_Arbol Izquierdo; 
        public Nodo_Arbol Derecho; 
        public Nodo_Arbol Padre; 
        public int altura;
        public int nivel;
        public Rectangle nodo; 


        private const int Radio = 30;
        private const int DistanciaH = 30; 
        private const int DistanciaV = 10;



        public int CoordenadaX { get; set; }
        public int CoordenadaY { get; set; }
        // Objeto gráfico para operaciones de dibujo
        private Graphics col;
        // Referencia al árbol contenedor
        private Arbol_Binario arbol;



        public Nodo_Arbol() // Constructor por defecto
        {
        }

        // Propiedad para acceder al árbol contenedor
        public Arbol_Binario Arbol
        {
            get { return arbol; }
            set { arbol = value; }
        }

        // Inicializa un nodo con valor y conexiones específicas
        public Nodo_Arbol(int nueva_info, Nodo_Arbol izquierdo, Nodo_Arbol derecho, Nodo_Arbol padre)
        {
            info = nueva_info;
            Izquierdo = izquierdo;
            Derecho = derecho;
            Padre = padre;
            altura = 0;
        }

        // Implementa la lógica BST recursiva para inserción de nodos
        public Nodo_Arbol Insertar(int x, Nodo_Arbol t, int Level)
        {
            if (t == null)
            {
                // Crea nuevo nodo en posición vacía
                t = new Nodo_Arbol(x, null, null, null);
                t.nivel = Level;
            }
            else if (x < t.info)
            {
                // Navega al subárbol izquierdo aumentando nivel
                Level++;
                t.Izquierdo = Insertar(x, t.Izquierdo, Level);
            }
            else if (x > t.info)
            {
                // Navega al subárbol derecho aumentando nivel
                Level++;
                t.Derecho = Insertar(x, t.Derecho, Level);
            }
            else
            {
                // Evita duplicados
                MessageBox.Show("Dato existente en el Arbol", "Error de Ingreso");
            }
            return t;
        }





        // Elimina un nodo manteniendo la estructura BST
        public void Eliminar(int x, ref Nodo_Arbol t)
        {
            if (t != null)
            {
                if (x < t.info)
                {
                    // Búsqueda recursiva en subárbol izquierdo
                    Eliminar(x, ref t.Izquierdo);
                }
                else if (x > t.info)
                {
                    // Búsqueda recursiva en subárbol derecho
                    Eliminar(x, ref t.Derecho);
                }
                else
                {
                    // Caso 1: Nodo hoja o con un solo hijo
                    Nodo_Arbol NodoEliminar = t;

                    if (NodoEliminar.Derecho == null)
                    {
                        // Sin hijo derecho: el izquierdo toma su lugar
                        t = NodoEliminar.Izquierdo;
                    }
                    else if (NodoEliminar.Izquierdo == null)
                    {
                        // Sin hijo izquierdo: el derecho toma su lugar
                        t = NodoEliminar.Derecho;
                    }
                    else
                    {
                        // Caso 2: Nodo con dos hijos

                        // Si subárbol izquierdo es más alto
                        if (Alturas(t.Izquierdo) - Alturas(t.Derecho) > 0)
                        {
                            // Busca el sucesor in-order (mayor valor del subárbol izquierdo)
                            Nodo_Arbol AuxiliarNodo = null;
                            Nodo_Arbol Auxiliar = t.Izquierdo;
                            bool bandera = false;

                            // Navega hasta el nodo más a la derecha
                            while (Auxiliar.Derecho != null)
                            {
                                AuxiliarNodo = Auxiliar;
                                Auxiliar = Auxiliar.Derecho;
                                bandera = true;
                            }

                            // Reemplaza valor y reconecta los enlaces
                            t.info = Auxiliar.info;
                            NodoEliminar = Auxiliar;

                            if (bandera)
                                AuxiliarNodo.Derecho = Auxiliar.Izquierdo;
                            else
                                t.Izquierdo = Auxiliar.Izquierdo;
                        }
                        else if (Alturas(t.Derecho) - Alturas(t.Izquierdo) > 0)
                        {
                            // Si subárbol derecho es más alto
                            // Busca el predecesor in-order (menor valor del subárbol derecho)
                            Nodo_Arbol AuxiliarNodo = null;
                            Nodo_Arbol Auxiliar = t.Derecho;
                            bool bandera = false;

                            // Navega hasta el nodo más a la izquierda
                            while (Auxiliar.Izquierdo != null)
                            {
                                AuxiliarNodo = Auxiliar;
                                Auxiliar = Auxiliar.Izquierdo;
                                bandera = true;
                            }

                            // Reemplaza y reconecta
                            t.info = Auxiliar.info;
                            NodoEliminar = Auxiliar;

                            if (bandera)
                                AuxiliarNodo.Izquierdo = Auxiliar.Derecho;
                            else
                                t.Derecho = Auxiliar.Derecho;
                        }
                        else
                        {
                            // Si ambos subárboles tienen igual altura, prioriza izquierdo
                            // Usa la misma lógica del primer caso
                            Nodo_Arbol AuxiliarNodo = null;
                            Nodo_Arbol Auxiliar = t.Izquierdo;
                            bool bandera = false;

                            while (Auxiliar.Derecho != null)
                            {
                                AuxiliarNodo = Auxiliar;
                                Auxiliar = Auxiliar.Derecho;
                                bandera = true;
                            }

                            t.info = Auxiliar.info;
                            NodoEliminar = Auxiliar;

                            if (bandera)
                                AuxiliarNodo.Derecho = Auxiliar.Izquierdo;
                            else
                                t.Izquierdo = Auxiliar.Izquierdo;
                        }
                    }
                }
            }
            else
            {
                // El nodo a eliminar no existe
                MessageBox.Show("Nodo NO existente el Arbol", "Error de eliminación");
            }
        }






        // Busca un nodo con valor específico usando el principio BST
        public void buscar(int x, Nodo_Arbol t)
        {
            if (t != null)
            {
                if (x == t.info)
                {
                    // Nodo encontrado - muestra su ubicación visual
                    MessageBox.Show("Nodo encontrado en la posición X: " + t.CoordenadaX + " Y:" + t.CoordenadaY);
                    encontrado(t);
                }
                else if (x < t.info)
                {
                    // Valor menor - busca en subárbol izquierdo
                    buscar(x, t.Izquierdo);
                }
                else
                {
                    // Valor mayor - busca en subárbol derecho
                    buscar(x, t.Derecho);
                }
            }
            else
            {
                // Llegó a nodo nulo - valor no existe
                MessageBox.Show("Nodo NO encontrado", "Error de búsqueda");
            }
        }





        // Función posición nodo (donde se ha creado dibujo del nodo)
        public void PosicionNodo(ref int xmin, int ymin)
        {
            CoordenadaY = ymin + Radio / 2;

            // Almacenar la posición inicial para este nodo
            int inicialX = xmin;

            // Procesar el subárbol izquierdo
            if (Izquierdo != null)
            {
                Izquierdo.PosicionNodo(ref xmin, ymin + Radio + DistanciaV);
            }

            // Establecer la posición X de este nodo después de procesar el izquierdo
            CoordenadaX = xmin + Radio / 2;
            xmin += Radio + DistanciaH; // Avanzar para el siguiente nodo/subárbol

            // Procesar el subárbol derecho
            if (Derecho != null)
            {
                Derecho.PosicionNodo(ref xmin, ymin + Radio + DistanciaV);
            }

            // Si hay nodos hijos, ajustar la posición X para que esté centrada entre ellos
            if (Izquierdo != null && Derecho != null)
            {
                CoordenadaX = (Izquierdo.CoordenadaX + Derecho.CoordenadaX) / 2;
            }
            else if (Izquierdo != null)
            {
                // No hacer nada, ya se estableció correctamente durante el recorrido del hijo izquierdo
            }
            else if (Derecho != null)
            {
                // No hacer nada, ya se estableció correctamente
            }
        }





        // Función para dibujar las ramas de los nodos izquierdo y derecho
        public void DibujarRamas(Graphics grafo, Pen Lapiz)
        {
            if (Izquierdo != null)
            // Dibujará rama izquierda
            {
                grafo.DrawLine(Lapiz, CoordenadaX, CoordenadaY, Izquierdo.CoordenadaX, Izquierdo.CoordenadaY);
                Izquierdo.DibujarRamas(grafo, Lapiz);
            }
            if (Derecho != null)
            // Dibujará rama derecha
            {
                grafo.DrawLine(Lapiz, CoordenadaX, CoordenadaY, Derecho.CoordenadaX, Derecho.CoordenadaY);
                Derecho.DibujarRamas(grafo, Lapiz);
            }
        }





        // Función para dibujar el nodo en la posición especificada
        public void DibujarNodo(Graphics grafo, Font fuente, Brush Relleno, Brush RellenoFuente, Pen Lapiz, Brush encuentro)
        {
            col = grafo;
            // Dibuja el contorno del nodo
            Rectangle rect = new Rectangle((int)(CoordenadaX - Radio / 2), (int)(CoordenadaY - Radio / 2), Radio, Radio);
            Rectangle prueba = new Rectangle((int)(CoordenadaX - Radio / 2), (int)(CoordenadaY - Radio / 2), Radio, Radio);
            grafo.FillEllipse(encuentro, rect);
            grafo.FillEllipse(Relleno, rect);
            grafo.DrawEllipse(Lapiz, rect);
            // Para dibujar el nombre del nodo, es decir el contenido
            StringFormat formato = new StringFormat();
            formato.Alignment = StringAlignment.Center;
            formato.LineAlignment = StringAlignment.Center;
            grafo.DrawString(info.ToString(), fuente, RellenoFuente, CoordenadaX, CoordenadaY, formato);
            // Dibuja los nodos hijos derecho e izquierdo.
            if (Izquierdo != null)
            {
                Izquierdo.DibujarNodo(grafo, fuente, Relleno, RellenoFuente, Lapiz, encuentro);
            }
            if (Derecho != null)
            {
                Derecho.DibujarNodo(grafo, fuente, Relleno, RellenoFuente, Lapiz, encuentro);
            }
        }





        public void colorear(Graphics grafo, Font fuente, Brush Relleno, Brush RellenoFuente, Pen Lapiz)
        {
            // Dibuja el contorno del nodo.
            Rectangle rect = new Rectangle((int)(CoordenadaX - Radio / 2), (int)(CoordenadaY - Radio / 2), Radio, Radio);
            Rectangle prueba = new Rectangle((int)(CoordenadaX - Radio / 2), (int)(CoordenadaY - Radio / 2), Radio, Radio);
            grafo.FillEllipse(Relleno, rect);
            grafo.DrawEllipse(Lapiz, rect);
            // Dibuja el nombre
            StringFormat formato = new StringFormat();
            formato.Alignment = StringAlignment.Center;
            formato.LineAlignment = StringAlignment.Center;
            grafo.DrawString(info.ToString(), fuente, RellenoFuente, CoordenadaX, CoordenadaY, formato);
        }






        // Obtiene altura de un nodo (devuelve -1 si es nulo)
        private static int Alturas(Nodo_Arbol t)
        {
            return t == null ? -1 : t.altura;
        }





        // Método para marcar nodo encontrado (incompleto - falta implementación visual)
        public void encontrado(Nodo_Arbol t)
        {
            Rectangle rec = new Rectangle(t.CoordenadaX, t.CoordenadaY, 40, 40);
        }
    }
}
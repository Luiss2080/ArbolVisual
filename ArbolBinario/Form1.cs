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
    // Define el formulario principal dentro del espacio de nombres ArbolBinario
    public partial class Form1 : Form
    {
        // Variables globales del formulario
        int Dato = 0;  
        int cont = 0; 
        Arbol_Binario mi_Arbol = new Arbol_Binario(null);  // Instancia principal del árbol
        Graphics g;  // Contexto gráfico para dibujar

        // Variables para el control de la animación
        private PictureBox pictureBoxArbol;      
        private PictureBox pictureBoxAnimacion; 
        private bool enAnimacion = false;        // Bandera para controlar estado de animación

        // Constructor del formulario
        public Form1()
        {
            InitializeComponent();

            // Configurar el PictureBox principal para el árbol
            pictureBoxArbol = new PictureBox();
            pictureBoxArbol.Dock = DockStyle.None;
            pictureBoxArbol.BackColor = Color.FromArgb(240, 240, 240); // Color de fondo que coincide con el formulario
            this.Controls.Add(pictureBoxArbol);

            // Configurar el PictureBox para la animación
            pictureBoxAnimacion = new PictureBox();
            pictureBoxAnimacion.Dock = DockStyle.None;
            pictureBoxAnimacion.BackColor = Color.FromArgb(240, 240, 240);
            pictureBoxAnimacion.Visible = false;
            this.Controls.Add(pictureBoxAnimacion);

            // Configurar el área de dibujo después de crear ambos PictureBox
            ConfigurarAreaDibujo();

            // Asegurarnos de que los controles de entrada estén por encima de los PictureBox
            foreach (Control control in this.Controls)
            {
                if (control != pictureBoxArbol && control != pictureBoxAnimacion)
                {
                    control.BringToFront();
                }
            }

            // Inicializar los bitmaps para los PictureBox
            ActualizarBitmaps();

            // Suscribe el método Form1_Resize al evento Resize para redibujar automáticamente
            this.Resize += new EventHandler(Form1_Resize);

            // Suscribirse al evento Paint del formulario
            this.Paint += new PaintEventHandler(Form1_Paint);
        }






        // Método para actualizar los bitmaps cuando cambia el tamaño del formulario
        private void ActualizarBitmaps()
        {
            // Verificar que los PictureBox existen y tienen dimensiones válidas
            if (pictureBoxArbol != null && pictureBoxAnimacion != null &&
                pictureBoxArbol.Width > 0 && pictureBoxArbol.Height > 0)
            {
                // Crear nuevos bitmaps del tamaño adecuado
                pictureBoxArbol.Image = new Bitmap(pictureBoxArbol.Width, pictureBoxArbol.Height);
                pictureBoxAnimacion.Image = new Bitmap(pictureBoxAnimacion.Width, pictureBoxAnimacion.Height);

                // Si no estamos en animación, dibujar el árbol
                if (!enAnimacion)
                {
                    DibujarArbol();
                }
            }
        }





        // Método para configurar el área de dibujo
        private void ConfigurarAreaDibujo()
        {
            // Encontrar la posición más baja de los controles superiores
            int topY = 0;
            foreach (Control control in this.Controls)
            {
                if (control != pictureBoxArbol && control != pictureBoxAnimacion &&
                    control.Top < this.Height / 3 && control.Bottom > topY)
                {
                    topY = control.Bottom + 10; // Añadimos un margen de 10 píxeles
                }
            }

            // Encontrar la posición más alta de los controles inferiores
            int bottomY = this.ClientSize.Height;
            foreach (Control control in this.Controls)
            {
                if (control != pictureBoxArbol && control != pictureBoxAnimacion &&
                    control.Top > this.Height * 2 / 3 && control.Top < bottomY)
                {
                    bottomY = control.Top - 10; // Restamos un margen de 10 píxeles
                }
            }

            // Calculamos el ancho disponible (aprovechamos todo el ancho del formulario)
            int width = this.ClientSize.Width;

            // Calculamos la altura disponible
            int height = bottomY - topY;

            // Si por alguna razón el cálculo no resulta en un área válida, usamos valores predeterminados
            if (height <= 0)
            {
                topY = 160; // Valor aproximado para dejar espacio a los controles superiores
                height = this.ClientSize.Height - 250; // Dejamos espacio para los controles inferiores
            }

            // Configuramos el área de los PictureBox
            pictureBoxArbol.Location = new Point(0, topY);
            pictureBoxArbol.Size = new Size(width, height);

            // Actualizamos también el área del PictureBox de animación
            pictureBoxAnimacion.Location = new Point(0, topY);
            pictureBoxAnimacion.Size = new Size(width, height);
        }





        // Redibuja el formulario al cambiar su tamaño
        private void Form1_Resize(object sender, EventArgs e)
        {
            // Recalculamos el área de dibujo
            ConfigurarAreaDibujo();

            // Actualizar los bitmaps con el nuevo tamaño
            ActualizarBitmaps();
        }





        // Método responsable de renderizar el árbol en el formulario
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            if (!enAnimacion)
            {
                DibujarArbol();
            }
        }





        // Método para dibujar el árbol en el PictureBox principal
        private void DibujarArbol()
        {
            if (pictureBoxArbol != null && pictureBoxArbol.Image != null)
            {
                using (Graphics g = Graphics.FromImage(pictureBoxArbol.Image))
                {
                    // Limpia el área de dibujo con el color de fondo actual
                    g.Clear(pictureBoxArbol.BackColor);

                    // Configura opciones para mejorar la calidad visual del texto
                    g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;

                    // Activa suavizado para líneas y formas
                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                    // Llama al método de dibujo del árbol pasando parámetros de estilo
                    if (mi_Arbol.Raiz != null)
                    {
                        // Pasamos el ancho del PictureBox para centrar el árbol en el área disponible
                        mi_Arbol.DibujarArbol(g, this.Font, Brushes.Blue, Brushes.White, Pens.Black, Brushes.White, pictureBoxArbol.Width);
                    }
                }
                pictureBoxArbol.Invalidate(); // Solicita repintado del control
            }
        }





        // Método para iniciar la animación de recorrido
        private void IniciarAnimacionRecorrido(bool postOrden, bool enOrden, bool preOrden)
        {
            if (mi_Arbol.Raiz != null && pictureBoxAnimacion != null && pictureBoxAnimacion.Image != null)
            {
                // Indicar que estamos en animación
                enAnimacion = true;

                // Ocultar el PictureBox del árbol y mostrar el de animación
                pictureBoxArbol.Visible = false;
                pictureBoxAnimacion.Visible = true;
                pictureBoxAnimacion.BringToFront();

                // Asegurarse de que los controles están por encima
                foreach (Control control in this.Controls)
                {
                    if (control != pictureBoxArbol && control != pictureBoxAnimacion)
                    {
                        control.BringToFront();
                    }
                }

                // Ejecutar la animación en un hilo separado para no bloquear la interfaz
                Thread hiloAnimacion = new Thread(() =>
                {
                    try
                    {
                        using (Graphics g = Graphics.FromImage(pictureBoxAnimacion.Image))
                        {
                            g.Clear(pictureBoxAnimacion.BackColor);
                            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
                            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                            // Ejecutar la animación
                            AnimarRecorrido(g, postOrden, enOrden, preOrden);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error durante la animación: " + ex.Message);
                    }
                    finally
                    {
                        // Al terminar, volver al hilo de UI para restaurar el árbol original
                        if (this.IsHandleCreated)
                        {
                            this.Invoke(new Action(() =>
                            {
                                try
                                {
                                    // Ocultar el PictureBox de animación y mostrar el original
                                    pictureBoxAnimacion.Visible = false;
                                    pictureBoxArbol.Visible = true;

                                    // Ya no estamos en animación
                                    enAnimacion = false;

                                    // Redibujar el árbol original
                                    DibujarArbol();
                                }
                                catch { }
                            }));
                        }
                    }
                });

                hiloAnimacion.IsBackground = true;
                hiloAnimacion.Start();
            }
        }





        // Método para animar el recorrido en el bitmap
        private void AnimarRecorrido(Graphics g, bool postOrden, bool enOrden, bool preOrden)
        {
            // Primero dibujamos el árbol completo para tener una base
            mi_Arbol.DibujarArbol(g, this.Font, Brushes.Blue, Brushes.White, Pens.Black, Brushes.White, pictureBoxAnimacion.Width);

            // Invalidar el PictureBox para que muestre el dibujo inicial
            this.Invoke(new Action(() =>
            {
                if (pictureBoxAnimacion != null && pictureBoxAnimacion.IsHandleCreated)
                {
                    pictureBoxAnimacion.Invalidate();
                }
            }));

            // Ahora hacemos la animación del recorrido específico
            if (enOrden)
            {
                AnimarRecorridoEnOrden(g, mi_Arbol.Raiz);
            }
            else if (preOrden)
            {
                AnimarRecorridoPreOrden(g, mi_Arbol.Raiz);
            }
            else if (postOrden)
            {
                AnimarRecorridoPostOrden(g, mi_Arbol.Raiz);
            }
        }





        // Métodos para animar los diferentes tipos de recorrido
        private void AnimarRecorridoEnOrden(Graphics g, Nodo_Arbol nodo)
        {
            if (nodo != null)
            {
                // Recorrer subárbol izquierdo
                AnimarRecorridoEnOrden(g, nodo.Izquierdo);

                // Resaltar nodo actual
                ResaltarNodo(g, nodo, Brushes.Red);
                Thread.Sleep(1000); // Pausa para visualización

                // Restaurar color original
                ResaltarNodo(g, nodo, Brushes.Blue);

                // Recorrer subárbol derecho
                AnimarRecorridoEnOrden(g, nodo.Derecho);
            }
        }





        private void AnimarRecorridoPreOrden(Graphics g, Nodo_Arbol nodo)
        {
            if (nodo != null)
            {
                // Resaltar nodo actual
                ResaltarNodo(g, nodo, Brushes.Red);
                Thread.Sleep(1000); // Pausa para visualización

                // Restaurar color original
                ResaltarNodo(g, nodo, Brushes.Blue);

                // Recorrer subárbol izquierdo
                AnimarRecorridoPreOrden(g, nodo.Izquierdo);

                // Recorrer subárbol derecho
                AnimarRecorridoPreOrden(g, nodo.Derecho);
            }
        }





        private void AnimarRecorridoPostOrden(Graphics g, Nodo_Arbol nodo)
        {
            if (nodo != null)
            {
                // Recorrer subárbol izquierdo
                AnimarRecorridoPostOrden(g, nodo.Izquierdo);

                // Recorrer subárbol derecho
                AnimarRecorridoPostOrden(g, nodo.Derecho);

                // Resaltar nodo actual
                ResaltarNodo(g, nodo, Brushes.Red);
                Thread.Sleep(1000); // Pausa para visualización

                // Restaurar color original
                ResaltarNodo(g, nodo, Brushes.Blue);
            }
        }





        // Método auxiliar para resaltar un nodo con un color específico
        private void ResaltarNodo(Graphics g, Nodo_Arbol nodo, Brush colorRelleno)
        {
            try
            {
                if (nodo == null) return;

                // Constante Radio para coincidir con la clase Nodo_Arbol
                const int Radio = 30;

                // Dibujar el nodo con el color especificado
                Rectangle rect = new Rectangle(
                    (int)(nodo.CoordenadaX - Radio / 2),
                    (int)(nodo.CoordenadaY - Radio / 2),
                    Radio, Radio);

                g.FillEllipse(colorRelleno, rect);
                g.DrawEllipse(Pens.Black, rect);

                // Dibujar el texto del nodo
                StringFormat formato = new StringFormat();
                formato.Alignment = StringAlignment.Center;
                formato.LineAlignment = StringAlignment.Center;
                g.DrawString(nodo.info.ToString(), this.Font, Brushes.White, nodo.CoordenadaX, nodo.CoordenadaY, formato);

                // Invalidar el PictureBox para que se actualice la visualización
                this.Invoke(new Action(() =>
                {
                    if (pictureBoxAnimacion != null && pictureBoxAnimacion.IsHandleCreated)
                    {
                        pictureBoxAnimacion.Invalidate();
                    }
                }));
            }
            catch { }
        }





        // Manejador del evento click del botón Insertar
        private void btnInsertar_Click(object sender, EventArgs e)
        {
            // Verifica que el campo no esté vacío
            if (txtDato.Text == "")
            {
                MessageBox.Show("Debe Ingresar un Valor");
            }
            else
            {
                try
                {
                    // Convierte el texto a entero
                    Dato = int.Parse(txtDato.Text);

                    // Valida el rango permitido (1-99)
                    if (Dato <= 0 || Dato >= 100)
                        MessageBox.Show("Solo Recibe Valores desde 1 hasta 99", "Error de Ingreso");
                    else
                    {
                        // Inserta el valor en el árbol
                        mi_Arbol.Insertar(Dato);

                        // Limpia y enfoca el campo de texto para nueva entrada
                        txtDato.Clear();
                        txtDato.Focus();

                        // Incrementa contador de nodos
                        cont++;

                        // Actualiza la visualización del árbol
                        DibujarArbol();
                    }
                }
                catch (FormatException)
                {
                    MessageBox.Show("Ingrese un valor numérico válido", "Error de formato");
                    txtDato.Clear();
                    txtDato.Focus();
                }
            }
        }





        // Manejador del evento click del botón Eliminar
        private void btnEliminar_Click(object sender, EventArgs e)
        {
            // Verifica que el campo no esté vacío
            if (txtEliminar.Text == "")
            {
                MessageBox.Show("Debe ingresar el valor a eliminar");
            }
            else
            {
                try
                {
                    // Convierte el texto a entero usando Convert.ToInt32
                    Dato = Convert.ToInt32(txtEliminar.Text);

                    // Valida el rango permitido (1-99)
                    if (Dato <= 0 || Dato >= 100)
                    {
                        MessageBox.Show("Sólo se admiten valores entre 1 y 99", "Error de Ingreso");
                    }
                    else
                    {
                        // Elimina el nodo con el valor especificado
                        mi_Arbol.Eliminar(Dato);

                        // Limpia y enfoca el campo para nueva entrada
                        txtEliminar.Clear();
                        txtEliminar.Focus();

                        // Decrementa contador de nodos
                        cont--;

                        // Actualiza la visualización del árbol
                        DibujarArbol();
                    }
                }
                catch (FormatException)
                {
                    MessageBox.Show("Ingrese un valor numérico válido", "Error de formato");
                    txtEliminar.Clear();
                    txtEliminar.Focus();
                }
            }
        }






        // Manejador del evento click del botón Buscar
        private void btnBuscar_Click(object sender, EventArgs e)
        {
            // Verifica que el campo no esté vacío
            if (txtBuscar.Text == "")
            {
                MessageBox.Show("Debe ingresar el valor a buscar");
            }
            else
            {
                try
                {
                    // Convierte el texto a entero
                    Dato = Convert.ToInt32(txtBuscar.Text);

                    // Valida el rango permitido
                    if (Dato <= 0 || Dato >= 100)
                    {
                        MessageBox.Show("Sólo se admiten valores entre 1 y 99", "Error de Ingreso");
                    }
                    else
                    {
                        // Busca el nodo con el valor especificado
                        mi_Arbol.Buscar(Dato);

                        // Limpia y enfoca el campo para nueva búsqueda
                        txtBuscar.Clear();
                        txtBuscar.Focus();

                        // Actualiza la visualización (aunque la búsqueda no modifica estructura)
                        DibujarArbol();
                    }
                }
                catch (FormatException)
                {
                    MessageBox.Show("Ingrese un valor numérico válido", "Error de formato");
                    txtBuscar.Clear();
                    txtBuscar.Focus();
                }
            }
        }






        // Manejador del evento click del botón En-Orden
        private void btnEnOrden_Click(object sender, EventArgs e)
        {
            if (mi_Arbol.Raiz != null)
            {
                // Muestra el recorrido en-orden (izquierda-raíz-derecha) en el label
                lblRecorrido.Text = "Recorrido En Orden: " + mi_Arbol.RecorridoEnOrden();

                // Inicia la animación del recorrido
                IniciarAnimacionRecorrido(false, true, false);
            }
            else
            {
                lblRecorrido.Text = "El árbol está vacío";
            }
        }





        // Manejador del evento click del botón Pre-Orden
        private void btnPreOrden_Click(object sender, EventArgs e)
        {
            if (mi_Arbol.Raiz != null)
            {
                // Muestra el recorrido pre-orden (raíz-izquierda-derecha) en el label
                lblRecorrido.Text = "Recorrido Pre-Orden: " + mi_Arbol.RecorridoPreOrden();

                // Inicia la animación del recorrido
                IniciarAnimacionRecorrido(false, false, true);
            }
            else
            {
                lblRecorrido.Text = "El árbol está vacío";
            }
        }





        // Manejador del evento click del botón Post-Orden
        private void btnPostOrden_Click(object sender, EventArgs e)
        {
            if (mi_Arbol.Raiz != null)
            {
                // Muestra el recorrido post-orden (izquierda-derecha-raíz) en el label
                lblRecorrido.Text = "Recorrido Post-Orden: " + mi_Arbol.RecorridoPostOrden();

                // Inicia la animación del recorrido
                IniciarAnimacionRecorrido(true, false, false);
            }
            else
            {
                lblRecorrido.Text = "El árbol está vacío";
            }
        }






        // Manejador del evento click del botón Altura
        private void btnAltura_Click(object sender, EventArgs e)
        {
            if (mi_Arbol.Raiz != null)
            {
                // Obtiene la altura calculada desde el método del árbol
                int altura = mi_Arbol.ObtenerAltura();

                // Muestra el resultado en el control de etiqueta
                lblRecorrido.Text = "Altura del árbol: " + altura;
            }
            else
            {
                lblRecorrido.Text = "El árbol está vacío";
            }
        }






        // Manejador del evento click del botón Sumar Nodos
        private void btnSumarNodos_Click(object sender, EventArgs e)
        {
            if (mi_Arbol.Raiz != null)
            {
                // Calcula la suma de todos los valores almacenados
                int suma = mi_Arbol.SumarNodos();

                // Muestra el resultado en la etiqueta
                lblRecorrido.Text = "Suma de los valores de los nodos: " + suma;
            }
            else
            {
                lblRecorrido.Text = "El árbol está vacío";
            }
        }





        // Manejador del evento click del botón Contar Nodos
        private void btnContarNodos_Click(object sender, EventArgs e)
        {
            if (mi_Arbol.Raiz != null)
            {
                // Obtiene la cantidad total de nodos existentes
                int cantidad = mi_Arbol.ContarNodos();

                // Muestra el resultado en la etiqueta
                lblRecorrido.Text = "Número de nodos en el árbol: " + cantidad;
            }
            else
            {
                lblRecorrido.Text = "El árbol está vacío";
            }
        }






        // Manejador del evento click del botón Profundidad
        private void btnProfundidad_Click(object sender, EventArgs e)
        {
            // Verifica que el campo no esté vacío
            if (txtProfundidad.Text == "")
            {
                MessageBox.Show("Debe ingresar un valor para buscar su profundidad");
            }
            else
            {
                // Verifica que el árbol no esté vacío
                if (mi_Arbol.Raiz != null)
                {
                    int valor;
                    // Intenta convertir usando TryParse (más seguro que Parse)
                    if (int.TryParse(txtProfundidad.Text, out valor))
                    {
                        // Busca la profundidad del nodo con el valor especificado
                        int profundidad = mi_Arbol.ObtenerProfundidad(valor);

                        if (profundidad != -1)
                            // Nodo encontrado - muestra su nivel de profundidad
                            lblRecorrido.Text = "Profundidad del nodo " + valor + ": " + profundidad;
                        else
                            // Nodo no encontrado
                            lblRecorrido.Text = "El nodo " + valor + " no existe en el árbol";
                    }
                    else
                    {
                        MessageBox.Show("Ingrese un valor numérico válido", "Error de entrada");
                    }
                }
                else
                {
                    lblRecorrido.Text = "El árbol está vacío";
                }

                // Limpia y enfoca el campo para nueva consulta
                txtProfundidad.Clear();
                txtProfundidad.Focus();
            }
        }
    }
}
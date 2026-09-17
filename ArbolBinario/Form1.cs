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
        Arbol_Binario mi_Arbol = new Arbol_Binario(null);  // Instancia principal del �rbol
        Graphics g;  // Contexto gr�fico para dibujar

        // Variables para el control de la animaci�n
        private PictureBox pictureBoxArbol;      
        private PictureBox pictureBoxAnimacion; 
        private bool enAnimacion = false;        // Bandera para controlar estado de animaci�n

        // Constructor del formulario
        public Form1()
        {
            InitializeComponent();

            // Configurar el PictureBox principal para el �rbol
            pictureBoxArbol = new PictureBox();
            pictureBoxArbol.Dock = DockStyle.None;
            pictureBoxArbol.BackColor = Color.FromArgb(240, 240, 240); // Color de fondo que coincide con el formulario
            this.Controls.Add(pictureBoxArbol);

            // Configurar el PictureBox para la animaci�n
            pictureBoxAnimacion = new PictureBox();
            pictureBoxAnimacion.Dock = DockStyle.None;
            pictureBoxAnimacion.BackColor = Color.FromArgb(240, 240, 240);
            pictureBoxAnimacion.Visible = false;
            this.Controls.Add(pictureBoxAnimacion);

            // Configurar el �rea de dibujo despu�s de crear ambos PictureBox
            ConfigurarAreaDibujo();

            // Asegurarnos de que los controles de entrada est�n por encima de los PictureBox
            foreach (Control control in this.Controls)
            {
                if (control != pictureBoxArbol && control != pictureBoxAnimacion)
                {
                    control.BringToFront();
                }
            }

            // Inicializar los bitmaps para los PictureBox
            ActualizarBitmaps();

            // Suscribe el m�todo Form1_Resize al evento Resize para redibujar autom�ticamente
            this.Resize += new EventHandler(Form1_Resize);

            // Suscribirse al evento Paint del formulario
            this.Paint += new PaintEventHandler(Form1_Paint);
        }






        // M�todo para actualizar los bitmaps cuando cambia el tama�o del formulario
        private void ActualizarBitmaps()
        {
            // Verificar que los PictureBox existen y tienen dimensiones v�lidas
            if (pictureBoxArbol != null && pictureBoxAnimacion != null &&
                pictureBoxArbol.Width > 0 && pictureBoxArbol.Height > 0)
            {
                // Crear nuevos bitmaps del tama�o adecuado
                pictureBoxArbol.Image = new Bitmap(pictureBoxArbol.Width, pictureBoxArbol.Height);
                pictureBoxAnimacion.Image = new Bitmap(pictureBoxAnimacion.Width, pictureBoxAnimacion.Height);

                // Si no estamos en animaci�n, dibujar el �rbol
                if (!enAnimacion)
                {
                    DibujarArbol();
                }
            }
        }





        // M�todo para configurar el �rea de dibujo
        private void ConfigurarAreaDibujo()
        {
            // Encontrar la posici�n m�s baja de los controles superiores
            int topY = 0;
            foreach (Control control in this.Controls)
            {
                if (control != pictureBoxArbol && control != pictureBoxAnimacion &&
                    control.Top < this.Height / 3 && control.Bottom > topY)
                {
                    topY = control.Bottom + 10; // A�adimos un margen de 10 p�xeles
                }
            }

            // Encontrar la posici�n m�s alta de los controles inferiores
            int bottomY = this.ClientSize.Height;
            foreach (Control control in this.Controls)
            {
                if (control != pictureBoxArbol && control != pictureBoxAnimacion &&
                    control.Top > this.Height * 2 / 3 && control.Top < bottomY)
                {
                    bottomY = control.Top - 10; // Restamos un margen de 10 p�xeles
                }
            }

            // Calculamos el ancho disponible (aprovechamos todo el ancho del formulario)
            int width = this.ClientSize.Width;

            // Calculamos la altura disponible
            int height = bottomY - topY;

            // Si por alguna raz�n el c�lculo no resulta en un �rea v�lida, usamos valores predeterminados
            if (height <= 0)
            {
                topY = 160; // Valor aproximado para dejar espacio a los controles superiores
                height = this.ClientSize.Height - 250; // Dejamos espacio para los controles inferiores
            }

            // Configuramos el �rea de los PictureBox
            pictureBoxArbol.Location = new Point(0, topY);
            pictureBoxArbol.Size = new Size(width, height);

            // Actualizamos tambi�n el �rea del PictureBox de animaci�n
            pictureBoxAnimacion.Location = new Point(0, topY);
            pictureBoxAnimacion.Size = new Size(width, height);
        }





        // Redibuja el formulario al cambiar su tama�o
        private void Form1_Resize(object sender, EventArgs e)
        {
            // Recalculamos el �rea de dibujo
            ConfigurarAreaDibujo();

            // Actualizar los bitmaps con el nuevo tama�o
            ActualizarBitmaps();
        }





        // M�todo responsable de renderizar el �rbol en el formulario
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            if (!enAnimacion)
            {
                DibujarArbol();
            }
        }





        // M�todo para dibujar el �rbol en el PictureBox principal
        private void DibujarArbol()
        {
            if (pictureBoxArbol != null && pictureBoxArbol.Image != null)
            {
                using (Graphics g = Graphics.FromImage(pictureBoxArbol.Image))
                {
                    // Limpia el �rea de dibujo con el color de fondo actual
                    g.Clear(pictureBoxArbol.BackColor);

                    // Configura opciones para mejorar la calidad visual del texto
                    g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;

                    // Activa suavizado para l�neas y formas
                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                    // Llama al m�todo de dibujo del �rbol pasando par�metros de estilo
                    if (mi_Arbol.Raiz != null)
                    {
                        // Pasamos el ancho del PictureBox para centrar el �rbol en el �rea disponible
                        mi_Arbol.DibujarArbol(g, this.Font, Brushes.Blue, Brushes.White, Pens.Black, Brushes.White, pictureBoxArbol.Width);
                    }
                }
                pictureBoxArbol.Invalidate(); // Solicita repintado del control
            }
        }





        // M�todo para iniciar la animaci�n de recorrido
        private void IniciarAnimacionRecorrido(bool postOrden, bool enOrden, bool preOrden)
        {
            if (mi_Arbol.Raiz != null && pictureBoxAnimacion != null && pictureBoxAnimacion.Image != null)
            {
                // Indicar que estamos en animaci�n
                enAnimacion = true;

                // Ocultar el PictureBox del �rbol y mostrar el de animaci�n
                pictureBoxArbol.Visible = false;
                pictureBoxAnimacion.Visible = true;
                pictureBoxAnimacion.BringToFront();

                // Asegurarse de que los controles est�n por encima
                foreach (Control control in this.Controls)
                {
                    if (control != pictureBoxArbol && control != pictureBoxAnimacion)
                    {
                        control.BringToFront();
                    }
                }

                // Ejecutar la animaci�n en un hilo separado para no bloquear la interfaz
                Thread hiloAnimacion = new Thread(() =>
                {
                    try
                    {
                        using (Graphics g = Graphics.FromImage(pictureBoxAnimacion.Image))
                        {
                            g.Clear(pictureBoxAnimacion.BackColor);
                            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
                            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                            // Ejecutar la animaci�n
                            AnimarRecorrido(g, postOrden, enOrden, preOrden);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error durante la animaci�n: " + ex.Message);
                    }
                    finally
                    {
                        // Al terminar, volver al hilo de UI para restaurar el �rbol original
                        if (this.IsHandleCreated)
                        {
                            this.Invoke(new Action(() =>
                            {
                                try
                                {
                                    // Ocultar el PictureBox de animaci�n y mostrar el original
                                    pictureBoxAnimacion.Visible = false;
                                    pictureBoxArbol.Visible = true;

                                    // Ya no estamos en animaci�n
                                    enAnimacion = false;

                                    // Redibujar el �rbol original
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





        // M�todo para animar el recorrido en el bitmap
        private void AnimarRecorrido(Graphics g, bool postOrden, bool enOrden, bool preOrden)
        {
            // Primero dibujamos el �rbol completo para tener una base
            mi_Arbol.DibujarArbol(g, this.Font, Brushes.Blue, Brushes.White, Pens.Black, Brushes.White, pictureBoxAnimacion.Width);

            // Invalidar el PictureBox para que muestre el dibujo inicial
            this.Invoke(new Action(() =>
            {
                if (pictureBoxAnimacion != null && pictureBoxAnimacion.IsHandleCreated)
                {
                    pictureBoxAnimacion.Invalidate();
                }
            }));

            // Ahora hacemos la animaci�n del recorrido espec�fico
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





        // M�todos para animar los diferentes tipos de recorrido
        private void AnimarRecorridoEnOrden(Graphics g, Nodo_Arbol nodo)
        {
            if (nodo != null)
            {
                // Recorrer sub�rbol izquierdo
                AnimarRecorridoEnOrden(g, nodo.Izquierdo);

                // Resaltar nodo actual
                ResaltarNodo(g, nodo, Brushes.Red);
                Thread.Sleep(1000); // Pausa para visualizaci�n

                // Restaurar color original
                ResaltarNodo(g, nodo, Brushes.Blue);

                // Recorrer sub�rbol derecho
                AnimarRecorridoEnOrden(g, nodo.Derecho);
            }
        }





        private void AnimarRecorridoPreOrden(Graphics g, Nodo_Arbol nodo)
        {
            if (nodo != null)
            {
                // Resaltar nodo actual
                ResaltarNodo(g, nodo, Brushes.Red);
                Thread.Sleep(1000); // Pausa para visualizaci�n

                // Restaurar color original
                ResaltarNodo(g, nodo, Brushes.Blue);

                // Recorrer sub�rbol izquierdo
                AnimarRecorridoPreOrden(g, nodo.Izquierdo);

                // Recorrer sub�rbol derecho
                AnimarRecorridoPreOrden(g, nodo.Derecho);
            }
        }





        private void AnimarRecorridoPostOrden(Graphics g, Nodo_Arbol nodo)
        {
            if (nodo != null)
            {
                // Recorrer sub�rbol izquierdo
                AnimarRecorridoPostOrden(g, nodo.Izquierdo);

                // Recorrer sub�rbol derecho
                AnimarRecorridoPostOrden(g, nodo.Derecho);

                // Resaltar nodo actual
                ResaltarNodo(g, nodo, Brushes.Red);
                Thread.Sleep(1000); // Pausa para visualizaci�n

                // Restaurar color original
                ResaltarNodo(g, nodo, Brushes.Blue);
            }
        }





        // M�todo auxiliar para resaltar un nodo con un color espec�fico
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

                // Invalidar el PictureBox para que se actualice la visualizaci�n
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





        // Manejador del evento click del bot�n Insertar
        private void btnInsertar_Click(object sender, EventArgs e)
        {
            // Verifica que el campo no est� vac�o
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
                        // Inserta el valor en el �rbol
                        bool insertado = mi_Arbol.Insertar(Dato);

                        if (!insertado)
                        {
                            MessageBox.Show("Dato existente en el Arbol", "Error de Ingreso");
                        }
                        else
                        {
                            // Solo cuenta el nodo si realmente se inserto
                            cont++;
                        }

                        // Limpia y enfoca el campo de texto para nueva entrada
                        txtDato.Clear();
                        txtDato.Focus();

                        // Actualiza la visualizaci�n del �rbol
                        DibujarArbol();
                    }
                }
                catch (FormatException)
                {
                    MessageBox.Show("Ingrese un valor num�rico v�lido", "Error de formato");
                    txtDato.Clear();
                    txtDato.Focus();
                }
            }
        }





        // Manejador del evento click del bot�n Eliminar
        private void btnEliminar_Click(object sender, EventArgs e)
        {
            // Verifica que el campo no est� vac�o
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
                        MessageBox.Show("S�lo se admiten valores entre 1 y 99", "Error de Ingreso");
                    }
                    else
                    {
                        // Elimina el nodo con el valor especificado
                        bool eliminado = mi_Arbol.Eliminar(Dato);

                        if (!eliminado)
                        {
                            MessageBox.Show("Nodo NO existente en el Arbol", "Error de eliminacion");
                        }
                        else
                        {
                            // Solo descuenta el nodo si realmente se elimino
                            cont--;
                        }

                        // Limpia y enfoca el campo para nueva entrada
                        txtEliminar.Clear();
                        txtEliminar.Focus();

                        // Actualiza la visualizaci�n del �rbol
                        DibujarArbol();
                    }
                }
                catch (FormatException)
                {
                    MessageBox.Show("Ingrese un valor num�rico v�lido", "Error de formato");
                    txtEliminar.Clear();
                    txtEliminar.Focus();
                }
            }
        }






        // Manejador del evento click del bot�n Buscar
        private void btnBuscar_Click(object sender, EventArgs e)
        {
            // Verifica que el campo no est� vac�o
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
                        MessageBox.Show("S�lo se admiten valores entre 1 y 99", "Error de Ingreso");
                    }
                    else
                    {
                        // Busca el nodo con el valor especificado
                        Nodo_Arbol encontrado = mi_Arbol.Buscar(Dato);

                        if (encontrado != null)
                        {
                            MessageBox.Show("Nodo encontrado en la posicion X: " + encontrado.CoordenadaX + " Y:" + encontrado.CoordenadaY);
                        }
                        else
                        {
                            MessageBox.Show("Nodo NO encontrado", "Error de busqueda");
                        }

                        // Limpia y enfoca el campo para nueva b�squeda
                        txtBuscar.Clear();
                        txtBuscar.Focus();

                        // Actualiza la visualizaci�n (aunque la b�squeda no modifica estructura)
                        DibujarArbol();
                    }
                }
                catch (FormatException)
                {
                    MessageBox.Show("Ingrese un valor num�rico v�lido", "Error de formato");
                    txtBuscar.Clear();
                    txtBuscar.Focus();
                }
            }
        }






        // Manejador del evento click del bot�n En-Orden
        private void btnEnOrden_Click(object sender, EventArgs e)
        {
            if (mi_Arbol.Raiz != null)
            {
                // Muestra el recorrido en-orden (izquierda-ra�z-derecha) en el label
                lblRecorrido.Text = "Recorrido En Orden: " + mi_Arbol.RecorridoEnOrden();

                // Inicia la animaci�n del recorrido
                IniciarAnimacionRecorrido(false, true, false);
            }
            else
            {
                lblRecorrido.Text = "El �rbol est� vac�o";
            }
        }





        // Manejador del evento click del bot�n Pre-Orden
        private void btnPreOrden_Click(object sender, EventArgs e)
        {
            if (mi_Arbol.Raiz != null)
            {
                // Muestra el recorrido pre-orden (ra�z-izquierda-derecha) en el label
                lblRecorrido.Text = "Recorrido Pre-Orden: " + mi_Arbol.RecorridoPreOrden();

                // Inicia la animaci�n del recorrido
                IniciarAnimacionRecorrido(false, false, true);
            }
            else
            {
                lblRecorrido.Text = "El �rbol est� vac�o";
            }
        }





        // Manejador del evento click del bot�n Post-Orden
        private void btnPostOrden_Click(object sender, EventArgs e)
        {
            if (mi_Arbol.Raiz != null)
            {
                // Muestra el recorrido post-orden (izquierda-derecha-ra�z) en el label
                lblRecorrido.Text = "Recorrido Post-Orden: " + mi_Arbol.RecorridoPostOrden();

                // Inicia la animaci�n del recorrido
                IniciarAnimacionRecorrido(true, false, false);
            }
            else
            {
                lblRecorrido.Text = "El �rbol est� vac�o";
            }
        }






        // Manejador del evento click del bot�n Altura
        private void btnAltura_Click(object sender, EventArgs e)
        {
            if (mi_Arbol.Raiz != null)
            {
                // Obtiene la altura calculada desde el m�todo del �rbol
                int altura = mi_Arbol.ObtenerAltura();

                // Muestra el resultado en el control de etiqueta
                lblRecorrido.Text = "Altura del �rbol: " + altura;
            }
            else
            {
                lblRecorrido.Text = "El �rbol est� vac�o";
            }
        }






        // Manejador del evento click del bot�n Sumar Nodos
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
                lblRecorrido.Text = "El �rbol est� vac�o";
            }
        }





        // Manejador del evento click del bot�n Contar Nodos
        private void btnContarNodos_Click(object sender, EventArgs e)
        {
            if (mi_Arbol.Raiz != null)
            {
                // Obtiene la cantidad total de nodos existentes
                int cantidad = mi_Arbol.ContarNodos();

                // Muestra el resultado en la etiqueta
                lblRecorrido.Text = "N�mero de nodos en el �rbol: " + cantidad;
            }
            else
            {
                lblRecorrido.Text = "El �rbol est� vac�o";
            }
        }






        // Manejador del evento click del bot�n Profundidad
        private void btnProfundidad_Click(object sender, EventArgs e)
        {
            // Verifica que el campo no est� vac�o
            if (txtProfundidad.Text == "")
            {
                MessageBox.Show("Debe ingresar un valor para buscar su profundidad");
            }
            else
            {
                // Verifica que el �rbol no est� vac�o
                if (mi_Arbol.Raiz != null)
                {
                    int valor;
                    // Intenta convertir usando TryParse (m�s seguro que Parse)
                    if (int.TryParse(txtProfundidad.Text, out valor))
                    {
                        // Busca la profundidad del nodo con el valor especificado
                        int profundidad = mi_Arbol.ObtenerProfundidad(valor);

                        if (profundidad != -1)
                            // Nodo encontrado - muestra su nivel de profundidad
                            lblRecorrido.Text = "Profundidad del nodo " + valor + ": " + profundidad;
                        else
                            // Nodo no encontrado
                            lblRecorrido.Text = "El nodo " + valor + " no existe en el �rbol";
                    }
                    else
                    {
                        MessageBox.Show("Ingrese un valor num�rico v�lido", "Error de entrada");
                    }
                }
                else
                {
                    lblRecorrido.Text = "El �rbol est� vac�o";
                }

                // Limpia y enfoca el campo para nueva consulta
                txtProfundidad.Clear();
                txtProfundidad.Focus();
            }
        }
    }
}
namespace ArbolBinario
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            lblTitulo = new Label();
            txtDato = new TextBox();
            txtEliminar = new TextBox();
            txtBuscar = new TextBox();
            button1 = new Button();
            button2 = new Button();
            button3 = new Button();
            button4 = new Button();
            button5 = new Button();
            button6 = new Button();
            lblRecorrido = new Label();
            btnAltura = new Button();
            btnSumarNodos = new Button();
            btnContarNodos = new Button();
            txtProfundidad = new TextBox();
            btnProfundidad = new Button();
            SuspendLayout();
            // 
            // lblTitulo
            // 
            lblTitulo.AutoSize = true;
            lblTitulo.BackColor = SystemColors.ButtonShadow;
            lblTitulo.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTitulo.ForeColor = SystemColors.ControlLightLight;
            lblTitulo.Location = new Point(212, 9);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(486, 28);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "\"SIMULADOR DE ÁRBOL BINARIO DE BÚSQUEDA\"";
            // 
            // txtDato
            // 
            txtDato.Location = new Point(36, 100);
            txtDato.Name = "txtDato";
            txtDato.Size = new Size(245, 27);
            txtDato.TabIndex = 1;
            // 
            // txtEliminar
            // 
            txtEliminar.Location = new Point(334, 100);
            txtEliminar.Name = "txtEliminar";
            txtEliminar.Size = new Size(245, 27);
            txtEliminar.TabIndex = 2;
            // 
            // txtBuscar
            // 
            txtBuscar.Location = new Point(626, 100);
            txtBuscar.Name = "txtBuscar";
            txtBuscar.Size = new Size(245, 27);
            txtBuscar.TabIndex = 3;
            // 
            // button1
            // 
            button1.BackColor = Color.LimeGreen;
            button1.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            button1.ForeColor = SystemColors.ControlLightLight;
            button1.Location = new Point(36, 53);
            button1.Name = "button1";
            button1.Size = new Size(245, 41);
            button1.TabIndex = 4;
            button1.Text = "Insertar Nodo";
            button1.UseVisualStyleBackColor = false;
            button1.Click += btnInsertar_Click;
            // 
            // button2
            // 
            button2.BackColor = Color.Red;
            button2.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            button2.ForeColor = SystemColors.ControlLightLight;
            button2.Location = new Point(334, 53);
            button2.Name = "button2";
            button2.Size = new Size(245, 41);
            button2.TabIndex = 5;
            button2.Text = "Eliminar Nodo";
            button2.UseVisualStyleBackColor = false;
            button2.Click += btnEliminar_Click;
            // 
            // button3
            // 
            button3.BackColor = Color.DeepSkyBlue;
            button3.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            button3.ForeColor = SystemColors.ControlLightLight;
            button3.Location = new Point(626, 53);
            button3.Name = "button3";
            button3.Size = new Size(245, 41);
            button3.TabIndex = 6;
            button3.Text = "Buscar Nodo";
            button3.UseVisualStyleBackColor = false;
            button3.Click += btnBuscar_Click;
            // 
            // button4
            // 
            button4.BackColor = Color.Magenta;
            button4.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            button4.ForeColor = SystemColors.ControlLightLight;
            button4.Location = new Point(36, 560);
            button4.Name = "button4";
            button4.Size = new Size(245, 40);
            button4.TabIndex = 7;
            button4.Text = "Recorrido In-Orden";
            button4.UseVisualStyleBackColor = false;
            button4.Click += btnEnOrden_Click;
            // 
            // button5
            // 
            button5.BackColor = Color.Magenta;
            button5.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            button5.ForeColor = SystemColors.ControlLightLight;
            button5.Location = new Point(334, 560);
            button5.Name = "button5";
            button5.Size = new Size(245, 40);
            button5.TabIndex = 8;
            button5.Text = "Recorrido Pre-Orden";
            button5.UseVisualStyleBackColor = false;
            button5.Click += btnPreOrden_Click;
            // 
            // button6
            // 
            button6.BackColor = Color.Magenta;
            button6.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            button6.ForeColor = SystemColors.ControlLightLight;
            button6.Location = new Point(626, 560);
            button6.Name = "button6";
            button6.Size = new Size(245, 40);
            button6.TabIndex = 9;
            button6.Text = "Recorrido Post-Orden";
            button6.UseVisualStyleBackColor = false;
            button6.Click += btnPostOrden_Click;
            // 
            // lblRecorrido
            // 
            lblRecorrido.BackColor = SystemColors.ControlDarkDark;
            lblRecorrido.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblRecorrido.ForeColor = SystemColors.ControlLightLight;
            lblRecorrido.Location = new Point(36, 617);
            lblRecorrido.Name = "lblRecorrido";
            lblRecorrido.Size = new Size(835, 34);
            lblRecorrido.TabIndex = 11;
            lblRecorrido.Text = "Recorrido:";
            lblRecorrido.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // btnAltura
            // 
            btnAltura.BackColor = Color.Gold;
            btnAltura.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnAltura.Location = new Point(36, 664);
            btnAltura.Name = "btnAltura";
            btnAltura.Size = new Size(245, 37);
            btnAltura.TabIndex = 12;
            btnAltura.Text = "Mostrar Altura";
            btnAltura.UseVisualStyleBackColor = false;
            btnAltura.Click += btnAltura_Click;
            // 
            // btnSumarNodos
            // 
            btnSumarNodos.BackColor = Color.Gold;
            btnSumarNodos.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnSumarNodos.Location = new Point(334, 664);
            btnSumarNodos.Name = "btnSumarNodos";
            btnSumarNodos.Size = new Size(245, 37);
            btnSumarNodos.TabIndex = 13;
            btnSumarNodos.Text = "Sumar Nodos";
            btnSumarNodos.UseVisualStyleBackColor = false;
            btnSumarNodos.Click += btnSumarNodos_Click;
            // 
            // btnContarNodos
            // 
            btnContarNodos.BackColor = Color.Gold;
            btnContarNodos.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnContarNodos.Location = new Point(626, 664);
            btnContarNodos.Name = "btnContarNodos";
            btnContarNodos.Size = new Size(245, 37);
            btnContarNodos.TabIndex = 14;
            btnContarNodos.Text = "Contar Nodos";
            btnContarNodos.UseVisualStyleBackColor = false;
            btnContarNodos.Click += btnContarNodos_Click;
            // 
            // txtProfundidad
            // 
            txtProfundidad.Location = new Point(453, 725);
            txtProfundidad.Name = "txtProfundidad";
            txtProfundidad.Size = new Size(245, 27);
            txtProfundidad.TabIndex = 15;
            // 
            // btnProfundidad
            // 
            btnProfundidad.BackColor = Color.RoyalBlue;
            btnProfundidad.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnProfundidad.ForeColor = SystemColors.ControlLightLight;
            btnProfundidad.Location = new Point(178, 717);
            btnProfundidad.Name = "btnProfundidad";
            btnProfundidad.Size = new Size(245, 40);
            btnProfundidad.TabIndex = 16;
            btnProfundidad.Text = "Mostrar Profundidad";
            btnProfundidad.UseVisualStyleBackColor = false;
            btnProfundidad.Click += btnProfundidad_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(971, 769);
            Controls.Add(btnProfundidad);
            Controls.Add(txtProfundidad);
            Controls.Add(btnContarNodos);
            Controls.Add(btnSumarNodos);
            Controls.Add(btnAltura);
            Controls.Add(lblRecorrido);
            Controls.Add(button6);
            Controls.Add(button5);
            Controls.Add(button4);
            Controls.Add(button3);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(txtBuscar);
            Controls.Add(txtEliminar);
            Controls.Add(txtDato);
            Controls.Add(lblTitulo);
            Name = "Form1";
            Text = "-";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblTitulo;
        private TextBox txtDato;
        private TextBox txtEliminar;
        private TextBox txtBuscar;
        private Button button1;
        private Button button2;
        private Button button3;
        private Button button4;
        private Button button5;
        private Button button6;
        private Label lblRecorrido;
        private Button btnAltura;
        private Button btnSumarNodos;
        private Button btnContarNodos;
        private TextBox txtProfundidad;
        private Button btnProfundidad;
    }
}

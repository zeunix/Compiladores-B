namespace Primeros
{
    partial class Form1
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.ColumnHeader Primero;
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.PrimerosView = new System.Windows.Forms.ListView();
            this.Productor = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.GramaticaTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.NTerminalesBox = new System.Windows.Forms.ListBox();
            this.TerminalesBox = new System.Windows.Forms.ListBox();
            this.primeros_button = new System.Windows.Forms.Button();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.button4 = new System.Windows.Forms.Button();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.listView1 = new System.Windows.Forms.ListView();
            this.Estado = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader11 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.listView2 = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.CadenaW = new System.Windows.Forms.TextBox();
            this.button5 = new System.Windows.Forms.Button();
            Primero = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // Primero
            // 
            Primero.Tag = "";
            Primero.Text = "Primeros";
            Primero.Width = 98;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // button1
            // 
            this.button1.Cursor = System.Windows.Forms.Cursors.Default;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button1.Location = new System.Drawing.Point(12, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(103, 39);
            this.button1.TabIndex = 0;
            this.button1.Text = "Seleccionar un Archivo";
            this.button1.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button2.Location = new System.Drawing.Point(121, 12);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(85, 39);
            this.button2.TabIndex = 3;
            this.button2.Text = "cargar";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // PrimerosView
            // 
            this.PrimerosView.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.PrimerosView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Productor,
            Primero});
            this.PrimerosView.Font = new System.Drawing.Font("Lucida Sans", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PrimerosView.GridLines = true;
            this.PrimerosView.Location = new System.Drawing.Point(405, 66);
            this.PrimerosView.Name = "PrimerosView";
            this.PrimerosView.Size = new System.Drawing.Size(210, 188);
            this.PrimerosView.TabIndex = 4;
            this.PrimerosView.UseCompatibleStateImageBehavior = false;
            this.PrimerosView.View = System.Windows.Forms.View.Details;
            // 
            // Productor
            // 
            this.Productor.Tag = "Productor";
            this.Productor.Text = "Productor";
            this.Productor.Width = 107;
            // 
            // GramaticaTextBox
            // 
            this.GramaticaTextBox.Font = new System.Drawing.Font("Lucida Sans", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GramaticaTextBox.Location = new System.Drawing.Point(12, 66);
            this.GramaticaTextBox.Multiline = true;
            this.GramaticaTextBox.Name = "GramaticaTextBox";
            this.GramaticaTextBox.Size = new System.Drawing.Size(194, 188);
            this.GramaticaTextBox.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(225, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "No terminales";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(306, 34);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(54, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "terminales";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(402, 34);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 13);
            this.label3.TabIndex = 11;
            this.label3.Text = "Primeros";
            // 
            // NTerminalesBox
            // 
            this.NTerminalesBox.Font = new System.Drawing.Font("Lucida Sans", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NTerminalesBox.FormattingEnabled = true;
            this.NTerminalesBox.ItemHeight = 18;
            this.NTerminalesBox.Location = new System.Drawing.Point(228, 66);
            this.NTerminalesBox.Name = "NTerminalesBox";
            this.NTerminalesBox.Size = new System.Drawing.Size(75, 184);
            this.NTerminalesBox.TabIndex = 12;
            // 
            // TerminalesBox
            // 
            this.TerminalesBox.Font = new System.Drawing.Font("Lucida Sans", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TerminalesBox.FormattingEnabled = true;
            this.TerminalesBox.ItemHeight = 15;
            this.TerminalesBox.Location = new System.Drawing.Point(309, 66);
            this.TerminalesBox.Name = "TerminalesBox";
            this.TerminalesBox.Size = new System.Drawing.Size(67, 184);
            this.TerminalesBox.TabIndex = 13;
            // 
            // primeros_button
            // 
            this.primeros_button.Location = new System.Drawing.Point(464, 21);
            this.primeros_button.Name = "primeros_button";
            this.primeros_button.Size = new System.Drawing.Size(121, 39);
            this.primeros_button.TabIndex = 14;
            this.primeros_button.Text = "Calcular Primeros";
            this.primeros_button.UseVisualStyleBackColor = true;
            this.primeros_button.Click += new System.EventHandler(this.primeros_button_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(618, 17);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(132, 39);
            this.button4.TabIndex = 20;
            this.button4.Text = "Generar Lr1";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // treeView1
            // 
            this.treeView1.Location = new System.Drawing.Point(602, 276);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(539, 314);
            this.treeView1.TabIndex = 21;
            // 
            // listView1
            // 
            this.listView1.Activation = System.Windows.Forms.ItemActivation.TwoClick;
            this.listView1.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Estado,
            this.columnHeader11});
            this.listView1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)), true);
            this.listView1.ForeColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.listView1.GridLines = true;
            this.listView1.LabelEdit = true;
            this.listView1.Location = new System.Drawing.Point(756, 17);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(385, 233);
            this.listView1.TabIndex = 22;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // Estado
            // 
            this.Estado.Text = "Estado";
            // 
            // columnHeader11
            // 
            this.columnHeader11.Text = "";
            // 
            // listView2
            // 
            this.listView2.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4});
            this.listView2.Location = new System.Drawing.Point(107, 276);
            this.listView2.Name = "listView2";
            this.listView2.Size = new System.Drawing.Size(489, 314);
            this.listView2.TabIndex = 23;
            this.listView2.UseCompatibleStateImageBehavior = false;
            this.listView2.View = System.Windows.Forms.View.Details;
            this.listView2.SelectedIndexChanged += new System.EventHandler(this.listView2_SelectedIndexChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Pila";
            this.columnHeader1.Width = 102;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "token";
            this.columnHeader2.Width = 54;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Cadena de Entrada";
            this.columnHeader3.Width = 106;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Accion";
            // 
            // CadenaW
            // 
            this.CadenaW.Location = new System.Drawing.Point(12, 330);
            this.CadenaW.Name = "CadenaW";
            this.CadenaW.Size = new System.Drawing.Size(89, 20);
            this.CadenaW.TabIndex = 25;
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(12, 276);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(89, 39);
            this.button5.TabIndex = 26;
            this.button5.Text = "Evaluar";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1154, 618);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.CadenaW);
            this.Controls.Add(this.listView2);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.treeView1);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.primeros_button);
            this.Controls.Add(this.TerminalesBox);
            this.Controls.Add(this.NTerminalesBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.GramaticaTextBox);
            this.Controls.Add(this.PrimerosView);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.ListView PrimerosView;
        private System.Windows.Forms.TextBox GramaticaTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ListBox NTerminalesBox;
        private System.Windows.Forms.ListBox TerminalesBox;
        private System.Windows.Forms.Button primeros_button;
        private System.Windows.Forms.ColumnHeader Productor;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader Estado;
        private System.Windows.Forms.ColumnHeader columnHeader11;
        private System.Windows.Forms.ListView listView2;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.TextBox CadenaW;
        private System.Windows.Forms.Button button5;
    }
}


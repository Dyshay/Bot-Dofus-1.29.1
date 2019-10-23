namespace Bot_Dofus_1._29._1.Interfaces
{
    partial class UI_Inventario
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

        #region Código generado por el Diseñador de componentes

        /// <summary> 
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UI_Inventario));
            this.tabControl_mision = new System.Windows.Forms.TabControl();
            this.tabPage_equipamiento = new System.Windows.Forms.TabPage();
            this.dataGridView_equipamientos = new System.Windows.Forms.DataGridView();
            this.id_Inventario_equipamiento = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.id_modelo_equipamiento = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nombre_equipamiento = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cantidad_equipamiento = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.posicion_equipamiento = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.accion_equipamiento = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.eliminar_equipamiento = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabPage_varios = new System.Windows.Forms.TabPage();
            this.dataGridView_varios = new System.Windows.Forms.DataGridView();
            this.id_inventario_varios = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.id_modelo_varios = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nombre_varios = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cantidad_varios = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pods_varios = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.eliminar_varios = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabPage_recursos = new System.Windows.Forms.TabPage();
            this.dataGridView_recursos = new System.Windows.Forms.DataGridView();
            this.id_inventario_recursos = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.id_modelo_recursos = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nombre_recursos = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cantidad_recursos = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Pods_recursos = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.eliminar_recursos = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabPage_mision = new System.Windows.Forms.TabPage();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.dataGridView_mision = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabControl_mision.SuspendLayout();
            this.tabPage_equipamiento.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_equipamientos)).BeginInit();
            this.tabPage_varios.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_varios)).BeginInit();
            this.tabPage_recursos.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_recursos)).BeginInit();
            this.tabPage_mision.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_mision)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl_mision
            // 
            this.tabControl_mision.Controls.Add(this.tabPage_equipamiento);
            this.tabControl_mision.Controls.Add(this.tabPage_varios);
            this.tabControl_mision.Controls.Add(this.tabPage_recursos);
            this.tabControl_mision.Controls.Add(this.tabPage_mision);
            this.tabControl_mision.Cursor = System.Windows.Forms.Cursors.Default;
            this.tabControl_mision.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl_mision.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.tabControl_mision.ImageList = this.imageList1;
            this.tabControl_mision.ItemSize = new System.Drawing.Size(67, 26);
            this.tabControl_mision.Location = new System.Drawing.Point(0, 0);
            this.tabControl_mision.Name = "tabControl_mision";
            this.tabControl_mision.SelectedIndex = 0;
            this.tabControl_mision.Size = new System.Drawing.Size(790, 500);
            this.tabControl_mision.TabIndex = 0;
            // 
            // tabPage_equipamiento
            // 
            this.tabPage_equipamiento.Controls.Add(this.dataGridView_equipamientos);
            this.tabPage_equipamiento.ImageIndex = 0;
            this.tabPage_equipamiento.Location = new System.Drawing.Point(4, 30);
            this.tabPage_equipamiento.Name = "tabPage_equipamiento";
            this.tabPage_equipamiento.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_equipamiento.Size = new System.Drawing.Size(782, 466);
            this.tabPage_equipamiento.TabIndex = 0;
            this.tabPage_equipamiento.Text = "Equipement";
            this.tabPage_equipamiento.UseVisualStyleBackColor = true;
            // 
            // dataGridView_equipamientos
            // 
            this.dataGridView_equipamientos.AllowUserToAddRows = false;
            this.dataGridView_equipamientos.AllowUserToDeleteRows = false;
            this.dataGridView_equipamientos.AllowUserToOrderColumns = true;
            this.dataGridView_equipamientos.AllowUserToResizeColumns = false;
            this.dataGridView_equipamientos.AllowUserToResizeRows = false;
            this.dataGridView_equipamientos.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dataGridView_equipamientos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dataGridView_equipamientos.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.id_Inventario_equipamiento,
            this.id_modelo_equipamiento,
            this.nombre_equipamiento,
            this.cantidad_equipamiento,
            this.posicion_equipamiento,
            this.accion_equipamiento,
            this.eliminar_equipamiento});
            this.dataGridView_equipamientos.Cursor = System.Windows.Forms.Cursors.Default;
            this.dataGridView_equipamientos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView_equipamientos.Location = new System.Drawing.Point(3, 3);
            this.dataGridView_equipamientos.Name = "dataGridView_equipamientos";
            this.dataGridView_equipamientos.ReadOnly = true;
            this.dataGridView_equipamientos.RowHeadersVisible = false;
            this.dataGridView_equipamientos.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView_equipamientos.Size = new System.Drawing.Size(776, 460);
            this.dataGridView_equipamientos.TabIndex = 0;
            this.dataGridView_equipamientos.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView_equipamientos_CellContentClick);
            // 
            // id_Inventario_equipamiento
            // 
            this.id_Inventario_equipamiento.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.id_Inventario_equipamiento.HeaderText = "ID Inventaire";
            this.id_Inventario_equipamiento.MinimumWidth = 110;
            this.id_Inventario_equipamiento.Name = "id_Inventario_equipamiento";
            this.id_Inventario_equipamiento.ReadOnly = true;
            this.id_Inventario_equipamiento.Width = 110;
            // 
            // id_modelo_equipamiento
            // 
            this.id_modelo_equipamiento.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.id_modelo_equipamiento.FillWeight = 90F;
            this.id_modelo_equipamiento.HeaderText = "ID Model";
            this.id_modelo_equipamiento.MinimumWidth = 90;
            this.id_modelo_equipamiento.Name = "id_modelo_equipamiento";
            this.id_modelo_equipamiento.ReadOnly = true;
            this.id_modelo_equipamiento.Width = 95;
            // 
            // nombre_equipamiento
            // 
            this.nombre_equipamiento.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.nombre_equipamiento.HeaderText = "Nom";
            this.nombre_equipamiento.MinimumWidth = 120;
            this.nombre_equipamiento.Name = "nombre_equipamiento";
            this.nombre_equipamiento.ReadOnly = true;
            this.nombre_equipamiento.Width = 120;
            // 
            // cantidad_equipamiento
            // 
            this.cantidad_equipamiento.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.cantidad_equipamiento.FillWeight = 80F;
            this.cantidad_equipamiento.HeaderText = "Quantité";
            this.cantidad_equipamiento.MinimumWidth = 80;
            this.cantidad_equipamiento.Name = "cantidad_equipamiento";
            this.cantidad_equipamiento.ReadOnly = true;
            this.cantidad_equipamiento.Width = 85;
            // 
            // posicion_equipamiento
            // 
            this.posicion_equipamiento.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.posicion_equipamiento.HeaderText = "Position";
            this.posicion_equipamiento.MinimumWidth = 100;
            this.posicion_equipamiento.Name = "posicion_equipamiento";
            this.posicion_equipamiento.ReadOnly = true;
            // 
            // accion_equipamiento
            // 
            this.accion_equipamiento.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.accion_equipamiento.HeaderText = "Action";
            this.accion_equipamiento.MaxInputLength = 50;
            this.accion_equipamiento.MinimumWidth = 100;
            this.accion_equipamiento.Name = "accion_equipamiento";
            this.accion_equipamiento.ReadOnly = true;
            // 
            // eliminar_equipamiento
            // 
            this.eliminar_equipamiento.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.eliminar_equipamiento.HeaderText = "Supprimer";
            this.eliminar_equipamiento.MinimumWidth = 100;
            this.eliminar_equipamiento.Name = "eliminar_equipamiento";
            this.eliminar_equipamiento.ReadOnly = true;
            // 
            // tabPage_varios
            // 
            this.tabPage_varios.Controls.Add(this.dataGridView_varios);
            this.tabPage_varios.ImageIndex = 1;
            this.tabPage_varios.Location = new System.Drawing.Point(4, 30);
            this.tabPage_varios.Name = "tabPage_varios";
            this.tabPage_varios.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_varios.Size = new System.Drawing.Size(782, 466);
            this.tabPage_varios.TabIndex = 1;
            this.tabPage_varios.Text = "Divers";
            this.tabPage_varios.UseVisualStyleBackColor = true;
            // 
            // dataGridView_varios
            // 
            this.dataGridView_varios.AllowUserToAddRows = false;
            this.dataGridView_varios.AllowUserToDeleteRows = false;
            this.dataGridView_varios.AllowUserToOrderColumns = true;
            this.dataGridView_varios.AllowUserToResizeColumns = false;
            this.dataGridView_varios.AllowUserToResizeRows = false;
            this.dataGridView_varios.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dataGridView_varios.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_varios.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.id_inventario_varios,
            this.id_modelo_varios,
            this.nombre_varios,
            this.cantidad_varios,
            this.pods_varios,
            this.eliminar_varios});
            this.dataGridView_varios.Cursor = System.Windows.Forms.Cursors.Default;
            this.dataGridView_varios.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView_varios.Location = new System.Drawing.Point(3, 3);
            this.dataGridView_varios.Name = "dataGridView_varios";
            this.dataGridView_varios.ReadOnly = true;
            this.dataGridView_varios.RowHeadersVisible = false;
            this.dataGridView_varios.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView_varios.Size = new System.Drawing.Size(776, 460);
            this.dataGridView_varios.TabIndex = 2;
            // 
            // id_inventario_varios
            // 
            this.id_inventario_varios.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.id_inventario_varios.HeaderText = "ID Inventaire";
            this.id_inventario_varios.MinimumWidth = 110;
            this.id_inventario_varios.Name = "id_inventario_varios";
            this.id_inventario_varios.ReadOnly = true;
            this.id_inventario_varios.Width = 110;
            // 
            // id_modelo_varios
            // 
            this.id_modelo_varios.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.id_modelo_varios.HeaderText = "ID Model";
            this.id_modelo_varios.MinimumWidth = 100;
            this.id_modelo_varios.Name = "id_modelo_varios";
            this.id_modelo_varios.ReadOnly = true;
            // 
            // nombre_varios
            // 
            this.nombre_varios.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.nombre_varios.HeaderText = "Nom";
            this.nombre_varios.MinimumWidth = 100;
            this.nombre_varios.Name = "nombre_varios";
            this.nombre_varios.ReadOnly = true;
            // 
            // cantidad_varios
            // 
            this.cantidad_varios.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.cantidad_varios.HeaderText = "Quantité";
            this.cantidad_varios.MinimumWidth = 100;
            this.cantidad_varios.Name = "cantidad_varios";
            this.cantidad_varios.ReadOnly = true;
            // 
            // pods_varios
            // 
            this.pods_varios.HeaderText = "Pods";
            this.pods_varios.Name = "pods_varios";
            this.pods_varios.ReadOnly = true;
            // 
            // eliminar_varios
            // 
            this.eliminar_varios.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.eliminar_varios.HeaderText = "Supprimer";
            this.eliminar_varios.MinimumWidth = 100;
            this.eliminar_varios.Name = "eliminar_varios";
            this.eliminar_varios.ReadOnly = true;
            // 
            // tabPage_recursos
            // 
            this.tabPage_recursos.Controls.Add(this.dataGridView_recursos);
            this.tabPage_recursos.ImageIndex = 2;
            this.tabPage_recursos.Location = new System.Drawing.Point(4, 30);
            this.tabPage_recursos.Name = "tabPage_recursos";
            this.tabPage_recursos.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_recursos.Size = new System.Drawing.Size(782, 466);
            this.tabPage_recursos.TabIndex = 2;
            this.tabPage_recursos.Text = "Ressources";
            this.tabPage_recursos.UseVisualStyleBackColor = true;
            // 
            // dataGridView_recursos
            // 
            this.dataGridView_recursos.AllowUserToAddRows = false;
            this.dataGridView_recursos.AllowUserToDeleteRows = false;
            this.dataGridView_recursos.AllowUserToOrderColumns = true;
            this.dataGridView_recursos.AllowUserToResizeColumns = false;
            this.dataGridView_recursos.AllowUserToResizeRows = false;
            this.dataGridView_recursos.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dataGridView_recursos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_recursos.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.id_inventario_recursos,
            this.id_modelo_recursos,
            this.nombre_recursos,
            this.cantidad_recursos,
            this.Pods_recursos,
            this.eliminar_recursos});
            this.dataGridView_recursos.Cursor = System.Windows.Forms.Cursors.Default;
            this.dataGridView_recursos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView_recursos.Location = new System.Drawing.Point(3, 3);
            this.dataGridView_recursos.Name = "dataGridView_recursos";
            this.dataGridView_recursos.ReadOnly = true;
            this.dataGridView_recursos.RowHeadersVisible = false;
            this.dataGridView_recursos.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView_recursos.Size = new System.Drawing.Size(776, 460);
            this.dataGridView_recursos.TabIndex = 1;
            this.dataGridView_recursos.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView_recursos_CellContentClick);
            // 
            // id_inventario_recursos
            // 
            this.id_inventario_recursos.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.id_inventario_recursos.HeaderText = "ID Inventaire";
            this.id_inventario_recursos.MinimumWidth = 110;
            this.id_inventario_recursos.Name = "id_inventario_recursos";
            this.id_inventario_recursos.ReadOnly = true;
            this.id_inventario_recursos.Width = 110;
            // 
            // id_modelo_recursos
            // 
            this.id_modelo_recursos.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.id_modelo_recursos.HeaderText = "ID Model";
            this.id_modelo_recursos.MinimumWidth = 100;
            this.id_modelo_recursos.Name = "id_modelo_recursos";
            this.id_modelo_recursos.ReadOnly = true;
            // 
            // nombre_recursos
            // 
            this.nombre_recursos.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.nombre_recursos.HeaderText = "Nom";
            this.nombre_recursos.MinimumWidth = 100;
            this.nombre_recursos.Name = "nombre_recursos";
            this.nombre_recursos.ReadOnly = true;
            // 
            // cantidad_recursos
            // 
            this.cantidad_recursos.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.cantidad_recursos.HeaderText = "Quantité";
            this.cantidad_recursos.MinimumWidth = 100;
            this.cantidad_recursos.Name = "cantidad_recursos";
            this.cantidad_recursos.ReadOnly = true;
            // 
            // Pods_recursos
            // 
            this.Pods_recursos.HeaderText = "Pods";
            this.Pods_recursos.Name = "Pods_recursos";
            this.Pods_recursos.ReadOnly = true;
            // 
            // eliminar_recursos
            // 
            this.eliminar_recursos.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.eliminar_recursos.HeaderText = "Supprimer";
            this.eliminar_recursos.MinimumWidth = 100;
            this.eliminar_recursos.Name = "eliminar_recursos";
            this.eliminar_recursos.ReadOnly = true;
            // 
            // tabPage_mision
            // 
            this.tabPage_mision.Controls.Add(this.dataGridView_mision);
            this.tabPage_mision.ImageIndex = 3;
            this.tabPage_mision.Location = new System.Drawing.Point(4, 30);
            this.tabPage_mision.Name = "tabPage_mision";
            this.tabPage_mision.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_mision.Size = new System.Drawing.Size(782, 466);
            this.tabPage_mision.TabIndex = 3;
            this.tabPage_mision.Text = "Objet de quête";
            this.tabPage_mision.UseVisualStyleBackColor = true;
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "espada.png");
            this.imageList1.Images.SetKeyName(1, "pocion.png");
            this.imageList1.Images.SetKeyName(2, "recursos.png");
            this.imageList1.Images.SetKeyName(3, "llave.png");
            // 
            // dataGridView_mision
            // 
            this.dataGridView_mision.AllowUserToAddRows = false;
            this.dataGridView_mision.AllowUserToDeleteRows = false;
            this.dataGridView_mision.AllowUserToOrderColumns = true;
            this.dataGridView_mision.AllowUserToResizeColumns = false;
            this.dataGridView_mision.AllowUserToResizeRows = false;
            this.dataGridView_mision.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dataGridView_mision.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_mision.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2,
            this.dataGridViewTextBoxColumn3,
            this.dataGridViewTextBoxColumn4});
            this.dataGridView_mision.Cursor = System.Windows.Forms.Cursors.Default;
            this.dataGridView_mision.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView_mision.Location = new System.Drawing.Point(3, 3);
            this.dataGridView_mision.Name = "dataGridView_mision";
            this.dataGridView_mision.ReadOnly = true;
            this.dataGridView_mision.RowHeadersVisible = false;
            this.dataGridView_mision.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView_mision.Size = new System.Drawing.Size(776, 460);
            this.dataGridView_mision.TabIndex = 2;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.dataGridViewTextBoxColumn1.HeaderText = "ID Inventaire";
            this.dataGridViewTextBoxColumn1.MinimumWidth = 110;
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Width = 110;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.dataGridViewTextBoxColumn2.HeaderText = "ID Modelo";
            this.dataGridViewTextBoxColumn2.MinimumWidth = 100;
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.dataGridViewTextBoxColumn3.HeaderText = "Nom";
            this.dataGridViewTextBoxColumn3.MinimumWidth = 100;
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.dataGridViewTextBoxColumn4.HeaderText = "Quantité";
            this.dataGridViewTextBoxColumn4.MaxInputLength = 200;
            this.dataGridViewTextBoxColumn4.MinimumWidth = 100;
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.ReadOnly = true;
            // 
            // UI_Inventario
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl_mision);
            this.Name = "UI_Inventario";
            this.Size = new System.Drawing.Size(790, 500);
            this.tabControl_mision.ResumeLayout(false);
            this.tabPage_equipamiento.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_equipamientos)).EndInit();
            this.tabPage_varios.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_varios)).EndInit();
            this.tabPage_recursos.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_recursos)).EndInit();
            this.tabPage_mision.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_mision)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl_mision;
        private System.Windows.Forms.TabPage tabPage_equipamiento;
        private System.Windows.Forms.TabPage tabPage_varios;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.TabPage tabPage_recursos;
        private System.Windows.Forms.TabPage tabPage_mision;
        private System.Windows.Forms.DataGridView dataGridView_equipamientos;
        private System.Windows.Forms.DataGridView dataGridView_recursos;
        private System.Windows.Forms.DataGridView dataGridView_varios;
        private System.Windows.Forms.DataGridViewTextBoxColumn id_inventario_varios;
        private System.Windows.Forms.DataGridViewTextBoxColumn id_modelo_varios;
        private System.Windows.Forms.DataGridViewTextBoxColumn nombre_varios;
        private System.Windows.Forms.DataGridViewTextBoxColumn cantidad_varios;
        private System.Windows.Forms.DataGridViewTextBoxColumn pods_varios;
        private System.Windows.Forms.DataGridViewTextBoxColumn eliminar_varios;
        private System.Windows.Forms.DataGridViewTextBoxColumn id_inventario_recursos;
        private System.Windows.Forms.DataGridViewTextBoxColumn id_modelo_recursos;
        private System.Windows.Forms.DataGridViewTextBoxColumn nombre_recursos;
        private System.Windows.Forms.DataGridViewTextBoxColumn cantidad_recursos;
        private System.Windows.Forms.DataGridViewTextBoxColumn Pods_recursos;
        private System.Windows.Forms.DataGridViewTextBoxColumn eliminar_recursos;
        private System.Windows.Forms.DataGridViewTextBoxColumn id_Inventario_equipamiento;
        private System.Windows.Forms.DataGridViewTextBoxColumn id_modelo_equipamiento;
        private System.Windows.Forms.DataGridViewTextBoxColumn nombre_equipamiento;
        private System.Windows.Forms.DataGridViewTextBoxColumn cantidad_equipamiento;
        private System.Windows.Forms.DataGridViewTextBoxColumn posicion_equipamiento;
        private System.Windows.Forms.DataGridViewTextBoxColumn accion_equipamiento;
        private System.Windows.Forms.DataGridViewTextBoxColumn eliminar_equipamiento;
        private System.Windows.Forms.DataGridView dataGridView_mision;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
    }
}

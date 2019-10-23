using Bot_Dofus_1._29._1.Controles.ControlMapa;

namespace Bot_Dofus_1._29._1.Interfaces
{
    partial class UI_Mapa
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.label_mapa_id = new System.Windows.Forms.Label();
            this.checkBox_mostrar_celdas = new System.Windows.Forms.CheckBox();
            this.checkBox_animaciones = new System.Windows.Forms.CheckBox();
            this.comboBox_calidad_minimapa = new System.Windows.Forms.ComboBox();
            this.control_mapa = new Bot_Dofus_1._29._1.Controles.ControlMapa.ControlMapa();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.control_mapa, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 92F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(811, 514);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 4;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel3.Controls.Add(this.label_mapa_id, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.checkBox_mostrar_celdas, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.checkBox_animaciones, 2, 0);
            this.tableLayoutPanel3.Controls.Add(this.comboBox_calidad_minimapa, 3, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 475);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 36F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(805, 36);
            this.tableLayoutPanel3.TabIndex = 0;
            // 
            // label_mapa_id
            // 
            this.label_mapa_id.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label_mapa_id.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.label_mapa_id.Location = new System.Drawing.Point(3, 0);
            this.label_mapa_id.Name = "label_mapa_id";
            this.label_mapa_id.Size = new System.Drawing.Size(195, 36);
            this.label_mapa_id.TabIndex = 0;
            this.label_mapa_id.Text = "Position : ";
            this.label_mapa_id.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // checkBox_mostrar_celdas
            // 
            this.checkBox_mostrar_celdas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkBox_mostrar_celdas.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.checkBox_mostrar_celdas.Location = new System.Drawing.Point(204, 3);
            this.checkBox_mostrar_celdas.Name = "checkBox_mostrar_celdas";
            this.checkBox_mostrar_celdas.Size = new System.Drawing.Size(195, 30);
            this.checkBox_mostrar_celdas.TabIndex = 1;
            this.checkBox_mostrar_celdas.Text = "Montrer les cellules";
            this.checkBox_mostrar_celdas.UseVisualStyleBackColor = true;
            this.checkBox_mostrar_celdas.CheckedChanged += new System.EventHandler(this.checkBox_mostrar_celdas_CheckedChanged);
            // 
            // checkBox_animaciones
            // 
            this.checkBox_animaciones.Checked = true;
            this.checkBox_animaciones.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_animaciones.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkBox_animaciones.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.checkBox_animaciones.Location = new System.Drawing.Point(405, 3);
            this.checkBox_animaciones.Name = "checkBox_animaciones";
            this.checkBox_animaciones.Size = new System.Drawing.Size(195, 30);
            this.checkBox_animaciones.TabIndex = 2;
            this.checkBox_animaciones.Text = "Montrer les animations";
            this.checkBox_animaciones.UseVisualStyleBackColor = true;
            this.checkBox_animaciones.CheckedChanged += new System.EventHandler(this.checkBox_animaciones_CheckedChanged);
            // 
            // comboBox_calidad_minimapa
            // 
            this.comboBox_calidad_minimapa.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBox_calidad_minimapa.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_calidad_minimapa.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.comboBox_calidad_minimapa.FormattingEnabled = true;
            this.comboBox_calidad_minimapa.Items.AddRange(new object[] {
            "Qualité basse",
            "Qualité moyenne",
            "Qualité haute"});
            this.comboBox_calidad_minimapa.Location = new System.Drawing.Point(606, 3);
            this.comboBox_calidad_minimapa.Name = "comboBox_calidad_minimapa";
            this.comboBox_calidad_minimapa.Size = new System.Drawing.Size(196, 25);
            this.comboBox_calidad_minimapa.TabIndex = 3;
            this.comboBox_calidad_minimapa.SelectedIndexChanged += new System.EventHandler(this.comboBox_calidad_minimapa_SelectedIndexChanged);
            // 
            // control_mapa
            // 
            this.control_mapa.BorderColorOnOver = System.Drawing.Color.Empty;
            this.control_mapa.ColorCeldaActiva = System.Drawing.Color.Gray;
            this.control_mapa.ColorCeldaInactiva = System.Drawing.Color.DarkGray;
            this.control_mapa.Dock = System.Windows.Forms.DockStyle.Fill;
            this.control_mapa.Location = new System.Drawing.Point(3, 3);
            this.control_mapa.mapa_altura = ((byte)(17));
            this.control_mapa.mapa_anchura = ((byte)(15));
            this.control_mapa.Mostrar_Animaciones = true;
            this.control_mapa.Mostrar_Celdas_Id = false;
            this.control_mapa.Name = "control_mapa";
            this.control_mapa.Size = new System.Drawing.Size(805, 466);
            this.control_mapa.TabIndex = 1;
            this.control_mapa.TipoCalidad = Bot_Dofus_1._29._1.Controles.ControlMapa.CalidadMapa.MEDIA;
            this.control_mapa.TraceOnOver = false;
            // 
            // UI_Mapa
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "UI_Mapa";
            this.Size = new System.Drawing.Size(811, 514);
            this.Load += new System.EventHandler(this.UI_Mapa_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Label label_mapa_id;
        private System.Windows.Forms.CheckBox checkBox_mostrar_celdas;
        private System.Windows.Forms.CheckBox checkBox_animaciones;
        private System.Windows.Forms.ComboBox comboBox_calidad_minimapa;
        private ControlMapa control_mapa;
    }
}

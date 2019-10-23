using Bot_Dofus_1._29._1.Otros;
using Bot_Dofus_1._29._1.Otros.Game.Personaje.Inventario;
using Bot_Dofus_1._29._1.Otros.Game.Personaje.Inventario.Enums;
using Microsoft.VisualBasic;
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bot_Dofus_1._29._1.Interfaces
{
    public partial class UI_Inventario : UserControl
    {
        private Cuenta cuenta = null;

        public UI_Inventario(Cuenta _cuenta)
        {
            InitializeComponent();
            set_DoubleBuffered(dataGridView_equipamientos);
            set_DoubleBuffered(dataGridView_varios);
            set_DoubleBuffered(dataGridView_recursos);

            cuenta = _cuenta;
            cuenta.juego.personaje.inventario.inventario_actualizado += actualizar_Inventario;
        }

        private void actualizar_Inventario(bool objetos_actualizados)
        {
            if (!objetos_actualizados)
                return;

            Task.Run(() =>
            {
                if (!IsHandleCreated)
                    return;

                BeginInvoke((Action)(() =>
                {
                    dataGridView_equipamientos.Rows.Clear();
                    foreach (ObjetosInventario obj in cuenta.juego.personaje.inventario.equipamiento)
                        dataGridView_equipamientos.Rows.Add(new object[] { obj.id_inventario, obj.id_modelo, obj.nombre, obj.cantidad, obj.posicion, obj.posicion == InventarioPosiciones.NON_ÉQUIPÉ ? "Équiper" : "Déséquiper", "Supprimer" });

                    dataGridView_varios.Rows.Clear();
                    foreach (ObjetosInventario obj in cuenta.juego.personaje.inventario.varios)
                        dataGridView_varios.Rows.Add(new object[] { obj.id_inventario, obj.id_modelo, obj.nombre, obj.cantidad, obj.pods, "Supprimer" });

                    dataGridView_recursos.Rows.Clear();
                    foreach (ObjetosInventario obj in cuenta.juego.personaje.inventario.recursos)
                        dataGridView_recursos.Rows.Add(new object[6] { obj.id_inventario, obj.id_modelo, obj.nombre, obj.cantidad, obj.pods, "Supprimer" });

                    dataGridView_mision.Rows.Clear();
                    foreach (ObjetosInventario obj in cuenta.juego.personaje.inventario.mision)
                        dataGridView_mision.Rows.Add(new object[4] { obj.id_inventario, obj.id_modelo, obj.nombre, obj.cantidad });

                }));
            });
        }

        private void dataGridView_equipamientos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 5)
                return;

            ObjetosInventario objeto = cuenta.juego.personaje.inventario.equipamiento.ElementAt(e.RowIndex);
            
            string accion = dataGridView_equipamientos.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();

            switch (accion)
            {
                case "Équiper":
                    cuenta.juego.personaje.inventario.equipar_Objeto(objeto);
                break;

                case "Déséquiper":
                    cuenta.juego.personaje.inventario.desequipar_Objeto(objeto);
                break;

                case "Supprimer":
                    if (MessageBox.Show("Realmente deseas eliminar " + objeto.nombre + "?", "Eliminar un objeto", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        cuenta.juego.personaje.inventario.eliminar_Objeto(objeto, 1, true);
                break;
            }
        }

        private void dataGridView_recursos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 5)
                return;

            ObjetosInventario objeto = cuenta.juego.personaje.inventario.recursos.ElementAt(e.RowIndex);
            string accion = dataGridView_recursos.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();

            if (!int.TryParse(Interaction.InputBox($"Choisissez la quantité à {accion.ToLower()} de l'objet {objeto.nombre} (0 = la totalité):", accion, "1"), out int cantidad))
                return;

            switch (accion)
            {
                case "Supprimer":
                    cuenta.juego.personaje.inventario.eliminar_Objeto(objeto, cantidad, true);
                break;
            }
        }

        public static void set_DoubleBuffered(Control control) => typeof(Control).InvokeMember("DoubleBuffered", BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic, null, control, new object[] { true });
    }
}

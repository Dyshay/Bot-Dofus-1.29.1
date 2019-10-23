using Bot_Dofus_1._29._1.Otros.Game.Personaje.Inventario.Enums;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bot_Dofus_1._29._1.Otros.Game.Personaje.Inventario
{
    public class InventarioGeneral : IDisposable, IEliminable
    {
        private Cuenta cuenta;
        private ConcurrentDictionary<uint, ObjetosInventario> _objetos;
        private bool disposed;

        //Propiedades
        public int kamas { get; private set; }
        public short pods_actuales { get; set; }
        public short pods_maximos { get; set; }

        public IEnumerable<ObjetosInventario> objetos => _objetos.Values;
        public IEnumerable<ObjetosInventario> equipamiento => objetos.Where(o => o.tipo_inventario == TipoObjetosInventario.EQUIPAMIENTO);
        public IEnumerable<ObjetosInventario> varios => objetos.Where(o => o.tipo_inventario == TipoObjetosInventario.VARIOS);
        public IEnumerable<ObjetosInventario> recursos => objetos.Where(o => o.tipo_inventario == TipoObjetosInventario.RECURSOS);
        public IEnumerable<ObjetosInventario> mision => objetos.Where(o => o.tipo_inventario == TipoObjetosInventario.OBJETOS_MISION);
        public int porcentaje_pods => (int)((double)pods_actuales / pods_maximos * 100);

        public event Action<bool> inventario_actualizado;
        public event Action almacenamiento_abierto;
        public event Action almacenamiento_cerrado;

        // Constructor
        internal InventarioGeneral(Cuenta _cuenta)
        {
            cuenta = _cuenta;
            _objetos = new ConcurrentDictionary<uint, ObjetosInventario>();
        }

        public ObjetosInventario get_Objeto_Modelo_Id(int gid) => objetos.FirstOrDefault(f => f.id_modelo == gid);
        public ObjetosInventario get_Objeto_en_Posicion(InventarioPosiciones posicion) => objetos.FirstOrDefault(o => o.posicion == posicion);

        public void agregar_Objetos(string paquete)
        {
            Task.Run(() =>
            {
                foreach (string obj in paquete.Split(';'))
                {
                    if (!string.IsNullOrEmpty(obj))
                    {
                        string[] separador = obj.Split('~');
                        uint id_inventario = Convert.ToUInt32(separador[0], 16);
                        ObjetosInventario objeto = new ObjetosInventario(obj);
                        _objetos.TryAdd(id_inventario, objeto);
                    }
                }
            }).Wait();

            inventario_actualizado?.Invoke(true);
        }

        public void modificar_Objetos(string paquete)
        {
            if (!string.IsNullOrEmpty(paquete))
            {
                string[] separador = paquete.Split('|');
                ObjetosInventario objeto = objetos.FirstOrDefault(f => f.id_inventario == uint.Parse(separador[0]));

                if (objeto != null)
                {
                    int cantidad = int.Parse(separador[1]);
                    ObjetosInventario nuevo_objeto = objeto;
                    nuevo_objeto.cantidad = cantidad;

                    if (_objetos.TryUpdate(objeto.id_inventario, nuevo_objeto, objeto))
                        inventario_actualizado?.Invoke(true);
                }
            }
        }

        public void eliminar_Objeto(ObjetosInventario obj, int cantidad, bool paquete_eliminar)
        {
            if (obj == null)
                return;

            cantidad = cantidad == 0 ? obj.cantidad : cantidad > obj.cantidad ? obj.cantidad : cantidad;

            if (obj.cantidad > cantidad)
            {
                ObjetosInventario nuevo_objeto = obj;
                nuevo_objeto.cantidad -= cantidad;
                _objetos.TryUpdate(obj.id_inventario, nuevo_objeto, obj);
            }
            else
                _objetos.TryRemove(obj.id_inventario, out ObjetosInventario objeto);

            if (paquete_eliminar)
            {
                cuenta.conexion.enviar_Paquete($"Od{obj.id_inventario}|{cantidad}");
                cuenta.logger.log_informacion("INVENTAIRE", $"{cantidad} {obj.nombre} éliminée(s).");
            }

            inventario_actualizado?.Invoke(true);
        }

        public void eliminar_Objeto(uint id_inventario, int cantidad, bool paquete_eliminar)
        {
            if(!_objetos.TryGetValue(id_inventario, out ObjetosInventario obj))
                return;

            eliminar_Objeto(obj, cantidad, paquete_eliminar);
        }

        public bool equipar_Objeto(ObjetosInventario objeto)
        {
            if (objeto == null || objeto.cantidad == 0 || cuenta.esta_ocupado())
            {
                cuenta.logger.log_Error("INVENTAIRE", $"L'objet {objeto.nombre} ne peux être équipé");
                return false;
            }

            if (objeto.nivel > cuenta.juego.personaje.nivel)
            {
                cuenta.logger.log_Error("INVENTAIRE", $"Le niveau de l'objet {objeto.nombre} est supérieur à ton niveau");
                return false;
            }

            if (objeto.posicion != InventarioPosiciones.NON_ÉQUIPÉ)//objeto ya esta equipado
            {
                cuenta.logger.log_Error("INVENTAIRE", $"l'objet {objeto.nombre} est équipé");
                return false;
            }

            List<InventarioPosiciones> possibles_posiciones = InventarioUtiles.get_Posibles_Posiciones(objeto.tipo);

            if (possibles_posiciones == null || possibles_posiciones.Count == 0)//objeto no equipable
            {
                cuenta.logger.log_Error("INVENTARIO", $"L'objet {objeto.nombre} n'est pas équipable");
                return false;
            }

            foreach (InventarioPosiciones posicion in possibles_posiciones)
            {
                if (get_Objeto_en_Posicion(posicion) == null)
                {
                    cuenta.conexion.enviar_Paquete("OM" + objeto.id_inventario + "|" + (sbyte)posicion, true);
                    cuenta.logger.log_informacion("INVENTAIRE", $"{objeto.nombre} équipé.");
                    objeto.posicion = posicion;
                    inventario_actualizado?.Invoke(true);
                    return true;
                }
            }

            //desequipa X objeto si ya esta equipado
            if (_objetos.TryGetValue(get_Objeto_en_Posicion(possibles_posiciones[0]).id_inventario, out ObjetosInventario objeto_equipado))
            {
                objeto_equipado.posicion = InventarioPosiciones.NON_ÉQUIPÉ;
                cuenta.conexion.enviar_Paquete("OM" + objeto_equipado.id_inventario + "|" + (sbyte)InventarioPosiciones.NON_ÉQUIPÉ);
            }

            cuenta.conexion.enviar_Paquete("OM" + objeto.id_inventario + "|" + (sbyte)possibles_posiciones[0]);

            if (objeto.cantidad == 1)
                objeto.posicion = possibles_posiciones[0];

            cuenta.logger.log_informacion("INVENTAIRE", $"{objeto.nombre} équipé.");
            inventario_actualizado?.Invoke(true);
            return true;
        }

        public bool desequipar_Objeto(ObjetosInventario objeto)
        {
            if (objeto == null)
                return false;

            if (objeto.posicion == InventarioPosiciones.NON_ÉQUIPÉ)
                return false;

            cuenta.conexion.enviar_Paquete("OM" + objeto.id_inventario + "|" + (sbyte)InventarioPosiciones.NON_ÉQUIPÉ);
            objeto.posicion = InventarioPosiciones.NON_ÉQUIPÉ;
            cuenta.logger.log_informacion("INVENTAIRE", $"{objeto.nombre} déséquipé.");
            inventario_actualizado?.Invoke(true);
            return true;
        }

        public void utilizar_Objeto(ObjetosInventario objeto)
        {
            if (objeto == null)
                return;

            if(objeto.cantidad == 0)
            {
                cuenta.logger.log_Error("INVENTAIRE", $"L'objet {objeto.nombre} ne peut être mis à cause de tes caractéristiques");
                return;
            }

            cuenta.conexion.enviar_Paquete("OU" + objeto.id_inventario + "|");
            eliminar_Objeto(objeto, 1, false);
            cuenta.logger.log_informacion("INVENTAIRE", $"{objeto.nombre} utilisée.");
        }

        public void evento_Almacenamiento_Abierto() => almacenamiento_abierto?.Invoke();
        public void evento_Almacenamiento_Cerrado() => almacenamiento_cerrado?.Invoke();

        #region Zona Dispose
        public void Dispose() => Dispose(true);
        ~InventarioGeneral() => Dispose(false);

        public void limpiar()
        {
            kamas = 0;
            pods_actuales = 0;
            pods_maximos = 0;
            _objetos.Clear();
        }

        public virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                }
                _objetos.Clear();
                _objetos = null;
                cuenta = null;
                disposed = true;
            }
        }
        #endregion
    }
}

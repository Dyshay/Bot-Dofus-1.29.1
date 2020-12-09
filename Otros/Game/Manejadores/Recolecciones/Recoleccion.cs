using Bot_Dofus_1._29._1.Otros.Enums;
using Bot_Dofus_1._29._1.Otros.Game.Entidades.Manejadores.Movimientos;
using Bot_Dofus_1._29._1.Otros.Game.Character;
using Bot_Dofus_1._29._1.Otros.Game.Character.Inventory;
using Bot_Dofus_1._29._1.Otros.Game.Character.Inventory.Enums;
using Bot_Dofus_1._29._1.Otros.Mapas;
using Bot_Dofus_1._29._1.Otros.Mapas.Interactivo;
using Bot_Dofus_1._29._1.Otros.Mapas.Movimiento.Mapas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

/*
    Este archivo es parte del proyecto BotDofus_1.29.1

    BotDofus_1.29.1 Copyright (C) 2019 Alvaro Prendes — Todos los derechos reservados.
    Creado por Alvaro Prendes
    web: http://www.salesprendes.com
*/

namespace Bot_Dofus_1._29._1.Otros.Game.Entidades.Manejadores.Recolecciones
{
    public class Recoleccion : IDisposable
    {
        private Account cuenta;
        private Map mapa;
        public ObjetoInteractivo interactivo_recolectando;
        private List<int> interactivos_no_utilizables;
        private bool robado;
        private Pathfinder pathfinder;
        private bool disposed;

        public event Action recoleccion_iniciada;
        public event Action<RecoleccionResultado> recoleccion_acabada;

        public static readonly int[] herramientas_pescar = { 8541, 6661, 596, 1866, 1865, 1864, 1867, 2188, 1863, 1862, 1868, 1861, 1860, 2366 };
        
        public Recoleccion(Account _cuenta, Movimiento movimientos, Map _mapa)
        {
            cuenta = _cuenta;
            interactivos_no_utilizables = new List<int>();
            pathfinder = new Pathfinder();
            mapa = _mapa;

            movimientos.movimiento_finalizado += get_Movimiento_Finalizado;
            mapa.mapRefreshEvent += evento_Mapa_Actualizado;
        }

        public bool get_Puede_Recolectar(List<short> elementos_recolectables) => get_Interactivos_Utilizables(elementos_recolectables).Count > 0;
        public void get_Cancelar_Interactivo() => interactivo_recolectando = null;

        public bool get_Recolectar(List<short> elementos)
        {
            if (cuenta.Is_Busy() || interactivo_recolectando != null)
                return false;

            foreach (KeyValuePair<short, ObjetoInteractivo> kvp in get_Interactivos_Utilizables(elementos))
            {
                if (get_Intentar_Mover_Interactivo(kvp))
                    return true;
            }

            cuenta.Logger.LogDanger("RECOLTE", "Aucun objet de collectable trouvé");
            return false;
        }

        private Dictionary<short, ObjetoInteractivo> get_Interactivos_Utilizables(List<short> elementos_ids)
        {
            Dictionary<short, ObjetoInteractivo> elementos_utilizables = new Dictionary<short, ObjetoInteractivo>();
            CharacterClass personaje = cuenta.game.character;

            InventoryObject arma = personaje.inventario.get_Objeto_en_Posicion(InventorySlots.WEAPON);
            byte distancia_arma = 1;
            bool es_herramienta_pescar = false;

            if (arma != null)
            {
                distancia_arma = get_Distancia_herramienta(arma.id_modelo);
                es_herramienta_pescar = herramientas_pescar.Contains(arma.id_modelo);
            }


            foreach (ObjetoInteractivo interactivo in mapa.interactives.Values.OrderBy(x => x.celda.GetDistanceBetweenCells(personaje.celda)))
            {
                if (!interactivo.es_utilizable || !interactivo.modelo.recolectable)
                    continue;

                List<Cell> path = pathfinder.get_Path(personaje.celda, interactivo.celda, mapa.celdas_ocupadas(), true, distancia_arma);

                if (path == null || path.Count == 0)
                    continue;

                foreach (short habilidad in interactivo.modelo.habilidades)
                {
                    if (!elementos_ids.Contains(habilidad))
                        continue;

                    if (!es_herramienta_pescar && path.Last().GetDistanceBetweenCells(interactivo.celda) > 1)
                        continue;

                    if (es_herramienta_pescar && path.Last().GetDistanceBetweenCells(interactivo.celda) > distancia_arma)
                        continue;

                    elementos_utilizables.Add(interactivo.celda.cellId, interactivo);
                }
            }
            
            return elementos_utilizables;
        }

        private bool get_Intentar_Mover_Interactivo(KeyValuePair<short, ObjetoInteractivo> interactivo)
        {
            interactivo_recolectando = interactivo.Value;
            byte distancia_detener = 1;
            InventoryObject arma = cuenta.game.character.inventario.get_Objeto_en_Posicion(InventorySlots.WEAPON);

            if(arma != null)
                distancia_detener = get_Distancia_herramienta(arma.id_modelo);
            
            switch (cuenta.game.manager.movimientos.get_Mover_A_Celda(interactivo_recolectando.celda, mapa.celdas_ocupadas(), true, distancia_detener))
            {
                case ResultadoMovimientos.EXITO:
                case ResultadoMovimientos.SameCell:
                    get_Intentar_Recolectar_Interactivo();
                return true;

                default:
                    get_Cancelar_Interactivo();
               return false;
            }
        }

        private void get_Intentar_Recolectar_Interactivo()
        {
            if (!robado)
            {
                foreach (short habilidad in interactivo_recolectando.modelo.habilidades)
                {
                    if (cuenta.game.character.get_Skills_Recoleccion_Disponibles().Contains(habilidad))
                        cuenta.connexion.SendPacket("GA500" + interactivo_recolectando.celda.cellId + ";" + habilidad);
                }
            }
            else
                evento_Recoleccion_Acabada(RecoleccionResultado.ROBADO, interactivo_recolectando.celda.cellId);
        }

        private void get_Movimiento_Finalizado(bool correcto)
        {
            if (interactivo_recolectando == null)
                return;

            if (!correcto && cuenta.game.manager.movimientos.actual_path != null)
                evento_Recoleccion_Acabada(RecoleccionResultado.FALLO, interactivo_recolectando.celda.cellId);
        }

        public async Task evento_Recoleccion_Iniciada(int id_personaje, int tiempo_delay, short celda_id, byte tipo_gkk)
        {
            if (interactivo_recolectando == null || interactivo_recolectando.celda.cellId != celda_id)
                return;

            if (cuenta.game.character.id != id_personaje)
            {
                robado = true;
                cuenta.Logger.LogInfo("INFORMATION", "Un personnage a volé votre ressource.");
                evento_Recoleccion_Acabada(RecoleccionResultado.ROBADO, interactivo_recolectando.celda.cellId);
            }
            else
            {
                cuenta.AccountState = AccountStates.GATHERING;
                recoleccion_iniciada?.Invoke();
                await Task.Delay(tiempo_delay);
                cuenta.connexion.SendPacket("GKK" + tipo_gkk);
            }
        }

        public void evento_Recoleccion_Acabada(RecoleccionResultado resultado, short celda_id)
        {
            if (interactivo_recolectando == null || interactivo_recolectando.celda.cellId != celda_id)
                return;

            robado = false;
            interactivo_recolectando = null;
            cuenta.AccountState = AccountStates.CONNECTED_INACTIVE;
            recoleccion_acabada?.Invoke(resultado);
        }

        private void evento_Mapa_Actualizado()
        {
            pathfinder.set_Mapa(cuenta.game.map);
            interactivos_no_utilizables.Clear();
        }

        public byte get_Distancia_herramienta(int id_objeto)
        {
            switch (id_objeto)
            {
                case 8541://Caña de aprendiz de pescador
                case 6661://Caña para kuakuás
                case 596://Caña de pescar corta
                    return 2;

                case 1866://Caña de pescar estándar
                    return 3;

                case 1865://Caña cúbica
                case 1864://La aguja de tejer
                    return 4;

                case 1867://Gran caña de pescar
                case 2188://Caña para pischis
                    return 5;

                case 1863://Caña Jillo
                case 1862://El Palo de Amor
                    return 6;

                case 1868:
                    return 7;

                case 1861://La Gran Perca
                case 1860://Caña arpón
                    return 8;

                case 2366://Caña para kralamares
                    return 9;
            }

            return 1;
        }

        public void Clear()
        {
            interactivo_recolectando = null;
            interactivos_no_utilizables.Clear();
            robado = false;
        }

        #region Zona Dispose
        ~Recoleccion() => Dispose(false);
        public void Dispose() => Dispose(true);

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    pathfinder.Dispose();
                }

                interactivos_no_utilizables.Clear();
                interactivos_no_utilizables = null;
                interactivo_recolectando = null;
                pathfinder = null;
                cuenta = null;
                disposed = true;
            }
        }
        #endregion
    }
}

using Bot_Dofus_1._29._1.Otros.Game.Character;
using Bot_Dofus_1._29._1.Otros.Game.Entidades.Manejadores.Movimientos;
using Bot_Dofus_1._29._1.Otros.Mapas;
using Bot_Dofus_1._29._1.Otros.Mapas.Interactivo;
using Bot_Dofus_1._29._1.Otros.Mapas.Movimiento.Mapas;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bot_Dofus_1._29._1.Otros.Game.Entidades.Manejadores.Teleports
{
    public class Teleport : IDisposable
    {
        private Account cuenta;
        private Map mapa;
        private Pathfinder pathfinder;

        private int cell_Id;
        private string map_Id;
        private bool disposed;


        public Teleport(Account _cuenta, Movimiento movimientos, Map _mapa)
        {
            cuenta = _cuenta;
            mapa = _mapa;
            pathfinder = new Pathfinder();

            movimientos.movimiento_finalizado += get_Movimiento_Finalizado;
            mapa.mapRefreshEvent += evento_Mapa_Actualizado;
        }

        public bool get_teleport(string _map_Id)
        {
            if (cuenta.Is_Busy())
                return false;

            this.map_Id = _map_Id;

            foreach (KeyValuePair<short, ObjetoInteractivo> kvp in get_Interactivos_Utilizables())
            {
                if (get_Intentar_Mover_Interactivo(kvp))
                    return true;
            }

            cuenta.Logger.LogDanger("TELEPORTE", "Aucun zaapi trouvé");
            return false;
        }

        private Dictionary<short, ObjetoInteractivo> get_Interactivos_Utilizables()
        {
            Dictionary<short, ObjetoInteractivo> elementos_utilizables = new Dictionary<short, ObjetoInteractivo>();
            CharacterClass personaje = cuenta.game.character;



            foreach (ObjetoInteractivo interactivo in mapa.interactives.Values)
            {
                if (!interactivo.es_utilizable)
                    continue;

                List<Cell> path = pathfinder.get_Path(personaje.celda, interactivo.celda, mapa.celdas_ocupadas(), true, 1);

                if (path == null || path.Count == 0)
                    continue;

                foreach (short habilidad in interactivo.modelo.habilidades)
                {
                    if(habilidad==157)
                    {
                        elementos_utilizables.Add(interactivo.celda.cellId, interactivo);
                        this.cell_Id = interactivo.celda.cellId;
                        return elementos_utilizables;
                    }
                }
            }

            return elementos_utilizables;
        }

        private bool get_Intentar_Mover_Interactivo(KeyValuePair<short, ObjetoInteractivo> interactivo)
        {

            switch (cuenta.game.manager.movimientos.get_Mover_A_Celda(interactivo.Value.celda, mapa.celdas_ocupadas(), true, 1))
            {
                case ResultadoMovimientos.EXITO:
                case ResultadoMovimientos.SameCell:
                    get_Tentative_teleportation();
                    return true;

                default:
                    return false;
            }
        }




        private void get_Tentative_teleportation()
        {
            cuenta.connexion.SendPacket("GA500" + this.cell_Id + ";157");
        }

        public async Task evento_Teleport_Iniciada()
        {
            cuenta.connexion.SendPacket("Wu" + this.map_Id);
        }


        private void get_Movimiento_Finalizado(bool correcto)
        {

        }

        private void evento_Mapa_Actualizado()
        {
            pathfinder.set_Mapa(cuenta.game.map);
        }


        public void Clear()
        {

        }

        #region Zona Dispose
        ~Teleport() => Dispose(false);
        public void Dispose() => Dispose(true);

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    pathfinder.Dispose();
                }
                pathfinder = null;
                cuenta = null;
                disposed = true;
            }
        }
        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot_Dofus_1._29._1.Otros.Scripts.Acciones
{
    class TeleportAccion : ScriptAction
    {
        public string map_Id { get; private set; }
        public TeleportAccion(string _map_Id)
        {
            map_Id = _map_Id;
        }

        internal override async Task<ResultadosAcciones> process(Account account)
        {
            account.game.manager.teleport.get_teleport( map_Id);
            await Task.Delay(3000);
            return ResultadosAcciones.HECHO;
        }

    }
}

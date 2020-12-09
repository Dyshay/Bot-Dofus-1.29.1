using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot_Dofus_1._29._1.Otros.Scripts.Banderas
{
    class CambiarTeleport : Bandera
    {
        public string map_Id { get; private set; }

        public CambiarTeleport(string info) => map_Id = info;

    }
}

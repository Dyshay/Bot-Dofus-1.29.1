using Bot_Dofus_1._29._1.Otros.Game.Personaje.Inventario.Enums;
using System;
using System.IO;
using System.Xml.Linq;

/*
    Este archivo es parte del proyecto BotDofus_1.29.1

    BotDofus_1.29.1 Copyright (C) 2019 Alvaro Prendes — Todos los derechos reservados.
    Creado por Alvaro Prendes
    web: http://www.salesprendes.com
*/

namespace Bot_Dofus_1._29._1.Otros.Game.Personaje.Inventario
{
    public class ObjetosInventario
    {
        public uint id_inventario { get; private set; }
        public int id_modelo { get; private set; }
        public string nombre { get; private set; } = "Inconnu";
        public int cantidad { get; set; }
        public InventarioPosiciones posicion { get; set; } = InventarioPosiciones.NON_ÉQUIPÉ;
        public short pods { get; private set; }
        public short nivel { get; private set; } = 0;
        public byte tipo { get; private set; }
        public short vida_regenerada { get; }
        public TipoObjetosInventario tipo_inventario { get; private set; } = TipoObjetosInventario.DESCONOCIDO;
        private readonly XElement archivo_objeto;

        public ObjetosInventario(string paquete)
        {
            string[] separador = paquete.Split('~');

            id_inventario = Convert.ToUInt32(separador[0], 16);
            id_modelo = Convert.ToInt32(separador[1], 16);
            cantidad = Convert.ToInt32(separador[2], 16);

            if (!string.IsNullOrEmpty(separador[3]))
                posicion = (InventarioPosiciones)Convert.ToSByte(separador[3], 16);

            string[] split = separador[4].Split(',');
            foreach (string stat in split)
            {
                string[] separador_stats = stat.Split('#');
                string id = separador_stats[0];

                if (string.IsNullOrEmpty(id))
                    continue;

                int stat_id = Convert.ToInt32(id, 16);
                if (stat_id == 110)
                    vida_regenerada = Convert.ToInt16(separador_stats[1], 16);
            }

            FileInfo archivo_item = new FileInfo("items/" + id_modelo + ".xml");
            if (archivo_item.Exists)
            {
                archivo_objeto = XElement.Load(archivo_item.FullName);
                nombre = archivo_objeto.Element("NOMBRE").Value;
                pods = short.Parse(archivo_objeto.Element("PODS").Value);
                tipo = byte.Parse(archivo_objeto.Element("TIPO").Value);
                nivel = short.Parse(archivo_objeto.Element("NIVEL").Value);
                tipo_inventario = InventarioUtiles.get_Objetos_Inventario(tipo);

                archivo_objeto = null;
            }
        }

        public bool objeto_esta_equipado() => posicion > InventarioPosiciones.NON_ÉQUIPÉ;
    }
}

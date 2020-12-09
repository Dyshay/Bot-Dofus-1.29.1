using Bot_Dofus_1._29._1.Comun.Frames.Transporte;
using Bot_Dofus_1._29._1.Comun.Network;
using Bot_Dofus_1._29._1.Otros;
using Bot_Dofus_1._29._1.Otros.Enums;
using Bot_Dofus_1._29._1.Otros.Game.Entidades.Manejadores.Recolecciones;
using Bot_Dofus_1._29._1.Otros.Game.Character;
using Bot_Dofus_1._29._1.Otros.Mapas;
using Bot_Dofus_1._29._1.Otros.Mapas.Entidades;
using Bot_Dofus_1._29._1.Otros.Peleas;
using Bot_Dofus_1._29._1.Otros.Peleas.Enums;
using Bot_Dofus_1._29._1.Otros.Peleas.Peleadores;
using Bot_Dofus_1._29._1.Utilities.Config;
using Bot_Dofus_1._29._1.Utilities.Crypto;
using System.Threading.Tasks;

/*
    Este archivo es parte del proyecto BotDofus_1.29.1

    BotDofus_1.29.1 Copyright (C) 2019 Alvaro Prendes — Todos los derechos reservados.
    Creado por Alvaro Prendes
    web: http://www.salesprendes.com
*/

namespace Bot_Dofus_1._29._1.Comun.Frames.Juego
{
    internal class MapFrame : Frame
    {
        [PaqueteAtributo("GM")]
        public async Task get_Movimientos_Personajes(TcpClient cliente, string paquete)
        {
            Account cuenta = cliente.account;
            string[] separador_jugadores = paquete.Substring(3).Split('|'), informaciones;
            string _loc6, nombre_template, tipo;

            for (int i = 0; i < separador_jugadores.Length; ++i)
            {
                _loc6 = separador_jugadores[i];
                if (_loc6.Length != 0)
                {
                    informaciones = _loc6.Substring(1).Split(';');
                    if (_loc6[0].Equals('+'))
                    {
                        Cell celda = cuenta.game.map.GetCellFromId(short.Parse(informaciones[0]));
                        Fight pelea = cuenta.game.fight;
                        int id = int.Parse(informaciones[3]);
                        nombre_template = informaciones[4];
                        tipo = informaciones[5];
                        if (tipo.Contains(","))
                            tipo = tipo.Split(',')[0];

                        switch (int.Parse(tipo))
                        {
                            case -1:
                            case -2:
                                if (cuenta.AccountState == AccountStates.FIGHTING)
                                {
                                    int vida = int.Parse(informaciones[12]);
                                    byte pa = byte.Parse(informaciones[13]);
                                    byte pm = byte.Parse(informaciones[14]);
                                    byte equipo = byte.Parse(informaciones[15]);

                                    pelea.get_Agregar_Luchador(new Luchadores(id, true, vida, pa, pm, celda, vida, equipo));
                                }
                                break;

                            case -3://monstruos
                                string[] templates = nombre_template.Split(',');
                                string[] niveles = informaciones[7].Split(',');

                                Monstruos monstruo = new Monstruos(id, int.Parse(templates[0]), celda, int.Parse(niveles[0]));
                                monstruo.lider_grupo = monstruo;

                                for (int m = 1; m < templates.Length; ++m)
                                    monstruo.moobs_dentro_grupo.Add(new Monstruos(id, int.Parse(templates[m]), celda, int.Parse(niveles[m])));

                                cuenta.game.map.entities.TryAdd(id, monstruo);
                                break;

                            case -4://NPC
                                cuenta.game.map.entities.TryAdd(id, new Npcs(id, int.Parse(nombre_template), celda));
                                break;

                            case -5:
                            case -6:
                            case -8:
                            case -9:
                            case -10:
                                break;
                            default:// jugador
                                if (cuenta.AccountState != AccountStates.FIGHTING)
                                {
                                    if (cuenta.game.character.id != id)
                                        cuenta.game.map.entities.TryAdd(id, new Personajes(id, nombre_template, byte.Parse(informaciones[7].ToString()), celda));
                                    else
                                        cuenta.game.character.celda = celda;
                                }
                                else
                                {
                                    int vida = int.Parse(informaciones[14]);
                                    byte pa = byte.Parse(informaciones[15]);
                                    byte pm = byte.Parse(informaciones[16]);
                                    byte equipo = byte.Parse(informaciones[24]);

                                    pelea.get_Agregar_Luchador(new Luchadores(id, true, vida, pa, pm, celda, vida, equipo));

                                    if (cuenta.game.character.id == id && cuenta.fightExtension.configuracion.posicionamiento != PosicionamientoInicioPelea.INMOVIL)
                                    {
                                        await Task.Delay(500);

                                        /** la posicion es aleatoria pero el paquete GP siempre aparecera primero el team donde esta el pj **/
                                        short celda_posicion = pelea.get_Celda_Mas_Cercana_O_Lejana(cuenta.fightExtension.configuracion.posicionamiento == PosicionamientoInicioPelea.CERCA_DE_ENEMIGOS, pelea.celdas_preparacion);
                                        await Task.Delay(500);

                                        if (celda_posicion != celda.cellId)
                                            cuenta.connexion.SendPacket("Gp" + celda_posicion, true);
                                        else
                                        {
                                            
                                            if(cuenta.isGroupLeader ==true)
                                                await Task.Delay(3800);
                                            else
                                                await Task.Delay(1800);
                                            cuenta.connexion.SendPacket("GR1");
                                        }
                                            
                                    }
                                    else if (cuenta.game.character.id == id)
                                    {
                                         if(cuenta.isGroupLeader ==true)
                                                await Task.Delay(3800);
                                         else
                                         await Task.Delay(1800);
                                        cuenta.connexion.SendPacket("GR1");//boton listo
                                    }
                                }
                                break;
                        }
                    }
                    else if (_loc6[0].Equals('-'))
                    {
                        if (cuenta.AccountState != AccountStates.FIGHTING)
                        {
                            int id = int.Parse(_loc6.Substring(1));
                            cuenta.game.map.entities.TryRemove(id, out Entidad entidad);
                        }
                    }
                }
            }
        }

        [PaqueteAtributo("GAF")]
        public void get_Finalizar_Accion(TcpClient cliente, string paquete)
        {
            string[] id_fin_accion = paquete.Substring(3).Split('|');

            cliente.account.connexion.SendPacket("GKK" + id_fin_accion[0]);
        }

        [PaqueteAtributo("GAS")]
        public async Task get_Inicio_Accion(TcpClient cliente, string paquete) => await Task.Delay(200);

        [PaqueteAtributo("GA")]
        public async Task get_Iniciar_Accion(TcpClient cliente, string paquete)
        {
            string[] separador = paquete.Substring(2).Split(';');
            int id_accion = int.Parse(separador[1]);
            Account cuenta = cliente.account;
            CharacterClass personaje = cuenta.game.character;

            if (id_accion > 0)
            {
                int id_entidad = int.Parse(separador[2]);
                byte tipo_gkk_movimiento;
                Cell celda;
                Luchadores luchador;
                Map mapa = cuenta.game.map;
                Fight pelea = cuenta.game.fight;

                switch (id_accion)
                {
                    case 1:
                        celda = mapa.GetCellFromId(Hash.Get_Cell_From_Hash(separador[3].Substring(separador[3].Length - 2)));

                        if (!cuenta.IsFighting())
                        {
                            if (id_entidad == personaje.id && celda.cellId > 0 && personaje.celda.cellId != celda.cellId)
                            {
                                tipo_gkk_movimiento = byte.Parse(separador[0]);

                                await cuenta.game.manager.movimientos.evento_Movimiento_Finalizado(celda, tipo_gkk_movimiento, true);
                            }
                            else if (mapa.entities.TryGetValue(id_entidad, out Entidad entidad))
                            {
                                entidad.celda = celda;

                                if (GlobalConfig.show_debug_messages)
                                    cuenta.Logger.LogInfo("DEBUG", "Mouvement détecté d'une entité vers la cellule : " + celda.cellId);
                            }
                            mapa.GetEntitiesRefreshEvent();
                        }
                        else
                        {
                            luchador = pelea.get_Luchador_Por_Id(id_entidad);
                            if (luchador != null)
                            {
                                luchador.celda = celda;

                                if (luchador.id == personaje.id)
                                {
                                    tipo_gkk_movimiento = byte.Parse(separador[0]);

                                    await Task.Delay(400 + (100 * personaje.celda.GetDistanceBetweenCells(celda)));
                                    cuenta.connexion.SendPacket("GKK" + tipo_gkk_movimiento);
                                }
                            }
                        }
                        break;

                    case 4:
                        separador = separador[3].Split(',');
                        celda = mapa.GetCellFromId(short.Parse(separador[1]));

                        if (!cuenta.IsFighting() && id_entidad == personaje.id && celda.cellId > 0 && personaje.celda.cellId != celda.cellId)
                        {
                            personaje.celda = celda;
                            await Task.Delay(150);
                            cuenta.connexion.SendPacket("GKK1");
                            mapa.GetEntitiesRefreshEvent();
                            cuenta.game.manager.movimientos.movimiento_Actualizado(true);
                        }
                        break;

                    case 5:
                        if (cuenta.IsFighting())
                        {
                            separador = separador[3].Split(',');
                            luchador = pelea.get_Luchador_Por_Id(int.Parse(separador[0]));

                            if (luchador != null)
                                luchador.celda = mapa.GetCellFromId(short.Parse(separador[1]));
                        }
                        break;

                    case 102:
                        if (cuenta.IsFighting())
                        {
                            luchador = pelea.get_Luchador_Por_Id(id_entidad);
                            byte pa_utilizados = byte.Parse(separador[3].Split(',')[1].Substring(1));

                            if (luchador != null)
                                luchador.pa -= pa_utilizados;
                        }
                        break;

                    case 103:
                        if (cuenta.IsFighting())
                        {
                            int id_muerto = int.Parse(separador[3]);

                            luchador = pelea.get_Luchador_Por_Id(id_muerto);
                            if (luchador != null)
                                luchador.esta_vivo = false;
                        }
                        break;

                    case 129: //movimiento en pelea con exito
                        if (cuenta.IsFighting())
                        {
                            luchador = pelea.get_Luchador_Por_Id(id_entidad);
                            byte pm_utilizados = byte.Parse(separador[3].Split(',')[1].Substring(1));

                            if (luchador != null)
                                luchador.pm -= pm_utilizados;

                            if (luchador.id == personaje.id)
                                pelea.get_Movimiento_Exito(true);
                        }
                        break;

                    case 151://obstaculos invisibles
                        if (cuenta.IsFighting())
                        {
                            luchador = pelea.get_Luchador_Por_Id(id_entidad);

                            if (luchador != null && luchador.id == personaje.id)
                            {
                                cuenta.Logger.LogError("INFORMATION", "Il n'est pas possible d'effectuer cette action à cause d'un obstacle invisible.");
                                pelea.get_Hechizo_Lanzado(short.Parse(separador[3]), false);
                            }
                        }
                        break;

                    case 181: //efecto de invocacion (pelea)
                        celda = mapa.GetCellFromId(short.Parse(separador[3].Substring(1)));
                        short id_luchador = short.Parse(separador[6]);
                        short vida = short.Parse(separador[15]);
                        byte pa = byte.Parse(separador[16]);
                        byte pm = byte.Parse(separador[17]);
                        byte equipo = byte.Parse(separador[25]);

                        pelea.get_Agregar_Luchador(new Luchadores(id_luchador, true, vida, pa, pm, celda, vida, equipo, 10));
                        break;

                    case 302://fallo critico
                        if (cuenta.IsFighting() && id_entidad == cuenta.game.character.id)
                            pelea.get_Hechizo_Lanzado(0, false);
                        break;

                    case 300: //hechizo lanzado con exito
                        if (cuenta.IsFighting() && id_entidad == cuenta.game.character.id)
                        {
                            short celda_id_lanzado = short.Parse(separador[3].Split(',')[1]);
                            pelea.get_Hechizo_Lanzado(celda_id_lanzado, true);
                        }
                        break;

                    case 501:
                        int tiempo_recoleccion = int.Parse(separador[3].Split(',')[1]);
                        celda = mapa.GetCellFromId(short.Parse(separador[3].Split(',')[0]));
                        byte tipo_gkk_recoleccion = byte.Parse(separador[0]);

                        await cuenta.game.manager.recoleccion.evento_Recoleccion_Iniciada(id_entidad, tiempo_recoleccion, celda.cellId, tipo_gkk_recoleccion);
                        break;

                    case 900:
                        cuenta.connexion.SendPacket("GA902" + id_entidad, true);
                        cuenta.Logger.LogInfo("INFORMATION", "Le défi avec le personnage ID : " + id_entidad + " est annulée");
                        break;
                }
            }
        }

        [PaqueteAtributo("GDF")]
        public void get_Estado_Interactivo(TcpClient cliente, string paquete)
        {
            foreach (string interactivo in paquete.Substring(4).Split('|'))
            {
                string[] separador = interactivo.Split(';');
                if (separador.Length < 2)
                    return;
                Account cuenta = cliente.account;
                short celda_id = short.Parse(separador[0]);
                byte estado = byte.Parse(separador[1]);

                switch (estado)
                {
                    case 2:
                        cuenta.game.map.interactives[celda_id].es_utilizable = false;
                        break;

                    case 3:
                        if (cuenta.game.map.interactives.TryGetValue(celda_id, out var value))
                        {
                            if(value.modelo.habilidades[0]==157)
                                cuenta.game.manager.teleport.evento_Teleport_Iniciada();
                            else
                            {
                                value.es_utilizable = false;

                                if (cuenta.IsGathering())
                                    cuenta.game.manager.recoleccion.evento_Recoleccion_Acabada(RecoleccionResultado.RECOLECTADO, celda_id);
                                else
                                    cuenta.game.manager.recoleccion.evento_Recoleccion_Acabada(RecoleccionResultado.ROBADO, celda_id);
                            }
                        }

                        break;

                    case 4:// reaparece asi se fuerza el cambio de mapa 
                        cuenta.game.map.interactives[celda_id].es_utilizable = false;
                        break;
                }
            }
        }

        [PaqueteAtributo("GDM")]
        public void get_Nuevo_Mapa(TcpClient cliente, string paquete) => cliente.account.game.map.GetRefreshMap(paquete.Substring(4));

        [PaqueteAtributo("GDK")]
        public void get_Mapa_Cambiado(TcpClient cliente, string paquete) => cliente.account.game.map.GetMapRefreshEvent();

        [PaqueteAtributo("GV")]
        public void get_Reiniciar_Pantalla(TcpClient cliente, string paquete) => cliente.account.connexion.SendPacket("GC1");
    }
}

using Bot_Dofus_1._29._1.Otros.Enums;
using Bot_Dofus_1._29._1.Otros.Game.Character;
using Bot_Dofus_1._29._1.Otros.Game.Character.Inventory;
using Bot_Dofus_1._29._1.Otros.Scripts.Acciones;
using Bot_Dofus_1._29._1.Otros.Scripts.Acciones.Almacenamiento;
using Bot_Dofus_1._29._1.Otros.Scripts.Acciones.Global;
using Bot_Dofus_1._29._1.Otros.Scripts.Acciones.Npcs;
using Bot_Dofus_1._29._1.Otros.Scripts.Api;
using Bot_Dofus_1._29._1.Otros.Scripts.Banderas;
using Bot_Dofus_1._29._1.Otros.Scripts.Manejadores;
using Bot_Dofus_1._29._1.Utilities.Extensions;
using MoonSharp.Interpreter;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

/*
    Este archivo es parte del proyecto BotDofus_1.29.1

    BotDofus_1.29.1 Copyright (C) 2019 Alvaro Prendes — Todos los derechos reservados.
    Creado por Alvaro Prendes
    web: http://www.salesprendes.com
*/

namespace Bot_Dofus_1._29._1.Otros.Scripts
{
    public class ScriptManager : IDisposable
    {
        private Account account;
        private LuaScriptManager script_manager;
        public ActionsManager actions_manager { get; private set; }
        private ScriptState script_state;
        private List<Bandera> banderas;
        private int bandera_id;
        private API api;
        private bool es_dung = false;
        private bool disposed;
        private bool _activated;
        private bool _stopped;
        public bool Activated
        {
            get => _activated;
            set
            {
                _activated = value;
                if (account.hasGroup && account.isGroupLeader)
                {
                    foreach (var member in account.group.members)
                    {
                        member.script.Activated = value;
                    }
                }
            }
        }
        public bool Stopped
        {
            get => _stopped;
            set
            {
                _stopped = value;
                if (account.hasGroup && account.isGroupLeader)
                {
                    foreach (var member in account.group.members)
                    {
                        member.script.Stopped = value;
                    }
                }
            }
        }
        public bool InExecution => Activated && !Stopped;

        public event Action<string> evento_script_cargado;
        public event Action evento_script_iniciado;
        public event Action<string> evento_script_detenido;

        public ScriptManager(Account _cuenta)
        {
            account = _cuenta;
            script_manager = new LuaScriptManager();
            actions_manager = new ActionsManager(account, script_manager);
            banderas = new List<Bandera>();
            api = new API(account, actions_manager);
            

            actions_manager.evento_accion_normal += get_Accion_Finalizada;
            actions_manager.evento_accion_personalizada += get_Accion_Personalizada_Finalizada;
            account.game.fight.pelea_creada += get_Pelea_Creada;
            account.game.fight.fightFinished += get_Pelea_Acabada;
        }

        public KeyValuePair<int,int> getCaptureIdAndQuantity()
        {
            int captureQuantity = script_manager.get_Global_Or("NOMBRE_CAPTURE", DataType.Number, 0);
            int captureId = script_manager.get_Global_Or("CAPTURE_ID", DataType.Number, 0);

            KeyValuePair<int, int> retour = new KeyValuePair<int, int>(captureId, captureQuantity);
            return retour;
        }

        public int getCACId()
        {
            return  script_manager.get_Global_Or("CAC_ID", DataType.Number, 0);
        }

        public void get_Desde_Archivo(string ruta_archivo)
        {
            if (Activated)
                throw new Exception("Un script est déjà en cours d'exécution.");

            if (!File.Exists(ruta_archivo) || !ruta_archivo.EndsWith(".lua"))
                throw new Exception("Fichier non trouvé ou non valide.");

            script_manager.cargar_Desde_Archivo(ruta_archivo, funciones_Personalizadas);
            evento_script_cargado?.Invoke(Path.GetFileNameWithoutExtension(ruta_archivo));
        }

        private void funciones_Personalizadas()
        {
            script_manager.Set_Global("api", api);

            //no necesita coroutines
            script_manager.Set_Global("personnage", api.personaje);

            script_manager.Set_Global("message", new Action<string>((mensaje) => account.Logger.LogInfo("SCRIPT", mensaje)));
            script_manager.Set_Global("messageErreur", new Action<string>((mensaje) => account.Logger.LogError("SCRIPT", mensaje)));
            script_manager.Set_Global("stopScript", new Action(() => detener_Script()));
            script_manager.Set_Global("delayFFonction", new Action<int>((ms) => actions_manager.enqueue_Accion(new DelayAccion(ms), true)));

            script_manager.Set_Global("estenCollection", (Func<bool>)account.IsGathering);
            script_manager.Set_Global("estenDialogue", (Func<bool>)account.Is_In_Dialog);

            script_manager.script.DoString(Properties.Resources.api_ayuda);
        }

        public void activar_Script()
        {
            if (Activated || account.Is_Busy())
                return;

            Activated = true;
            evento_script_iniciado?.Invoke();
            script_state = ScriptState.MOUVEMENT;
            iniciar_Script();
        }

        public void detener_Script(string mensaje = "script pause")
        {
            if (!Activated)
                return;

            Activated = false;
            Stopped = false;
            banderas.Clear();
            bandera_id = 0;
            actions_manager.get_Borrar_Todo();
            evento_script_detenido?.Invoke(mensaje);
        }

        private void iniciar_Script() => Task.Run(async () =>
        {
            if (!InExecution)
                return;

            try
            {
                Table mapas_dung = script_manager.get_Global_Or<Table>("MAPS_DONJON", DataType.Table, null);

                if (mapas_dung != null)
                {
                    IEnumerable<int> test = mapas_dung.Values.Where(m => m.Type == DataType.Number).Select(n => (int)n.Number);

                    if (test.Contains(account.game.map.mapId))
                        es_dung = true;
                }

                await applyChecks();

                if (!InExecution)
                    return;

                IEnumerable<Table> entradas = script_manager.get_Entradas_Funciones(script_state.ToString().ToLower());
                if (entradas == null)
                {
                    detener_Script($"La fonction {script_state.ToString().ToLower()} n'existe pas");
                    return;
                }

                foreach (Table entrada in entradas)
                {
                    if (entrada["map"] == null)
                        continue;


                    if (!account.game.map.esta_En_Mapa(entrada["map"].ToString()))
                        continue;

                    procesar_Entradas(entrada);
                    procesar_Actual_Entrada();
                    return;
                }

                detener_Script("Aucune autre action trouvée dans le script");
            }
            catch (Exception ex)
            {
                account.Logger.LogError("SCRIPT", ex.ToString());
                detener_Script();
            }
        });

        private async Task applyChecks()
        {
            await verifyDeath();

            if (!InExecution)
                return;

            await verifyScriptRegen();

            if (!InExecution)
                return;

            await verifyRegen();

            if (!InExecution)
                return;

            await VerifyCaptureNumber();

            await verifyBags();

            if (!InExecution)
                return;

            verifyMaxPods();

            if (account.hasGroup && account.isGroupLeader)
                await verifyFollowers();
        }

        private async Task verifyFollowers()
        {
            bool followerInactiv = false;
            while (!followerInactiv)
            {
                bool inactiv = true;
                foreach (var follower in account.group.members)
                {
                    if (follower.AccountState != AccountStates.CONNECTED_INACTIVE || follower.game.map.mapId != account.game.map.mapId)
                        inactiv = false;
                }
                if (inactiv)
                    followerInactiv = true;
                await Task.Delay(200);
            }
        }

        private async Task verifyDeath()
        {
            if (account.game.character.caracteristicas.energia_actual == 0)
            {
                account.Logger.LogInfo("SCRIPT", "Le personnage est mort, passage en mode phénix.");
                script_state = ScriptState.PHENIX;
            }
            await Task.Delay(50);
        }

        private async Task VerifyCaptureNumber()
        {
            bool capture = script_manager.get_Global_Or("CAPTURE", DataType.Boolean, false);
            int captureId = script_manager.get_Global_Or("CAPTURE_ID", DataType.Number, 0);
            string persoCapture = script_manager.get_Global_Or("CAPTURE_PERSO", DataType.String, string.Empty);
            if (capture == true && script_state != ScriptState.BANQUE )
            {
                if (account.game.character.nombre.ToLower() == persoCapture.ToLower() && account.isGroupLeader == true)
                {
                    if (account.game.character.inventario.equipamiento.FirstOrDefault(o => o.id_modelo == captureId) == null)
                    {
                        account.Logger.LogInfo("SCRIPT", "Plus de capture en stock, passage en mode banque pour en recuperer");
                        script_state = ScriptState.BANQUE;
                        
                    }
                }
                else if (account.hasGroup == true && account.isGroupLeader == true )
                {
                    foreach (var membre in account.group.members)
                    {
                        if (membre.game.character.nombre.ToLower() == persoCapture.ToLower())
                        {
                            if (membre.game.character.inventario.equipamiento.FirstOrDefault(o => o.id_modelo == captureId) == null)
                            {
                                account.Logger.LogInfo("SCRIPT", "Plus de capture en stock, passage en mode banque pour en recuperer");
                                script_state = ScriptState.BANQUE;
                            }
                        }
                    }
                }
                await Task.Delay(50);
            }
        }

        private void verifyMaxPods()
        {
            if (!getMaxPods())//if you do not have the limit of unchecked pods for each map
                return;

            if (!es_dung && script_state != ScriptState.BANQUE)
            {
                if (!InExecution)
                    return;

                account.Logger.LogInfo("SCRIPT", "Inventaire complet, passage en mode banque");
                script_state = ScriptState.BANQUE;
            }
        }

        private bool getMaxPods()
        {
            int maxPods = script_manager.get_Global_Or("MAX_PODS", DataType.Number, 90);
            bool isMaxPods = account.game.character.inventario.porcentaje_pods >= maxPods;
            if (account.hasGroup && account.isGroupLeader)
            {
                foreach (var follower in account.group.members)
                    isMaxPods = isMaxPods || follower.game.character.inventario.porcentaje_pods >= maxPods;
            }

            return isMaxPods;
        }

        private void procesar_Entradas(Table valor)
        {
            banderas.Clear();
            bandera_id = 0;

            DynValue bandera = valor.Get("custom");
            if (!bandera.IsNil() && bandera.Type == DataType.Function)
                banderas.Add(new FuncionPersonalizada(bandera));

            if (script_state == ScriptState.MOUVEMENT)
            {
                bandera = valor.Get("recolte");
                if (!bandera.IsNil() && bandera.Type == DataType.Boolean && bandera.Boolean)
                    banderas.Add(new RecoleccionBandera());

                bandera = valor.Get("combat");
                if (!bandera.IsNil() && bandera.Type == DataType.Boolean && bandera.Boolean)
                    banderas.Add(new PeleaBandera());
            }

            if (script_state == ScriptState.BANQUE)
            {
                bandera = valor.Get("npc_banque");
                if (!bandera.IsNil() && bandera.Type == DataType.Boolean && bandera.Boolean)
                    banderas.Add(new NPCBancoBandera());
            }

            bandera = valor.Get("cell");
            if (!bandera.IsNil() && bandera.Type == DataType.String)
                banderas.Add(new CambiarMapa(bandera.String));

            bandera = valor.Get("zaapi");
            if (!bandera.IsNil() && bandera.Type == DataType.String)
                banderas.Add(new CambiarTeleport(bandera.String));

            bandera = valor.Get("direction");
            if (!bandera.IsNil() && bandera.Type == DataType.String)
            {
                string cellsDirection = "";
                var elementsDirection = bandera.String.Split('|');
                if (account.game.map.haveTeleport() && elementsDirection.Length > 1)
                    cellsDirection = account.game.map.TransformToCellId(elementsDirection);
                else
                    cellsDirection = account.game.map.TransformToCellId(bandera.String);

                banderas.Add(new CambiarMapa(cellsDirection));
            }

            if (banderas.Count == 0)
                detener_Script("aucune action trouvée sur cette carte");
        }

        private void procesar_Actual_Entrada(ScriptAction tiene_accion_disponible = null)
        {
            if (!InExecution)
                return;

            Bandera bandera_actual = banderas[bandera_id];

            switch (bandera_actual)
            {
                case RecoleccionBandera _:
                    manejar_Recoleccion_Bandera(tiene_accion_disponible as RecoleccionAccion);
                    break;

                case PeleaBandera _:
                    manejar_Pelea_mapa(tiene_accion_disponible as PeleasAccion);
                    break;

                case NPCBancoBandera _:
                    manejar_Npc_Banco_Bandera();
                    break;

                case FuncionPersonalizada fp:
                    actions_manager.get_Funcion_Personalizada(fp.funcion);
                    break;

                case CambiarMapa mapa:
                    manejar_Cambio_Mapa(mapa);
                    break;

                case CambiarTeleport _TeleportDetails:
                    manejar_Teleport(_TeleportDetails);
                    break;
            }
        }

        private void manejar_Recoleccion_Bandera(RecoleccionAccion accion_recoleccion)
        {
            RecoleccionAccion accion = accion_recoleccion ?? crear_Accion_Recoleccion();

            if (accion == null)
                return;

            if (account.game.manager.recoleccion.get_Puede_Recolectar(accion.elementos))
                actions_manager.enqueue_Accion(accion, true);
            else
                procesar_Actual_Bandera();
        }

        private RecoleccionAccion crear_Accion_Recoleccion()
        {
            Table elementos_para_recolectar = script_manager.get_Global_Or<Table>("ELEMENTS_RECOLTABLE", DataType.Table, null);
            List<short> recursos_id = new List<short>();

            if (elementos_para_recolectar != null)
            {
                foreach (DynValue etg in elementos_para_recolectar.Values)
                {
                    if (etg.Type != DataType.Number)
                        continue;

                    if (account.game.character.get_Tiene_Skill_Id((int)etg.Number))
                        recursos_id.Add((short)etg.Number);
                }
            }

            if (recursos_id.Count == 0)
                recursos_id.AddRange(account.game.character.get_Skills_Recoleccion_Disponibles());

            if (recursos_id.Count == 0)
            {
                account.script.detener_Script("Liste des ressources vides, ou aucune transaction disponible");
                return null;
            }

            return new RecoleccionAccion(recursos_id);
        }

        private void manejar_Npc_Banco_Bandera()
        {
            actions_manager.enqueue_Accion(new NpcBankAction(-1));
            actions_manager.enqueue_Accion(new StoreAllObjectsAction());

            //recuperar objetos almacenados
            Table recuperar_objetos = script_manager.get_Global_Or<Table>("PREND_OBJET_BANQUE", DataType.Table, null);

            if (recuperar_objetos != null)
            {
                foreach (DynValue valor in recuperar_objetos.Values)
                {
                    if (valor.Type != DataType.Table)
                        continue;

                    DynValue objeto = valor.Table.Get("objet");
                    DynValue cantidad = valor.Table.Get("quantite");

                    if (objeto.IsNil() || objeto.Type != DataType.Number || cantidad.IsNil() || cantidad.Type != DataType.Number)
                        continue;

                    //meter enqueue
                }
            }

            actions_manager.enqueue_Accion(new CerrarVentanaAccion(), true);
        }

        private void manejar_Cambio_Mapa(CambiarMapa mapa)
        {
            if (ChangeMapAction.TryParse(mapa.celda_id, out ChangeMapAction accion))
                actions_manager.enqueue_Accion(accion, true);
            else
                detener_Script("La cellule n'est pas valide pour changer de map");
        }

        private void manejar_Teleport(CambiarTeleport TeleportDetails)
        {
            actions_manager.enqueue_Accion(new TeleportAccion(TeleportDetails.map_Id), true);
        }

        private async Task verifyBags()
        {
            if (!script_manager.get_Global_Or("OUVRIR_SAC", DataType.Boolean, false))
                return;

            CharacterClass personaje = account.game.character;
            List<InventoryObject> sacos = personaje.inventario.objetos.Where(o => o.tipo == 100).ToList();

            if (sacos.Count > 0)
            {
                foreach (InventoryObject saco in sacos)
                {
                    personaje.inventario.utilizar_Objeto(saco);
                    await Task.Delay(500);
                }

                account.Logger.LogInfo("SCRIPT", $"{sacos.Count} sac(s) ouvert(s).");
            }
        }

        private void manejar_Pelea_mapa(PeleasAccion pelea_accion)
        {
            PeleasAccion accion = pelea_accion ?? get_Crear_Pelea_Accion();

            int maximas_peleas_mapa = script_manager.get_Global_Or("COMBAT_PAR_MAP", DataType.Number, -1);
            if (maximas_peleas_mapa != -1 && actions_manager.contador_peleas_mapa >= maximas_peleas_mapa)
            {
                account.Logger.LogInfo("SCRIPT", "Limite des combats atteints sur cette map");
                procesar_Actual_Bandera();
                return;
            }

            if (!es_dung && !account.game.map.get_Puede_Luchar_Contra_Grupo_Monstruos(accion.monstruos_minimos, accion.monstruos_maximos, accion.monstruo_nivel_minimo, accion.monstruo_nivel_maximo, accion.monstruos_prohibidos, accion.monstruos_obligatorios))
            {
                account.Logger.LogInfo("SCRIPT", "Aucun groupe de monstres disponible sur cette map");
                procesar_Actual_Bandera();
                return;
            }

            while (es_dung && !account.game.map.get_Puede_Luchar_Contra_Grupo_Monstruos(accion.monstruos_minimos, accion.monstruos_maximos, accion.monstruo_nivel_minimo, accion.monstruo_nivel_maximo, accion.monstruos_prohibidos, accion.monstruos_obligatorios))
                accion = get_Crear_Pelea_Accion();

            actions_manager.enqueue_Accion(accion, true);
        }

        private async Task verifyRegen()
        {
            if (account.fightExtension.configuracion.iniciar_regeneracion == 0)
                return;

            if (account.fightExtension.configuracion.detener_regeneracion <= account.fightExtension.configuracion.iniciar_regeneracion)
                return;




            if (account.hasGroup == true)
            {
                int MaxLifeToRgenerate = 0;
                int MaxTime = 0;



                /* régéneration leader */
                int vida_final_leader = account.fightExtension.configuracion.detener_regeneracion * account.game.character.caracteristicas.vitalidad_maxima / 100;
                int vida_para_regenerar_leader = vida_final_leader - account.game.character.caracteristicas.vitalidad_actual;

                if (vida_para_regenerar_leader > 0)
                {
                    if (account.Is_Busy())
                        return;
                    await Task.Delay(1150);
                    account.connexion.SendPacket("eU1", true);
                    if (MaxLifeToRgenerate < vida_para_regenerar_leader)
                    {
                        MaxTime = vida_para_regenerar_leader ;
                        MaxLifeToRgenerate = vida_para_regenerar_leader;
                    }
                }


                /* regénratino du groupe */
                foreach (var membre in account.group.members)
                {
                    int vida_final = membre.fightExtension.configuracion.detener_regeneracion * membre.game.character.caracteristicas.vitalidad_maxima / 100;
                    int vida_para_regenerar = vida_final - membre.game.character.caracteristicas.vitalidad_actual;

                    if (vida_para_regenerar > 0)
                    {
                        if (membre.Is_Busy())
                            return;
                        await Task.Delay(1150);
                        membre.connexion.SendPacket("eU1", true);
                        if (MaxLifeToRgenerate < vida_para_regenerar)
                        {
                            MaxTime = vida_para_regenerar;
                            MaxLifeToRgenerate = vida_para_regenerar;
                        }
                    }
                }

                int vida_final_leader2 = account.fightExtension.configuracion.detener_regeneracion * account.game.character.caracteristicas.vitalidad_maxima / 100;
                int vida_para_regenerar_leader2 = vida_final_leader2 - account.game.character.caracteristicas.vitalidad_actual;
                if (vida_para_regenerar_leader2 > 0)
                    account.Logger.LogInfo("SCRIPTS", $"Régénération commencée, points de vie à récupérer: {vida_para_regenerar_leader2}, temps: {MaxTime} secondes.");


                foreach (var membre in account.group.members)
                {
                    int vida_final = membre.fightExtension.configuracion.detener_regeneracion * membre.game.character.caracteristicas.vitalidad_maxima / 100;
                    int vida_para_regenerar = vida_final - membre.game.character.caracteristicas.vitalidad_actual;
                    if (vida_para_regenerar >0)
                        membre.Logger.LogInfo("SCRIPTS", $"Régénération commencée, points de vie à récupérer: {vida_para_regenerar}, temps: {MaxTime} secondes.");
                }

                for (int i = 0; i < MaxTime && InExecution; i++)
                    await Task.Delay(1000);

                if (InExecution)
                {
                    if (account.AccountState == AccountStates.REGENERATION )
                    {
                        account.connexion.SendPacket("eU1", true);
                        account.Logger.LogInfo("SCRIPTS", "Régénération terminée.");
                    }

                }
                if(MaxTime!=0)
                foreach (var membre in account.group.members )
                {
                    if (InExecution)
                    {
                        if (membre.AccountState == AccountStates.REGENERATION)
                            membre.connexion.SendPacket("eU1", true);
                        membre.Logger.LogInfo("SCRIPTS", "Régénération terminée.");
                    }
                }


            }
            else if (account.game.character.caracteristicas.porcentaje_vida <= account.fightExtension.configuracion.iniciar_regeneracion)
            {
                int vida_final = account.fightExtension.configuracion.detener_regeneracion * account.game.character.caracteristicas.vitalidad_maxima / 100;
                int vida_para_regenerar = vida_final - account.game.character.caracteristicas.vitalidad_actual;

                if (vida_para_regenerar > 0)
                {
                    int tiempo_estimado = vida_para_regenerar / 2;

                    if (account.AccountState != AccountStates.REGENERATION)
                    {
                        if (account.Is_Busy())
                            return;

                        account.connexion.SendPacket("eU1", true);
                    }

                    account.Logger.LogInfo("SCRIPTS", $"Régénération commencée, points de vie à récupérer: {vida_para_regenerar}, temps: {tiempo_estimado} secondes.");

                    for (int i = 0; i < tiempo_estimado && account.game.character.caracteristicas.porcentaje_vida <= account.fightExtension.configuracion.detener_regeneracion && InExecution; i++)
                        await Task.Delay(1000);

                    if (InExecution)
                    {
                        if (account.AccountState == AccountStates.REGENERATION)
                            account.connexion.SendPacket("eU1", true);

                        account.Logger.LogInfo("SCRIPTS", "Régénération terminée.");
                    }
                }
            }
        }

        private async Task verifyScriptRegen()
        {
            Table auto_regeneracion = script_manager.get_Global_Or<Table>("AUTO_REGENERATION", DataType.Table, null);

            if (auto_regeneracion == null)
                return;

            CharacterClass personaje = account.game.character;
            int vida_minima = auto_regeneracion.get_Or("VITA_MIN", DataType.Number, 0);
            int vida_maxima = auto_regeneracion.get_Or("VITA_MAX", DataType.Number, 100);
            var regenAll = auto_regeneracion.get_Or("REGENERATION_ALL", DataType.Boolean, false);

            int fin_vida = vida_maxima * personaje.caracteristicas.vitalidad_maxima / 100;
            int vida_para_regenerar = fin_vida - personaje.caracteristicas.vitalidad_actual;
            List<int> objetos = auto_regeneracion.Get("OBJET").ToObject<List<int>>();

            if (vida_minima > 0 && personaje.caracteristicas.porcentaje_vida < vida_minima)
            {
                foreach (int id_objeto in objetos)
                {
                    if (vida_para_regenerar < 20)
                        break;

                    InventoryObject objeto = personaje.inventario.get_Objeto_Modelo_Id(id_objeto);

                    if (objeto == null)
                        continue;

                    if (objeto.vida_regenerada <= 0)
                        continue;

                    int cantidad_necesaria = (int)Math.Floor(vida_para_regenerar / (double)objeto.vida_regenerada);
                    int cantidad_correcta = Math.Min(cantidad_necesaria, objeto.cantidad);

                    for (int j = 0; j < cantidad_correcta; j++)
                    {
                        personaje.inventario.utilizar_Objeto(objeto);
                        await Task.Delay(800);
                    }

                    vida_para_regenerar -= objeto.vida_regenerada * cantidad_correcta;
                }
            }
            if (regenAll && account.hasGroup && account.isGroupLeader)
            {
                foreach (var member in account.group.members)
                {
                    fin_vida = vida_maxima * member.game.character.caracteristicas.vitalidad_maxima / 100;
                    vida_para_regenerar = fin_vida - member.game.character.caracteristicas.vitalidad_actual;
                    if (vida_minima <= 0 || member.game.character.caracteristicas.porcentaje_vida > vida_minima)
                        continue;
                    foreach (int id_objeto in objetos)
                    {
                        if (vida_para_regenerar < 20)
                            break;
                        InventoryObject objeto = member.game.character.inventario.get_Objeto_Modelo_Id(id_objeto);

                        if (objeto == null)
                            continue;

                        if (objeto.vida_regenerada <= 0)
                            continue;

                        int cantidad_necesaria = (int)Math.Floor(vida_para_regenerar / (double)objeto.vida_regenerada);
                        int cantidad_correcta = Math.Min(cantidad_necesaria, objeto.cantidad);

                        for (int j = 0; j < cantidad_correcta; j++)
                        {
                            member.game.character.inventario.utilizar_Objeto(objeto);
                            await Task.Delay(800);
                        }

                        vida_para_regenerar -= objeto.vida_regenerada * cantidad_correcta;
                    }
                }
            }
        }

        private void procesar_Actual_Bandera()
        {
            if (!InExecution)
                return;

            if (!es_dung && getMaxPods())
            {
                iniciar_Script();
                return;
            }

            switch (banderas[bandera_id])
            {
                case RecoleccionBandera _:
                    RecoleccionAccion accion_recoleccion = crear_Accion_Recoleccion();

                    if (account.game.manager.recoleccion.get_Puede_Recolectar(accion_recoleccion.elementos))
                    {
                        procesar_Actual_Entrada(accion_recoleccion);
                        return;
                    }
                    break;

                case PeleaBandera _:
                    PeleasAccion accion_pelea = get_Crear_Pelea_Accion();

                    if (account.game.map.get_Puede_Luchar_Contra_Grupo_Monstruos(accion_pelea.monstruos_minimos, accion_pelea.monstruos_maximos, accion_pelea.monstruo_nivel_minimo, accion_pelea.monstruo_nivel_maximo, accion_pelea.monstruos_prohibidos, accion_pelea.monstruos_obligatorios))
                    {
                        procesar_Actual_Entrada(accion_pelea);
                        return;
                    }
                    break;
            }

            bandera_id++;
            if (bandera_id == banderas.Count)
                detener_Script("Aucune action trouvée sur cette map");
            else
                procesar_Actual_Entrada();
        }

        private PeleasAccion get_Crear_Pelea_Accion()
        {
            int monstruos_minimos = script_manager.get_Global_Or("MONSTERS_MIN", DataType.Number, 1);
            int monstruos_maximos = script_manager.get_Global_Or("MONSTERS_MAX", DataType.Number, 8);
            int monstruo_nivel_minimo = script_manager.get_Global_Or("MONSTERS_LVL_MIN", DataType.Number, 1);
            int monstruo_nivel_maximo = script_manager.get_Global_Or("MONSTERS_LVL_MAX", DataType.Number, 1000);
            /* gestion capture */
            bool doCapture = script_manager.get_Global_Or("CAPTURE", DataType.Boolean, false);
            string characterName = script_manager.get_Global_Or("PERSO_CAPTURE", DataType.String, string.Empty);
            int capture_id = script_manager.get_Global_Or("CAPTURE_ID", DataType.Number, 0);
            int cac_id = script_manager.get_Global_Or("CAC_ID", DataType.Number, 0);

        



            List<KeyValuePair<int, int>> monstre_capturable_nombre = new List<KeyValuePair<int, int>>();

            Table table_Monstre_Capturable = script_manager.get_Global_Or<Table>("CAPTURE_MONSTRES", DataType.Table, null);
            Table table_quantite_Monstre_Capturable = script_manager.get_Global_Or<Table>("CAPTURE_MONSTRES_QUANTITE", DataType.Table, null);

            if (table_Monstre_Capturable != null)
            {
                foreach (var item in table_Monstre_Capturable.Values)
                {
                    monstre_capturable_nombre.Add(new KeyValuePair<int, int>((int)item.Number, 1));
                }
            }
            if (table_quantite_Monstre_Capturable != null)
            {
                int index = 0;
                foreach (var item in table_quantite_Monstre_Capturable.Values)
                {
                    int key = monstre_capturable_nombre[index].Key;
                    monstre_capturable_nombre[index] = new KeyValuePair<int, int>(key, (int)item.Number);
                    index++;
                }
            }

            /* fin gestion capture */
            List<int> monstruos_prohibidos = new List<int>();
            List<int> monstruos_obligatorios = new List<int>();


            Table entrada = script_manager.get_Global_Or<Table>("MONSTRES_INTERDIT", DataType.Table, null);
            if (entrada != null)
            {
                foreach (var fm in entrada.Values)
                {
                    if (fm.Type != DataType.Number)
                        continue;

                    monstruos_prohibidos.Add((int)fm.Number);
                }
            }




            entrada = script_manager.get_Global_Or<Table>("MONSTRES_OBLIGATOIRE", DataType.Table, null);
            if (entrada != null)
            {
                foreach (DynValue mm in entrada.Values)
                {
                    if (mm.Type != DataType.Number)
                        continue;
                    monstruos_obligatorios.Add((int)mm.Number);
                }
            }
            if (doCapture == true)
                return new PeleasAccion(monstruos_minimos, monstruos_maximos, monstruo_nivel_minimo, monstruo_nivel_maximo, monstruos_prohibidos, monstruos_obligatorios, doCapture, monstre_capturable_nombre, characterName, capture_id, cac_id);
            else
                return new PeleasAccion(monstruos_minimos, monstruos_maximos, monstruo_nivel_minimo, monstruo_nivel_maximo, monstruos_prohibidos, monstruos_obligatorios);
        }

        private bool verificar_Acciones_Especiales()
        {
            if (script_state == ScriptState.BANQUE && !getMaxPods())
            {
                script_state = ScriptState.MOUVEMENT;
                iniciar_Script();
                return true;
            }
            return false;
        }

        #region Zona Eventos
        private void get_Accion_Finalizada(bool mapa_cambiado)
        {
            if (verificar_Acciones_Especiales())
                return;

            if (mapa_cambiado)
                iniciar_Script();
            else
                procesar_Actual_Bandera();
        }

        private void get_Accion_Personalizada_Finalizada(bool mapa_cambiado)
        {
            if (verificar_Acciones_Especiales())
                return;

            if (mapa_cambiado)
                iniciar_Script();
            else
                procesar_Actual_Bandera();
        }

        private void get_Pelea_Creada()
        {
            if (!Activated)
                return;

            Stopped = true;
            account.game.manager.recoleccion.get_Cancelar_Interactivo();
        }

        private void get_Pelea_Acabada()
        {
            if (!Activated)
                return;

            Stopped = false;
        }
        #endregion

        #region Zona Dispose
        public void Dispose() => Dispose(true);
        ~ScriptManager() => Dispose(false);

        public virtual void Dispose(bool disposing)
        {
            if (disposed)
            {
                if (disposing)
                {
                    script_manager.Dispose();
                    api.Dispose();
                    actions_manager.Dispose();
                }

                actions_manager = null;
                script_manager = null;
                api = null;
                Activated = false;
                Stopped = false;
                account = null;
                disposed = true;
            }
        }
        #endregion
    }
}

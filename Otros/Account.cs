using Bot_Dofus_1._29._1.Comun.Network;
using Bot_Dofus_1._29._1.Otros.Enums;
using Bot_Dofus_1._29._1.Otros.Game;
using Bot_Dofus_1._29._1.Otros.Grupos;
using Bot_Dofus_1._29._1.Otros.Peleas;
using Bot_Dofus_1._29._1.Otros.Scripts;
using Bot_Dofus_1._29._1.Utilities.Config;
using Bot_Dofus_1._29._1.Utilities.Logs;
using System;
using System.Net;

/*
    Este archivo es parte del proyecto BotDofus_1.29.1

    BotDofus_1.29.1 Copyright (C) 2019 Alvaro Prendes — Todos los derechos reservados.
	Creado por Alvaro Prendes
    web: http://www.salesprendes.com
*/

namespace Bot_Dofus_1._29._1.Otros
{
    public class Account : IDisposable
    {
        public string nickname { get; set; } = string.Empty;
        public string welcomeKey { get; set; } = string.Empty;
        public string gameTicket { get; set; } = string.Empty;
        public bool needToCapture { get; set; } = false;
        public bool capturelance { get; set; } = false;
        public Logger Logger { get; private set; }
        public TcpClient connexion { get; set; }
        public GameClass game { get; private set; }
        public ScriptManager script { get; set; }
        public FightExtensions fightExtension { get; set; }
        public AccountConfig accountConfig { get; private set; }
        private AccountStates _accountState = AccountStates.DISCONNECTED;
        public bool canUseMount = false;
        public bool isInGroupInGame { get; set; } = false;
        public bool capturefight { get; set; } = false;

        public Grupo group { get; set; }
        public bool hasGroup => group != null;
        public bool isGroupLeader => !hasGroup || group.lider == this;

        private bool _disposed;
        public event Action accountStateEvent;
        public event Action accountDisconnectEvent;

        public Account(AccountConfig prmAccountConfig)
        {
            accountConfig = prmAccountConfig;
            Logger = new Logger();
            game = new GameClass(this);
            fightExtension = new FightExtensions(this, capturefight);
            script = new ScriptManager(this);
        }

        public void Connect()
        {
            connexion = new TcpClient(this);
            connexion.ConnectToServer(Dns.GetHostAddresses("54.194.75.38"), GlobalConfig.loginPort);
        }

        public void Disconnect()
        {
            connexion?.Dispose();
            connexion = null;

            script.detener_Script();
            game.Clear();
            AccountState = AccountStates.DISCONNECTED;
            accountDisconnectEvent?.Invoke();
        }

        public void SwitchToGameServer(string ip, int port)
        {
            connexion.DisconnectSocket();
            connexion.ConnectToServer(Dns.GetHostAddresses(ip), port);
        }

        public AccountStates AccountState
        {
            get => _accountState;
            set
            {
                _accountState = value;
                accountStateEvent?.Invoke();
            }
        }

        public bool Is_Busy() => AccountState != AccountStates.CONNECTED_INACTIVE && AccountState != AccountStates.REGENERATION;
        public bool Is_In_Dialog() => AccountState == AccountStates.STORAGE || AccountState == AccountStates.DIALOG || AccountState == AccountStates.EXCHANGE || AccountState == AccountStates.BUYING || AccountState == AccountStates.SELLING;
        public bool IsFighting() => AccountState == AccountStates.FIGHTING;
        public bool IsGathering() => AccountState == AccountStates.GATHERING;
        public bool IsMoving() => AccountState == AccountStates.MOVING;

        #region Zona Dispose
        public void Dispose() => Dispose(true);
        ~Account() => Dispose(false);

        public virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    script.Dispose();
                    connexion?.Dispose();
                    game.Dispose();
                }
                AccountState = AccountStates.DISCONNECTED;
                script = null;
                welcomeKey = null;
                connexion = null;
                Logger = null;
                game = null;
                nickname = null;
                accountConfig = null;
                _disposed = true;
            }
        }
        #endregion
    }
}

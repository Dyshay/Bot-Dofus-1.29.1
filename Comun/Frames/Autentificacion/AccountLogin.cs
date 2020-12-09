using Bot_Dofus_1._29._1.Comun.Frames.Transporte;
using Bot_Dofus_1._29._1.Comun.Network;
using Bot_Dofus_1._29._1.Otros;
using Bot_Dofus_1._29._1.Otros.Enums;
using Bot_Dofus_1._29._1.Otros.Game.Server;
using Bot_Dofus_1._29._1.Utilities.Crypto;
using System.Threading.Tasks;

/*
    Este archivo es parte del proyecto BotDofus_1.29.1

    BotDofus_1.29.1 Copyright (C) 2019 Alvaro Prendes — Todos los derechos reservados.
    Creado por Alvaro Prendes
    web: http://www.salesprendes.com
*/

namespace Bot_Dofus_1._29._1.Comun.Frames.LoginCuenta
{
    public class AccountLogin : Frame
    {
        [PaqueteAtributo("HC")]
        public void GetWelcomeKeyAsync(TcpClient prmClient, string prmPacket)
        {
            Account account = prmClient.account;

            account.AccountState = AccountStates.CONNECTED;
            account.welcomeKey = prmPacket.Substring(2);

            prmClient.SendPacket("1.34");
            prmClient.SendPacket(prmClient.account.accountConfig.accountUsername + "\n" + Hash.Crypt_Password(prmClient.account.accountConfig.accountPassword, prmClient.account.welcomeKey));
            prmClient.SendPacket("Af");
        }

        [PaqueteAtributo("Ad")]
        public void GetNickname(TcpClient prmClient, string prmPacket) => prmClient.account.nickname = prmPacket.Substring(2);

        [PaqueteAtributo("Af")]
        public void GetLoginQueue(TcpClient prmClient, string prmPacket) => prmClient.account.Logger.LogInfo("File d'attente", "Position " + prmPacket[2] + "/" + prmPacket[4]);

        [PaqueteAtributo("AH")]
        public void GetServerState(TcpClient prmClient, string prmPacket)
        {
            Account account = prmClient.account;
            string[] serverList = prmPacket.Substring(2).Split('|');
            GameServer server = account.game.server;
            bool firstTime = true;

            foreach (string sv in serverList)
            {
                string[] separator = sv.Split(';');

                int id = int.Parse(separator[0]);
                ServerStates serverState = (ServerStates)byte.Parse(separator[1]);
                string serverName = account.accountConfig.server;

                // Add Method to take name with Id

                if (id == account.accountConfig.Get_Server_ID())
                {
                    server.RefreshData(id, serverName, serverState);
                    account.Logger.LogInfo("LOGIN", $"Le serveur {serverName} est {account.game.server.GetState(serverState)}");

                    if (serverState != ServerStates.ONLINE)
                        firstTime = false;
                }
            }

            if (!firstTime && server.serverState == ServerStates.ONLINE)
                prmClient.SendPacket("Ax");
        }

        [PaqueteAtributo("AQ")]
        public void GetSecretQuestion(TcpClient prmClient, string prmPacket)
        {
            if (prmClient.account.game.server.serverState == ServerStates.ONLINE)
                prmClient.SendPacket("Ax", true);
        }

        [PaqueteAtributo("AxK")]
        public void GetServerList(TcpClient prmClient, string prmPacket)
        {
            Account account = prmClient.account;
            string[] loc5 = prmPacket.Substring(3).Split('|');
            int counter = 1;
            bool picked = false;

            while (counter < loc5.Length && !picked)
            {
                string[] _loc10_ = loc5[counter].Split(',');
                int serverId = int.Parse(_loc10_[0]);

                if (serverId == account.game.server.serverId)
                {
                    if (account.game.server.serverState == ServerStates.ONLINE)
                    {
                        picked = true;
                        account.game.character.evento_Servidor_Seleccionado();
                    }
                    else
                        account.Logger.LogError("LOGIN", "Serveur non accessible lorsque celui-ci se reconnectera");
                }
                counter++;
            }

            if (picked)
                prmClient.SendPacket($"AX{account.game.server.serverId}", true);
        }

        [PaqueteAtributo("AXK")]
        public void GetServerSelection(TcpClient prmClient, string prmPacket)
        {
            prmClient.account.gameTicket = prmPacket.Substring(14);
            prmClient.account.SwitchToGameServer(Hash.Decrypt_IP(prmPacket.Substring(3, 8)), Hash.Decrypt_Port(prmPacket.Substring(11, 3).ToCharArray()));
        }

        [PaqueteAtributo("AYK")]
        public void GetServerSelectionRemastered(TcpClient prmClient, string prmPacket)
        {
            string[] DataPackage = prmPacket.Substring(3).Split(';');

            if (DataPackage.Length != 0)
            {
                prmClient.account.gameTicket = DataPackage[1];
                prmClient.account.SwitchToGameServer(DataPackage[0], 443);
                prmClient.account.Logger.LogInfo("[BOT]", "Connexion au world server");
            }
            else
            {
                prmClient.account.Logger.LogError("[BOT]", "Redirection world impossible");
                prmClient.account.Disconnect();
                prmClient.account.Logger.LogError("[BOT]", "Déconnexion effectuée");
            }
        }
    }
}

using Bot_Dofus_1._29._1.Controles.ControlMapa;
using System.IO;

/*
    Este archivo es parte del proyecto BotDofus_1.29.1

    BotDofus_1.29.1 Copyright (C) 2019 Alvaro Prendes — Todos los derechos reservados.
	Creado por Alvaro Prendes
    web: http://www.salesprendes.com
*/

namespace Bot_Dofus_1._29._1.Utilities.Config
{
    public class AccountConfig
    {
        public string accountUsername { get; set; } = string.Empty;
        public string accountPassword { get; set; } = string.Empty;
        public string server { get; set; } = string.Empty;
        public string characterName { get; set; } = string.Empty;

        public AccountConfig(string _accountUsername, string _accountPassword, string _server, string _characterName)
        {
            accountUsername = _accountUsername;
            accountPassword = _accountPassword;
            server = _server;
            characterName = _characterName;
        }

        public void SaveAccount(BinaryWriter bw)
        {
            bw.Write(accountUsername);
            bw.Write(accountPassword);
            bw.Write(server);
            bw.Write(characterName);
        }

        public static AccountConfig LoadAcccount(BinaryReader br)
        {
            try
            {
                return new AccountConfig(br.ReadString(), br.ReadString(), br.ReadString(), br.ReadString());
            }
            catch
            {
                return null;
            }
        }

        public int Get_Server_ID()
        {
            switch (server)
            {
                case "Eratz":
                    return 601;
                case "Henual":
                    return 602;
                case "Crail":
                    return 613;
                case "Galgarion":
                    return 614;
                default:
                    return 601;
            }
        }
    }
}

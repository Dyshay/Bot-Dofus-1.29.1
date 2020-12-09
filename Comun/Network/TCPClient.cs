using Bot_Dofus_1._29._1.Comun.Frames.Transporte;
using Bot_Dofus_1._29._1.Otros;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

/*
    Este archivo es parte del proyecto BotDofus_1.29.1

    BotDofus_1.29.1 Copyright (C) 2019 Alvaro Prendes — Todos los derechos reservados.
    Creado por Alvaro Prendes
    web: http://www.salesprendes.com
*/

namespace Bot_Dofus_1._29._1.Comun.Network
{
    public class TcpClient : IDisposable
    {
        private Socket socket { get; set; }
        private byte[] buffer { get; set; }
        public Account account;
        private SemaphoreSlim _semaphore;
        private bool _disposed;

        public event Action<string> packetReceivedEvent;
        public event Action<string> packetSendEvent;
        public event Action<string> socketInformationEvent;

        /** ping **/
        private bool _isWaitingPacket = false;
        private int _ticks;
        private List<int> _pings;

        public TcpClient(Account prmAccount) => account = prmAccount;

        public void ConnectToServer(IPAddress ip, int port)
        {
            try
            {
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                buffer = new byte[socket.ReceiveBufferSize];
                _semaphore = new SemaphoreSlim(1);
                _pings = new List<int>(50);
                socket.BeginConnect(ip, port, new AsyncCallback(ConnectCallBack), socket);
            }
            catch (Exception ex)
            {
                socketInformationEvent?.Invoke(ex.ToString());
                DisconnectSocket();
            }
        }

        private void ConnectCallBack(IAsyncResult ar)
        {
            try
            {
                if (IsConnected())
                {
                    socket = ar.AsyncState as Socket;
                    socket.EndConnect(ar);

                    socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(ReceptionCallBack), socket);
                    socketInformationEvent?.Invoke("Socket connectée correctement");
                }
                else
                {
                    DisconnectSocket();
                    socketInformationEvent?.Invoke("Impossible de joindre le serveur hôte");
                }
            }
            catch (Exception ex)
            {
                socketInformationEvent?.Invoke(ex.ToString());
                DisconnectSocket();
            }
        }

        public void ReceptionCallBack(IAsyncResult ar)
        {
            if (!IsConnected() || _disposed)
            {
                DisconnectSocket();
                return;
            }

            int bytes_leidos = socket.EndReceive(ar, out SocketError reply);

            if (bytes_leidos > 0 && reply == SocketError.Success)
            {
                string datos = Encoding.UTF8.GetString(buffer, 0, bytes_leidos);

                foreach (string packet in datos.Replace("\x0a", string.Empty).Split('\0').Where(x => x != string.Empty))
                {
                    packetReceivedEvent?.Invoke(packet);

                    if (_isWaitingPacket)
                    {
                        _pings.Add(Environment.TickCount - _ticks);

                        if (_pings.Count > 48)
                            _pings.RemoveAt(1);

                        _isWaitingPacket = false;
                    }

                    PaqueteRecibido.Recibir(this, packet);
                }

                if (IsConnected())
                    socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(ReceptionCallBack), socket);
            }
            else
                account.Disconnect();
        }

        public async Task SendPacketAsync(string packet, bool necesita_respuesta)
        {
            try
            {
                if (!IsConnected())
                    return;

                packet += "\n\x00";
                byte[] byte_packet = Encoding.UTF8.GetBytes(packet);

                await _semaphore.WaitAsync().ConfigureAwait(false);

                if (necesita_respuesta)
                    _isWaitingPacket = true;

                socket.Send(byte_packet);

                if (necesita_respuesta)
                    _ticks = Environment.TickCount;

                packetSendEvent?.Invoke(packet);
                _semaphore.Release();
            }
            catch (Exception ex)
            {
                socketInformationEvent?.Invoke(ex.ToString());
                DisconnectSocket();
            };
        }

        public void SendPacket(string packet, bool necesita_respuesta = false) => SendPacketAsync(packet, necesita_respuesta).Wait();

        public void DisconnectSocket()
        {
            if (IsConnected())
            {
                if (socket != null && socket.Connected)
                {
                    socket.Shutdown(SocketShutdown.Both);
                    socket.Disconnect(false);
                    socket.Close();
                }

                socketInformationEvent?.Invoke("Socket deconnecté de l'hôte");
            }
        }

        public bool IsConnected()
        {
            try
            {
                return !(_disposed || socket == null || !socket.Connected && socket.Available == 0);
            }
            catch (SocketException)
            {
                return false;
            }
            catch (ObjectDisposedException)
            {
                return false;
            }
        }

        public int GetTotalPings() => _pings.Count();
        public int GetPingAverage() => (int)_pings.Average();
        public int GetPing() => Environment.TickCount - _ticks;

        #region Zona Dispose
        ~TcpClient() => Dispose(false);
        public void Dispose() => Dispose(true);

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (socket != null && socket.Connected)
                {
                    socket.Shutdown(SocketShutdown.Both);
                    socket.Disconnect(false);
                    socket.Close();
                }

                if (disposing)
                {
                    socket.Dispose();
                    _semaphore.Dispose();
                }

                _semaphore = null;
                account = null;
                socket = null;
                buffer = null;
                packetReceivedEvent = null;
                packetSendEvent = null;
                _disposed = true;
            }
        }
        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MSCLoader;
using System.Net.Sockets;
using UnityEngine;

namespace Multiplayer
{
    class Handler
    {
        public Socket socket;
        public Player[] players;
        public GameObject playerPref;
        public void Reading()
        {
            while (true)
            {
                byte[] buffer = new byte[0];
                ModConsole.Log("Pre");

                socket.Receive(buffer);
                ModConsole.Log("pass");
                Queue<byte> buf = new Queue<byte>();
                foreach (byte b in buffer)
                {
                    buf.Enqueue(b);
                }
                switch (buf.Dequeue())
                {
                    case 0:
                        byte[] buff = new byte[0];
                        socket.Receive(buff, 21, 0);
                        players[buff[0]] = new Player();
                        players[buff[0]].name = BitConverter.ToString(buff, 1);
                        players[buff[0]].player = GameObject.Instantiate(playerPref);
                        break;
                    case 3:
                        byte tempid = buf.Dequeue();
                        byte[] flt = new byte[4];
                        for (int i = 0; i < 4; i++)
                        {
                            flt[i] = buf.Dequeue();
                        }
                        float x = BitConverter.ToSingle(flt, 0);
                        for (int i = 0; i < 4; i++)
                        {
                            flt[i] = buf.Dequeue();
                        }
                        float y = BitConverter.ToSingle(flt, 0);
                        for (int i = 0; i < 4; i++)
                        {
                            flt[i] = buf.Dequeue();
                        }
                        float z = BitConverter.ToSingle(flt, 0);

                        players[tempid].player.transform.position = new Vector3(x, y, z);
                        for (int i = 0; i < 4; i++)
                        {
                            flt[i] = buf.Dequeue();
                        }
                        float rot = BitConverter.ToSingle(flt, 0);
                        players[tempid].player.transform.rotation = new Quaternion(0, rot, 0, 0);
                        break;
                }
            }
        }
    }
}

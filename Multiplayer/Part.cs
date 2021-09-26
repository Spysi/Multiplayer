using UnityEngine;
using HutongGames.PlayMaker;
using System.Net.Sockets;
using System;

namespace Multiplayer
{
    class Part
    {
        public string assembleEvent;
        public string removeEvent;
        //Названия событий: REMOVE; ASSEMBLE
        public delegate void Assembly(string eventName);
        public Assembly remove, assemble;
        public GameObject gameObj;
        public void Assemble()
        {
            assemble(assembleEvent);
        }
        public void Remove()
        {
            remove(removeEvent);
        }
    }

    class Assembly : FsmStateAction
    {
        //public delegate void Send(int id, int doi);
        Socket send;
        public Assembly(Socket socket, int id1) { send = socket; id = id1; }

        public int id;

        public override void OnEnter()
        {

            send.Send(BitConverter.GetBytes(id));
            Finish();
        }
    }

    class RemovePart : FsmStateAction
    {
        //public delegate void Send(int id, int doi);
        Socket send;
        public RemovePart(Socket socket, int id1) { send = socket; id = id1; }

        public int id;

        public override void OnEnter()
        {
            send.Send(BitConverter.GetBytes(id));
            Finish();
        }
    }
}

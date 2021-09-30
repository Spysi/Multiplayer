using HutongGames.PlayMaker;
using System;
using System.Net.Sockets;
using UnityEngine;

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

    class PickUp : FsmStateAction
    {
        //public delegate void Send(int id, int doi);
        Socket send;
        
        //public ref bool isPicked;
        public PickUp(Socket socket) { send = socket;}

        public override void OnEnter()
        {
            send.Send(ByteConvertor.PickUp(Fsm.Variables.GetFsmGameObject("RaycastHitObject").Value.GetInstanceID()));
            Finish();
        }
    }

    class Drop : FsmStateAction
    {
        //public delegate void Send(int id, int doi);
        Socket send;

        //public ref bool isPicked;
        public Drop(Socket socket) { send = socket; }

        public override void OnEnter()
        {
            send.Send(ByteConvertor.Drop(Fsm.Variables.GetFsmGameObject("RaycastHitObject").Value.GetInstanceID()));
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

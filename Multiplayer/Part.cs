using HutongGames.PlayMaker;
using MSCLoader;
using MSCLoader.Helper;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using System.Linq;
namespace Multiplayer
{
    class Part
    {
        public string assembleEvent;
        public string removeEvent;
        //Названия событий: REMOVE; ASSEMBLE
        public delegate void Assembly(string eventName);
        public Assembly remove, assemble;
        public FsmBool boo;
        public GameObject gameObjDB;
        public FsmGameObject part;
        public void Assemble()
        {
            part.Value.layer = 19;
            assemble("FINISHED");
            assemble(assembleEvent);
            if (boo != null)
            {
                boo.Value = true;
            }            
        }
        public void Remove()
        {
            if (boo != null)
            {
                boo.Value = false;
            }
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
            send.Send(ByteConvertor.Assembly(id, Fsm.Variables.GetFsmGameObject("Part").Value.GetInstanceID()));
            //gamobj.Enqueue(Fsm.Variables.GetFsmGameObject("Part").Value);
            Finish();
        }
    }

    class RemovePart : FsmStateAction
    {
        //public delegate void Send(int id, int doi);
        Socket send;
        public RemovePart(Socket socket, int id1, FsmBool boo) { send = socket; id = id1; this.boo = boo; }

        public int id;
        public FsmBool boo;

        public override void OnEnter()
        {
            if (boo != null) boo.Value = false;
            send.Send(ByteConvertor.RemovePart(id));
            Finish();
        }
    }

    class FixPos : FsmStateAction
    {
        public Queue<GameObject> temp = new Queue<GameObject>();
        public override void OnEnter()
        {
            Thread t = new Thread(new ThreadStart(Fix));
            t.Start();
            Finish();
        }

        void Fix()
        {
            Thread.Sleep(200);
            temp.Enqueue(Fsm.Variables.GetFsmGameObject("Part").Value);
        }
    }

    class PickUp : FsmStateAction
    {
        //public delegate void Send(int id, int doi);
        Socket send;
        public List<GameObject> items;
        //public ref bool isPicked;
        public PickUp(Socket socket) { send = socket; }

        public override void OnEnter()
        {
            int id = items.IndexOf(Fsm.Variables.GetFsmGameObject("PickedObject").Value);
            if (id == -1) {
                items.Clear();
                items.AddRange(Resources.FindObjectsOfTypeAll<GameObject>().Where(obj => obj.tag == "PART"));
                id = items.IndexOf(Fsm.Variables.GetFsmGameObject("PickedObject").Value);
                send.Send(ByteConvertor.RecalculateID());
            }
            send.Send(ByteConvertor.PickUp(id, true));
            Finish();
        }
    }

    class Drop : FsmStateAction
    {
        //public delegate void Send(int id, int doi);
        Socket send;
        public List<GameObject> items;
        //public ref bool isPicked;
        public Drop(Socket socket) { send = socket; }

        public override void OnEnter()
        {
            int id = items.IndexOf(Fsm.Variables.GetFsmGameObject("PickedObject").Value);
            if (id == -1)
            {
                items.Clear();
                items.AddRange(Resources.FindObjectsOfTypeAll<GameObject>().Where(obj => obj.tag == "PART"));
                id = items.IndexOf(Fsm.Variables.GetFsmGameObject("PickedObject").Value);
                send.Send(ByteConvertor.RecalculateID());
            }
            send.Send(ByteConvertor.PickUp(id, false));
            Finish();
        }
    }


}

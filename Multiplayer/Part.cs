using HutongGames.PlayMaker;
using MSCLoader;
using System.Threading;
using UnityEngine;
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
        public Assembly(int id) {this.id = id; }

        public int id;
        public override void OnEnter()
        {
            Multiplayer.socket.Send(ByteConvertor.Assembly(id, Multiplayer.items.IndexOf(Fsm.Variables.GetFsmGameObject("Part").Value)));
            Finish();
        }
    }

    class RemovePart : FsmStateAction
    {
        public RemovePart(int id, FsmBool boo) { this.id = id; this.boo = boo; }

        public int id;
        public FsmBool boo;

        public override void OnEnter()
        {
            if (boo != null) boo.Value = false;
            Multiplayer.socket.Send(ByteConvertor.RemovePart(id));
            Finish();
        }
    }

    class FixPos : FsmStateAction
    {
        public override void OnEnter()
        {
            Thread t = new Thread(new ThreadStart(Fix));
            t.Start();
            Finish();
        }

        void Fix()
        {
            Thread.Sleep(200);
           Multiplayer.setZeroTransform.Enqueue(Fsm.Variables.GetFsmGameObject("Part").Value);
        }
    }

    class PickUp : FsmStateAction
    {
        public override void OnEnter()
        {
            int id = Multiplayer.items.IndexOf(Fsm.Variables.GetFsmGameObject("PickedObject").Value);
            if (id == -1) {
                ModConsole.Log("memem2");
                Multiplayer.items.Clear();
                Multiplayer.items.AddRange(Resources.FindObjectsOfTypeAll<GameObject>().Where(obj => obj.tag == "PART"));
                id = Multiplayer.items.IndexOf(Fsm.Variables.GetFsmGameObject("PickedObject").Value);
                Multiplayer.socket.Send(ByteConvertor.RecalculateID());
            }
            Multiplayer.socket.Send(ByteConvertor.PickUp(id, true));
            Finish();
        }
    }

    class Drop : FsmStateAction
    {
        public override void OnEnter()
        {
            int id = Multiplayer.items.IndexOf(Fsm.Variables.GetFsmGameObject("PickedObject").Value);
            if (id == -1)
            {
                ModConsole.Log("memem3");
                Multiplayer.items.Clear();
                Multiplayer.items.AddRange(Resources.FindObjectsOfTypeAll<GameObject>().Where(obj => obj.tag == "PART"));
                id = Multiplayer.items.IndexOf(Fsm.Variables.GetFsmGameObject("PickedObject").Value);
                Multiplayer.socket.Send(ByteConvertor.RecalculateID());
            }
            Multiplayer.socket.Send(ByteConvertor.PickUp(id, false));
            Finish();
        }
    }


}

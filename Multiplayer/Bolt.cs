using HutongGames.PlayMaker;
using System.Net.Sockets;

namespace Multiplayer
{
    class Bolt
    {
        public delegate void Screwing(string eventName);
        public Screwing screw;
        public PlayMakerFSM bolt;
        public void Screw()
        {
            screw("TIGHTEN");
        }
        public void UnScrew()
        {
            screw("UNTIGHTEN");
        }
    }

    class Screw : FsmStateAction
    {
        public delegate void Send(int id, int doi);
        Socket send;
        public Screw(Socket send, int id) { this.send = send; this.id = id; }
        int id;
        public override void OnEnter()
        {
            if (!Fsm.Variables.BoolVariables[0].Value) send.Send(ByteConvertor.Tingen(id, true));
            else Fsm.Variables.BoolVariables[0].Value = false;
            Finish();
        }
    }

    class UnScrew : FsmStateAction
    {
        public delegate void Send(int id, int doi);
        Socket send;
        public UnScrew(Socket send, int id) { this.send = send; this.id = id;}
        int id;
        public override void OnEnter()
        {
            if (!Fsm.Variables.BoolVariables[0].Value) send.Send(ByteConvertor.Tingen(id, false));
            else Fsm.Variables.BoolVariables[0].Value = false;
            Finish();
        }
    }
}

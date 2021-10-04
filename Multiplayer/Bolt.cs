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
        public Screw(Socket temp, int id1) { send = temp; id = id1; }

        public int id;

        public override void OnEnter()
        {
            send.Send(ByteConvertor.Tingen(id));
            Finish();
        }
    }

    class UnScrew : FsmStateAction
    {
        public delegate void Send(int id, int doi);
        Socket send;
        public UnScrew(Socket temp, int id1) { send = temp; id = id1; }

        public int id;

        public override void OnEnter()
        {
            send.Send(ByteConvertor.UnTingen(id));
            Finish();
        }
    }
}

using HutongGames.PlayMaker;

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
        Send send;
        public Screw(Send temp, int id1) { send = temp; id = id1; }

        public int id;

        public override void OnEnter()
        {
            send(id, 3);
            Finish();
        }
    }

    class UnScrew : FsmStateAction
    {
        public delegate void Send(int id, int doi);
        Send send;
        public UnScrew(Send temp, int id1) { send = temp; id = id1; }

        public int id;

        public override void OnEnter()
        {
            send(id, 4);
            Finish();
        }
    }
}

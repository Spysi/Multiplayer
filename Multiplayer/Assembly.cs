using UnityEngine;
using MSCLoader;

namespace HutongGames.PlayMaker.Actions
{
    class Assembly : FsmStateAction
    {
        public delegate void Send(int id, int doi);
        Send send;
        public Assembly(Send temp, int id1) { send = temp; id = id1; }

        public int id;



        public override void OnEnter()
        {
            send(id, 2);
            Finish();
        }
    }
}

using UnityEngine;
using MSCLoader;

namespace HutongGames.PlayMaker.Actions
{
    class RemovePart : FsmStateAction
    {
        public delegate void Send(int id,int doi);
        Send send;
        public RemovePart(Send temp, int id1) { send = temp; id = id1;}

        public int id;



        public override void OnEnter()
        {
            send(id,1);
            Finish();
        }
    }
}

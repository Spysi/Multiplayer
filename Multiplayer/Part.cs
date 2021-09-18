using UnityEngine;
using HutongGames.PlayMaker;
using System;
using System.Collections.Generic;

namespace Multiplayer
{
    class Part
    {
        //public string preassembleEvent;
        public string assembleEvent;
        public string removeEvent;
        //Названия событий: REMOVE; ASSEMBLE
        public delegate void Assembly(string eventName);
        public Assembly remove, assemble;
        public GameObject gameObj;
        public List<PlayMakerFSM> bolts = new List<PlayMakerFSM>();
        public void Assemble()
        {
            //preassemble(preassembleEvent);
            assemble(assembleEvent);
        }
        public void Remove()
        {
            remove(removeEvent);
        }
    }
}

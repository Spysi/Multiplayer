﻿using UnityEngine;
using HutongGames.PlayMaker;
using System;
using System.Collections.Generic;

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

    class RemovePart : FsmStateAction
    {
        public delegate void Send(int id, int doi);
        Send send;
        public RemovePart(Send temp, int id1) { send = temp; id = id1; }

        public int id;

        public override void OnEnter()
        {
            send(id, 1);
            Finish();
        }
    }
}

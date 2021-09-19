﻿using MSCLoader.Helper;
using UnityEngine;
using HutongGames.PlayMaker;
using System;
using System.Text;
using System.Collections.Generic;
using HutongGames.PlayMaker.Actions;
using MSCLoader;
using System.Net.Sockets;
using UnityEngine.UI;

namespace Multiplayer
{
    public class Multiplayer : Mod
    {
        public override string ID => "Multiplayer"; //Your mod ID (unique)
        public override string Name => "Multiplayer"; //You mod name
        public override string Author => "Spysi, Kiri111enz"; //Your Username
        public override string Version => "A0.1"; //Version

        public override string Description => "Multiplayer mod";

        public static void SendMsg(int id,int doi)
        {
            ModConsole.Log(id);
            ModConsole.Log(doi);
        }
        private List<Part> parts;
        private List<Bolt> bolts;
        private List<GameObject> tempParts;
        static RemovePart.Send send = SendMsg;
        static Assembly.Send send2 = SendMsg;
        static Screw.Send send3 = SendMsg;
        static UnScrew.Send send4 = SendMsg;
        GameObject databaseBody, databaseMechanics, databaseMotor, databaseOrders, databaseWiring, playerDatabase, player, ui;
        public string serverIP = "Server IP ";
        Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        public override void MenuOnLoad()
        {
            AssetBundle ab = ModAssets.LoadBundle(this, "multiplayer");
             ;

            ui = GameObject.Instantiate(ab.LoadAsset("UIPrefab.prefab") as GameObject);
            ab.Unload(false);
            ui.transform.position = new Vector3(Screen.width, Screen.height, 0);
            ui.transform.SetParent(ModLoader.UICanvas.transform);
            ui.transform.localScale = new Vector3(1, 1, 1);
            ui.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(Connect);
            
            socket.SendTimeout = 3000;
            socket.ReceiveTimeout = 3000;
            //ModLoader.GetModAssetsFolder(new Multiplayer(),true);
            //style.fontSize = 0;
            //style.fontStyle = GameObject.Find("Buttons").transform.GetChild(0).GetChild(0).GetComponent<TextMesh>().fontStyle;

        }
        public override void MenuUpdate()
        {
            //if (socket.Poll(-1, SelectMode.SelectError)) Console.WriteLine("This Socket has an error.");
        }


        private void Connect()
        {
            //socket.Disconnect();
            try
            {
                socket.Connect(System.Net.IPAddress.Parse(ui.transform.GetChild(1).GetComponent<InputField>().text), 25565);
            }
            catch (SocketException) { ui.transform.GetChild(2).GetComponent<Text>().text = "Connection failed!"; }
            if (!socket.Connected) ui.transform.GetChild(2).GetComponent<Text>().text = "Connection failed!";
            else ui.transform.GetChild(2).GetComponent<Text>().text = "Сonnection successful!";
            short i = 0;

            socket.Send(BitConverter.GetBytes(i));
            socket.Send(Encoding.UTF8.GetBytes("aboba"));


        }
        public override void PreLoad()
        {
            ui.SetActive(false);
            UnityEngine.Object.Destroy(ui);
            GameObject carParts = GameObject.Find("CARPARTS");
            PlayMakerFSM[] objs = carParts.transform.GetComponentsInChildren<PlayMakerFSM>(true);
            bolts = new List<Bolt>();
            foreach(PlayMakerFSM obj in objs)
            {
                if (obj.FsmName != "Screw") continue;
                obj.Initialize();
                Bolt bolt = new Bolt();
                bolt.screw = obj.SendEvent;
                bolt.bolt = obj;
                foreach(FsmState state in obj.FsmStates)
                {
                    switch (state.Name)
                    {
                        case "Screw":
                            obj.AddAction("Screw", new Screw(send3, bolts.Count));
                            break;
                        case "Screw 2":
                            obj.AddAction("Screw 2", new Screw(send3, bolts.Count));
                            break;
                        case "Wait 3":
                            obj.AddAction("Wait 3", new Screw(send3, bolts.Count));
                            break;
                        case "Unscrew":
                            obj.AddAction("Unscrew", new UnScrew(send4, bolts.Count));
                            break;
                        case "Unscrew 2":
                            obj.AddAction("Unscrew 2", new UnScrew(send4, bolts.Count));
                            break;
                        case "Wait 4":
                            obj.AddAction("Wait 4", new UnScrew(send4, bolts.Count));
                            break;
                    }
                }
                bolts.Add(bolt);

            }
            carParts = GameObject.Find("SATSUMA(557kg, 248)");
            objs = carParts.transform.GetComponentsInChildren<PlayMakerFSM>(true);
            foreach (PlayMakerFSM obj in objs)
            {
                if (obj.FsmName != "Screw") continue;
                obj.Initialize();
                Bolt bolt = new Bolt();
                bolt.screw = obj.SendEvent;
                bolt.bolt = obj;
                foreach (FsmState state in obj.FsmStates)
                {
                    switch (state.Name)
                    {
                        case "Screw":
                            obj.AddAction("Screw", new Screw(send3, bolts.Count));
                            break;
                        case "Screw 2":
                            obj.AddAction("Screw 2", new Screw(send3, bolts.Count));
                            break;
                        case "Wait 3":
                            obj.AddAction("Wait 3", new Screw(send3, bolts.Count));
                            break;
                        case "Unscrew":
                            obj.AddAction("Unscrew", new UnScrew(send4, bolts.Count));
                            break;
                        case "Unscrew 2":
                            obj.AddAction("Unscrew 2", new UnScrew(send4, bolts.Count));
                            break;
                        case "Wait 4":
                            obj.AddAction("Wait 4", new UnScrew(send4, bolts.Count));
                            break;
                    }
                }
                bolts.Add(bolt);

            }
        }
        public override void FixedUpdate()
        {
            short i = 1;
            socket.Send(BitConverter.GetBytes(i));
            socket.Send(BitConverter.GetBytes(player.transform.position.x));
            socket.Send(BitConverter.GetBytes(player.transform.position.y));
            socket.Send(BitConverter.GetBytes(player.transform.position.z));
            socket.Send(BitConverter.GetBytes(player.transform.rotation.eulerAngles.y));
        }
        public override void OnLoad()
        {
            
            player = GameObject.Find("PLAYER");
            databaseBody = GameObject.Find("DatabaseBody");
            databaseMechanics = GameObject.Find("DatabaseMechanics");
            databaseMotor = GameObject.Find("DatabaseMotor");
            databaseOrders = GameObject.Find("DatabaseOrders");
            databaseWiring = GameObject.Find("DatabaseWiring");
            playerDatabase = GameObject.Find("PlayerDatabase");
            parts = new List<Part>();

            //databaseBody

            for (int i = 1; i <= 8; i++)
            {
                Parsing1(databaseBody, "Remove part", "Assemble 2",i, "Wait", "Trigger", "Check for Part collision");
            }
            Parsing1(databaseBody, "Remove part", "Assemble", 9, "Wait", "Trigger", "Check for Part collision");

            Parsing2(databaseBody, "Remove part", "Assemble", "SpawnPartLocation", 10);
            Parsing2(databaseBody, "Remove part", "Assemble", "SpawnPartLocation", 11);
            for (int i = 12; i <= 21; i++)
            {
                Parsing2(databaseBody, "Remove part", "Assemble", "SpawnPartLocation", i);
            }
            Parsing3(databaseBody, "Remove part", "Assemble", 22);
            Parsing3(databaseBody, "Remove part", "Assemble", 23);

            Parsing2(databaseBody, "Remove part", "Assemble", "SpawnPartLocation", 24);
            Parsing2(databaseBody, "Remove part", "Assemble", "SpawnPartLocation", 25);

            Parsing2(databaseBody, "Remove part", "Assemble", "SpawnPartLocation", 26);
            Parsing2(databaseBody, "Remove part", "Assemble", "SpawnPartLocation", 27);

            Parsing3(databaseBody, "Remove part", "Assemble", 28);
            Parsing3(databaseBody, "Remove part", "Assemble", 29);

            Parsing2(databaseBody, "Remove part", "Assemble", "Trigger", 30);

            for(int i = 31; i <= 35; i++)
            {
                Parsing1(databaseBody, "Remove part", "Assemble 2", i, "Wait", "Trigger", "Check for Part collision");
            }

            Parsing2(databaseBody, "Remove part", "Assemble", "Trigger", 42);

            Parsing2(databaseBody, "Remove part", "Assemble", "SpawnPartLocation", 43);
            Parsing2(databaseBody, "Remove part", "Assemble", "SpawnPartLocation", 44);

            for(int i = 45; i <= 48; i++)
            {
                Parsing2(databaseBody, "Remove part", "Assemble", "Trigger", i);
            }

            //databaseMechanics

            Parsing1(databaseMechanics, "Remove part", "Assemble 2", 0, "Wait", "Trigger", "Check for Part collision");
            Parsing1(databaseMechanics, "Remove part", "Assemble", 1, "Wait", "Trigger", "Check for Part collision");

            for (int i = 2; i <= 5; i++)
            {
                Parsing1(databaseMechanics, "Remove part", "Assemble 2", i, "Wait", "Trigger", "Check for Part collision");
            }
            for(int i = 6; i <= 14; i++)
            {
                Parsing2(databaseMechanics, "Remove part", "Assemble", "Trigger", i);
            }

            Parsing1(databaseMechanics, "Remove part", "Assemble 2", 15, "Wait", "Trigger", "Check for Part collision");
            Parsing1(databaseMechanics, "Remove part", "Assemble 2", 16, "Wait", "Trigger", "Check for Part collision");

            Parsing2(databaseMechanics, "Remove part", "Assemble", "Trigger", 17);

            Parsing1(databaseMechanics, "Remove part", "Assemble 2", 18, "Wait", "Trigger", "Check for Part collision");

            for(int i =19; i <= 21; i++)
            {
                Parsing2(databaseMechanics, "Remove part", "Assemble", "Trigger", i);
            }

            Parsing1(databaseMechanics, "Remove part", "Assemble", 22, "Wait", "Trigger", "Check for Part collision");

            for (int i = 23; i <= 25; i++)
            {
                Parsing2(databaseMechanics, "Remove part", "Assemble", "Trigger", i);
            }

            Parsing1(databaseMechanics, "Remove part", "Stock", 26, "Find correct part 3", "Trigger", "Check for Part collision");
            Parsing1(databaseMechanics, "Remove part", "Assemble", 27, "Wait", "Trigger", "Check for Part collision");
            Parsing1(databaseMechanics, "Remove part", "Assemble 2", 28, "Wait 2", "Trigger", "Check for Part collision");

            for (int i = 1; i <= 12; i++)
            {
                
                Parsing1(databaseMotor, "Remove part", "Assemble", i, "Wait", "Trigger", "Check for Part collision");
            }
            Parsing1(databaseMotor, "Remove part", "Assemble", 13, "Wait", "TriggerRacing", "Check for Part collision");
            Parsing1(databaseMotor, "Remove part", "Assemble", 13, "Wait", "TriggerStock", "Check for Part collision");

            for(int i = 14; i <= 24; i++)
            {
                
                Parsing1(databaseMotor, "Remove part", "Assemble", i, "Wait", "Trigger", "Check for Part collision");
            }
            Parsing2(databaseMechanics, "Remove part", "Assemble", "Trigger", 25);
            
            for (int i = 26; i <= 29; i++)
            {
                Parsing1(databaseMotor, "Remove part", "Assemble", i, "Wait", "Trigger", "Check for Part collision");
            }
            Parsing1(databaseMotor, "Remove part", "Assemble 2", 30, "Wait 2", "Trigger", "Check for Part ");
            for(int i = 31; i <= 36; i++)
            {
                Parsing1(databaseMotor, "Remove part", "Assemble", i, "Wait", "Trigger", "Check for Part collision");
            }
            
        }
        private void Parsing1(GameObject database,string stateName, string stateName2, int ino, string nameToState, string triggerName, string stateName3)
        {
            Part part = new Part();
            part.gameObj = database.transform.GetChild(ino).gameObject;
            FsmTransition[] transition = new FsmTransition[3];
            transition[2] = new FsmTransition();
            part.gameObj.GetComponent<PlayMakerFSM>().FsmVariables.GetFsmGameObject(triggerName).Value.GetPlayMakerFSM("Assembly").Initialize();
            transition[2].FsmEvent = part.gameObj.GetComponent<PlayMakerFSM>().FsmVariables.GetFsmGameObject(triggerName).Value.GetPlayMakerFSM("Assembly").FsmEvents[3];
            transition[2].ToState = nameToState;
            transition[1] = part.gameObj.GetComponent<PlayMakerFSM>().FsmVariables.GetFsmGameObject(triggerName).Value.GetPlayMakerFSM("Assembly").GetState(stateName3).Transitions[1];
            transition[0] = part.gameObj.GetComponent<PlayMakerFSM>().FsmVariables.GetFsmGameObject(triggerName).Value.GetPlayMakerFSM("Assembly").GetState(stateName3).Transitions[0];
            part.gameObj.GetComponent<PlayMakerFSM>().FsmVariables.GetFsmGameObject(triggerName).Value.GetPlayMakerFSM("Assembly").GetState(stateName3).Transitions = transition;

            part.gameObj.GetComponent<PlayMakerFSM>().FsmVariables.GetFsmGameObject("ThisPart").Value.GetPlayMakerFSM("Removal").Initialize();
            part.gameObj.GetComponent<PlayMakerFSM>().FsmVariables.GetFsmGameObject("ThisPart").Value.GetPlayMakerFSM("Removal").AddAction(stateName, new RemovePart(send, parts.Count));
            part.removeEvent = "REMOVE";
            part.remove = part.gameObj.GetComponent<PlayMakerFSM>().FsmVariables.GetFsmGameObject("ThisPart").Value.GetPlayMakerFSM("Removal").SendEvent;

            part.gameObj.GetComponent<PlayMakerFSM>().FsmVariables.GetFsmGameObject(triggerName).Value.GetPlayMakerFSM("Assembly").AddAction(stateName2, new Assembly(socket, parts.Count));
            part.assembleEvent = "DISASSEMBLE";
            part.assemble = part.gameObj.GetComponent<PlayMakerFSM>().FsmVariables.GetFsmGameObject(triggerName).Value.GetPlayMakerFSM("Assembly").SendEvent;

            parts.Add(part);
        }

        private void Parsing2(GameObject database, string stateName, string stateName2,string triggerName, int ino)
        {
            Part part = new Part();
            part.gameObj = database.transform.GetChild(ino).gameObject;
            
            part.gameObj.GetComponent<PlayMakerFSM>().FsmVariables.GetFsmGameObject("ActivateThis").Value.GetPlayMakerFSM("Removal").Initialize();
            part.gameObj.GetComponent<PlayMakerFSM>().FsmVariables.GetFsmGameObject("ActivateThis").Value.GetPlayMakerFSM("Removal").AddAction(stateName, new RemovePart(send, parts.Count));
            part.remove = part.gameObj.GetComponent<PlayMakerFSM>().FsmVariables.GetFsmGameObject("ActivateThis").Value.GetPlayMakerFSM("Removal").SendEvent;
            part.removeEvent = "REMOVE";

            GameObject temp = part.gameObj.GetComponent<PlayMakerFSM>().FsmVariables.GetFsmGameObject("ActivateThis").Value.GetPlayMakerFSM("Removal").FsmVariables.GetFsmGameObject(triggerName).Value;
            
            temp.GetPlayMakerFSM("Assembly").Initialize();
            part.assemble = temp.GetPlayMakerFSM("Assembly").SendEvent;
            temp.GetPlayMakerFSM("Assembly").AddAction(stateName2, new Assembly(socket, parts.Count));
            part.assembleEvent = "ASSEMBLE";
            
            FsmTransition[] transition = new FsmTransition[2];
            transition[1] = new FsmTransition();
            transition[1].FsmEvent = temp.GetPlayMakerFSM("Assembly").FsmEvents[2];
            transition[1].ToState = "Assemble";
            transition[0] = temp.GetPlayMakerFSM("Assembly").GetState("Check for Part collision").Transitions[0];

            temp.GetPlayMakerFSM("Assembly").GetState("Check for Part collision").Transitions = transition;
           
            parts.Add(part);
        }

        private void Parsing3(GameObject database, string stateName, string stateName2, int ino)
        {
            Part part = new Part();
            part.gameObj = database.transform.GetChild(ino).gameObject;
            GameObject temp = part.gameObj.GetComponent<PlayMakerFSM>().FsmVariables.GetFsmGameObject("ActivateThis").Value;
            
            temp.transform.GetChild(temp.transform.childCount-1).GetPlayMakerFSM("Removal").Initialize();
            temp.transform.GetChild(temp.transform.childCount - 1).GetPlayMakerFSM("Removal").AddAction(stateName, new RemovePart(send, parts.Count));
            part.remove = temp.transform.GetChild(temp.transform.childCount - 1).GetPlayMakerFSM("Removal").SendEvent;
            part.removeEvent = "REMOVE";

            temp = part.gameObj.GetComponent<PlayMakerFSM>().FsmVariables.GetFsmGameObject("ActivateThis").Value.transform.GetChild(temp.transform.childCount - 1).GetPlayMakerFSM("Removal").FsmVariables.GetFsmGameObject("SpawnPartLocation").Value;

            temp.GetPlayMakerFSM("Assembly").Initialize();
            part.assemble = temp.GetPlayMakerFSM("Assembly").SendEvent;
            temp.GetPlayMakerFSM("Assembly").AddAction(stateName2, new Assembly(socket, parts.Count));
            part.assembleEvent = "ASSEMBLE";

            FsmTransition[] transition = new FsmTransition[2];
            transition[1] = new FsmTransition();
            transition[1].FsmEvent = temp.GetPlayMakerFSM("Assembly").FsmEvents[2];
            transition[1].ToState = "Assemble";
            transition[0] = temp.GetPlayMakerFSM("Assembly").GetState("Check for Part collision").Transitions[0];

            temp.GetPlayMakerFSM("Assembly").GetState("Check for Part collision").Transitions = transition;
            
            parts.Add(part);
        }

        public override void Update()
        {
            if (Input.GetKeyDown(KeyCode.Home)) parts[2].Assemble();
        }
    }
}

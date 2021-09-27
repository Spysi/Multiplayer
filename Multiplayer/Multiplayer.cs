using HutongGames.PlayMaker;
using MSCLoader;
using MSCLoader.Helper;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

namespace Multiplayer
{
    public class Multiplayer : Mod
    {
        public override string ID => "Multiplayer"; //Your mod ID (unique)
        public override string Name => "Multiplayer"; //You mod name
        public override string Author => "Spysi, Kiri111enz"; //Your Username
        public override string Version => "A0.2"; //Version

        public override bool Enabled => true;

        public override string Description => "Multiplayer mod";

        public static void SendMsg(int id, int doi)
        {
            ModConsole.Log(id);
            ModConsole.Log(doi);
        }
        private List<Part> parts;
        private List<Bolt> bolts;
        public byte id;
        //static RemovePart.Send send = SendMsg;
        static Screw.Send send3 = SendMsg;
        static UnScrew.Send send4 = SendMsg;
        GameObject databaseBody, databaseMechanics, databaseMotor, databaseOrders, databaseWiring, playerDatabase, player, playerPref, ui;
        Player[] players = new Player[16];
        Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        public override void MenuOnLoad()
        {
            AssetBundle ab = ModAssets.LoadBundle(this, "multiplayer");
            ui = GameObject.Instantiate(ab.LoadAsset("UIPrefab.prefab") as GameObject);
            playerPref = ab.LoadAsset("Player.prefab") as GameObject;
            ab.Unload(false);
            ui.transform.position = new Vector3(Screen.width, Screen.height, 0);
            ui.transform.SetParent(ModLoader.UICanvas.transform);
            ui.transform.localScale = new Vector3(1, 1, 1);
            ui.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(Connect);

            socket.SendTimeout = 5000;
            socket.ReceiveTimeout = 5000;

            //GameObject.Find("")

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
            ModConsole.Log("amogus");
            socket.Send(ByteConvertor.Convert("aboba"));
            ModConsole.Log("amogus");
            byte[] buff = new byte[25];
            socket.Receive(buff, 1, 0);
            id = buff[0];
            ModConsole.Log("amogus");
            ModConsole.Log(buff[0]);
            socket.Receive(buff, 1, 0);
            int count = buff[0];
            ModConsole.Log(buff[0]);
            for (int i = 0; i < count; i++)
            {
                socket.Receive(buff, 21, 0);
                players[buff[0]] = new Player();
                players[buff[0]].name = BitConverter.ToString(buff, 1);
                ModConsole.Log(players[buff[0]].name);
                // players[buff[0]].player = GameObject.Instantiate(playerPref);
            }

            socket.Blocking = false;
        }
        public override void PreLoad()
        {
            UnityEngine.Object.Destroy(ui);
            foreach (Player player in players)
            {
                //ModConsole.Log("aboba");
                if (player != null) player.player = GameObject.Instantiate(playerPref);
            }


            GameObject carParts = GameObject.Find("CARPARTS");
            PlayMakerFSM[] objs = carParts.transform.GetComponentsInChildren<PlayMakerFSM>(true);
            bolts = new List<Bolt>();
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
            foreach (Player player in players)
            {
                if (player != null)
                {
                    player.player.transform.position = player.position;
                    player.player.transform.rotation = player.rotation;
                }
            }
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
                Parsing1(databaseBody, "Remove part", "Assemble 2", i, "Wait", "Trigger", "Check for Part collision");
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

            for (int i = 31; i <= 35; i++)
            {
                Parsing1(databaseBody, "Remove part", "Assemble 2", i, "Wait", "Trigger", "Check for Part collision");
            }

            Parsing2(databaseBody, "Remove part", "Assemble", "Trigger", 42);

            Parsing2(databaseBody, "Remove part", "Assemble", "SpawnPartLocation", 43);
            Parsing2(databaseBody, "Remove part", "Assemble", "SpawnPartLocation", 44);

            for (int i = 45; i <= 48; i++)
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
            for (int i = 6; i <= 14; i++)
            {
                Parsing2(databaseMechanics, "Remove part", "Assemble", "Trigger", i);
            }

            Parsing1(databaseMechanics, "Remove part", "Assemble 2", 15, "Wait", "Trigger", "Check for Part collision");
            Parsing1(databaseMechanics, "Remove part", "Assemble 2", 16, "Wait", "Trigger", "Check for Part collision");

            Parsing2(databaseMechanics, "Remove part", "Assemble", "Trigger", 17);

            Parsing1(databaseMechanics, "Remove part", "Assemble 2", 18, "Wait", "Trigger", "Check for Part collision");

            for (int i = 19; i <= 21; i++)
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

            for (int i = 14; i <= 24; i++)
            {

                Parsing1(databaseMotor, "Remove part", "Assemble", i, "Wait", "Trigger", "Check for Part collision");
            }
            //Parsing2(databaseMotor, "Remove part", "Assemble", "Trigger", 25);

            for (int i = 26; i <= 29; i++)
            {
                Parsing1(databaseMotor, "Remove part", "Assemble", i, "Wait", "Trigger", "Check for Part collision");
            }
            Parsing1(databaseMotor, "Remove part", "Assemble 2", 30, "Wait 2", "Trigger", "Check for Part ");
            for (int i = 31; i <= 36; i++)
            {
                Parsing1(databaseMotor, "Remove part", "Assemble", i, "Wait", "Trigger", "Check for Part collision");
            }

            // Thread myThread = new Thread(new ThreadStart(Reading));
            //myThread.Start();

        }
        private void Parsing1(GameObject database, string stateName, string stateName2, int ino, string nameToState, string triggerName, string stateName3)
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
            part.gameObj.GetComponent<PlayMakerFSM>().FsmVariables.GetFsmGameObject("ThisPart").Value.GetPlayMakerFSM("Removal").AddAction(stateName, new RemovePart(socket, parts.Count));
            part.removeEvent = "REMOVE";
            part.remove = part.gameObj.GetComponent<PlayMakerFSM>().FsmVariables.GetFsmGameObject("ThisPart").Value.GetPlayMakerFSM("Removal").SendEvent;

            part.gameObj.GetComponent<PlayMakerFSM>().FsmVariables.GetFsmGameObject(triggerName).Value.GetPlayMakerFSM("Assembly").AddAction(stateName2, new Assembly(socket, parts.Count));
            part.assembleEvent = "DISASSEMBLE";
            part.assemble = part.gameObj.GetComponent<PlayMakerFSM>().FsmVariables.GetFsmGameObject(triggerName).Value.GetPlayMakerFSM("Assembly").SendEvent;

            parts.Add(part);
        }

        private void Parsing2(GameObject database, string stateName, string stateName2, string triggerName, int ino)
        {
            Part part = new Part();
            part.gameObj = database.transform.GetChild(ino).gameObject;

            part.gameObj.GetComponent<PlayMakerFSM>().FsmVariables.GetFsmGameObject("ActivateThis").Value.GetPlayMakerFSM("Removal").Initialize();
            part.gameObj.GetComponent<PlayMakerFSM>().FsmVariables.GetFsmGameObject("ActivateThis").Value.GetPlayMakerFSM("Removal").AddAction(stateName, new RemovePart(socket, parts.Count));
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

            temp.transform.GetChild(temp.transform.childCount - 1).GetPlayMakerFSM("Removal").Initialize();
            temp.transform.GetChild(temp.transform.childCount - 1).GetPlayMakerFSM("Removal").AddAction(stateName, new RemovePart(socket, parts.Count));
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

        public override void PostLoad()
        {
            Thread t = new Thread(new ThreadStart(Updater));
            t.Start();
        }
        void Updater()
        {
            while (true)
            {
                Thread.Sleep(20);
                socket.Send(ByteConvertor.Convert(player.transform.position, player.transform.rotation.eulerAngles.y, id));
                byte[] buffer = new byte[1];

                while (socket.Available > 0)
                {
                    socket.Receive(buffer, 1, 0);
                    byte[] buff = new byte[25];
                    switch (buffer[0])
                    {
                        case 0:
                            socket.Receive(buff, 21, 0);
                            players[buff[0]] = new Player();
                            players[buff[0]].name = BitConverter.ToString(buff, 1);
                            players[buff[0]].player = GameObject.Instantiate(playerPref);
                            break;
                        case 3:
                            socket.Receive(buff, 17, 0);
                            byte tempid = buff[0];
                            float x = BitConverter.ToSingle(buff, 1);
                            float y = BitConverter.ToSingle(buff, 5);
                            float z = BitConverter.ToSingle(buff, 9);
                            float rot = BitConverter.ToSingle(buff, 13);
                            lock (players[tempid])
                            {
                                players[tempid].rotation = new Quaternion(0, rot, 0, 0);
                                players[tempid].position = new Vector3(x, y, z);
                            }
                            break;
                        default:
                            ModConsole.Log("Aboba");
                            break;
                    }
                }
            }
        }



    }
}

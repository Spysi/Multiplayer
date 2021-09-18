using MSCLoader.Helper;
using UnityEngine;
using HutongGames.PlayMaker;
using System;
using System.Collections.Generic;
using HutongGames.PlayMaker.Actions;
using MSCLoader;

namespace Multiplayer
{
    public class Multiplayer : Mod
    {
        public override string ID => "Multiplayer"; //Your mod ID (unique)
        public override string Name => "Multiplayer"; //You mod name
        public override string Author => "Your Username"; //Your Username
        public override string Version => "1.0"; //Version

        // Set this to true if you will be load custom assets from Assets folder.
        // This will create subfolder in Assets folder for your mod.
        //public override bool UseAssetsFolder => false;

        public override void OnNewGame()
        {
            // Called once, when starting a New Game, you can reset your saves here
        }
        public static void SendMsg(int id,int doi)
        {
            ModConsole.Log(id);
            ModConsole.Log(doi);
        }
        private List<Part> parts;
        private List<GameObject> carParts, trigerParts, tempParts;
        static RemovePart.Send send = SendMsg;
        static Assembly.Send send2 = SendMsg;
        GameObject databaseBody, databaseMechanics, databaseMotor, databaseOrders, databaseWiring, playerDatabase;
        public override void PreLoad()
        {
            



        }
        void Asmbl()
        {

        }
        //CarPart part = new CarPart(send);
        public override void OnLoad()
        {
            databaseBody = GameObject.Find("DatabaseBody");
            databaseMechanics = GameObject.Find("DatabaseMechanics");
            databaseMotor = GameObject.Find("DatabaseMotor");
            databaseOrders = GameObject.Find("DatabaseOrders");
            databaseWiring = GameObject.Find("DatabaseWiring");
            playerDatabase = GameObject.Find("PlayerDatabase");
            parts = new List<Part>();
            for (int i = 1; i <= 8; i++)
            {
                Parsing1(databaseBody, "Remove part", "Assemble 2",i);
            }
            Parsing1(databaseBody, "Remove part", "Assemble", 9);

            Parsing2(databaseBody, "Remove part", "Assemble", 10, true);
            Parsing2(databaseBody, "Remove part", "Assemble", 11, true);
            for (int i = 12; i <= 21; i++)
            {
                Parsing2(databaseBody, "Remove part", "Assemble", i, false);
            }   

            

            /*
            tempParts = new List<GameObject>();
            carParts = new List<GameObject>();
            trigerParts = new List<GameObject>();
            //part.Awake();
            Parsing("Removal", "Remove part",null, 1);
            carParts = tempParts;
            tempParts = new List<GameObject>();
            Parsing("Assembly", "Assemble", "Assemble 2", 2);
            */
            //carParts.AddRange(PartsCarParsingINA(GameObject.Find("dashboard(Clone)").transform.GetComponentsInChildren<Transform>(true), "RemovePart"));
            //carParts.AddRange(PartsCarParsingINA(GameObject.Find("dashboard(Clone)").transform.GetComponentsInChildren<Transform>(true), "RemovePart"));

            //engineParts = GameObject.Find("CARPARTS").transform.GetChild(1).gameObject;
            //radiator = GameObject.Find("SATSUMA(557kg, 248)").transform.GetChild(11).GetChild(36).gameObject;

            /*
             radiator.SetActive(true);
             FsmState stat = radiator.transform.GetChild(1).GetComponent<PlayMakerFSM>().FsmStates[12];

             stat.Actions[0] = accation;

             PartsCarParsing(engineParts);
            */
            //radiator.transform.GetChild(1).GetComponent<PlayMakerFSM>().FsmStates[12].Actions[2] = act;
        }
        private void Parsing1(GameObject database,string stateName, string stateName2, int ino)
        {
            Part part = new Part();
            part.gameObj = database.transform.GetChild(ino).gameObject;
            FsmTransition[] transition = new FsmTransition[3];
            transition[2] = new FsmTransition();
            transition[2].FsmEvent = part.gameObj.GetComponent<PlayMakerFSM>().FsmVariables.GetFsmGameObject("Trigger").Value.GetPlayMakerFSM("Assembly").FsmEvents[5];
            transition[2].ToState = "Wait";
            transition[1] = part.gameObj.GetComponent<PlayMakerFSM>().FsmVariables.GetFsmGameObject("Trigger").Value.GetPlayMakerFSM("Assembly").GetState("Check for Part collision").Transitions[1];
            transition[0] = part.gameObj.GetComponent<PlayMakerFSM>().FsmVariables.GetFsmGameObject("Trigger").Value.GetPlayMakerFSM("Assembly").GetState("Check for Part collision").Transitions[0];
            part.gameObj.GetComponent<PlayMakerFSM>().FsmVariables.GetFsmGameObject("Trigger").Value.GetPlayMakerFSM("Assembly").GetState("Check for Part collision").Transitions = transition;
            
            part.gameObj.GetComponent<PlayMakerFSM>().FsmVariables.GetFsmGameObject("ThisPart").Value.GetPlayMakerFSM("Removal").AddAction(stateName, new RemovePart(send, parts.Count));
            part.removeEvent = "REMOVE";
            part.remove = part.gameObj.GetComponent<PlayMakerFSM>().FsmVariables.GetFsmGameObject("ThisPart").Value.GetPlayMakerFSM("Removal").SendEvent;

            part.gameObj.GetComponent<PlayMakerFSM>().FsmVariables.GetFsmGameObject("Trigger").Value.GetPlayMakerFSM("Assembly").AddAction(stateName2, new Assembly(send2, parts.Count));
            part.assembleEvent = "SETUP";
            part.assemble = part.gameObj.GetComponent<PlayMakerFSM>().FsmVariables.GetFsmGameObject("Trigger").Value.GetPlayMakerFSM("Assembly").SendEvent;

            part.bolts.AddRange(part.gameObj.GetComponentsInChildren<PlayMakerFSM>(true));

            parts.Add(part);
        }

        private void Parsing2(GameObject database, string stateName, string stateName2, int ino, bool altBolt)
        {
            Part part = new Part();
            part.gameObj = database.transform.GetChild(ino).gameObject;
            
            part.gameObj.GetComponent<PlayMakerFSM>().FsmVariables.GetFsmGameObject("ActivateThis").Value.GetPlayMakerFSM("Removal").Initialize();
            part.gameObj.GetComponent<PlayMakerFSM>().FsmVariables.GetFsmGameObject("ActivateThis").Value.GetPlayMakerFSM("Removal").AddAction(stateName, new RemovePart(send, parts.Count));
            part.remove = part.gameObj.GetComponent<PlayMakerFSM>().FsmVariables.GetFsmGameObject("ActivateThis").Value.GetPlayMakerFSM("Removal").SendEvent;
            part.removeEvent = "REMOVE";

            GameObject temp = part.gameObj.GetComponent<PlayMakerFSM>().FsmVariables.GetFsmGameObject("ActivateThis").Value.GetPlayMakerFSM("Removal").FsmVariables.GetFsmGameObject("SpawnPartLocation").Value;
            
            temp.GetPlayMakerFSM("Assembly").Initialize();
            part.assemble = temp.GetPlayMakerFSM("Assembly").SendEvent;
            temp.GetPlayMakerFSM("Assembly").AddAction(stateName2, new Assembly(send2, parts.Count));
            part.assembleEvent = "ASSEMBLE";
            
            FsmTransition[] transition = new FsmTransition[2];
            transition[1] = new FsmTransition();
            transition[1].FsmEvent = temp.GetPlayMakerFSM("Assembly").FsmEvents[2];
            transition[1].ToState = "Assemble";
            transition[0] = temp.GetPlayMakerFSM("Assembly").GetState("Check for Part collision").Transitions[0];

            temp.GetPlayMakerFSM("Assembly").GetState("Check for Part collision").Transitions = transition;
            
            part.bolts.AddRange(part.gameObj.GetComponent<PlayMakerFSM>().FsmVariables.GetFsmGameObject("ActivateThis").Value.GetComponentsInChildren<PlayMakerFSM>(true));
            if (altBolt)
            {
                try
                {
                    part.bolts.Add(part.gameObj.GetComponent<PlayMakerFSM>().FsmVariables.GetFsmGameObject("ActivateBolt").Value.GetComponent<PlayMakerFSM>());
                }
                catch (NullReferenceException){}
                try
                {
                    part.bolts.AddRange(part.gameObj.GetComponent<PlayMakerFSM>().FsmVariables.GetFsmGameObject("ActivateBolts").Value.GetComponentsInChildren<PlayMakerFSM>(true));
                }
                catch (NullReferenceException) { }
            }
            parts.Add(part);
        }

        private void Parsing3(GameObject database, string stateName, string stateName2, int ino)
        {
            Part part = new Part();
            part.gameObj = database.transform.GetChild(ino).gameObject;
            GameObject temp = part.gameObj.GetComponent<PlayMakerFSM>().FsmVariables.GetFsmGameObject("ActivateThis").Value;
            temp.GetPlayMakerFSM("Removal").Initialize();
            part.gameObj.GetComponent<PlayMakerFSM>().FsmVariables.GetFsmGameObject("ActivateThis").Value.GetPlayMakerFSM("Removal").AddAction(stateName, new RemovePart(send, parts.Count));
            part.remove = part.gameObj.GetComponent<PlayMakerFSM>().FsmVariables.GetFsmGameObject("ActivateThis").Value.GetPlayMakerFSM("Removal").SendEvent;
            part.removeEvent = "REMOVE";

            temp.GetPlayMakerFSM("Assembly").Initialize();
            part.assemble = temp.GetPlayMakerFSM("Assembly").SendEvent;
            temp.GetPlayMakerFSM("Assembly").AddAction(stateName2, new Assembly(send2, parts.Count));
            part.assembleEvent = "ASSEMBLE";

            FsmTransition[] transition = new FsmTransition[2];
            transition[1] = new FsmTransition();
            transition[1].FsmEvent = temp.GetPlayMakerFSM("Assembly").FsmEvents[2];
            transition[1].ToState = "Assemble";
            transition[0] = temp.GetPlayMakerFSM("Assembly").GetState("Check for Part collision").Transitions[0];

            temp.GetPlayMakerFSM("Assembly").GetState("Check for Part collision").Transitions = transition;

            part.bolts.AddRange(part.gameObj.GetComponent<PlayMakerFSM>().FsmVariables.GetFsmGameObject("ActivateThis").Value.GetComponentsInChildren<PlayMakerFSM>(true));
            
            parts.Add(part);
        }



        public override void ModSettings()
        {
            // All settings should be created here. 
            // DO NOT put anything else here that settings.
        }

        public override void OnSave()
        {
            // Called once, when save and quit
            // Serialize your save file here.
        }

        public override void Update()
        {
            if (Input.GetKeyDown(KeyCode.Home)) parts[2].Assemble();
        }

        private void PartsCarParsingA(GameObject[] parts, string fsmName, string stateName, string stateName2, int doi)
        {
            for (int i = 0; i < parts.Length; i++)
            {
                //if (tempParts.Exists(x => parts[i] == x)) continue;
                PlayMakerFSM part = parts[i].GetPlayMakerFSM(fsmName);
                if (part == null) continue;
                part.Initialize();
                if (IsStateExists(part, stateName))
                {
                    //part.AddAction(stateName, new RemovePart(send, tempParts.Count, doi));
                    tempParts.Add(parts[i].gameObject);
                }
                else if (IsStateExists(part, stateName2))
                {
                    //part.AddAction(stateName2, new RemovePart(send, tempParts.Count, doi));
                    tempParts.Add(parts[i].gameObject);
                }
                else ModConsole.Log("ABOBA");
            }
        }
        private void PartsCarParsingINARem(Transform[] parts, string fsmName, string stateName, string stateName2, int doi)
        {
            for (int i = 0; i < parts.Length; i++)
            {
                if (parts[i].gameObject.activeSelf) continue;
                parts[i].gameObject.SetActive(true);

                if (tempParts.Exists(x => parts[i].gameObject == x))
                {
                    parts[i].gameObject.SetActive(false);
                    continue;
                }
                PlayMakerFSM part = parts[i].GetPlayMakerFSM(fsmName);
                if (part == null)
                {
                    parts[i].gameObject.SetActive(false);
                    continue;
                }
                part.Initialize();
                if (IsStateExists(part, stateName))
                {
                    //part.AddAction(stateName, new RemovePart(send, tempParts.Count, doi));
                    tempParts.Add(parts[i].gameObject);
                }
                else if (IsStateExists(part, stateName2))
                {
                    //part.AddAction(stateName2, new RemovePart(send, tempParts.Count, doi));
                    tempParts.Add(parts[i].gameObject);
                }
                else ModConsole.Log("ABOBA");
                parts[i].gameObject.SetActive(false);
            }
        }

        private static bool IsActionExists(FsmState state, string name)
        {
            FsmStateAction[] actions = state.ActionData.LoadActions(state);
            foreach (FsmStateAction action in actions)
            {
                ModConsole.Log(action.Name);
                if (action.Name == name) return true;
            }
            return false;
        }
        private static bool IsStateExists(PlayMakerFSM fsm, string name)
        {
            foreach (FsmState state in fsm.FsmStates)
            {
                //ModConsole.Log(action.Name);
                if (state.Name == name) return true;
            }
            return false;
        }
    }
}

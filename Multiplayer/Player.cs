using UnityEngine;

namespace Multiplayer
{
    class Player
    {
        public GameObject player;
        public string name = null;
        public Vector3 position = new Vector3(0,0,0);
        public Quaternion rotation = new Quaternion(0,0,0,0);
    }
}

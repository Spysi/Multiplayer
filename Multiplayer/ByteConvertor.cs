using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Multiplayer
{
    static class ByteConvertor
    {
        public static byte[] Transform(Vector3 vector, float rotation, byte id)
        {
            List<byte> msg = new List<byte>();
            const byte b = 3, p = 18;
            msg.Add(p);
            msg.Add(b);
            msg.Add(id);
            msg.AddRange(BitConverter.GetBytes(vector.x));
            msg.AddRange(BitConverter.GetBytes(vector.y));
            msg.AddRange(BitConverter.GetBytes(vector.z));
            msg.AddRange(BitConverter.GetBytes(rotation));
            return msg.ToArray();
        }

        public static byte[] Name(string name)
        {
            List<byte> msg = new List<byte>();
            const byte b = 0;
            msg.Add((byte)name.Length);
            msg.Add(b);
            msg.AddRange(Encoding.UTF8.GetBytes(name));
            return msg.ToArray();
        }

        public static byte[] Item(Vector3 vector, Quaternion rotation, int idItem)//29
        {
            List<byte> msg = new List<byte>();
            const byte b = 4, p = 29;
            msg.Add(p);
            msg.Add(b);
            msg.AddRange(BitConverter.GetBytes(idItem));
            msg.AddRange(BitConverter.GetBytes(vector.x));
            msg.AddRange(BitConverter.GetBytes(vector.y));
            msg.AddRange(BitConverter.GetBytes(vector.z));
            msg.AddRange(BitConverter.GetBytes(rotation.eulerAngles.x));
            msg.AddRange(BitConverter.GetBytes(rotation.eulerAngles.y));
            msg.AddRange(BitConverter.GetBytes(rotation.eulerAngles.z));
            return msg.ToArray();
        }

        public static byte[] PickUp(int idItem, bool isPickUp)//4
        {
            List<byte> msg = new List<byte>();
            const byte b = 5, p = 6;
            msg.Add(p);
            msg.Add(b);
            msg.AddRange(BitConverter.GetBytes(idItem));
            msg.AddRange(BitConverter.GetBytes(isPickUp));
            return msg.ToArray();
        }
        public static byte[] Assembly(int idItem, int instId)//4
        {
            List<byte> msg = new List<byte>();
            const byte b = 6, p = 9;
            msg.Add(p);
            msg.Add(b);
            msg.AddRange(BitConverter.GetBytes(idItem));
            msg.AddRange(BitConverter.GetBytes(instId));
            return msg.ToArray();
        }

        public static byte[] RemovePart(int idItem)//4
        {
            List<byte> msg = new List<byte>();
            const byte b = 7, p = 5;
            msg.Add(p);
            msg.Add(b);
            msg.AddRange(BitConverter.GetBytes(idItem));
            return msg.ToArray();
        }

        public static byte[] Tingen(int idBolt,bool isTingen)
        {
            List<byte> msg = new List<byte>();
            const byte b = 8, p = 6;
            msg.Add(p);
            msg.Add(b);
            msg.AddRange(BitConverter.GetBytes(isTingen));
            msg.AddRange(BitConverter.GetBytes(idBolt));
            return msg.ToArray();
        }
        public static byte[] RecalculateID()
        {
            List<byte> msg = new List<byte>();
            const byte b = 9, p = 1;
            msg.Add(p);
            msg.Add(b);
            return msg.ToArray();
        }


    }
}

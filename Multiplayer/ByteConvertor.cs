using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using MSCLoader;
namespace Multiplayer
{
    static class ByteConvertor
    {
        public static byte[] Transform(Vector3 vector, float rotation, byte id)
        {
            List<byte> msg = new List<byte>();
            byte b = 3;
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
            byte b = 0;
            msg.Add(b);
            msg.AddRange(Encoding.UTF8.GetBytes(name));
            return msg.ToArray();
        }

        public static byte[] Item(Vector3 vector, Quaternion rotation, int idItem)//29
        {
            List<byte> msg = new List<byte>();
            byte b = 4;
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

        public static byte[] PickUp(int idItem)//4
        {
            List<byte> msg = new List<byte>();
            byte b = 5;
            msg.Add(b);
            msg.AddRange(BitConverter.GetBytes(idItem));
            return msg.ToArray();
        }

        public static byte[] Drop(int idItem)//4
        {
            List<byte> msg = new List<byte>();
            byte b = 6;
            msg.Add(b);
            msg.AddRange(BitConverter.GetBytes(idItem));
            return msg.ToArray();
        }
    }
}

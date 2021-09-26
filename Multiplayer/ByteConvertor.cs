using UnityEngine;
using System;
using System.Text;
using System.Collections.Generic;
namespace Multiplayer
{
    static class ByteConvertor
    {
        public static byte[] Convert(Vector3 vector, float rotation, byte id)
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

        public static byte[] Convert(string name)
        {
            List<byte> msg = new List<byte>();
            byte b = 0;
            msg.Add(b);
            msg.AddRange(Encoding.UTF8.GetBytes(name));
            return msg.ToArray();
        }
    }
}

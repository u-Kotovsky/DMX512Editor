using System;
using UnityEngine;

namespace Core
{
    [Serializable]
    public class DmxUniverseContainer
    {
        [SerializeField]
        public short Universe;
        [SerializeField]
        public byte[] Bytes = new byte[512];
    }
}
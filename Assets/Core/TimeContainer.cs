using System;
using UnityEngine;

namespace Core
{
    [Serializable]
    public class TimeContainer
    {
        [SerializeField]
        public byte Hours;
        [SerializeField]
        public byte Minutes;
        [SerializeField]
        public byte Seconds;
        [SerializeField]
        public byte Frames;
    }
}
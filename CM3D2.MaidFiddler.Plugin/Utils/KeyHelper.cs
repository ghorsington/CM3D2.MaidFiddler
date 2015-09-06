using System.Linq;
using UnityEngine;

namespace CM3D2.MaidFiddler.Plugin.Utils
{
    public class KeyHelper
    {
        private readonly KeyCode[] keys;
        private bool old, current;

        public KeyHelper(KeyCode key)
        {
            old = false;
            current = false;
            keys = new[] {key};
        }

        public KeyHelper(KeyCode[] keys)
        {
            old = false;
            current = false;
            this.keys = keys;
        }

        public bool HasBeenPressed()
        {
            return current && !old;
        }

        public bool IsDown()
        {
            return current;
        }

        public void Update()
        {
            old = current;
            current = keys.All(Input.GetKey);
        }
    }
}
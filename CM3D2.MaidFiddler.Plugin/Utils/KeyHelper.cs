using System.Linq;
using UnityEngine;

namespace CM3D2.MaidFiddler.Plugin.Utils
{
    public class KeyHelper
    {
        private bool old, current;

        public KeyHelper(KeyCode key)
        {
            old = false;
            current = false;
            Keys = new[] {key};
        }

        public KeyHelper(KeyCode[] keys)
        {
            old = false;
            current = false;
            Keys = keys;
        }

        public KeyCode[] Keys { get; set; }

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
            current = Keys.All(Input.GetKey);
        }
    }
}
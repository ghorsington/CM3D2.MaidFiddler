using UnityEngine;

namespace CM3D2.MaidFiddler.Plugin
{
    public class KeyHelper
    {
        private readonly KeyCode key;
        private bool old, current;

        public KeyHelper(KeyCode key)
        {
            old = false;
            current = false;

            this.key = key;
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
            current = Input.GetKeyDown(key);
        }
    }
}
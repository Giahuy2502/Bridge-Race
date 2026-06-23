using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyNamespace
{
    public enum ColorType
    {
        None = 0,
        Red = 1,
        Green = 2,
        Blue = 3,
        Yellow = 4,
        Violet = 5,
        Black = 6,
    }

    public static class Variables
    {
        public const string PLAYER_TAG = "Player";
        public const string IDLE_ANIM = "idle";
        public const string RUN_ANIM = "run";
    }

    public static class MyCache
    {
        private static Dictionary<Collider, Character> dicBridge = new Dictionary<Collider, Character>();

        public static T GetCharacter<T>(Collider collider) where T : Character
        {
            if (!dicBridge.ContainsKey(collider))
            {
                Character newChar = collider.gameObject.GetComponent<Character>();
                dicBridge.Add(collider, newChar);   
            }
            return dicBridge[collider] as T;
        }
    }
}

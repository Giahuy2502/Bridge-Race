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
        public const string BOT_TAG = "Bot";
        public const string DOOR_TAG = "Door";
        public const string IDLE_ANIM = "idle";
        public const string RUN_ANIM = "run";
    }

    public static class MyCache
    {
        private static Dictionary<Collider, Character> dicChar = new Dictionary<Collider, Character>();
        private static Dictionary<Collider, Stair> dicStair = new Dictionary<Collider, Stair>();

        public static T GetCharacter<T>(Collider collider) where T : Character
        {
            if (!dicChar.ContainsKey(collider))
            {
                Character newChar = collider.gameObject.GetComponent<Character>();
                dicChar.Add(collider, newChar);   
            }
            return dicChar[collider] as T;
        }

        public static T GetStair<T>(Collider collider) where T : Stair
        {
            if (!dicStair.ContainsKey(collider))
            {
                Stair newStair = collider.gameObject.GetComponent<Stair>();
                dicStair.Add(collider, newStair);
            }
            return dicStair[collider] as T;
        }
    }
}

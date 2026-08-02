using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyNamespace
{
    public enum SoundID
    {
        BG_MainMenu = 0,
        BG_GamePlay = 1,
    }

    public enum FxID
    {
        SFX_BrickFall = 0,
        SFX_BuildBridge = 1,
        SFX_ButtonClick = 2,
        SFX_CollectBrick = 3,
        SFX_Lose = 4,
        SFX_Hit = 5,
        SFX_Win = 6,
    }
    public enum ColorType
    {
        None = 0,
        Red = 1,
        Green = 2,
        Blue = 3,
        Yellow = 4,
        Violet = 5,
        Black = 6,
        white = 7,
    }

    public enum GameState
    {
        OnMain = 0,
        Playing = 1,
        EndGame = 2,
        Pause = 3,
    }

    public static class Variables
    {
        public const string PLAYER_TAG = "Player";
        public const string BOT_TAG = "Bot";
        public const string DOOR_TAG = "Door";
        public const string IDLE_ANIM = "idle";
        public const string RUN_ANIM = "run";
        public const string CHEER_ANIM = "cheer";
        public const string OPEN_ANIM = "open";
        public const string SAVE_KEY = "PlayerSaveData";
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


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoveStopMove.Core.Data {
    public class CharacterData : ScriptableObject
    {
        private float size = 1;
        private int level = 1;

        public float Speed = 3;
        public int Score = 0;

        public float Size => size;
        public float BaseAttackRange = 3f;
        public int Hp = 1;
        public int AttackCount = 1;
        public int Level
        {
            get => level;
            set
            {
                size = Mathf.Pow(1.1f, value);
                level = value;
            }
        }
        public float AttackRange => BaseAttackRange * Size;
    }
}
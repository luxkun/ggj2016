﻿using System;
using OpenTK;

namespace AShamanJourney
{
    public class Damage
    {
        private Vector2 direction;

        public Damage(Character character, Character enemy)
        {
            Character = character;
            Enemy = enemy;
        }

        public Vector2 Direction
        {
            get { return direction; }
            set
            {
                direction = value;
                InverseDirection = value*-1;
            }
        }

        public Vector2 InverseDirection { get; private set; }

        public Func<Character, Character, float> DamageFunc { private get; set; }

        public Character Enemy { get; set; }

        public Character Character { get; set; }
        public float KnockBack { get; set; } = 20f;

        public float Calculate(Character character, Character enemy)
        {
            return DamageFunc(character, enemy);
        }
    }
}
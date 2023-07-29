﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Cards.EffectCards
{
    [CreateAssetMenu(fileName = "NewEffectCard", menuName = "EffectCard")]
    public class EffectCard : Card
    {
        [SerializeField] private int lifeTime; // Time in laps before the effect stop
        [SerializeField] public bool checkTurn;

        [FormerlySerializedAs("affectPV")] [SerializeField]
        public float affectPv;

        public List<EffectAbilityName> EffectAbilities = new List<EffectAbilityName>();

        private void Awake()
        {
            type = CardType.Effect;
        }
    }
}
using System;
using UnityEngine;


[Serializable]
public struct EffectPayload // Makes sure we can have multiple effects on card // a list of these on cardData
{
    [Tooltip("The logic to execute (e.g., DamageEffect, HealEffect)")]
    public CardEffect effect;

    [Tooltip("The number of the effect (e.g., 4 damage, 6 healing)")]
    public int amount;
}







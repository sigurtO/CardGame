using System;
using UnityEngine;


[Serializable]
public struct EffectPayload
{
    [Tooltip("The logic to execute (e.g., DamageEffect, HealEffect)")]
    public CardEffect effect;

    [Tooltip("The magnitude of the effect (e.g., 4 damage, 6 healing)")]
    public int amount;
}
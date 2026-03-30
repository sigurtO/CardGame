using System;
using UnityEngine;


[Serializable]
public struct PhasePayload
{
    [Tooltip("The logic to execute (e.g., Damage, Poision, stun)")]
    public EnemyPhase phases;

    [Tooltip("The magnitude of the effect (e.g., 4 damage, 6 healing)")]
    public int amount;
}
using UnityEngine;
using System;
public enum StatusType { Poison, Regen, Weak, Strength, Block, Burn, Stun }
[Serializable]
public class ActiveStatus
{
    public StatusType statusType;
    public int durationTurns;
    public int amount;
}

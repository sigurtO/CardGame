using UnityEngine;

public class EffectTick : CardEffect
{
    [SerializeField]
    public int durationTurns = 3;
    public override void Execute(GameObject source, GameObject target, int effectValue)
    {
    }
}

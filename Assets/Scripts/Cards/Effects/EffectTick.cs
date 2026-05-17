using UnityEngine;

public abstract class EffectTick : CardEffect //parent class
{
    [SerializeField]
    public int durationTurns = 3;
    [SerializeField] public Sprite effectIcon;

    public override void Execute(GameObject source, GameObject target, int effectValue)
    {
    }
}

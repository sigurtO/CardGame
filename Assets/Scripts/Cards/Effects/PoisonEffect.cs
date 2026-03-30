using UnityEngine;

[CreateAssetMenu(menuName = "Effects/Poison")]

public class PoisonEffect : EffectTick
{
    public override void Execute(GameObject source, GameObject target, int effectValue)
    {
        IStatusReciver reciver = target.GetComponent<IStatusReciver>();

        if (reciver != null )
        {
            reciver.ApplyStatus(StatusType.Poison, effectValue, durationTurns, effectIcon);
        }
    }
}

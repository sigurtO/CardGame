using UnityEngine;
using static Unity.VisualScripting.Member;

[CreateAssetMenu(menuName = "Effects/Strenght")]
public class StrenghtEffect : EffectTick
{

    public override void Execute(GameObject source, GameObject target, int effectValue)
    {
        IStatusReciver reciver = source.GetComponent<IStatusReciver>();

        if (reciver != null)
        {
            reciver.ApplyStatus(StatusType.Strength, effectValue, durationTurns);
        }
    }
}


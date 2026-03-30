using UnityEngine;

[CreateAssetMenu(menuName = "Phases/Poison")]
public class PhasePoison : PhaseTick
{
    public override void ExecutePhase(GameObject source, GameObject target, int amount)
    {
        IStatusReciver reciver = target.GetComponent<IStatusReciver>();

        if (reciver != null)
        {
            reciver.ApplyStatus(StatusType.Poison, amount, durationTurns, statusIconToApply);
        }
    }
}

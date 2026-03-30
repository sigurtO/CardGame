using UnityEngine;

[CreateAssetMenu(menuName = "Phases/Strength")]
public class PhaseStrength : PhaseTick
{
    public override void ExecutePhase(GameObject source, GameObject target, int amount)
    {
        IStatusReciver reciver = source.GetComponent<IStatusReciver>();

        if (reciver != null)
        {
            reciver.ApplyStatus(StatusType.Strength, amount, durationTurns);
        }
    }
}

using UnityEngine;

[CreateAssetMenu(menuName = "Phases/Heal")]
public class PhaseHeal : EnemyPhase
{
    public override void ExecutePhase(GameObject source, GameObject target, int baseAmount)
    {
        // 1. We grab IBuffable from the SOURCE (The Enemy), not the target!
        IBuffable self = source.GetComponent<IBuffable>();

        if (self != null)
        {
            self.Heal(baseAmount);
            Debug.Log($"[{source.name}] cast a Heal and recovered {baseAmount} HP!");
        }
    }
}
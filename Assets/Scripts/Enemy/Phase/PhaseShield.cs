using UnityEngine;

[CreateAssetMenu(menuName = "Phases/Shield")]
public class PhaseShield : EnemyPhase
{
    public override void ExecutePhase(GameObject source, GameObject target, int baseAmount)
    {
        IBuffable self = source.GetComponent<IBuffable>();

        if (self != null)
        {
            self.Shield(baseAmount);
            Debug.Log($"[{source.name}] braced themselves, gaining {baseAmount} Shield!");
        }
        else
        {
            Debug.LogError($"[{source.name}] tried to shield, but it doesn't implement IBuffable!");
        }
    }
}
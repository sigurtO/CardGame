using UnityEngine;

[CreateAssetMenu(menuName = "Phases/Attack")]
public class PhaseAttack : EnemyPhase
{
    public override void ExecutePhase(GameObject source, GameObject target, int baseAmount)
    {
        int finalDamage = baseAmount;

        //check for strength buffs on the source and add them to the damage
        if (source != null)
        {
            IStatusReciver sourceStatuses = source.GetComponent<IStatusReciver>();
            if (sourceStatuses != null)
            {
                // Ask the backpack: "How much strength do we have right now?"
                int strength = sourceStatuses.GetTotalStatusAmount(StatusType.Strength);
                finalDamage += strength;

                Debug.Log($"Source has {strength} Strength! Damage boosted to {finalDamage}");
            }
        }

        ////check for vunerable debuffs on the target and increase damage by 50% for each stack
        //if (target != null)
        //{
        //    IStatusReciver targetStatuses = target.GetComponent<IStatusReciver>();
        //    if (targetStatuses != null)
        //    {
        //        int vulnerable = targetStatuses.GetTotalStatusAmount(StatusType.Vulnerable);
        //        if (vulnerable > 0)
        //        {
        //            finalDamage = Mathf.RoundToInt(finalDamage * 1.5f); // 50% more damage!
        //        }
        //    }
        //}


        ITakeDamage damageable = target.GetComponent<ITakeDamage>();
        damageable?.TakeDamage(finalDamage);
    }
}

using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Effects/Damage")]
public class DamageEffect : CardEffect
{
    public override void Execute(GameObject source, GameObject target, int effectValue)
    {
        int finalDamage = effectValue;

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


        ITakeDamage damageable = target.GetComponent<ITakeDamage>();
        damageable?.TakeDamage(finalDamage);
    }
}

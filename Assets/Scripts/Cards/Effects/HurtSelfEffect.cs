using UnityEngine;

public class HurtSelfEffect : CardEffect
{
    public override void Execute(GameObject source, GameObject target, int effectValue)
    {
        ITakeDamage damageable = target.GetComponent<ITakeDamage>(); //we may delete this
        damageable?.TakeDamage(effectValue);
    }
}

using UnityEngine;

[CreateAssetMenu(menuName = "Effects/Shield")]
public class ShieldEffect : CardEffect
{
    public override void Execute(GameObject source, GameObject target, int shieldAmount)
    {
        source.GetComponent<IBuffable>()?.Shield(shieldAmount);
    }

}

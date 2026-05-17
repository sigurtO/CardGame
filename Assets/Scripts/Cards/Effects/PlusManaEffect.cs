using UnityEngine;

[CreateAssetMenu(menuName = "Effects/PlusMana")]
public class PlusManaEffect : CardEffect
{
    public override void Execute(GameObject source, GameObject target, int effectValue)
    {

        source.GetComponent<Player>()?.GivePlayerMana(effectValue);
    }
}


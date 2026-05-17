using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[CreateAssetMenu(menuName = "Effects/ManaTick")]
public class ManaTIckEffect : EffectTick
{
    public override void Execute(GameObject source, GameObject target, int effectValue)
    {
        IStatusReciver reciver = target.GetComponent<IStatusReciver>();

        if (reciver != null)
        {
            reciver.ApplyStatus(StatusType.Mana, effectValue, durationTurns, effectIcon);
        }
    }
}

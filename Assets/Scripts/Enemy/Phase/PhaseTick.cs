using UnityEngine;

public class PhaseTick : EnemyPhase
{
    [SerializeField] public int durationTurns = 3;
    [SerializeField] public Sprite statusIconToApply; // on the player aka the debuff (different icon than the enemy intent)

    public override void ExecutePhase(GameObject source, GameObject target, int amount)
    {
 
    }
}

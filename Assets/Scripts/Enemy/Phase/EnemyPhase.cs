using UnityEngine;
using UnityEngine.UI;

public enum PhaseType
{
    Attack,
    Buff,
    Debuff
}
public abstract class EnemyPhase : ScriptableObject
{
    [SerializeField] public Sprite IntentIcon;
    [SerializeField] public PhaseType phaseType;
    public abstract void ExecutePhase(GameObject source, GameObject target, int amount); //either attack or apply a buff or debuff.
}

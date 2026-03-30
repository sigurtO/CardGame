using UnityEngine;

public abstract class CardEffect : ScriptableObject
{
    public abstract void Execute(GameObject source, GameObject target, int effectValue); // effects needs to know how to execute themselves
}

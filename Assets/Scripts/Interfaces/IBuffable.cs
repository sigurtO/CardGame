using UnityEngine;

public interface IBuffable
{

    int CurrentStrength { get; }
    void Heal(int amount) { }

    void Shield(int amount) { }

    void Strengthen(int amount) { }
}

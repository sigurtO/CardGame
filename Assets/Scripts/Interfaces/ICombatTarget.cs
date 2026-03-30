using UnityEngine;

public interface ICombatTarget : ITakeDamage, IBuffable
{
    // This interface combines both damage-taking and buffing capabilities for combat targets.
}

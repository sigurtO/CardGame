using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Enemy : MonoBehaviour, ICombatTarget //this holds both ItakeDamage and IBuffable
{
    [SerializeField] private int currentHealth = 50;
    [SerializeField] private int maxHealth = 50;
    [SerializeField] private Animation dmgAnimation;

    [SerializeField] private PhasePayload[] phases; // Array of phases for the enemy

    public UnityAction<int, int> OnHealthChanged; // event for ui
    public UnityAction OnIntentChanged;


    public PhasePayload CurrentIntent { get; private set; }

    public int CurrentStrength { get; private set; }

    private bool forceDamageNextTurn = false;


    private void Start()
    {

        currentHealth = maxHealth;         // Initialize health

        OnHealthChanged?.Invoke(currentHealth, maxHealth); //update ui
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;


       // currentHealth = Mathf.Max(0, currentHealth);         // Prevent health from going below 0 visually

        OnHealthChanged?.Invoke(currentHealth, maxHealth);
        dmgAnimation.Play(); // Play damage animation


        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Strengthen(int amount) 
    {
        CurrentStrength += amount;
    }
    private void Die()
    {
        // Implement death logic, such as playing an animation or dropping loot
        Debug.Log("Enemy has died.");
        Destroy(gameObject);
    }


    public void ChoosePhase() // we dont need to execute just choose a phase
    {
        if (forceDamageNextTurn) // force to pick attack phase if the flag is set
        {
            CurrentIntent = GetPhasePayloadByType(PhaseType.Attack);
            forceDamageNextTurn = false; // Reset the flag after forcing an attack
            Debug.Log($"[Enemy AI] Forced to pick: {CurrentIntent.phases.phaseType}"); // call a ui to show the intent
            OnIntentChanged?.Invoke(); // Notify UI of intent change
            return;
        }


        //NORMAL behavior: pick a random phase
        int randomIndex = Random.Range(0, phases.Length);
        CurrentIntent = phases[randomIndex];
        Debug.Log($"[Enemy AI] Randomly picked: {CurrentIntent.phases.phaseType}");


        OnIntentChanged?.Invoke(); // Notify UI of intent change

        //if phase isnt attack, set flag to force attack next turn
        if (CurrentIntent.phases.phaseType == PhaseType.Debuff || CurrentIntent.phases.phaseType == PhaseType.Buff)
        {
            forceDamageNextTurn = true;
            Debug.Log("[Enemy AI] Picked a buff/debuff. Raising flag to force Attack next turn!");
        }

    }

    private PhasePayload GetPhasePayloadByType(PhaseType type)
    {
        foreach (var payload in phases)
        {
            if (payload.phases.phaseType == type)
            {
                return payload;
            }
        }
        Debug.LogError($"[Enemy AI] Could not find a phase of type {type}!");
        return phases[0]; // Fallback so the game doesn't crash
    }

    public void ExecutePhase(GameObject playerTarget)
    {
        // 1. Safety check: Did we somehow forget to choose an intent?
        if (CurrentIntent.phases == null)
        {
            Debug.LogError($"[Enemy] {gameObject.name} tried to execute a phase, but CurrentIntent is null!");
            return;
        }

        CurrentIntent.phases.ExecutePhase(this.gameObject, playerTarget, CurrentIntent.amount);

        Debug.Log($"[Enemy] {gameObject.name} executed {CurrentIntent.phases.phaseType} on player!");
    }

}

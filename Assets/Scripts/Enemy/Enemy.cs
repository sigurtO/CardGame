using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Enemy : MonoBehaviour, ICombatTarget //this holds both ItakeDamage and IBuffable
{
    [SerializeField] private int currentHealth = 50;
    [SerializeField] private int maxHealth = 50;

    private int currentShield = 0; // Starts at 0 every battle

    public static event Action<int> OnEnemyTookDamage; //for our statistics

    [SerializeField] private PhasePayload[] phases; // Array of phases for the enemy

    public UnityAction<int, int> OnHealthChanged; // event for ui
    public UnityAction OnIntentChanged;

    public static event Action<Enemy> OnEnemyDied;
    public UnityAction<int> OnShieldChanged;

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
        //shield absorbs damage first
        if (currentShield > 0)
        {
            if (currentShield >= amount)
            {
                currentShield -= amount;
                amount = 0;
            }
            else
            {
                amount -= currentShield;
                currentShield = 0;
            }
            OnShieldChanged?.Invoke(currentShield);
        }
        //no shield
        if (amount > 0)
        {
            currentHealth -= amount;
            OnEnemyTookDamage?.Invoke(amount);
            OnHealthChanged?.Invoke(currentHealth, maxHealth);

            if (currentHealth <= 0)
            {
                Die();
            }
        }
    }

    void Strengthen(int amount) //we dont use this
    {
        CurrentStrength += amount;
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Min(currentHealth, maxHealth); // Don't overheal

        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }
    public void Shield(int amount)
    {
        currentShield += amount;
        OnShieldChanged?.Invoke(currentShield);
    }

    public void ResetShield() //IDK if we want to use this yet.
    {
        if (currentShield > 0)
        {
            currentShield = 0;
            OnShieldChanged?.Invoke(currentShield);
        }
    }
    private void Die()
    {
        OnEnemyDied?.Invoke(this);
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
        int randomIndex = UnityEngine.Random.Range(0, phases.Length);
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

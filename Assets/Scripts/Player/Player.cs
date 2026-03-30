using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour, ICombatTarget //this holds both ItakeDamage and IBuffable
{
    [SerializeField] private int currentHealth = 100;
    [SerializeField] private int maxHealth = 100;

    [SerializeField] private int shield = 0;

    public int CurrentStrength { get; private set; }

    [SerializeField] private TurnManager turnManager;
    //[Header("Inventory")]
    //[SerializeField] private useables[] useables
    //[SerializeField] private totems[] totems

    public UnityAction<int, int> OnHealthChanged; 
    public UnityAction<int> OnShield;
    public UnityAction OnDeath;

    private void Start()
    {

        currentHealth = maxHealth;         // Initialize health

        OnHealthChanged?.Invoke(currentHealth, maxHealth); //update ui
    }

    private void OnEnable()
    {
        turnManager.OnPlayerTurnStart += ResetShield;
    }


    //Interfaces
    public void TakeDamage(int dmg)
    {
        // 1. Let the shield absorb damage first
        if (shield > 0)
        {
            if (shield >= dmg)
            {
                shield -= dmg; // Shield absorbs everything
                dmg = 0;       // No damage left to hit health
            }
            else
            {
                dmg -= shield; // Shield absorbs what it can, remainder goes to health
                shield = 0;    // Shield is destroyed
            }

            OnShield?.Invoke(shield); // Update shield UI
        }

        // 2. Apply remaining damage to health
        if (dmg > 0)
        {
            currentHealth -= dmg;

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                OnDeath?.Invoke();
                Debug.Log("Player has died.");
            }

            // Invoke health change regardless of if we died or not
            OnHealthChanged?.Invoke(currentHealth, maxHealth);
        }
    }

    void Strengthen(int amount)
    {
        CurrentStrength += amount;
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

    public void Shield(int amount) 
    { 
        shield += amount;
        OnShield?.Invoke(shield); //for updating shield UI


        // implement so the shield goes away after enemy round is over
    }

    private void ResetShield()
    {
        if (shield > 0)
        {
            shield = 0;
            Debug.Log("Shield reset at end of enemy turn.");
        }
        OnShield?.Invoke(shield); //update shield UI
    }

}

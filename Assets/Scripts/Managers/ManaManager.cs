using UnityEngine;
using UnityEngine.Events;

public class ManaManager : MonoBehaviour
{

    [SerializeField] private int startingMana = 3;
    public int CurrentMana { get; private set; }
    public int MaxMana { get; private set; }

    public UnityAction<int, int> OnManaChanged; // event for ui

    private void Start()
    {
        MaxMana = startingMana;
        ResetManaForNewTurn();

        OnManaChanged?.Invoke(CurrentMana, MaxMana); // Update UI at the start
    }

    public void ResetManaForNewTurn()
    {
        CurrentMana = MaxMana;
        OnManaChanged?.Invoke(CurrentMana, MaxMana); // Update UI
    }

    public void ResetManaForNewTurn(int bonusMana)    //status effect might give us more mana next turn
    {
        CurrentMana = bonusMana;
        OnManaChanged?.Invoke(CurrentMana, MaxMana); // Update UI
    }


    public bool TryConsumeMana(int amount)
    {
        if (CurrentMana >= amount)
        {
            CurrentMana -= amount;

            OnManaChanged?.Invoke(CurrentMana, MaxMana); //update ui
            return true;
        }

        // Not enough mana!
        return false;
    }

}

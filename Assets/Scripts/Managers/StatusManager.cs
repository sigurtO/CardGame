using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class StatusManager : MonoBehaviour, IStatusReciver
{
    //serialed for testing purpose //this goes on player/enemy
    [SerializeField] public List<ActiveStatus> activeStatuses = new List<ActiveStatus>();
    private ICombatTarget target;   // holds Ibuffable and ItakeDamage

    [SerializeField] private TurnManager turnManager;

    public event Action OnStatusesChanged;

    private void Awake()
    {
        target = GetComponent<ICombatTarget>();
    }

    private void OnEnable()
    {
        turnManager.OnTurnTick += OnTurnEndTick; // subscribe to the event when turn ticks
    }

    private void OnDisable()
    {
        turnManager.OnTurnTick -= OnTurnEndTick;

    }


    public void ApplyStatus(StatusType statusType, int amount, int durationTurns, Sprite icon)
    {
        activeStatuses.Add(new ActiveStatus { statusType = statusType, amount = amount, durationTurns = durationTurns, statusIcon = icon });

        OnStatusesChanged?.Invoke();
    }

    public int GetTotalStatusAmount(StatusType type)
    {
        int total = 0;
        foreach (var status in activeStatuses)
        {
            if (status.statusType == type)
            {
                total += status.amount;
            }
        }
        return total;
    }


    public void OnTurnEndTick()
    {
        for (int i = activeStatuses.Count - 1; i >= 0; i--)
        {
            ActiveStatus status = activeStatuses[i];

            // 1. ONLY execute Action Statuses here!
            if (status.statusType == StatusType.Poison)
            {
                target?.TakeDamage(status.amount);
            }

            // 2. Reduce duration
            status.durationTurns--;

            // 3. Remove if expired
            if (status.durationTurns <= 0)
            {
                activeStatuses.RemoveAt(i);
            }
        }
        OnStatusesChanged?.Invoke();
    }
}

using UnityEngine;

public interface IStatusReciver
{
    void ApplyStatus(StatusType status, int amount, int durationTurns);

    int GetTotalStatusAmount(StatusType type);
}

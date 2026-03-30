using UnityEngine;

public interface IStatusReciver
{
    void ApplyStatus(StatusType status, int amount, int durationTurns, Sprite statusIcon);

    int GetTotalStatusAmount(StatusType type);
}

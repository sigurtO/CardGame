using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game Data/Card Pool")]
public class CardPool : ScriptableObject
{
    [Tooltip("Drag every card that can possibly drop into this list!")]
    public List<CardData> availableCards = new List<CardData>();

    public List<CardData> RollRewards(int amountToRoll) //rewards
    {
        List<CardData> rewards = new List<CardData>();

        if (availableCards.Count < amountToRoll)
        {
            Debug.LogWarning("Not enough cards in the pool to generate rewards!");
            return availableCards;
        }


        List<CardData> tempPool = new List<CardData>(availableCards);

        for (int i = 0; i < amountToRoll; i++) // makes sure we don't 3 of the same cards
        {
            int randomIndex = Random.Range(0, tempPool.Count);

            rewards.Add(tempPool[randomIndex]);

            tempPool.RemoveAt(randomIndex);
        }

        return rewards;
    }
}
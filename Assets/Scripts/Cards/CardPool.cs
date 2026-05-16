using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game Data/Card Pool")]
public class CardPool : ScriptableObject
{
    [Tooltip("Drag every card that can possibly drop into this list!")]
    public List<CardData> availableCards = new List<CardData>();

    // This smart method rolls random cards and makes sure you don't get 3 of the exact same card in the reward screen!
    public List<CardData> RollRewards(int amountToRoll)
    {
        List<CardData> rewards = new List<CardData>();

        // Safety check: Do we even have enough cards in the database?
        if (availableCards.Count < amountToRoll)
        {
            Debug.LogWarning("Not enough cards in the pool to generate rewards!");
            return availableCards;
        }

        // Create a temporary copy of the pool so we can remove cards as we pick them
        List<CardData> tempPool = new List<CardData>(availableCards);

        for (int i = 0; i < amountToRoll; i++)
        {
            // Pick a random index
            int randomIndex = Random.Range(0, tempPool.Count);

            // Add the card to our rewards
            rewards.Add(tempPool[randomIndex]);

            // Remove it from the temporary pool so we don't pick it again!
            tempPool.RemoveAt(randomIndex);
        }

        return rewards;
    }
}
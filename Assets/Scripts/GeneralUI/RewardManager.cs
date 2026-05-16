using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RewardManager : MonoBehaviour
{
    [Header("Data Backpack")]
    [SerializeField] private CurrentRunState runState;

    [Header("UI References")]
    [SerializeField] private GameObject rewardScreenPanel; // The darkened background overlay
    [SerializeField] private Transform cardContainer;      // The Horizontal Layout Group for the 3 cards
    [SerializeField] private CardUi cardPrefab;            // Your exact same Card UI prefab!



    private void OnEnable()
    {
        EnemyManager.OnBattleWon += ShowRewards;
    }

    private void OnDisable()
    {
        EnemyManager.OnBattleWon -= ShowRewards;
    }

    private void ShowRewards()
    {
        // 1. Pause the game/turn off combat UI (Optional, but good practice)
        rewardScreenPanel.SetActive(true);

        // 2. Roll the Loot!
        List<CardData> rolledCards = runState.classCardPool.RollRewards(3);

        // 3. Spawn the UI Cards
        foreach (CardData cardData in rolledCards)
        {
            CardUi newCard = Instantiate(cardPrefab, cardContainer);

            // Reuse your existing Initialize method! 
            // We pass it a NEW callback specifically for the reward screen.
            newCard.Initialize(cardData, OnRewardCardSelected);
            newCard.gameObject.SetActive(true);
        }
    }

    // This is the callback the CardUi will trigger when clicked!
    private void OnRewardCardSelected(CardData chosenData, CardUi clickedCard)
    {

        runState.masterDeck.Add(chosenData);

        Debug.Log($"[Rewards] Added {chosenData.cardName} to the Master Deck!");

        // Return to the Map Scene!
        ReturnToMap();
    }

    // Every AAA card game needs a Skip button!
    public void SkipRewards()
    {
        Debug.Log("[Rewards] Player skipped rewards.");
        ReturnToMap();
    }

    private void ReturnToMap()
    {
        // Make sure this string EXACTLY matches the name of your map scene
        SceneManager.LoadScene("MapScene");
    }
}
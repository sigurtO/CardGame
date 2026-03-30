using System;
using System.Collections.Generic;
using UnityEngine;


public class MatchController : MonoBehaviour
{
    [Header("Inject systems")]
    [SerializeField] private GameObject playerGameObject; // Drag your player here!
    [SerializeField] private DeckManager deckManager;
    [SerializeField] private HandManagerUI handManager;
    [SerializeField] private ManaManager manaManager;
    [SerializeField] private CardUi cardUi; // for click
    [SerializeField] private TargetingManager targetingManager; // for click


    [Header("Game rules")]
    [SerializeField] private int minHandSize = 4;


    private CardData loadedCard = null; // save the selected card in memory
    private CardUi loadedCardUi = null; // ADD THIS: We now remember the physical UI!


    public void SetSelectedCard(CardData clickedCardData, CardUi clickedCardUi)
    {
        loadedCard = clickedCardData;
        loadedCardUi = clickedCardUi;

        targetingManager.StartTargeting(clickedCardUi.GetComponent<RectTransform>());
    }



    //First turn
    public void ShuffelCards()
    {
        deckManager.ShuffleDeck();
    }



    public void DrawCardOnPlayerTurn()
    {
        if(deckManager.CurrentHand.Count < minHandSize)
        {
            int cardsNeeded = minHandSize - deckManager.CurrentHand.Count;
           List<CardData> drawnCards = deckManager.DrawMultipleCards(cardsNeeded);

            foreach (CardData card in drawnCards)
            {
                handManager.DrawCardUi(card);
            }
        }
        else // if we are above minHandSize we simply draw 1 card
        {
            CardData drawnCard = deckManager.DrawCardAndAddToHand();

            if(drawnCard != null)
            {
                handManager.DrawCardUi(drawnCard);
            }
        }
    }

    public void ExecutePlayedCard(GameObject targetHit) //mana and do dmg and remove card from hand.
    {
        if (loadedCard == null || loadedCardUi == null) return;


        bool isEnemy = targetHit.CompareTag("Enemy");
        bool isAlly = targetHit.CompareTag("Player");

        if (loadedCard.targetType == TargetType.EnemyOnly && !isEnemy)
        {
            Debug.Log("Invalid Target! You can't cast a fireball on your friend.");
            return; // Stops the execution. Card stays in hand!
        }

        if (loadedCard.targetType == TargetType.AllyOnly && !isAlly)
        {
            Debug.Log("Invalid Target! You can't cast a shield on the enemy.");
            return; // Stops the execution. Card stays in hand!
        }

        if(!manaManager.TryConsumeMana(loadedCard.manaCost))
        {
            Debug.Log("Not enough mana to play this card!");

            CancelCardSelection();
            return;
        }

            // 1. EXECUTE ALL EFFECTS FIRST
            foreach (var payload in loadedCard.Effects)
            {
                if (payload.effect != null)
                {
                payload.effect.Execute(playerGameObject, targetHit, payload.amount); Debug.Log($"Executed effect {payload.effect.name} with amount {payload.amount} on target {targetHit.name}");
                }
                else
                {
                    Debug.LogWarning($"Card {loadedCard.cardName} has an effect payload but the effect is unassigned!");
                }
            }





        //2/ REMOVE CARD FROM HAND
        deckManager.RemoveCardFromHand(loadedCard);
        // 3. UPDATE HAND UI
        handManager.OnCardUse(loadedCardUi);

        loadedCard = null;
        loadedCardUi = null;
    }

    public void CancelCardSelection()
    {
        loadedCard = null; // clear memory
    }




}

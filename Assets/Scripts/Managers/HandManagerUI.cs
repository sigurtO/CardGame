using System.Collections.Generic;
using UnityEngine;

public class HandManagerUI : MonoBehaviour
{
    [SerializeField]
    private CardUi cardPrefab;

    [SerializeField] private GameObject cardContainer;
    [SerializeField] private Transform handContainer; // horizonGroup layout container for our hand cards
    [SerializeField] private MatchController matchController; // for click callback
    [SerializeField] private TurnManager turnManager; // for turn based card draw



    private List<CardUi> cardPool = new List<CardUi>();

    private void OnEnable()
    {
        turnManager.OnPlayerTurnEnded += DisableHand;
        turnManager.OnEnemyTurnEnded += EnableHand;
    }

    private void OnDisable()
    {
        turnManager.OnPlayerTurnEnded -= DisableHand;
        turnManager.OnEnemyTurnEnded -= EnableHand;
    }
    public void DrawCardUi(CardData drawnData)
    {
        CardUi card = GetAvailableCard();

        card.Initialize(drawnData, matchController.SetSelectedCard);

        card.gameObject.SetActive(true);

        card.transform.SetAsLastSibling();
    }

    public void OnCardUse(CardUi playedCard)
    {
        playedCard.gameObject.SetActive(false);
    }

    public CardUi GetAvailableCard()
    {
        foreach (CardUi card in cardPool)
        {
            if(!card.gameObject.activeInHierarchy)
            {
                return card;
            }
        }
        CardUi newCard = Instantiate(cardPrefab, handContainer); // the pool is empty or we have used all cards so we add a new card
        cardPool.Add(newCard);

        return newCard;
    }

    private void DisableHand()
    {
        cardContainer.SetActive(false);
    }

    private void EnableHand()
    {
        cardContainer.SetActive(true);
    }

}

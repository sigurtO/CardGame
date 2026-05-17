using System;
using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    [Header("Master Deck In Run State")]
    [SerializeField] private CurrentRunState runState; // Our master Deck

    //only serized for testing purpose
    [SerializeField]  private List<CardData> drawPile = new List<CardData>(); // our playTime Deck (shuffeled version of master deck)
    [SerializeField]  private List<CardData> hand = new List<CardData>(); // cards in our hand
    [SerializeField]  private List<CardData> discardPile = new List<CardData>(); // cards in our discard pile
    [SerializeField] private List<CardData> exhaustPile = new List<CardData>();


    public static event Action<int, int, int> OnDeckCountsChanged;

    public IReadOnlyList<CardData> CurrentHand => hand; //so other scripts can read out private hand but not change it

    public void SetupCombatDeck()
    {
        drawPile.Clear();
        hand.Clear();
        exhaustPile.Clear();
        discardPile.Clear();

        if (runState == null || runState.masterDeck.Count == 0)
        {
            Debug.LogError("[DeckManager] Master Deck is empty or missing!");
            return;
        }


        drawPile.AddRange(runState.masterDeck); //copy master deck to draw pile

        drawPile.Shuffle(); //shuffel the cards //from our ListExtension

        Debug.Log($"[DeckManager] Initialized with {drawPile.Count} cards.");

        OnDeckCountsChanged?.Invoke(drawPile.Count, discardPile.Count, exhaustPile.Count);
    }

    public CardData DrawCardAndAddToHand() //uses DrawCard method and adds the card to hand
    {
        CardData card = DrawCard();

        if (card != null)
        {
            hand.Add(card);
            return card;

        }
        return null;

    }

    public List<CardData> DrawMultipleCards(int amount) // uses DrawCardAndAddToHand.
    {
        List<CardData> drawnCards = new List<CardData>();

        for(int i = 0; i < amount; i++)
        {
            CardData drawnCard = DrawCardAndAddToHand();
            if(drawnCard != null)
            {
                drawnCards.Add(drawnCard);
            }
            else
            {
                break; // stop drawing if we run out of cards
            }
        }
        return drawnCards;
    }

    public CardData DrawCard() // draws card but doesnt add it to hand (maybe a card says to draw a card and discard it or something)
    {
        if (drawPile.Count == 0)
        {
            if (discardPile.Count == 0) // This happens if we keep drawing and dont use
            {
                Debug.Log("[DeckManager] Draw pile AND Discard pile are empty! Can't draw."); 
                return null;
            }
            ReshuffleDiscardIntoDraw();
        }

        int topCardIndex = drawPile.Count - 1; // top card is now at end of list (count -1 refers to the end of a list. this way we dont need to move the list up each time we draw a card)
        CardData drawnCard = drawPile[topCardIndex];

        drawPile.RemoveAt(topCardIndex); //remove card of our deck
        OnDeckCountsChanged?.Invoke(drawPile.Count, discardPile.Count, exhaustPile.Count);
        return drawnCard;
    }

    private void ReshuffleDiscardIntoDraw()
    {
        Debug.Log("[DeckManager] Reshuffling discard pile into draw pile!");

        // Move all discard cards to the draw pile
        drawPile.AddRange(discardPile);
        discardPile.Clear();

        // Shuffle the newly formed draw pile
        drawPile.Shuffle();
        OnDeckCountsChanged?.Invoke(drawPile.Count, discardPile.Count, exhaustPile.Count);
    }



    public void DiscardCard(CardData card)
    {
        hand.Remove(card);
        discardPile.Add(card);
        OnDeckCountsChanged?.Invoke(drawPile.Count, discardPile.Count, exhaustPile.Count);

    }

    public void ExhaustCard(CardData card)
    {
        hand.Remove(card);
        exhaustPile.Add(card);

        Debug.Log($"[DeckManager] {card.cardName} was Exhausted!");
        OnDeckCountsChanged?.Invoke(drawPile.Count, discardPile.Count, exhaustPile.Count);

    }
}

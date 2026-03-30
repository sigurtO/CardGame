using UnityEngine;
using System.Collections.Generic;

public class DeckManager : MonoBehaviour
{
    [Header("master data")]
    [SerializeField] private List<CardData> fullDeck; //this is the list of all our cards


    //only serized for testing purpose
  [SerializeField]  private List<CardData> playTimeDeck = new List<CardData>(); // this is the list of our cards we will slowly empty this list during each round
  [SerializeField]  private List<CardData> hand = new List<CardData>(); // cards in our hand
    // maybe a discard pile


    public IReadOnlyList<CardData> CurrentHand => hand; //so other scripts can read out private hand but not change it

    public void ShuffleDeck()
    {
        playTimeDeck = new List<CardData>(fullDeck);

        playTimeDeck.Shuffle();
    }

    public CardData DrawCardAndAddToHand() // we may want to make this DrawCardAndAddToHand()
    {
        CardData card = DrawCard();

        if (card != null)
        {
            hand.Add(card); 
            return card;
        }
        return null;
    }

    public List<CardData> DrawMultipleCards(int amount)
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
        if (playTimeDeck.Count <= 0)
        {
            //gotta figure out what we want to do when we run out of cards
            return null;
        }

        int topCardIndex = playTimeDeck.Count - 1; // top card is now at end of list (count -1 refers to the end of a list. this way we dont need to move the list up each time we draw a card)
        CardData drawnCard = playTimeDeck[topCardIndex];

        playTimeDeck.RemoveAt(topCardIndex); //remove card of our deck
        return drawnCard;
    }

    public void RemoveCardFromHand(CardData card)
    {
        hand.Remove(card);
    }

}

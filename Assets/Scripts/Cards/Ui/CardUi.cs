using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardUi : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private CardData cardData;
    [SerializeField] private Image cardImage;
    [SerializeField] private TextMeshProUGUI cardName;
    [SerializeField] private TextMeshProUGUI cardDescription;
    [SerializeField] private TextMeshProUGUI manaCost;

    private CardData myCardData;

    public UnityAction<CardData, CardUi> onCardSelected;
    public void Initialize(CardData newData, UnityAction<CardData, CardUi> clickCallback)
    {
        if(newData == null)
        {
            Debug.LogError($"[CardUI] Attempted to initialize {gameObject.name} with null data!", this);
            return;
        }
        myCardData = newData;
        onCardSelected = clickCallback;
        UpdateUi();
    }
    void UpdateUi()
    {
        cardImage.sprite = myCardData.image;
        cardName.text = myCardData.cardName;
        cardDescription.text = myCardData.description;
        manaCost.text = myCardData.manaCost.ToString();

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log($"[CardUi] I was grabbed! Starting Drag for {myCardData.cardName}!");
        onCardSelected?.Invoke(myCardData, this);

    }
}

using System.Text.RegularExpressions;
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

    [SerializeField] private StatusManager playerStatusManager;

    private CardData myCardData;

    public CardData GetCardData() => this.myCardData;

    public UnityAction<CardData, CardUi> onCardSelected;

    private void OnEnable()
    {
        EnsureStatusSubscription();

        // If we're being re-enabled (e.g. hand container toggled), refresh the text.
        UpdateUi();
    }

    public void Initialize(CardData newData, UnityAction<CardData, CardUi> clickCallback)
    {
        if(newData == null)
        {
            Debug.LogError($"[CardUI] Attempted to initialize {gameObject.name} with null data!", this);
            return;
        }
        myCardData = newData;
        onCardSelected = clickCallback;

        EnsureStatusSubscription();
        UpdateUi();
    }
    void UpdateUi()
    {
        if (myCardData == null)
        {
            return;
        }

        cardImage.sprite = myCardData.image;
        cardName.text = myCardData.cardName;
        cardDescription.text = FormatDescription(myCardData.description);
        manaCost.text = myCardData.manaCost.ToString();

    }

    private string FormatDescription(string raw)
    {
        if (string.IsNullOrEmpty(raw))
        {
            return raw;
        }

        bool cardHasDamageEffect = false;
        foreach (var payload in myCardData.Effects)
        {
            if (payload.effect is DamageEffect)
            {
                cardHasDamageEffect = true;
                break;
            }
        }

        int strength = 0;
        if (cardHasDamageEffect && playerStatusManager != null)
        {
            strength = playerStatusManager.GetTotalStatusAmount(StatusType.Strength);
        }

        // Replaces tokens like "{6}" with "6" (or "6 + Strength" for damage cards)
        return Regex.Replace(raw, @"\{(\d+)\}", match =>
        {
            if (!int.TryParse(match.Groups[1].Value, out int baseAmount))
            {
                return match.Value;
            }

            return (baseAmount + strength).ToString();
        });
    }

    private void EnsureStatusSubscription()
    {
        if (playerStatusManager == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                playerStatusManager = player.GetComponentInChildren<StatusManager>();
            }
        }

        if (playerStatusManager != null)
        {
            // Avoid duplicate subscription when objects get enabled/disabled repeatedly.
            playerStatusManager.OnStatusesChanged -= UpdateUi;
            playerStatusManager.OnStatusesChanged += UpdateUi;
        }
    }

    private void OnDisable()
    {
        if (playerStatusManager != null)
        {
            playerStatusManager.OnStatusesChanged -= UpdateUi;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log($"[CardUi] I was grabbed! Starting Drag for {myCardData.cardName}!");
        onCardSelected?.Invoke(myCardData, this);

    }
}

using UnityEngine;
using TMPro;

public class DeckUIManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI drawPileText;
    [SerializeField] private TextMeshProUGUI discardPileText;
    [SerializeField] private TextMeshProUGUI exaushPileText;

    private void OnEnable()
    {
        // Subscribe to the broadcaster
        DeckManager.OnDeckCountsChanged += UpdateCounters;
    }

    private void OnDisable()
    {
        // ALWAYS unsubscribe to prevent memory leaks!
        DeckManager.OnDeckCountsChanged -= UpdateCounters;
    }

    private void UpdateCounters(int drawCount, int discardCount, int exhaustCount)
    {
        // Update the physical text on the screen
        if (drawPileText != null)
            drawPileText.text = drawCount.ToString();

        if (discardPileText != null)
            discardPileText.text = discardCount.ToString();

        if (exaushPileText != null)
            exaushPileText.text = exhaustCount.ToString();

        // Optional: You could add a tiny scale animation here using DOTween or LeanTween
        // to make the numbers "pop" when they change!
    }
}
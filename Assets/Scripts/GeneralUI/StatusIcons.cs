using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class StatusIcons : MonoBehaviour
{
    [SerializeField] private Image statusImage;
    [SerializeField] private TextMeshProUGUI statusDuration;
    [SerializeField] private TextMeshProUGUI statusAmount;

    // The Pool Manager will call this and hand over the exact data it needs to show
    public void Setup(ActiveStatus status)
    {
        statusDuration.text = status.durationTurns.ToString() + "T";
        statusAmount.text = status.amount.ToString() + "A";

        if (status.statusIcon != null)
        {
            statusImage.sprite = status.statusIcon;
        }
    }
}

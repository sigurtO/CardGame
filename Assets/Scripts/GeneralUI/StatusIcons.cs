using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class StatusIcons : MonoBehaviour
{
    [SerializeField] private Image statusImage;
    [SerializeField] private TextMeshProUGUI statusDuration;

    // The Pool Manager will call this and hand over the exact data it needs to show
    public void Setup(ActiveStatus status)
    {
        statusDuration.text = status.durationTurns.ToString();

        //if (status.statusIcon != null)
        //{
        //    statusImage.sprite = status.statusIcon;
        //}
    }
}

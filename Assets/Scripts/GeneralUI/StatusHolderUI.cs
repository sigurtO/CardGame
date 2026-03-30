using System.Collections.Generic;
using UnityEngine;

public class StatusHolderUI : MonoBehaviour
{
    [SerializeField] private Transform statusContainer;
    [SerializeField] private StatusManager statusManager;
    [SerializeField] private StatusIcons statusIconPrefab;

    [SerializeField]
    private List<StatusIcons> statusPool = new List<StatusIcons>();

    private void OnEnable()
    {
        if (statusManager != null)
        {
            statusManager.OnStatusesChanged += UpdateStatusIcons;
        }
    }

    private void OnDisable()
    {
        if (statusManager != null)
        {
            statusManager.OnStatusesChanged -= UpdateStatusIcons;
        }
    }

    public void UpdateStatusIcons()
    {
        // Turn OFF all icons in the pool
        foreach (StatusIcons icon in statusPool)
        {
            icon.gameObject.SetActive(false);
        }

        // Loop through the actual statuses the player currently has
        for (int i = 0; i < statusManager.activeStatuses.Count; i++)
        {
            ActiveStatus currentStatus = statusManager.activeStatuses[i];

            // Grab an icon from the pool (or make a new one)
            StatusIcons pooledIcon = GetOrCreateIcon(i);

            // Feed it the data and turn it ON
            pooledIcon.Setup(currentStatus);
            pooledIcon.gameObject.SetActive(true);
        }
    }

    private StatusIcons GetOrCreateIcon(int index) // pooling
    {
        if (index < statusPool.Count)
        {
            return statusPool[index];
        }

        // Otherwise, we need to expand the pool! Instantiate a new one.
        StatusIcons newIcon = Instantiate(statusIconPrefab, statusContainer);
        statusPool.Add(newIcon);

        return newIcon;
    }

}

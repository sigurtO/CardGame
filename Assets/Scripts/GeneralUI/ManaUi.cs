using UnityEngine;
using TMPro;

public class ManaUi : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI manaText;
    [SerializeField] private ManaManager manaManager;


    private void OnEnable()
    {
        manaManager.OnManaChanged += UpdateManaUI; // Subscribe to mana change events
    }

    private void OnDisable()
    {
        manaManager.OnManaChanged -= UpdateManaUI;
    }

    private void UpdateManaUI(int currentMana, int maxMana)
    {
        manaText.text = $"{currentMana}/{maxMana}";
    }
}

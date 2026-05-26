using UnityEngine;
using UnityEngine.InputSystem;

public class TargetingManager : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private MatchController matchController;
    [SerializeField] private ArrowPointer arrowPointer;

    [Header("Settings")]
    [SerializeField] private LayerMask targetableLayer;

    private RectTransform activeCardTransform;
    private bool isTargeting = false;

    private void Start()
    {
        arrowPointer.EnableArrow(false);
    }

    public void StartTargeting(RectTransform cardTransform)
    {
        activeCardTransform = cardTransform;
        isTargeting = true;
        arrowPointer.EnableArrow(true);
    }

    private void Update()
    {
        if (!isTargeting) return;

        if (Pointer.current == null) return;

        Vector2 pointerPos = Pointer.current.position.ReadValue(); // hold the current position of the mouse or touch

        Vector2 startPos = activeCardTransform.position;
        arrowPointer.UpdateArrowPosition(startPos, pointerPos);

        if (Pointer.current.press.wasReleasedThisFrame) // release mouse (on target?)
        {
            ConfirmTarget(pointerPos);
        }

        if (Mouse.current != null && Mouse.current.rightButton.wasPressedThisFrame) //cancel
        {
            CancelTargeting();
        }
    }

    private void ConfirmTarget(Vector2 currentPointerPos)
    {
        Vector2 worldPoint = Camera.main.ScreenToWorldPoint(currentPointerPos); // UI to world

        // find collider
        RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero, Mathf.Infinity, targetableLayer);

        if (hit.collider != null) // hit collider
        {
            GameObject targetHit = hit.collider.gameObject;
            Debug.Log($"Direct hit on 2D Collider: {targetHit.name}");

            matchController.ExecutePlayedCard(targetHit);
        }
        else
        {
            Debug.Log("Let go over empty space. Cancelling card play.");
            matchController.CancelCardSelection();
        }

        // Always turn off the arrow when release
        isTargeting = false;
        arrowPointer.EnableArrow(false);
    }

    public void CancelTargeting()
    {
        isTargeting = false;
        arrowPointer.EnableArrow(false);
        matchController.CancelCardSelection();
    }
}
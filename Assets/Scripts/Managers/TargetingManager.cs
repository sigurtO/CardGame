// ✅ THE ELITE WAY: Universal Hold-and-Release Targeting
using UnityEngine;
using UnityEngine.InputSystem; // Still using the New Input System!

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

        // 1. SAFETY: Ensure there is an active touch or mouse
        if (Pointer.current == null) return;

        // 2. CROSS-PLATFORM: Read the position of the Touch OR the Mouse
        Vector2 pointerPos = Pointer.current.position.ReadValue();

        Vector2 startPos = activeCardTransform.position;
        arrowPointer.UpdateArrowPosition(startPos, pointerPos);

        // 3. THE RELEASE: Did the player let go of the mouse or lift their finger?
        if (Pointer.current.press.wasReleasedThisFrame)
        {
            ConfirmTarget(pointerPos);
        }

        // 4. THE CANCEL (Optional): Right-click on PC, or maybe a 2-finger tap on mobile to cancel
        if (Mouse.current != null && Mouse.current.rightButton.wasPressedThisFrame)
        {
            CancelTargeting();
        }
    }

    private void ConfirmTarget(Vector2 currentPointerPos)
    {
        // 1. Convert the UI screen pixels into an exact coordinate in your 2D game world
        Vector2 worldPoint = Camera.main.ScreenToWorldPoint(currentPointerPos);

        // 2. Ask the 2D Engine: "Is there a collider at this exact world point?"
        // (We pass Vector2.zero for the direction because we are just poking a single point, not shooting a line)
        RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero, Mathf.Infinity, targetableLayer);

        // 3. Did we hit something? (Notice it's hit.collider != null for 2D, instead of Physics.Raycast returning a bool!)
        if (hit.collider != null)
        {
            GameObject targetHit = hit.collider.gameObject;
            Debug.Log($"Direct hit on 2D Collider: {targetHit.name}");

            matchController.ExecutePlayedCard(targetHit);
        }
        else
        {
            // If they let go over empty space, cancel the targeting!
            Debug.Log("Let go over empty space. Cancelling card play.");
            matchController.CancelCardSelection();
        }

        // Always turn off the arrow when they let go
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
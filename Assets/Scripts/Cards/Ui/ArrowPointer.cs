using UnityEngine;

public class ArrowPointer : MonoBehaviour
{
    [Header("Ui Reference")]
    [SerializeField] private RectTransform arrowBody;
    [SerializeField] private RectTransform arrowHead;

    [Header("Tuning")]
    [SerializeField] private float headOffset = 50f;

    public void EnableArrow(bool isEnabled)
    {
        gameObject.SetActive(isEnabled);
    }

    public void UpdateArrowPosition(Vector2 startScreenPos, Vector2 endScreenPos)
    {
        // 1. Place the pieces first. Unity automatically handles the Canvas Scaling here!
        arrowBody.position = startScreenPos;
        arrowHead.position = endScreenPos;

        // 2. THE FIX: Calculate the distance using LOCAL positions, not Screen positions.
        Vector2 localDirection = arrowHead.localPosition - arrowBody.localPosition;
        float localDistance = localDirection.magnitude;

        // 3. Get the angle using the local direction
        float angle = Mathf.Atan2(localDirection.y, localDirection.x) * Mathf.Rad2Deg;

        // 4. Apply the rotation to both pieces
        arrowBody.rotation = Quaternion.Euler(0, 0, angle);
        arrowHead.rotation = Quaternion.Euler(0, 0, angle);

        // 5. Apply the safe, localized size
        float bodyLength = Mathf.Max(0, localDistance - headOffset);
        arrowBody.sizeDelta = new Vector2(bodyLength, arrowBody.sizeDelta.y);
    }
}
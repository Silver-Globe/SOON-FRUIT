using UnityEngine;
using UnityEngine.EventSystems;

public class PieClickHandler : MonoBehaviour, IPointerClickHandler
{
    private SphereTimerUI timerUI;

    void Start()
    {
        timerUI = GetComponent<SphereTimerUI>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (timerUI == null) return;

        // Convert click position into local UV coordinates (0–1)
        RectTransform rt = GetComponent<RectTransform>();
        Vector2 localPos;
        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(rt, eventData.position, eventData.pressEventCamera, out localPos))
            return;

        // Normalize to -1..1 (same as shader)
        Vector2 uv = new Vector2(localPos.x / rt.rect.width * 2f, localPos.y / rt.rect.height * 2f);

        // Convert UV → angle in degrees (match shader logic)
        float angle = Mathf.Atan2(uv.y, uv.x) * Mathf.Rad2Deg + 90f;
        if (angle < 0) angle += 360f;

        // Find slice index
        float sliceAngle = 360f / timerUI.TotalSections;
        int sliceIndex = Mathf.FloorToInt(angle / sliceAngle);

        // Toggle or activate that slice
        timerUI.SetSliceActive(sliceIndex, true);

        Debug.Log($"Clicked slice {sliceIndex} on {gameObject.name}");
    }
}

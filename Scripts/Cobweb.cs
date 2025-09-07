using UnityEngine;

public class CobwebLine : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private Transform startTransform;
    private Transform endTransform;

    [Range(2, 64)] public int segments = 12;
    public float wobbleStrength = 0.1f;
    public float wobbleSpeed = 5f;

    public void Init(Transform start, Transform end, LineRenderer lr)
    {
        startTransform = start;
        endTransform = end;
        lineRenderer = lr;

        lineRenderer.useWorldSpace = true;
        lineRenderer.positionCount = segments;
    }

    void Update()
    {
        if (!lineRenderer || !startTransform || !endTransform) return;

        Vector3 startPos = startTransform.position;
        Vector3 endPos = endTransform.position;

        lineRenderer.SetPosition(0, startPos);
        lineRenderer.SetPosition(segments - 1, endPos);

        Vector3 dir = (endPos - startPos).normalized;
        Vector3 perp = AnyPerpendicular(dir);

        for (int k = 1; k < segments - 1; k++)
        {
            float t = k / (float)(segments - 1);
            Vector3 point = Vector3.Lerp(startPos, endPos, t);

            float falloff = Mathf.Sin(t * Mathf.PI);
            float wobble = Mathf.Sin(Time.time * wobbleSpeed + k) * wobbleStrength * falloff;

            point += perp * wobble;

            lineRenderer.SetPosition(k, point);
        }
    }

    private static Vector3 AnyPerpendicular(Vector3 v)
    {
        if (Mathf.Abs(v.y) < 0.99f)
            return Vector3.Cross(v, Vector3.up).normalized;
        return Vector3.Cross(v, Vector3.right).normalized;
    }
}

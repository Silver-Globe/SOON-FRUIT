using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [Header("Grid Slots")]
    public List<Transform> slotTransforms; // assign all 9 GridSlot transforms in Inspector

    // Return nearest available slot to a position
    public Transform GetNearestSlot(Vector3 position)
    {
        Transform nearest = null;
        float minDist = Mathf.Infinity;

        foreach (var slot in slotTransforms)
        {
            if (IsSlotOccupied(slot)) continue;

            float dist = Vector3.Distance(position, slot.position);
            if (dist < minDist)
            {
                minDist = dist;
                nearest = slot;
            }
        }

        return nearest;
    }

    // Check if a slot already has a sphere
    public bool IsSlotOccupied(Transform slot)
    {
        Collider[] hits = Physics.OverlapSphere(slot.position, 0.1f);
        foreach (var hit in hits)
        {
            if (hit.GetComponent<Renderer>() != null) // any placed sphere with a renderer counts
                return true;
        }
        return false;
    }
}

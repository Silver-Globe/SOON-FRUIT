using UnityEngine;

public class PieColorController : MonoBehaviour
{
    public Material pieMaterial;
    public Color[] sliceColors; // assign in inspector (size = slice count)

    void Update()
    {
        for (int i = 0; i < sliceColors.Length; i++)
        {
            pieMaterial.SetColor("_SliceColor" + i, sliceColors[i]);
        }
    }

    public void SetSliceColor(int sliceIndex, Color color)
    {
        if (sliceIndex >= 0 && sliceIndex < sliceColors.Length)
        {
            sliceColors[sliceIndex] = color;
        }
    }
}

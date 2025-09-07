using UnityEngine;
using TMPro;

public class FloatingText : MonoBehaviour
{
    public float floatSpeed = 1f;
    public float duration = 1.2f;

    private Transform mainCamera;

    void Start()
    {
        mainCamera = Camera.main.transform; // cache the main camera
        Destroy(gameObject, duration);
    }

    void Update()
    {
        // Move upward
        transform.position += Vector3.up * floatSpeed * Time.deltaTime;

        // Always face the camera (billboard effect)
        if (mainCamera != null)
        {
            transform.LookAt(transform.position + mainCamera.forward);
        }
    }

    public void StartFloating()
    {
        // left here so GameManager can call it, 
        // but the destroy logic is now in Start
    }
}

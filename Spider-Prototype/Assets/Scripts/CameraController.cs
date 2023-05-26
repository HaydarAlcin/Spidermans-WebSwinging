using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform target; 
    [SerializeField] float distance = 5f;
    [SerializeField] float height = 2f; 
    [SerializeField] float smoothSpeed = 10f;
    [SerializeField] float rotationSpeed = 5f; 
    [SerializeField] float verticalRotationLimit;

    private float currentRotationX = 0f;

    private void Start()
    {
      
        Cursor.lockState = CursorLockMode.Locked;

        
        Cursor.visible = false;
    }

    private void LateUpdate()
    {
        // Fare yatay (X ekseni) ve dikey (Y ekseni) hareketini al
        float mouseX = Input.GetAxis("Mouse X") * rotationSpeed;
        float mouseY = Input.GetAxis("Mouse Y") * rotationSpeed;

        // Hedef nesneyi yatayda dondur
        target.Rotate(Vector3.up, mouseX);

        // Dikey donus açýsýný guncelle
        currentRotationX -= mouseY;
        currentRotationX = Mathf.Clamp(currentRotationX, -verticalRotationLimit, verticalRotationLimit);

        // Kamerayi yatay ve dikeyde dondur
        transform.localRotation = Quaternion.Euler(currentRotationX, target.eulerAngles.y, 0f);

        // Kameranin hedefi takip etmesi için pozisyonunu guncelle
        Vector3 desiredPosition = target.position - (transform.forward * distance) + (Vector3.up * height);
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
    }
}

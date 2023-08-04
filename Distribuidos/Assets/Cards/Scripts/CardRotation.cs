using UnityEngine;
public class CardRotation : MonoBehaviour
{
    [SerializeField] private float rotationSpeedY = 100f;
    [SerializeField] private float rotationSpeedX = 5f;
    private float currentRotationX = 0f;
    private bool isRotating = false;
    private Vector3 mouseStartPosition;

    private void Update()
    {
        if (!isRotating && Input.GetMouseButtonDown(0))
        {
            isRotating = true;
            mouseStartPosition = Input.mousePosition;
        }
        if (Input.GetMouseButtonUp(0)) isRotating = false;
        if (isRotating) RotateCard();
    }

    private void RotateCard()
    {
        Vector3 currentMousePosition = Input.mousePosition;
        Vector3 mouseOffset = currentMousePosition - mouseStartPosition;

        float rotationX = -mouseOffset.y * rotationSpeedX * Time.deltaTime;
        float rotationY = mouseOffset.x * rotationSpeedY * Time.deltaTime;
            
        float unclampedRotationX = currentRotationX + rotationX;
        currentRotationX = Mathf.Clamp(unclampedRotationX, -5f, 5f);
        transform.rotation = Quaternion.Euler(currentRotationX, transform.eulerAngles.y + rotationY, 0f);
        mouseStartPosition = currentMousePosition;
    }
}
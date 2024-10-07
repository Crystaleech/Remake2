using UnityEngine;

public class CameraControllerMouseEdge : MonoBehaviour
{
    public float moveSpeed = 10f;         
    public float edgeThreshold = 10f;     
    public float minX = -50f, maxX = 50f;   
    public float minZ = -50f, maxZ = 50f;   

    private float screenWidth;
    private float screenHeight;

    void Start()
    {
        screenWidth = Screen.width;
        screenHeight = Screen.height;
    }

    void Update()
    {
        Vector3 movement = Vector3.zero;

         
        if (Input.mousePosition.x <= edgeThreshold)
        {
            movement.x = -1;
        }
         
        else if (Input.mousePosition.x >= screenWidth - edgeThreshold)
        {
            movement.x = 1;
        }

         
        if (Input.mousePosition.y <= edgeThreshold)
        {
            movement.z = -1;
        }
         
        else if (Input.mousePosition.y >= screenHeight - edgeThreshold)
        {
            movement.z = 1;
        }

         
        transform.Translate(movement * moveSpeed * Time.deltaTime, Space.World);

         
        Vector3 clampedPosition = transform.position;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, minX, maxX);  
        clampedPosition.z = Mathf.Clamp(clampedPosition.z, minZ, maxZ);   
        transform.position = clampedPosition;
    }
}

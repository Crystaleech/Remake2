using UnityEngine;

public class CameraControllerArrowKeys : MonoBehaviour
{
    public float moveSpeed = 10f;  // Speed of the camera movement
    public float minX = -50f, maxX = 50f;  
    public float minZ = -50f, maxZ = 50f;  //add limit

    void Update()
    {
        
        float moveHorizontal = Input.GetAxis("Horizontal");  
        float moveVertical = Input.GetAxis("Vertical");       

        
        Vector3 movement = new Vector3(moveHorizontal, 0f, moveVertical);

         
        transform.Translate(movement * moveSpeed * Time.deltaTime, Space.World);

        
        Vector3 clampedPosition = transform.position;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, minX, maxX);   
        clampedPosition.z = Mathf.Clamp(clampedPosition.z, minZ, maxZ);  
        transform.position = clampedPosition;
    }
}

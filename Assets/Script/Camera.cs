using UnityEngine;

public class Camera : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    public Transform playerBody;

    public GameObject obj;
    float xRotation = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // Ẩn và khóa chuột vào giữa màn hình
    }

    void Update()
    {
        Movement movement =obj.GetComponent<Movement>();
        bool istextboxOn = movement.IsTextboxOn;

        if (istextboxOn == false)
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Giới hạn góc nhìn lên/xuống

            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f); // Xoay camera lên/xuống
            playerBody.Rotate(Vector3.up * mouseX); // Xoay nhân vật trái/phải
        }
        else
        {
            if (Input.GetMouseButtonDown(1) == true)
            {
                istextboxOn = false;
            }
        }
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    public float sensitivity = 1000.0f;
    public Transform playerCam;

    private float XRotation = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        //Cursor.lockState = CursorLockMode.;
        
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

        XRotation -= mouseY;
        XRotation = Mathf.Clamp(XRotation, -90.0f, 90.0f);

        playerCam.localRotation = Quaternion.Euler(XRotation, 0.0f, 0.0f);
        transform.Rotate(Vector3.up * mouseX);

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = new Ray(playerCam.position, playerCam.forward);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 10.0f, LayerMask.GetMask("UI")))
            {
                hit.collider.gameObject.GetComponent<Button>().onClick.Invoke();
                Debug.Log("Hit UI");
            }
            Debug.DrawRay(ray.origin, ray.direction, Color.red, 5.0f, false);
            //Debug.Log("Ray: " + ray.origin + " "+ ray.direction);
        }
    }
}

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements.Experimental;

public class EquipmentRotation : MonoBehaviour
{
    [SerializeField] private float zRot;
    Camera mainCamera;
    InputAction pointAction;
    private Vector2 pointVector;
    private void Start()
    {
        pointAction = InputSystem.actions.FindAction("Point");
        if (pointAction != null) pointAction.Enable();
        else Debug.LogError("Point action could not be found");
        mainCamera = Camera.main;
    }
    private void Update()
    {
        pointVector = pointAction.ReadValue<Vector2>();
        pointVector = mainCamera.ScreenToWorldPoint(pointVector);
        pointVector = pointVector - new Vector2(transform.position.x, transform.position.y);
        float angle;
        if (pointVector.x <= 0)
        {
            angle = Vector2.Angle(Vector2.up, pointVector);
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
        else
        {
            angle = -Vector2.Angle(Vector2.up, pointVector);
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        Debug.Log(angle);
        Vector3 rotation;
        rotation = new Vector3(0f, 0f, angle);
        Quaternion rot;
        rot = Quaternion.Euler(rotation);
        transform.rotation = rot;
    }
} 

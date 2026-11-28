using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using UnityEngine.UIElements.Experimental;

public class EquipmentRotation : MonoBehaviour
{
    [SerializeField] private float deadzoneAmount;
    private float tempAngle;
    public float Angle => tempAngle;
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
    public void Rotate()
    {
        pointVector = pointAction.ReadValue<Vector2>();
        pointVector = ScreenToWorld(pointVector, mainCamera);
        pointVector = pointVector - new Vector2(transform.position.x, transform.position.y);

        float angle;
        if (pointVector.x <= 0)
        {
            angle = Vector2.Angle(Vector2.up, pointVector);
        }
        else
        {
            angle = -Vector2.Angle(Vector2.up, pointVector);
        }
        if (pointVector.x < (0f - deadzoneAmount))
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
        else if (pointVector.x > (0f + deadzoneAmount))
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        Vector3 rotation;
        rotation = new Vector3(0f, 0f, angle);
        Quaternion rot;
        rot = Quaternion.Euler(rotation);
        transform.rotation = rot;
        tempAngle = angle;
    }
    private Vector3 ScreenToWorld(Vector2 screenCoordinates, Camera cam)
    {
        Vector2 normVect;
        Vector2 minValue;
        Vector2 screenRect;
        Vector2 world2;
        Vector3 world3;
        minValue.x = cam.rect.x * Screen.width;
        minValue.y = cam.rect.y * Screen.height;
        screenRect.x = cam.rect.width;
        screenRect.y = cam.rect.height;
        normVect.x = (screenCoordinates.x - minValue.x) / (screenRect.x * Screen.width);
        normVect.y = (screenCoordinates.y - minValue.y) / (screenRect.y * Screen.height);
        normVect.x -= 0.5f;
        normVect.y -= 0.5f;
        world2.y = (normVect.y * 2f * cam.orthographicSize) + cam.transform.position.y;
        world2.x = (normVect.x * cam.orthographicSize * 2f * ((float)Screen.width / (float)Screen.height)) + cam.transform.position.x;
        world3 = world2;
        world3.z = 0f;
        return world3;
    }
} 

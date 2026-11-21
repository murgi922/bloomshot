using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private EquipmentRotation eqRot;
    private void LateUpdate()
    {
        transform.position = player.position;
        eqRot.Rotate();
    }
}

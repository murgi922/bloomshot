using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform player;
    private void Update()
    {
        transform.position = player.position;
    }
}

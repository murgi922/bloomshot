using UnityEngine;

public class SpritePixelMovement : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float PPU = 16f;
    private Vector3 pixelPosition = Vector3.zero;
    private void Update()
    {
        pixelPosition.x = Mathf.Round(player.position.x * PPU) / PPU;
        pixelPosition.y = Mathf.Round(player.position.y * PPU) / PPU;
        transform.position = pixelPosition;
    }
}

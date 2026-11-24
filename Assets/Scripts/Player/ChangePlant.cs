using UnityEngine;
using UnityEngine.Tilemaps;

public class ChangePlant : MonoBehaviour
{
    public static ChangePlant Instance;
    [SerializeField] private TileBase[] plants;
    private void Awake()
    {
        Instance = this;
    }
    public void InteractActionPressed()
    {

    }
}

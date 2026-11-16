using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class ChangeGround : MonoBehaviour
{
    [SerializeField] private bool willMow;
    [SerializeField] private Tilemap groundTilemap;
    [SerializeField] private TileBase tilledSoil;
    [SerializeField] private TileBase waterSoil;
    [SerializeField] private float fadeTime;
    private TileBase baseTile;
    private InputAction interactAction;
    private Vector3Int gridPos;
    private void Start()
    {
        interactAction = InputSystem.actions.FindAction("Interact");
        if (interactAction != null) interactAction.Enable();
    }
    public void UpdateTilePos(Vector3Int gridPosIn)
    {
        gridPos = gridPosIn;
        Debug.Log("new Pos = " +  gridPos);
    }
}

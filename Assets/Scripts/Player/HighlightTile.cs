using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Tilemaps;

public class HighlightTile : MonoBehaviour
{
    [Header("Highlight Tile")]
    [SerializeField] private float maxDistance;
    [SerializeField] private bool willHighlight;
    [SerializeField] private InputAction pointAction;
    [SerializeField] private Tilemap tilemapHighlight;
    [SerializeField] private TileBase solidTile;
    [SerializeField] private TileBase frameTile;
    private Camera mainCamera;
    private Vector2 pointVector;
    private Vector3 worldPos;
    private Vector3Int prevGridPos;
    public Vector3Int PrevGridPos
    {
        get => prevGridPos;
        set
        {
            if (prevGridPos != value && willHighlight)
            {
                tilemapHighlight.SetTile(PrevGridPos, null);
                prevGridPos = value;
                tilemapHighlight.SetTile(PrevGridPos, frameTile);
                baseTilemap.GetComponent<ChangeGround>().UpdateTilePos(PrevGridPos);
            }
        }
    }
    [Header("Base Tile")]
    [SerializeField] private Tilemap baseTilemap;
    private void Start()
    {
        mainCamera = Camera.main;
        pointAction = InputSystem.actions.FindAction("Point");
        if (pointAction != null ) pointAction.Enable();
    }
    private void Update()
    {
        pointVector = pointAction.ReadValue<Vector2>();
        worldPos = mainCamera.ScreenToWorldPoint(new Vector3(pointVector.x, pointVector.y, -mainCamera.transform.position.z));
        Vector2 cameraPos = new Vector2(mainCamera.transform.position.x, mainCamera.transform.position.y);
        Vector2 worldPosTemp = new Vector2(worldPos.x, worldPos.y);
        if (Vector2.Distance(cameraPos, worldPosTemp) > maxDistance)
        {
            Vector2 temp = worldPosTemp - cameraPos;
            Vector2 temp1 = temp.normalized * maxDistance;
            temp = temp1 + cameraPos;
            worldPos = new Vector3(temp.x, temp.y, -mainCamera.transform.position.z);
        }
        PrevGridPos = tilemapHighlight.WorldToCell(worldPos);
    }
}

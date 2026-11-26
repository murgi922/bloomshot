using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Tilemaps;

public class HighlightTile : MonoBehaviour
{
    public static HighlightTile Instance;

    [Header("Highlight Tile")]
    private bool willHighlight;
    private InputAction pointAction;
    private Tilemap tilemapHighlight;
    private TileBase frameTile;
    private TileManager tileManager;
    private Color mainColor;
    public Color MainColor
    {
        get => mainColor;
        set
        {
            if (value != mainColor)
            {
                mainColor = value;
                tilemapHighlight.color = value;
            }
        }
    }
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

        tileManager = GetComponentInParent<TileManager>();
        if (tileManager == null) Debug.LogError("TileManager component could not be found by HighlightTile!");

        MainColor = tileManager.HighlightColor;
        frameTile = tileManager.BorderHighlight;

    }
    private void Awake()
    {
        Instance = this;
        tilemapHighlight = GetComponent<Tilemap>();
    }
    private void OnEnable()
    {
        EnableHighlight();
    }
    private void OnDisable()
    {
        DisableHighlight();
    }
    private void Update()
    {
        pointVector = pointAction.ReadValue<Vector2>();
        worldPos = mainCamera.ScreenToWorldPoint(new Vector3(pointVector.x, pointVector.y, -mainCamera.transform.position.z));
        Vector2 cameraPos = new Vector2(mainCamera.transform.position.x, mainCamera.transform.position.y);
        Vector2 worldPosTemp = new Vector2(worldPos.x, worldPos.y);
        if (Vector2.Distance(cameraPos, worldPosTemp) > tileManager.MaxDistance)
        {
            Vector2 temp = worldPosTemp - cameraPos;
            Vector2 temp1 = temp.normalized * tileManager.MaxDistance;
            temp = temp1 + cameraPos;
            worldPos = new Vector3(temp.x, temp.y, -mainCamera.transform.position.z);
        }
        PrevGridPos = tilemapHighlight.WorldToCell(worldPos);
    }
    public void DisableHighlight()
    {
        willHighlight = false;
        if (tilemapHighlight != null)
        {
            tilemapHighlight.ClearAllTiles();
        }
    }
    public void EnableHighlight()
    {
        willHighlight = true;
    }
    public void SetErrorColor()
    {
        MainColor = TileManager.Instance.RedColor;
    }
    public void ResetColor()
    {
        MainColor = TileManager.Instance.HighlightColor;
    }
}

using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class ChangeGround : MonoBehaviour
{
    public enum SoilState
    {
        WaterSoil,
        TilledSoil
    }
    public class TileData
    {
        public float tileTimer;
        public SoilState soilState;
        public TileBase tileBase;
    }
    Dictionary<Vector3Int, TileData> tileInfo = new Dictionary<Vector3Int, TileData>();
    [SerializeField] private bool willMow;
    [SerializeField] private bool willTill;
    [SerializeField] private Tilemap groundTilemap;
    [SerializeField] private TileBase tilledSoil;
    [SerializeField] private TileBase waterSoil;
    [SerializeField] private TileBase grassTile;
    [SerializeField] private float fadeTime;
    [SerializeField] private Tilemap baseTilemap;
    private InputAction interactAction;
    private Vector3Int gridPos;
    private void Start()
    {
        interactAction = InputSystem.actions.FindAction("Interact");
        if (interactAction != null) interactAction.Enable();
        else Debug.LogError("Could not find interact action!");
    }
    public void UpdateTilePos(Vector3Int gridPosIn)
    {
        gridPos = gridPosIn;
        Debug.Log("new Pos = " +  gridPos);
    }
    private void Update()
    {
        if (interactAction != null && interactAction.WasPressedThisFrame() && willMow && baseTilemap.GetTile(gridPos) == grassTile)
        {
            TileData tile = new TileData();
            tile.tileTimer = fadeTime;
            if (tileInfo.ContainsKey(gridPos))
            {
                if (tileInfo[gridPos].soilState == SoilState.WaterSoil)
                {
                    if (willTill)
                    {
                        tile.soilState = SoilState.TilledSoil;
                        tile.tileBase = tilledSoil;
                    }
                    else return;
                }
                else if (tileInfo[gridPos].soilState == SoilState.TilledSoil)
                {
                    return;
                }
            }
            else
            {
                tile.soilState = SoilState.WaterSoil;
                tile.tileBase = waterSoil;
            }
            AddToDict(gridPos, tile);
            tileInfo.TryGetValue(gridPos, out tile);
            groundTilemap.SetTile(gridPos, tile.tileBase);
        }
        foreach(KeyValuePair<Vector3Int, TileData> dictinfo in tileInfo)
        {
            Vector3Int coord = dictinfo.Key;
            TileData data = dictinfo.Value;
            data.tileTimer -= Time.deltaTime;
            if (data.tileTimer <= 0f)
            {
                data.tileTimer = fadeTime;
                if (data.soilState == SoilState.TilledSoil)
                {
                    data.tileBase = waterSoil;
                    data.soilState = SoilState.WaterSoil;
                    groundTilemap.SetTile(coord, waterSoil);
                }
                else if (data.soilState == SoilState.WaterSoil)
                {
                    groundTilemap.SetTile(coord, null);
                    tileInfo.Remove(coord);
                }
            }
        }
    }
    private void AddToDict(Vector3Int coord, TileData data)
    {
        tileInfo[coord] = data;
    }
}

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class ChangeGround : MonoBehaviour
{
    public static ChangeGround Instance;
    private TileManager tileManager;
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
    List<Vector3Int> toRemove = new List<Vector3Int>(); 
    [SerializeField] private bool willMow;
    [SerializeField] private bool willTill;
    [SerializeField] private Tilemap groundTilemap;
    [SerializeField] private TileBase tilledSoil;
    [SerializeField] private TileBase waterSoil;
    [SerializeField] private TileBase grassTile;
    [SerializeField] private float minFadeTime;
    [SerializeField] private float maxFadeTime;
    private float fadeTime;
    [SerializeField] private Tilemap baseTilemap;
    private void Start()
    {
        tileManager = GetComponentInParent<TileManager>();
        if (tileManager == null) Debug.LogError("Tile manager component could not be found!");
    }
    private void Awake()
    {
        Instance = this;
    }
    private void Update()
    {
        fadeTime = GetRandomValue(minFadeTime, maxFadeTime);
        TileFade();
    }
    private void AddToDict(Vector3Int coord, TileData data)
    {
        tileInfo[coord] = data;
    }

    private void TileSet(bool willWaternotTill)
    {
        if (willMow && baseTilemap.GetTile(HighlightTile.Instance.PrevGridPos) == grassTile)
        {
            TileData tile = new TileData();
            tile.tileTimer = fadeTime;
            if (tileInfo.ContainsKey(HighlightTile.Instance.PrevGridPos))
            {
                if (tileInfo[HighlightTile.Instance.PrevGridPos].soilState == SoilState.WaterSoil)
                {
                    if (!willWaternotTill)
                    {
                        tile.soilState = SoilState.TilledSoil;
                        tile.tileBase = tilledSoil;
                        SpawnTillSoilFX();
                    }
                    else if (willWaternotTill)
                    {
                        tile.soilState = SoilState.WaterSoil;
                        tile.tileBase = waterSoil;
                        SpawnWaterSoilFX();
                    }
                }
                else if (tileInfo[HighlightTile.Instance.PrevGridPos].soilState == SoilState.TilledSoil)
                {
                    return;
                }
            }
            else if (willWaternotTill)
            {
                tile.soilState = SoilState.WaterSoil;
                tile.tileBase = waterSoil;
                SpawnWaterSoilFX();
            }
            else return;
            AddToDict(HighlightTile.Instance.PrevGridPos, tile);
            groundTilemap.SetTile(HighlightTile.Instance.PrevGridPos, tile.tileBase);
        }
    }
    private void TileFade()
    {
        foreach (KeyValuePair<Vector3Int, TileData> dictinfo in tileInfo)
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
                    toRemove.Add(coord);
                }
            }
        }
        foreach(Vector3Int cache in toRemove)
        {
            tileInfo.Remove(cache);
        }
        toRemove.Clear();
    }
    private float GetRandomValue(float min, float max)
    {
        return Random.Range(min, max);
    }
    public void InteractActionPressed(bool willWater)
    {
        TileSet(willWater);
    }
    private void SpawnWaterSoilFX()
    {

    }
    private void SpawnTillSoilFX()
    {

    }
    private void OnValidate()
    {
        if (minFadeTime > maxFadeTime)
        {
            maxFadeTime = minFadeTime;
        }
    }
}

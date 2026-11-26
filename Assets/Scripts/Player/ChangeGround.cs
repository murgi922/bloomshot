using System.Collections.Generic;
using UnityEngine;
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
        public bool hasPlant;
    }
    Dictionary<Vector3Int, TileData> tileInfo = new Dictionary<Vector3Int, TileData>();
    List<Vector3Int> toRemove = new List<Vector3Int>(); 
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
        if (baseTilemap.GetTile(HighlightTile.Instance.PrevGridPos) == grassTile)
        {
            HighlightTile.Instance.ResetColor();
        }
        else
        {
            HighlightTile.Instance.SetErrorColor();
        }
    }
    private void OnDisable()
    {
        HighlightTile.Instance.ResetColor();
    }
    private void AddToDict(Vector3Int coord, TileData data)
    {
        tileInfo[coord] = data;
    }

    private void TileSet(bool willWaternotTill)
    {
        if (baseTilemap.GetTile(HighlightTile.Instance.PrevGridPos) == grassTile)
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
                        tile.hasPlant = false;
                        SpawnTillSoilFX();
                    }
                    else if (willWaternotTill)
                    {
                        tile.soilState = SoilState.WaterSoil;
                        tile.tileBase = waterSoil;
                        tile.hasPlant = false;
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
                tile.hasPlant = false;
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
            if (data.tileTimer <= 0f && !data.hasPlant)
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
    private void SpawnWaterSoilFX()
    {

    }
    private void SpawnTillSoilFX()
    {

    }
    #region Public func
    public void InteractActionPressed(bool willWater)
    {
        if (tileInfo.ContainsKey(HighlightTile.Instance.PrevGridPos))
        {
            if (tileInfo[HighlightTile.Instance.PrevGridPos].hasPlant)
            {
                return;
            }
        }
        TileSet(willWater);
    }
    public TileData GetTileData(Vector3Int gridPos)
    {
        if (tileInfo.ContainsKey(gridPos))
        {
            return(tileInfo[gridPos]);
        }
        return null;
    }
    public void AddPlantToTile(Vector3Int pos)
    {
        if (toRemove.Contains(pos)) toRemove.Remove(pos);
        tileInfo[pos].hasPlant = true;
        tileInfo[pos].soilState = SoilState.WaterSoil;
        tileInfo[pos].tileBase = waterSoil;
        tileInfo[pos].tileTimer = fadeTime;
        groundTilemap.SetTile(pos, waterSoil);
    }
    public void RemovePlant(Vector3Int pos)
    {
        tileInfo.Remove(pos);
        groundTilemap.SetTile(pos, null);
    }
    /*public bool IsContainTile(Vector3Int pos, SoilState soilState)
    {
        if (tileInfo.ContainsKey(pos))
        {
            if (tileInfo[pos].soilState == soilState)
            {
                return true;
            }
            else return false;
        }
        else return false;
    }*/
    #endregion
    private void OnValidate()
    {
        if (minFadeTime > maxFadeTime)
        {
            maxFadeTime = minFadeTime;
        }
    }
}

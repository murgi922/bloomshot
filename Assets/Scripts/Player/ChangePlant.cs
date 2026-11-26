using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using static ChangeGround;

public class ChangePlant : MonoBehaviour
{
    public static ChangePlant Instance;
    [SerializeField] private TileBase[] plants;
    [SerializeField] private float medianFadeTime;
    [SerializeField] private float randomRange;
    private float fadeTime;
    private Tilemap plantTileMap;
    private ChangeGround.TileData soilTileData;
    public class PlantTileData
    {
        public int plantIndex;
        public float tileTimer;
        public TileBase tileBase;

    }
    Dictionary<Vector3Int, PlantTileData> tileInfo = new Dictionary<Vector3Int, PlantTileData>();

    private void Awake()
    {
        Instance = this;
        plantTileMap = GetComponent<Tilemap>();
    }
    private void Update()
    {
        fadeTime = GetRandomValue(medianFadeTime - randomRange, medianFadeTime + randomRange);
        TileFade();
    }
    public void InteractActionPressed()
    {
        TileSet();
        RemovePlant();
    }
    private void TileSet()
    {
        soilTileData = ChangeGround.Instance.GetTileData(HighlightTile.Instance.PrevGridPos);
        if (soilTileData != null && soilTileData.soilState == ChangeGround.SoilState.TilledSoil)
        {
            if (!soilTileData.hasPlant)
            {
                PlantTileData plantTileData = new PlantTileData();
                plantTileMap.SetTile(HighlightTile.Instance.PrevGridPos, plants[0]);
                ChangeGround.Instance.AddPlantToTile(HighlightTile.Instance.PrevGridPos);

                plantTileData.plantIndex = 0;
                plantTileData.tileTimer = fadeTime;
                plantTileData.tileBase = plants[0];
                tileInfo[HighlightTile.Instance.PrevGridPos] = plantTileData;

                AddPlantFX(HighlightTile.Instance.PrevGridPos);
            }
        }
    }
    private void TileFade()
    {
        foreach (KeyValuePair<Vector3Int, PlantTileData> dictinfo in tileInfo)
        {
            Vector3Int coord = dictinfo.Key;
            PlantTileData data = dictinfo.Value;
            data.tileTimer -= Time.deltaTime;
            if (data.tileTimer <= 0f)
            {
                data.tileTimer = fadeTime;
                if (data.plantIndex >= 0 && data.plantIndex < plants.Length - 1)
                {
                    data.tileBase = plants[data.plantIndex++];
                    plantTileMap.SetTile(coord, plants[data.plantIndex]);
                    AddGrowthFX(coord);
                }
                else
                {
                    return;
                }
            }
        }
    }
    private void RemovePlant()
    {
        if(tileInfo.ContainsKey(HighlightTile.Instance.PrevGridPos))
        {
            if(tileInfo[HighlightTile.Instance.PrevGridPos].plantIndex == plants.Length - 1)
            {
                tileInfo.Remove(HighlightTile.Instance.PrevGridPos);
                plantTileMap.SetTile(HighlightTile.Instance.PrevGridPos, null);
                ChangeGround.Instance.RemovePlant(HighlightTile.Instance.PrevGridPos);
                AddDestroyFX(HighlightTile.Instance.PrevGridPos);
            }    
        }
    }
    private float GetRandomValue(float min, float max)
    {
        return Random.Range(min, max);   
    }
    private void AddPlantFX(Vector3Int pos)
    {

    }
    private void AddGrowthFX(Vector3Int pos)
    {

    }
    private void AddDestroyFX(Vector3Int pos)
    {

    }
    private void OnValidate()
    {
        if (medianFadeTime < randomRange) medianFadeTime = randomRange;
    }
}

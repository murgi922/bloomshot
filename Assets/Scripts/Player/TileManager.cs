using System;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileManager : MonoBehaviour
{
    public static TileManager Instance;

    #region HighlightTile
    [Header("Highlight Tile")]
    [SerializeField] private float maxDistance;
    [SerializeField] private TileBase borderHighlight;
    [SerializeField] private Color highlightColor;
    [SerializeField] private Color redColor;
    public Color RedColor => redColor;
    public float MaxDistance => maxDistance;
    public TileBase BorderHighlight => borderHighlight;
    public Color HighlightColor => highlightColor;
    private HighlightTile highlightTile;
    private bool isHighlight;
    public bool IsHighlight
    {
        get => isHighlight;
        set
        {
            if (value != isHighlight)
            {
                isHighlight = value;
                if(!value)
                {
                    highlightTile.enabled = false;
                }
                else
                {
                    highlightTile.enabled = true;
                }
            }
        }
    }
    #endregion
    #region Built in functions
    private void Awake()
    {
        highlightTile = GetComponentInChildren<HighlightTile>();
        if (highlightTile == null) Debug.LogError("Highlight tile could not be found by TileManager");
        Instance = this;
    }
    private void Update()
    {
        if (PlayerManager.Instance.Equipment == PlayerManager.CurrentEquipment.Hoe ||
            PlayerManager.Instance.Equipment == PlayerManager.CurrentEquipment.WateringCan ||
            PlayerManager.Instance.Equipment == PlayerManager.CurrentEquipment.Seed)
        {
            IsHighlight = true;
        }
        else IsHighlight = false;
    }
    #endregion
    #region Custom Functions
    public void InteractActionPressed()
    {
        if (PlayerManager.Instance.Equipment == PlayerManager.CurrentEquipment.WateringCan)
        {
            ChangeGround.Instance.InteractActionPressed(true);
        }
        else if (PlayerManager.Instance.Equipment == PlayerManager.CurrentEquipment.Hoe)
        {
            ChangeGround.Instance.InteractActionPressed(false);
        }
        else if (PlayerManager.Instance.Equipment == PlayerManager.CurrentEquipment.Seed)
        {
            ChangePlant.Instance.InteractActionPressed();
        }
    }
    #endregion
}

using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Equipment", menuName = "Scriptable Objects/Equipment")]
public class Equipment : ScriptableObject
{
    public string equipName;
    [TextArea]
    public string equipDescription;
    public enum EquipmentType
    {
        Weapon,
        Tool
    }
    public GameObject prefab;
}

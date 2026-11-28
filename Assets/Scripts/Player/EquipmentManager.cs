using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    #region Variables
    [SerializeField] private Transform weaponsParent;
    [SerializeField] private Transform harvestToolsParent;
    private List<EquipmentData> equipments = new List<EquipmentData>();
    private Equipment currentEquipment;
    private void Awake()
    {
        foreach(EquipmentData data in weaponsParent.GetComponentsInChildren<EquipmentData>(true))
        {
            equipments.Add(data);
        }
        foreach (EquipmentData data in harvestToolsParent.GetComponentsInChildren<EquipmentData>(true))
        {
            equipments.Add(data);
        }
    }
    public void CycleEquipment()
    {
        foreach(EquipmentData data in equipments)
        {

        }
    }
    public void Equip(Equipment toEquip)
    {
        foreach(EquipmentData data in equipments)
        {
            if (toEquip == data.Data)
            {
                data.gameObject.SetActive(true);
            }
            else data.gameObject.SetActive(false);
        }
    }
    #endregion
}

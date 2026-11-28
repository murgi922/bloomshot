using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    public static EquipmentManager Instance;
    #region Variables
    [SerializeField] private Transform weaponsParent;
    [SerializeField] private Transform harvestToolsParent;
    private List<EquipmentData> equipments = new List<EquipmentData>();
    private EquipmentData currentEquipment;
    private void Awake()
    {
        Instance = this;
        foreach(EquipmentData data in weaponsParent.GetComponentsInChildren<EquipmentData>(true))
        {
            equipments.Add(data);
        }
        foreach (EquipmentData data in harvestToolsParent.GetComponentsInChildren<EquipmentData>(true))
        {
            equipments.Add(data);
        }
        foreach(EquipmentData data in equipments)
        {
            if (data.gameObject.activeSelf)
            {
                currentEquipment = data;
                Equip(currentEquipment.Data);
            }
        }
    }
    public void CycleEquipment()
    {
        for(int i = 0; i < equipments.Count; i++)
        {
            int ind;
            if (currentEquipment != equipments[i])
            {
                continue;
            }
            else
            {
                ind = i;
            }
            if (ind == equipments.Count - 1)
            {
                Equip(equipments[0].Data);
            }
            else
            {
                Equip(equipments[ind + 1].Data);
            }
            break;
        }
    }
    public void Equip(Equipment toEquip)
    {
        foreach(EquipmentData data in equipments)
        {
            if (toEquip == data.Data)
            {
                currentEquipment = data;
                data.gameObject.SetActive(true);
            }
            else data.gameObject.SetActive(false);
        }
    }
    #endregion
}

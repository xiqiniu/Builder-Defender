using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="ScriptableObjects/BuildTypeList")]
public class BuildingTypeListSO : ScriptableObject
{
    public List<BuildingTypeSO> list;
}

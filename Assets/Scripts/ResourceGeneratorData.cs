using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ResourceGeneratorData
{
    public float timerMax;
    public List<ResourceTypeSO> resourceTypeList;
    public float resourceDecectionRadius;
    public int maxResourceAmount;
}

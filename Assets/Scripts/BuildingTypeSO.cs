using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/BuildingType")]
public class BuildingTypeSO : ScriptableObject
{
    public string nameString;
    public Transform prefab;
    public ResourceGeneratorData resourceGeneratorData;
    public Sprite sprite;
    public float minConstructionRadius;
    public ResourceAmount[] constructionResourceCostArray;
    public int healthAmountMax;
    public bool hasResourceGeneratorData = true;
    public float constructionTimerMax;
    public string GetConstructionResourceCostString()
    {
        string str = "";
        foreach(ResourceAmount resourceAmount in constructionResourceCostArray)
        {
            str +="<color=#"+resourceAmount.resourceType.colorHex+">"
                +resourceAmount.resourceType.nameString+": " + resourceAmount.amount+"</color> ";
        }
        return str;
    }
    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ResourceNearbyOverlay : MonoBehaviour
{
    private ResourceGeneratorData resourceGeneratorData;
    private void Awake()
    {
        Hide();
    }
    private void Update()
    {
        int nearbyResourceAmount = ResourceGenerator.GetNearbyResourceAmount(resourceGeneratorData, transform.position-transform.localPosition);
        float percent = Mathf.Round((float)nearbyResourceAmount / resourceGeneratorData.maxResourceAmount*100);
        transform.Find("Text").GetComponent<TextMeshPro>().SetText(percent + "%");
    }
    public void Show(ResourceGeneratorData resourceGeneratorData)
    {
        this.resourceGeneratorData = resourceGeneratorData;
        gameObject.SetActive(true);
        transform.Find("Icon").GetComponent<SpriteRenderer>().sprite=resourceGeneratorData.resourceTypeList[0].sprite;
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }
}

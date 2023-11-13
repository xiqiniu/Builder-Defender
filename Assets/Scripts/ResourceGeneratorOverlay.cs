using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ResourceGeneratorOverlay : MonoBehaviour
{
    [SerializeField] private ResourceGenerator resourceGenerator;
    private Transform barTransform;
    private void Start()
    {
        ResourceGeneratorData resourceGeneratorData=resourceGenerator.GetResourceGeneratorData();
        barTransform = transform.Find("Bar");
        transform.Find("Icon").GetComponent<SpriteRenderer>().sprite=resourceGeneratorData.resourceTypeList[0].sprite;
        transform.Find("Text").GetComponent<TextMeshPro>().SetText(resourceGenerator.GetAmountGeneratedPerSecond().ToString("F1"));
    }

    private void Update()
    {
        //当父类scale.x为1.4时,进度条刚好充满
        barTransform.localScale = new Vector3(1.4f*(1f - resourceGenerator.GetTimerNormalized()), 1, 1);
    }
}

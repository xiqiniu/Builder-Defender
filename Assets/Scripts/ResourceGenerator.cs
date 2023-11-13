using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceGenerator : MonoBehaviour
{
    public static int GetNearbyResourceAmount(ResourceGeneratorData resourceGeneratorData
        , Vector3 position)
    {
        int resourceAmount = 0;
        Collider2D[] collider2DArray = Physics2D.OverlapCircleAll(position, resourceGeneratorData.resourceDecectionRadius);
        foreach (Collider2D collider2D in collider2DArray)
        {
            ResourceNode resourceNode = collider2D.GetComponent<ResourceNode>();
            if (resourceNode != null)
            {
                if (resourceNode.resourceType == resourceGeneratorData.resourceTypeList[0])
                    resourceAmount++;
            }
        }
        return resourceAmount;
    }
    private ResourceGeneratorData resourceGeneratorData;
    private float timer;
    private float timerMax;
    private void Awake()
    {
        resourceGeneratorData = GetComponent<BuildingTypeHolder>().buildingType.resourceGeneratorData;       
        timerMax = resourceGeneratorData.timerMax;     
    }
    private void Start()
    {
        int nearbyResourceAmount = GetNearbyResourceAmount(resourceGeneratorData, transform.position);
        if (nearbyResourceAmount == 0)
        {
            timerMax = 0f;
            enabled = false;
        }
        else
        {
            //����Դ���ﵽ�����Դ��--��Դ����ʱ��Ϊ���ʱ���1/2
            //����Դ��Ϊ1--��Դ����ʱ��ӽ����ʱ��
            timerMax = resourceGeneratorData.timerMax /2f +
                (float)(1 - (float)nearbyResourceAmount / resourceGeneratorData.maxResourceAmount) * resourceGeneratorData.timerMax ;
        }
    }
    private void Update()
    {
        timer -= Time.deltaTime;
        if(timer<0f)
        {
            timer += timerMax;
            foreach (ResourceTypeSO resourceType in resourceGeneratorData.resourceTypeList)
            {
                ResourceManager.Instance.AddResource(resourceType, 1);
            }      
        }
    }
    public ResourceGeneratorData GetResourceGeneratorData()
    {
        return resourceGeneratorData;
    }
    public float GetTimerNormalized()
    {
        if(timerMax!=0)
        return timer / timerMax;
        return 1f;
    }
    public float GetAmountGeneratedPerSecond()
    {
        if (timerMax == 0f) return 0;
        return 1 / timerMax;
    }
}

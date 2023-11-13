using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance { get; private set; }

    //�趨��ʼ��Դ
    [SerializeField] private List<ResourceAmount> StartResources;
    //event
    public event EventHandler OnResourceAmountChanged;
    private Dictionary<ResourceTypeSO, int> resourceAmountDictionary;
    private void Awake()
    {
        Instance = this;
        resourceAmountDictionary = new Dictionary<ResourceTypeSO, int>();
        ResourceTypeListSO resourceTypeList = Resources.Load<ResourceTypeListSO>(typeof(ResourceTypeListSO).Name);
        foreach (ResourceTypeSO resourceType in resourceTypeList.list)
        {
            resourceAmountDictionary[resourceType] = 0;
        }
        //��ʼ��Դ����
         foreach(ResourceAmount resourceAmount in StartResources)
        {
            AddResource(resourceAmount.resourceType, resourceAmount.amount);
        }
    }

    private void TestLogResourceAmountDictionary()
    {
        foreach (ResourceTypeSO resourceType in resourceAmountDictionary.Keys)
        {
            print(resourceType.nameString + ":" + resourceAmountDictionary[resourceType]);
        }
    }

    public void AddResource(ResourceTypeSO resourceType, int amount)
    {
        resourceAmountDictionary[resourceType] += amount;
        //TestLogResourceAmountDictionary();
        OnResourceAmountChanged?.Invoke(this, EventArgs.Empty);
    }
    public int GetResourceAmount(ResourceTypeSO resourceType)
    {
        return resourceAmountDictionary[resourceType];
    }

    //�ж���Դ�Ƿ����Խ��콨��
    public bool CanAfford(ResourceAmount[] resourceAmountArray)
    {
        foreach (ResourceAmount resourceAmount in resourceAmountArray)
        {
            if (GetResourceAmount(resourceAmount.resourceType) >= resourceAmount.amount)
            {
                //can afford 
            }
            else
            {
                //cannot afford
                return false;
            }
        }
        return true;
    }

    //���콨��������Դ
    public void SpendResource(ResourceAmount[] resourceAmountArray)
    {
        foreach (ResourceAmount resourceAmount in resourceAmountArray)
        {
            resourceAmountDictionary[resourceAmount.resourceType] -= resourceAmount.amount;
        }
        
    }
}

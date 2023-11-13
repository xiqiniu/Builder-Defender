using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ResourcesUI : MonoBehaviour
{
    private ResourceTypeListSO resourceTypeList;
    private Dictionary<ResourceTypeSO, Transform> resourceTypeTransformDictionary;
    //与Find的作用一样,但需要在编辑器中设置
    //[SerializeField] private Transform resourceTemplate;
    private void Awake()
    {
        resourceTypeList = Resources.Load<ResourceTypeListSO>(typeof(ResourceTypeListSO).Name);
        resourceTypeTransformDictionary = new Dictionary<ResourceTypeSO, Transform>();
        Transform resourceTemplate = transform.Find("ResourcesTemplate");
        resourceTemplate.gameObject.SetActive(false);
        int index = 0;
        foreach(ResourceTypeSO resourceType in resourceTypeList.list)
        {
            
            Transform resourceTransform=Instantiate(resourceTemplate, transform);
            resourceTypeTransformDictionary[resourceType] = resourceTransform;
            resourceTransform.gameObject.SetActive(true);
            
            float offsetAmount = -160f;
            resourceTransform.GetComponent<RectTransform>().anchoredPosition = new Vector2(offsetAmount*index, 0);
            resourceTransform.Find("Image").GetComponent<Image>().sprite = resourceType.sprite;
            index++;
        }
    }

    private void Start()
    {
        UpdateResourceAmount();
        ResourceManager.Instance.OnResourceAmountChanged += ResourceManager_OnResourceAmountChanged;
    }

    private void ResourceManager_OnResourceAmountChanged(object sender, System.EventArgs e)
    {
        UpdateResourceAmount();
    }

    //将访问外部引用的代码从awake分离出来
    private void UpdateResourceAmount()
    {
        foreach (ResourceTypeSO resourceType in resourceTypeList.list)
        {
            int resourceAmount = ResourceManager.Instance.GetResourceAmount(resourceType);
            Transform resourceTransform = resourceTypeTransformDictionary[resourceType];
            resourceTransform.Find("Text").GetComponent<TextMeshProUGUI>().SetText(resourceAmount.ToString());
        }
    }
}

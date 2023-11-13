using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class BuildTypeSelectUI : MonoBehaviour
{
    [SerializeField] private List<BuildingTypeSO> ignoredBuildingTypeList;
    [SerializeField] private Sprite arrowSprite;
    private Dictionary<BuildingTypeSO, Transform> btnTransformDictionary;
    private Transform arrowBtn;
    private void Awake()
    {
        btnTransformDictionary = new Dictionary<BuildingTypeSO, Transform>();
        Transform btnTemplate = transform.Find("BtnTemplate");
        btnTemplate.gameObject.SetActive(false);
        BuildingTypeListSO buildingTypeList = Resources.Load<BuildingTypeListSO>(typeof(BuildingTypeListSO).Name);
        
        int index = 0;
        arrowBtn = Instantiate(btnTemplate, transform);
        arrowBtn.gameObject.SetActive(true);
        float offsetAmount = 130f;
        arrowBtn.GetComponent<RectTransform>().anchoredPosition = new Vector2(offsetAmount * index, 0);
        arrowBtn.Find("Image").GetComponent<Image>().sprite = arrowSprite;
        //…Ë÷√º˝Õ∑Õº±Í
        //arrowBtn.Find("Image").GetComponent<RectTransform>().sizeDelta = new Vector2(0, -30);
        arrowBtn.GetComponent<Button>().onClick.AddListener(() =>
        {
            BuildingManager.Instance.setActiveBuildingType(null);
        });
        MouseEnterExitEvents mouseEnterExitEvents = arrowBtn.GetComponent<MouseEnterExitEvents>();
        mouseEnterExitEvents.OnMouseEnter += (object sender, EventArgs e) =>
        {
            TooltipUI.Instance.Show("arrow");
        };
        mouseEnterExitEvents.OnMouseExit += (object sender, EventArgs e) =>
        {
            TooltipUI.Instance.Hide();
        };
        index++;
        foreach (BuildingTypeSO buildingType in buildingTypeList.list)
        {
            if (ignoredBuildingTypeList.Contains(buildingType)) continue;
            Transform btnTransform = Instantiate(btnTemplate, transform);
            btnTransform.gameObject.SetActive(true);
            offsetAmount = 130f;
            btnTransform.GetComponent<RectTransform>().anchoredPosition = new Vector2(offsetAmount * index, 0);
            btnTransform.Find("Image").GetComponent<Image>().sprite = buildingType.sprite;
            btnTransform.GetComponent<Button>().onClick.AddListener(()=>
            {
                BuildingManager.Instance.setActiveBuildingType(buildingType);
            });
            mouseEnterExitEvents = btnTransform.GetComponent<MouseEnterExitEvents>();
            mouseEnterExitEvents.OnMouseEnter += (object sender, EventArgs e) =>
              {
                  TooltipUI.Instance.Show(buildingType.name+"\n"+buildingType.GetConstructionResourceCostString());
              };
            mouseEnterExitEvents.OnMouseExit += (object sender, EventArgs e) =>
            {
                TooltipUI.Instance.Hide();
            };
            btnTransformDictionary[buildingType] = btnTransform;
            index++;
        }
    }
    private void Start()
    {
        BuildingManager.Instance.OnActiveBuildingTypeChanged += BuildingManager_OnActiveBuildingTypeChanged;
        UpdateActiveBuildingType();
    }

    private void BuildingManager_OnActiveBuildingTypeChanged(object sender, BuildingManager.OnActiveBuildingTypeChangedArgs e)
    {
        UpdateActiveBuildingType();
    }

    private void UpdateActiveBuildingType()
    {
        arrowBtn.Find("Selected").gameObject.SetActive(false);
        foreach(BuildingTypeSO buildingType in btnTransformDictionary.Keys)
        {
            Transform btnTransform = btnTransformDictionary[buildingType];
            btnTransform.Find("Selected").gameObject.SetActive(false);
        }
        BuildingTypeSO activeBuildingType = BuildingManager.Instance.GetActiveBuildingType();
        if(activeBuildingType==null)
        {
            arrowBtn.Find("Selected").gameObject.SetActive(true);
        }
        else
        {
            btnTransformDictionary[activeBuildingType].Find("Selected").gameObject.SetActive(true);
        }
    }
}

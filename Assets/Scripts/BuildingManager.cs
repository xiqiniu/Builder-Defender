using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class BuildingManager : MonoBehaviour
{
    public event EventHandler<OnActiveBuildingTypeChangedArgs> OnActiveBuildingTypeChanged;
    //�������뽨���ڸ����н����ĵط�
    private float maxConstructionRadius = 50f;
    public class OnActiveBuildingTypeChangedArgs : EventArgs
    {
        public BuildingTypeSO activeBuildingType;
    }

    [SerializeField] private Building hqBuilding;
    public static BuildingManager Instance { get; private set; }
    private BuildingTypeListSO buildingTypeList;
    private BuildingTypeSO activeBuildingType;

    private Camera mainCamera;

    private void Awake()
    {
        Instance = this;
        //����Ҫ�ⲿ���õķ���Awake
        buildingTypeList = Resources.Load<BuildingTypeListSO>(typeof(BuildingTypeListSO).Name);
    }
    private void Start()
    {
        //��Ҫ�ⲿ���õķ���Start
        //camera��Ҫ�ڳ�����ʼ����Ŵ���
        mainCamera = Camera.main;
        hqBuilding.GetComponent<HealthSystem>().OnDied += BuildingManager_OnDied;
    }

    private void BuildingManager_OnDied(object sender, EventArgs e)
    {
        GameOverUI.Instance.Show();
        SoundManager.Instance.PlaySound(SoundManager.Sound.GameOver);
    }

    private void Update()
    {
         if(Input.GetMouseButtonDown(0)&&!EventSystem.current.IsPointerOverGameObject())
        {
            if(activeBuildingType!=null)
                if(CanSpawnBuilding(activeBuildingType,UtilsClass.GetMouseWorldPosition(),out string errorMessage))
                    if(ResourceManager.Instance.CanAfford(activeBuildingType.constructionResourceCostArray))
                    {
                            ResourceManager.Instance.SpendResource(activeBuildingType.constructionResourceCostArray);
                        //Instantiate(activeBuildingType.prefab, UtilsClass.GetMouseWorldPosition(), Quaternion.identity);
                        BuildingConstruction.Create(UtilsClass.GetMouseWorldPosition(),activeBuildingType);
                        SoundManager.Instance.PlaySound(SoundManager.Sound.BuildingPlaced);
                    }
                    else
                    {
                        TooltipUI.Instance.Show("Cannot afford " + activeBuildingType.GetConstructionResourceCostString(), new TooltipUI.TooltipTimer { timer = 2f });
                    }
                else
                {
                        TooltipUI.Instance.Show(errorMessage,new TooltipUI.TooltipTimer { timer = 2f });
                }
        }
    }


    public void setActiveBuildingType(BuildingTypeSO buildingType)
    {
        activeBuildingType = buildingType;
        OnActiveBuildingTypeChanged?.Invoke(this, new OnActiveBuildingTypeChangedArgs
        {
            activeBuildingType = activeBuildingType
        });
    }

    public BuildingTypeSO GetActiveBuildingType()
    {
        return activeBuildingType;
    }

    private bool CanSpawnBuilding(BuildingTypeSO buildingType,Vector3 mousePosition,out string errorMessage)
    {
        BoxCollider2D boxCollider2D = buildingType.prefab.GetComponent<BoxCollider2D>();
        Collider2D[] collider2DArray = Physics2D.OverlapBoxAll((Vector3)boxCollider2D.offset + mousePosition, boxCollider2D.size,0);
        //����������ײ��
        if (collider2DArray.Length != 0)
        {
            errorMessage = "Area is not Clear!";
            return false;   
        }
        collider2DArray = Physics2D.OverlapCircleAll(mousePosition, buildingType.minConstructionRadius);
        foreach(Collider2D collider2D in collider2DArray)
        {
            BuildingTypeHolder buildingTypeHolder = collider2D.GetComponent<BuildingTypeHolder>();
            if (buildingTypeHolder!=null)
            {
                //���򸽽�������ͬ���ͽ���
                if (buildingTypeHolder.buildingType == buildingType)
                {
                    errorMessage = "Too close to another building of the same type!";
                    return false;
                }
            }
        }

        if(buildingType.hasResourceGeneratorData)
        {
            int nearbyResourceAmount = ResourceGenerator.GetNearbyResourceAmount(buildingType.resourceGeneratorData, mousePosition);
            if (nearbyResourceAmount == 0)
            {
                errorMessage = "There are no nearby Resource Nodes!";
                return false;
            }
        }


        collider2DArray = Physics2D.OverlapCircleAll(mousePosition, maxConstructionRadius);
        foreach (Collider2D collider2D in collider2DArray)
        {
            BuildingTypeHolder buildingTypeHolder = collider2D.GetComponent<BuildingTypeHolder>();
            if (buildingTypeHolder != null)
            {
                //ֻ�����н��������򸽽����콨��
                errorMessage = "";
                return true;
            }
        }
        errorMessage = "Too far from any other building!";
        return false;

    }

    public Building GetHQbuilding()
    {
        return hqBuilding;
    }
}

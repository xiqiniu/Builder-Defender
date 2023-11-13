using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingGhost : MonoBehaviour
{
    private GameObject spriteGameObject;
    private ResourceNearbyOverlay resourceNearbyOverlay;
    private void Awake()
    {
        spriteGameObject = transform.Find("Sprite").gameObject;
        resourceNearbyOverlay = transform.Find("pfResourceNearbyOverlay").GetComponent<ResourceNearbyOverlay>();
        Hide();
    }
    private void Start()
    {
        BuildingManager.Instance.OnActiveBuildingTypeChanged += BuildingManager_OnActiveBuildingTypeChanged;       
    }

    private void BuildingManager_OnActiveBuildingTypeChanged(object sender, BuildingManager.OnActiveBuildingTypeChangedArgs e)
    {
        if(e.activeBuildingType==null)
        {
            Hide();
            resourceNearbyOverlay.Hide();
        }
        else
        {
            Show(e.activeBuildingType.sprite);
            //仅当建筑类型的hasResourceGeneratorData为true时展示
            if(e.activeBuildingType.hasResourceGeneratorData)
            resourceNearbyOverlay.Show(e.activeBuildingType.resourceGeneratorData);
            else
            {
                resourceNearbyOverlay.Hide(); 
            }
        }
    }

    private void Update()
    {
        transform.position = UtilsClass.GetMouseWorldPosition();
    }
    private void Show(Sprite ghostSprite)
    {
        spriteGameObject.gameObject.SetActive(true);
        spriteGameObject.GetComponent<SpriteRenderer>().sprite = ghostSprite;
    }
    private void Hide()
    {
        spriteGameObject.gameObject.SetActive(false);
    }
}

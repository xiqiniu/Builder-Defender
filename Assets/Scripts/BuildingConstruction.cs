using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingConstruction : MonoBehaviour
{
    private float constructionTimer;
    private float constructionTimerMax;
    private BoxCollider2D boxCollider2D;
    private BuildingTypeSO buildingType;
    private SpriteRenderer spriteRenderer;
    private BuildingTypeHolder buildingTypeHolder;
    private Material constructionMaterial;

    public static BuildingConstruction Create(Vector3 position,BuildingTypeSO buildingType)
    {
       Transform pfBuildingConstruction = GameAssets.Instance.pfBuildingConstruction;
        Transform buildingConstructionTransform = Instantiate(pfBuildingConstruction, position, Quaternion.identity);
        BuildingConstruction buildingConstruction= buildingConstructionTransform.GetComponent<BuildingConstruction>();
        buildingConstruction.SetBuildingType(buildingType);
        return buildingConstruction;
    }

    private void Awake()
    {
        buildingTypeHolder = GetComponent<BuildingTypeHolder>();
        spriteRenderer = transform.Find("Sprite").GetComponent<SpriteRenderer>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        constructionMaterial = spriteRenderer.material;
        Instantiate(GameAssets.Instance.pfBuildingPlacedParticles, transform.position, Quaternion.identity);
    }
    private void Update()
    {
        constructionTimer -= Time.deltaTime;
        constructionMaterial.SetFloat("_Progress", 1f - GetConstructionTimerNormalized());
        if(constructionTimer<=0f)
        {
            print("destroy");
            SoundManager.Instance.PlaySound(SoundManager.Sound.BuildingPlaced);
            Instantiate(buildingType.prefab, transform.position, Quaternion.identity);
            Instantiate(GameAssets.Instance.pfBuildingPlacedParticles, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }
    public float GetConstructionTimerNormalized()
    {
        return constructionTimer / constructionTimerMax;
    }
    private void SetBuildingType(BuildingTypeSO buildingType)
    {
        this.constructionTimerMax = buildingType.constructionTimerMax;
        constructionTimer = constructionTimerMax;
        this.buildingType = buildingType;
        spriteRenderer.sprite = buildingType.sprite;
        buildingTypeHolder.buildingType = buildingType;
        boxCollider2D.offset = buildingType.prefab.GetComponent<BoxCollider2D>().offset;
        boxCollider2D.size= buildingType.prefab.GetComponent<BoxCollider2D>().size;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    private BuildingTypeSO buildingType;
    private HealthSystem healthSystem;
    private Transform buildingDemolishBtn;
    private Transform buildingRepairBtn;
    private void Awake()
    {
        buildingDemolishBtn=transform.Find("pfBuildingDemolishBtn");
        buildingRepairBtn=transform.Find("pfBuildingRepairBtn");
        HideBuildingDemolishBtn();
        HideBuildingRepairBtn();
    }
    private void Start()
    {
        buildingType = GetComponent<BuildingTypeHolder>().buildingType;
        healthSystem  = GetComponent<HealthSystem>();
        healthSystem.SetHealthAmountMax(buildingType.healthAmountMax,true);
        healthSystem.OnDied += HealthSystem_OnDied;
        healthSystem.OnDamaged += HealthSystem_OnDamaged;
        healthSystem.OnHealed += HealthSystem_OnHealed;
    }

    private void HealthSystem_OnHealed(object sender, System.EventArgs e)
    {
        if(healthSystem.isFullHealth())
            HideBuildingRepairBtn();
    }

    private void HealthSystem_OnDamaged(object sender, System.EventArgs e)
    {
        ShowBuildingRepairBtn();
        SoundManager.Instance.PlaySound(SoundManager.Sound.BuildingDamaged);
        CinemachineShake.Instance.ShakeCamera(5f, 0.1f);
        ChromaticAberrationEffect.Instance.SetWeight(1f);
    }

    private void HealthSystem_OnDied(object sender, System.EventArgs e)
    {
        Destroy(this.gameObject);
        SoundManager.Instance.PlaySound(SoundManager.Sound.BuildingDestroyed);
        Instantiate(GameAssets.Instance.pfBuildingDestroyedParticles, transform.position, Quaternion.identity);
        CinemachineShake.Instance.ShakeCamera(10f,0.2f);
        ChromaticAberrationEffect.Instance.SetWeight(2f);
    }
    private void OnMouseEnter()
    {
        ShowBuildingDemolishBtn();
    }
    private void OnMouseExit()
    {
        HideBuildingDemolishBtn();
    }
    private void ShowBuildingDemolishBtn()
    {
        if (buildingDemolishBtn != null)
            buildingDemolishBtn.gameObject.SetActive(true);
    }
    private void HideBuildingDemolishBtn()
    {
        if(buildingDemolishBtn!=null)
              buildingDemolishBtn.gameObject.SetActive(false);
    }
    private void ShowBuildingRepairBtn()
    {
        if (buildingRepairBtn != null)
            buildingRepairBtn.gameObject.SetActive(true);
    }
    private void HideBuildingRepairBtn()
    {
        if (buildingRepairBtn != null)
            buildingRepairBtn.gameObject.SetActive(false);
    }
}

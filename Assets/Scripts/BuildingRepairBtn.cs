using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingRepairBtn : MonoBehaviour
{
    [SerializeField] private HealthSystem healthSystem;
    [SerializeField] private ResourceTypeSO woodResourceType;
    private void Awake()
    {
        transform.Find("Button").GetComponent<Button>().onClick.AddListener(() =>
            {
                int missingHealth = healthSystem.GetHealthAmountMax() - healthSystem.GetHealthAmount();
                int repairCost = missingHealth / 2;
                ResourceAmount[] resourceAmountCost = new ResourceAmount[]{new ResourceAmount
                { resourceType = woodResourceType, amount = repairCost }};
                if (ResourceManager.Instance.CanAfford(resourceAmountCost))
                {
                    //can afford repair
                    ResourceManager.Instance.SpendResource(resourceAmountCost);
                    healthSystem.HealFull();
                }
                else
                {
                    //cannot afford repair
                    TooltipUI.Instance.Show("Cannot afford repair cost!", new TooltipUI.TooltipTimer { timer = 2f });
                }
            });
    }
}

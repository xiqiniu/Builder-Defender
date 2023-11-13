using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ConstructionTimerUI : MonoBehaviour
{
    [SerializeField] private BuildingConstruction buildingConstruction;
    private Image constuctionProgressImage;
    private void Awake()
    {
        constuctionProgressImage=transform.Find("Mask").Find("Image").GetComponent<Image>();
    }
    private void FixedUpdate()
    {
        constuctionProgressImage.fillAmount = 1f-buildingConstruction.GetConstructionTimerNormalized();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class TooltipUI : MonoBehaviour
{
    public static TooltipUI Instance { get; private set; }
    [SerializeField] private RectTransform canvasRectTransform;
    private TextMeshProUGUI textMeshPro;
    private RectTransform backgroundRectTransform;
    private RectTransform rectTransform;
    private TooltipTimer tooltipTimer;
    private void Awake()
    {
        Instance = this;
        rectTransform = GetComponent<RectTransform>();
        textMeshPro = transform.Find("Text").GetComponent<TextMeshProUGUI>();
        backgroundRectTransform = transform.Find("Background").GetComponent<RectTransform>();
         
        Hide();
    }
    private void Update()
    {
        HandleFollowMouse();
        if(tooltipTimer!=null)
        {
            tooltipTimer.timer -= Time.deltaTime;
            if(tooltipTimer.timer<=0)
            {
                Hide();
            }
        }
    }

    private void HandleFollowMouse()
    {
        Vector2 anchoredPosition = Input.mousePosition / canvasRectTransform.localScale.x;
        if (anchoredPosition.x + backgroundRectTransform.rect.width > canvasRectTransform.rect.width)
            anchoredPosition.x = canvasRectTransform.rect.width - backgroundRectTransform.rect.width;
        else if (anchoredPosition.y + backgroundRectTransform.rect.height > canvasRectTransform.rect.height)
            anchoredPosition.y = canvasRectTransform.rect.height - backgroundRectTransform.rect.height;
        rectTransform.anchoredPosition = anchoredPosition;
    }
    private void SetText(string tooltipText)
    {
        textMeshPro.SetText(tooltipText);
        textMeshPro.ForceMeshUpdate();
        Vector2 textSize = textMeshPro.GetRenderedValues(false);
        Vector2 padding = new Vector2(8, 8);
        backgroundRectTransform.sizeDelta = textSize+padding ;
    }
    public void Show(string tooltipText,TooltipTimer tooltipTimer=null)
    {
        HandleFollowMouse();
        this.tooltipTimer = tooltipTimer;
        gameObject.SetActive(true);
        SetText(tooltipText);
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public class TooltipTimer
    {
        public float timer;
    }
}
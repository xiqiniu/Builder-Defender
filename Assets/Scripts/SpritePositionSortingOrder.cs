using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpritePositionSortingOrder : MonoBehaviour
{
    [SerializeField] private bool runOnce = true;
    [SerializeField] private float positionOffsetY;
    private SpriteRenderer spriteRenderer;
    // Update is called once per frame
    void Update()
    {
        if (GetComponent<SpriteRenderer>() != null)
            spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void LateUpdate()
    {
        if (spriteRenderer!= null)
        {
            float precisionMutiplier = 5f;
            spriteRenderer.sortingOrder = (int)(-(transform.position.y + positionOffsetY) * precisionMutiplier);
            if (runOnce)
            {
                Destroy(this);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class CameraHandler : MonoBehaviour
{
    public static CameraHandler Instance { get; private set; }
    [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;
    private float orthographicSize;
    private float targetOrthoGraphicSize;
    private bool edgeScrolling;

    private void Awake()
    {
        Instance = this;
        edgeScrolling=PlayerPrefs.GetInt("edgeScrolling", 1)==1;
    }
    // Start is called before the first frame update
    void Start()
    {
        orthographicSize = cinemachineVirtualCamera.m_Lens.OrthographicSize;
        targetOrthoGraphicSize = orthographicSize;
    }

    // Update is called once per frame
    void Update()
    {
        HandleMove();
        HandleZoom();
    }

    private void HandleMove()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        if(edgeScrolling)
        {
            float edgeScrollingSize = 20f;
            if (Input.mousePosition.x > Screen.width - edgeScrollingSize)
            {
                x = 1f;
            }
            if (Input.mousePosition.x < edgeScrollingSize)
            {
                x = -1f;
            }
            if (Input.mousePosition.y > Screen.height - edgeScrollingSize)
            {
                y = 1f;
            }
            if (Input.mousePosition.y < edgeScrollingSize)
            {
                y = -1f;
            }
        }
      
        float moveSpeed = 20f;
        Vector3 moveDir = (Vector3)new Vector2(x, y).normalized;
        transform.position += moveDir * moveSpeed * Time.deltaTime;
    }
    private void HandleZoom()
    {
        float zoomAmount = 2f;
        targetOrthoGraphicSize += (-Input.mouseScrollDelta.y) * zoomAmount;
        float minOrthographicSize = 10f;
        float maxOrthographicSize = 30f;
        targetOrthoGraphicSize = Mathf.Clamp(targetOrthoGraphicSize, minOrthographicSize, maxOrthographicSize);
        float zoomSpeed = 5f;
        orthographicSize = Mathf.Lerp(orthographicSize, targetOrthoGraphicSize, Time.deltaTime * zoomSpeed);
        cinemachineVirtualCamera.m_Lens.OrthographicSize = orthographicSize;
    }

    public void SetEdgeScrolling(bool edgeScrolling)
    {
        this.edgeScrolling = edgeScrolling;
        PlayerPrefs.SetInt("edgeScrolling", edgeScrolling?1:0);
    }

    public bool GetEdgeScrolling()
    {
        return edgeScrolling;
    }
}

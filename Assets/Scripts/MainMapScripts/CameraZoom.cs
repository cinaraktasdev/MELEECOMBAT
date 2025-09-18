using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Tilemaps;

public class CameraZoom : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera vcam;

    public float zoomTarget; 

    private InMapMovement imp;

    [SerializeField]
    float multipler = 2f, minZoom = 1f, maxZoom = 10f, smoothTime = .1f;
    float velocity = 0f;

    public CanvasGroup CitynamesCanvasGroup;

    // Tilemap ve opaklık aralığı değişkenleri
    public TilemapRenderer tilemapRenderer;
    public float minOpacity = 0f;
    public float maxOpacity = 1f;



    private void Start()
    {
        imp = FindObjectOfType<InMapMovement>();  // imp nesnesini sahnede buluyoruz
        zoomTarget = vcam.m_Lens.OrthographicSize = 5f;
    }

    private void Update()
    {
        if (imp != null && imp.stopscene) return;

        zoomTarget -= Input.GetAxisRaw("Mouse ScrollWheel") * multipler;
        zoomTarget = Mathf.Clamp(zoomTarget, minZoom, maxZoom);
        vcam.m_Lens.OrthographicSize = Mathf.SmoothDamp(vcam.m_Lens.OrthographicSize, zoomTarget, ref velocity, smoothTime);


        // UI opacity ayarı
        float normalizedOpacity = Mathf.InverseLerp(12f, 19f, vcam.m_Lens.OrthographicSize);
        CitynamesCanvasGroup.alpha = Mathf.Lerp(0, 1, normalizedOpacity);

        // Tilemap opacity ayarı
        if (tilemapRenderer != null)
        {
            Color color = tilemapRenderer.material.color;
            color.a = Mathf.Lerp(minOpacity, maxOpacity, normalizedOpacity);
            tilemapRenderer.material.color = color;
        }

        

    }


}

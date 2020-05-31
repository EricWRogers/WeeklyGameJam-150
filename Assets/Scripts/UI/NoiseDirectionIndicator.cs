using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class NoiseDirectionIndicator : MonoBehaviour
{
    [HideInInspector] public Transform trackingTransform;
    public Image indicatorImage;
    public float fadeDuration = 0.5f;
    public float lifeSpan = 3f;

    private float timeElapsed; 

    void Awake()
    {
        var col = indicatorImage.color;
        col.a = 0;
        indicatorImage.color = col;

        indicatorImage.DOFade(1, fadeDuration).Play();
        ResetTimer();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateIndicatorRotation();

        if (timeElapsed >= lifeSpan)
        {
            indicatorImage.DOFade(0, fadeDuration).OnComplete(() =>
            {
                Destroy(gameObject);
                NoiseDirectionIndicatorManager.Instance.StopTrackingTransform(trackingTransform);
            }).Play();
        }

        timeElapsed += Time.deltaTime;
    }

    void UpdateIndicatorRotation()
    {
        var camForward = PlayerModel.Instance.mainCamera.transform.forward;
        camForward.y = 0;

        var playerToTarget = trackingTransform.position - PlayerModel.Instance.transform.position;
        playerToTarget.y = 0;

        var angle = Vector3.SignedAngle(playerToTarget, camForward, Vector3.up);

        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    public void ResetTimer()
    {
        timeElapsed = 0;
    }
}

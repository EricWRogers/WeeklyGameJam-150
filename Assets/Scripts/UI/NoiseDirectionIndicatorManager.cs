using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseDirectionIndicatorManager : SingletonMonoBehaviour<NoiseDirectionIndicatorManager>
{
    public enum IndicatorType
    {
        SimpleNoise,
    }

    public Transform container;
    public GameObject noiseIndicatorPrefab;

    private Dictionary<Transform, NoiseDirectionIndicator> trackingTransforms = new Dictionary<Transform, NoiseDirectionIndicator>();
    private HashSet<Transform> tempTransforms = new HashSet<Transform>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void IndicateNoiseFrom(Vector3 position, IndicatorType type = IndicatorType.SimpleNoise)
    {
        var tempTransform = new GameObject("TempNoiseTarget").transform;
        tempTransform.position = position;
        tempTransforms.Add(tempTransform);

        IndicateNoiseFrom(tempTransform);
    }

    public void IndicateNoiseFrom(Transform otherTransform, IndicatorType type = IndicatorType.SimpleNoise)
    {
        if (trackingTransforms.ContainsKey(otherTransform))
        {
            trackingTransforms[otherTransform].ResetTimer();
            return;
        }

        GameObject indicatorPrefab = null;
        switch (type)
        {
            case IndicatorType.SimpleNoise:
                indicatorPrefab = noiseIndicatorPrefab;
                break;
        }

        var damageDirIndicator = Instantiate(indicatorPrefab, container).GetComponent<NoiseDirectionIndicator>();
        damageDirIndicator.trackingTransform = otherTransform;

        trackingTransforms[otherTransform] = damageDirIndicator;
    }

    public void StopTrackingTransform(Transform otherTransform)
    {
        trackingTransforms.Remove(otherTransform);

        if (tempTransforms.Contains(otherTransform))
        {
            tempTransforms.Remove(otherTransform);
            Destroy(otherTransform.gameObject);
        }
    }
}

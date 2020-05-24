using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CamperManager : SingletonMonoBehaviour<CamperManager>
{
    public GameObject camperPrefab;
    public Transform hidingSpotsContainer;

    public int campersCount = 6;

    public List<Camper> campers { get; } = new List<Camper>();
    public Randomizer<HidingSpot> hidingSpotsRandomizer { get; private set; }

    new void Awake()
    {
        base.Awake();

        hidingSpotsRandomizer = new Randomizer<HidingSpot>(hidingSpotsContainer.GetComponentsInChildren<HidingSpot>());
        if (hidingSpotsRandomizer.count <= 0)
        {
            Debug.LogWarning("No Hiding Spots Found");
        }

        SpawnCampers();
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    void SpawnCampers()
    {
        for (int i = 0; i < campersCount; i++)
        {
            var hidingSpot = hidingSpotsRandomizer.GetRandomItem();
            var camper = Instantiate(camperPrefab, hidingSpot.transform.position, hidingSpot.transform.rotation)
                .GetComponent<Camper>();

            campers.Add(camper);
        }
    }
}
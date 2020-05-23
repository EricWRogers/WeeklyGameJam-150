using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CamperManager : MonoBehaviour
{
    public GameObject camperPrefab;
    public Transform hidingSpotsContainer;

    public int campersCount = 6;

    [HideInInspector] public List<Camper> campers = new List<Camper>();
    private Randomizer<HidingSpot> hidingSpotsRandomizer;

    void Awake()
    {
        hidingSpotsRandomizer = new Randomizer<HidingSpot>(hidingSpotsContainer.GetComponentsInChildren<HidingSpot>());
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
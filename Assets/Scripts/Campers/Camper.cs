using System.Collections;
using System.Collections.Generic;
using BasicTools.ButtonInspector;
using UnityEngine;
using UnityEngine.AI;

public class Camper : MonoBehaviour
{
    [Header("Hiding Params")]
    public RangeFloat maxHideDurationRange;

    [Header("Dev Tools")]
    [SerializeField] [Button("Move to new hiding spot", "MoveToNewHidingSpot")]
    private bool _btnMoveToNewHidingSpot;

    private NavMeshAgent navMeshAgent;

    void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void MoveToNewHidingSpot()
    {
        var hidingSpot = FindNewHidingSpot();

        navMeshAgent.SetDestination(hidingSpot.transform.position);
    }

    HidingSpot FindNewHidingSpot()
    {
        return CamperManager.Instance.hidingSpotsRandomizer.GetRandomItem();
    }
}
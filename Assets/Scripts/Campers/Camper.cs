using System.Collections;
using System.Collections.Generic;
using BasicTools.ButtonInspector;
using UnityEngine;
using UnityEngine.AI;

public class Camper : MonoBehaviour
{
    [Header("References")] public GameObject avatar;

    [Header("Stats")]
    public float maxVisionRange = 30;
    public float maxVisionAngle = 45;
    public Transform visionRoot => avatar.transform;

    [Header("Hiding Params")]
    public RangeFloat maxHideDurationRange;

    [Header("Dev Tools")]
    [SerializeField] [Button("Move to new hiding spot", "MoveToNewHidingSpot")]
    private bool _btnMoveToNewHidingSpot;

    [Header("Logs")] [SerializeField]
    [ReadOnly] private bool _canSeePlayer;

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
        _canSeePlayer = CanSeePlayer();
    }

    void OnDrawGizmosSelected()
    {
        DebugExtension.DebugCone(visionRoot.position, visionRoot.forward * maxVisionRange, maxVisionAngle);
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

    public bool CanSeePlayer()
    {
        var playerLoc = LevelManager.Instance.playerLocation;

        var toPlayer = playerLoc.position - visionRoot.position;

        if (toPlayer.magnitude <= maxVisionRange)
        {
            var angleToPlayer = Vector3.Angle(visionRoot.forward, toPlayer.normalized);
            if (angleToPlayer <= maxVisionAngle)
            {
                if (Physics.Raycast(visionRoot.position, toPlayer.normalized, out var hit, maxVisionRange))
                {
                    if (hit.transform.CompareTag("Player"))
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using BasicTools.ButtonInspector;
using UnityEngine;
using UnityEngine.AI;

public enum CamperState
{
    Hiding,
    Moving,
}

[Serializable]
public class CamperStateData
{
    public float timeSinceStateActive = 0;
    public Dictionary<string, object> metaData = new Dictionary<string, object>();
}

public class Camper : MonoBehaviour
{
    [Header("References")] public GameObject avatar;

    [Header("Stats")] public float maxVisionRange = 30;
    public float maxVisionAngle = 45;

    [Header("Hiding Params")] public RangeFloat maxHideDurationRange;

    [Header("Dev Tools")] [SerializeField] [Button("Move to new hiding spot", "MoveToNewHidingSpot")]
    private bool _btnMoveToNewHidingSpot;

    [Header("Logs")] [SerializeField] [ReadOnly]
    private bool _canSeePlayer;

    [SerializeField] [ReadOnly] private CamperState _curState;
    [SerializeField] [ReadOnly] private CamperState _prevState;
    [SerializeField] [ReadOnly] private CamperStateData curStateData = new CamperStateData();

    private NavMeshAgent navMeshAgent;

    public Transform visionRoot => avatar.transform;
    public CamperState curState => _curState;
    public CamperState prevState => _prevState;

    void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    // Start is called before the first frame update
    void Start()
    {
        SwitchState(CamperState.Hiding);
    }

    // Update is called once per frame
    void Update()
    {
        _canSeePlayer = CanSeePlayer();

        UpdateState();
    }

    void OnDrawGizmosSelected()
    {
        DebugExtension.DebugCone(visionRoot.position, visionRoot.forward * maxVisionRange, Color.yellow,
            maxVisionAngle);
    }

    public void MoveToNewHidingSpot()
    {
        var hidingSpot = FindNewHidingSpot();
        SwitchState(CamperState.Moving, new Dictionary<string, object>
        {
            {"target", hidingSpot.transform.position},
        });
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

    void UpdateState()
    {
        curStateData.timeSinceStateActive += Time.deltaTime;

        switch (curState)
        {
            case CamperState.Hiding:
                if (curStateData.timeSinceStateActive >= curStateData.metaData.GetOrDefault("timeout", 0f))
                {
                    MoveToNewHidingSpot();
                    return;
                }

                if (_canSeePlayer)
                {
                    MoveToNewHidingSpot();
                    return;
                }

                break;
            case CamperState.Moving:
                if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
                {
                    SwitchState(CamperState.Hiding);
                    return;
                }

                break;
        }
    }

    void SwitchState(CamperState newState, Dictionary<string, object> data = null)
    {
        if (data == null)
        {
            data = new Dictionary<string, object>();
        }

        _prevState = _curState;
        _curState = newState;

        // On State Exit
        switch (_prevState)
        {
            default:
                break;
        }

        curStateData = new CamperStateData();

        // On State Enter
        switch (_curState)
        {
            case CamperState.Hiding:
                navMeshAgent.ResetPath();
                curStateData.metaData["timeout"] = maxHideDurationRange.GetRandom();
                break;
            case CamperState.Moving:
                navMeshAgent.SetDestination(data.GetOrDefault("target", navMeshAgent.transform.position));
                break;
            default:
                break;
        }
    }
}
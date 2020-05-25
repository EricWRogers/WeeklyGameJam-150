using System;
using System.Collections;
using System.Collections.Generic;
using BasicTools.ButtonInspector;
using DG.Tweening;
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
    public Transform visionRoot;

    [Header("Stats")] public float maxVisionRange = 30;
    public float maxVisionAngle = 45;

    [Header("Hiding Params")] public float hidingOrientationChangeDuration = 0.3f;
    public RangeFloat maxHideDurationRange;
    public float maxDistanceToSenseMonster = 5;

    [Header("Dev Tools")] [SerializeField] [Button("Move to new hiding spot", "MoveToNewHidingSpot")]
    private bool _btnMoveToNewHidingSpot;

    [Header("Logs")] [SerializeField] [ReadOnly]
    private bool _canSeePlayer = false;

    [SerializeField] [ReadOnly] private bool _isLineToPlayerBlocked = true;

    [SerializeField] [ReadOnly] private CamperState _curState;
    [SerializeField] [ReadOnly] private CamperState _prevState;
    [SerializeField] [ReadOnly] private CamperStateData curStateData = new CamperStateData();

    private NavMeshAgent navMeshAgent;
    private Animator animator;

    public CamperState curState => _curState;
    public CamperState prevState => _prevState;

    void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    // Start is called before the first frame update
    void Start()
    {
        animator = avatar.GetComponentInChildren<Animator>();

        SwitchState(CamperState.Hiding);
    }

    // Update is called once per frame
    void Update()
    {
        ComputeIsLineToPlayerBlocked();
        ComputeCanSeePlayer();

        UpdateState();
    }

    void OnDrawGizmosSelected()
    {
        DebugExtension.DebugCone(visionRoot.position, visionRoot.forward * maxVisionRange, Color.yellow,
            maxVisionAngle);
        DebugExtension.DebugWireSphere(transform.position, Color.red, maxDistanceToSenseMonster);
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

    private void ComputeIsLineToPlayerBlocked()
    {
        _isLineToPlayerBlocked = true;

        var playerLoc = LevelManager.Instance.playerLocation;
        var toPlayer = playerLoc.position - visionRoot.position;

        if (Physics.Raycast(visionRoot.position, toPlayer.normalized, out var hit))
        {
            if (hit.transform.CompareTag("Player"))
            {
                _isLineToPlayerBlocked = false;
            }
        }
    }

    private void ComputeCanSeePlayer()
    {
        _canSeePlayer = false;

        var playerLoc = LevelManager.Instance.playerLocation;

        var toPlayer = playerLoc.position - visionRoot.position;

        if (toPlayer.magnitude <= maxVisionRange)
        {
            var angleToPlayer = Vector3.Angle(visionRoot.forward, toPlayer.normalized);
            if (angleToPlayer <= maxVisionAngle)
            {
                if (!_isLineToPlayerBlocked)
                {
                    _canSeePlayer = true;
                }
            }
        }
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

                if (!_isLineToPlayerBlocked)
                {
                    var playerLoc = LevelManager.Instance.playerLocation;
                    var toPlayer = playerLoc.position - visionRoot.position;

                    if (toPlayer.magnitude <= maxDistanceToSenseMonster)
                    {
                        MoveToNewHidingSpot();
                        return;
                    }
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
            case CamperState.Hiding:
                animator.SetBool("hiding", false);
                transform.DOKill(true);
                break;
            case CamperState.Moving:
                animator.SetBool("running", false);
                break;
            default:
                break;
        }

        curStateData = new CamperStateData();

        // On State Enter
        switch (_curState)
        {
            case CamperState.Hiding:
                navMeshAgent.ResetPath();
                animator.SetBool("hiding", true);
                curStateData.metaData["timeout"] = maxHideDurationRange.GetRandom();

                var hidingSpotNearby = CamperManager.Instance.hidingSpotsRandomizer.GetItems().Find(hidingSpot =>
                    Vector3.Distance(transform.position, hidingSpot.transform.position) <=
                    navMeshAgent.stoppingDistance * 2);

                curStateData.metaData["hidingSpot"] = hidingSpotNearby;

                if (hidingSpotNearby)
                {
                    var lookAtTarget = transform.position + hidingSpotNearby.facingDir;
                    transform.DOLookAt(lookAtTarget, hidingOrientationChangeDuration).Play();
                }
                break;
            case CamperState.Moving:
                animator.SetBool("running", true);
                navMeshAgent.SetDestination(data.GetOrDefault("target", navMeshAgent.transform.position));
                break;
            default:
                break;
        }
    }
}
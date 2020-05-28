using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BasicTools.ButtonInspector;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;

public enum CamperState
{
    Hiding,
    Moving,
    Captured,
    Eaten,
    Safe,
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
    public GameObject model;
    public Transform visionRoot;
    public NavMeshAgent navMeshAgent;

    [Header("Stats")] public float maxVisionRange = 30;
    public float maxVisionAngle = 45;

    [Header("Hiding Params")] public float hidingOrientationChangeDuration = 0.3f;
    public RangeFloat maxHideDurationRange;
    public float maxDistanceToSenseMonster = 5;

    [Header("Captured Params")]
    public Vector3 capturedLocalOffset;
    public Vector3 capturedLocalRotation;

    [Header("Dev Tools")] [SerializeField] [Button("Move to new hiding spot", "MoveToNewHidingSpot")]
    private bool _btnMoveToNewHidingSpot;

    [Header("Logs")] [SerializeField] [ReadOnly]
    private bool _canSeePlayer = false;

    [SerializeField] [ReadOnly] private bool _isLineToPlayerBlocked = true;

    [SerializeField] [ReadOnly] private CamperState _curState;
    [SerializeField] [ReadOnly] private CamperState _prevState;
    [SerializeField] [ReadOnly] private CamperStateData curStateData = new CamperStateData();

    private Animator animator;

    public CamperState curState => _curState;
    public CamperState prevState => _prevState;
    public bool isLineToPlayerBlocked => _isLineToPlayerBlocked;

    void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        animator = model.GetComponent<Animator>();
        RandomizeModel();

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
        DebugExtension.DebugWireSphere(transform.position, Color.cyan, navMeshAgent.stoppingDistance);
    }

    public void RandomizeModel()
    {
        if (CamperManager.Instance.camperModelPrefabsRandomizer.count > 0)
        {
            ChangeModel(CamperManager.Instance.camperModelPrefabsRandomizer.GetRandomItem());
        }
    }

    public void ChangeModel(GameObject modelPrefab)
    {
        var newModel = Instantiate(modelPrefab, model.transform.parent);
        newModel.transform.localPosition = model.transform.localPosition;
        newModel.transform.localRotation = model.transform.localRotation;

        var newAnimator = newModel.GetComponent<Animator>();
        if (newAnimator == null)
        {
            newAnimator = newModel.AddComponent<Animator>();
        }
        newAnimator.runtimeAnimatorController = animator.runtimeAnimatorController;
        newAnimator.applyRootMotion = false;

        model.SetActive(false);
        model = newModel;
        animator = newAnimator;
    }

    public void RunToTargetBase()
    {
        SwitchState(CamperState.Moving, new Dictionary<string, object>
        {
            {"target", TargetBase.Instance.transform.position},
            {"stateOnReachedTarget", CamperState.Safe},
        });
    }

    public void MoveToNewHidingSpot(HidingSpot overrideHidingSpot = null)
    {
        var hidingSpot = overrideHidingSpot ?? FindNewHidingSpot();
        SwitchState(CamperState.Moving, new Dictionary<string, object>
        {
            {"target", hidingSpot.transform.position},
            {"stateOnReachedTarget", CamperState.Hiding},
        });
    }

    HidingSpot FindNewHidingSpot()
    {
        return CamperManager.Instance.hidingSpotsRandomizer.GetRandomItem();
    }

    public void OnCaptured()
    {
        SwitchState(CamperState.Captured);
    }

    public void OnReleased()
    {
        MoveToNewHidingSpot();
    }

    public void OnBeingEaten()
    {
        animator.SetTrigger("beingEaten");
    }

    public void OnEaten()
    {
        SwitchState(CamperState.Eaten);
    }

    private void ComputeIsLineToPlayerBlocked()
    {
        _isLineToPlayerBlocked = true;

        var playerLoc = LevelManager.Instance.playerVisiblePoint;
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

        var playerLoc = LevelManager.Instance.playerVisiblePoint;
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
                if (LevelManager.Instance.isGameEnded)
                {
                    return;
                }

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
                    var playerLoc = LevelManager.Instance.playerVisiblePoint;
                    var toPlayer = playerLoc.position - visionRoot.position;

                    if (toPlayer.magnitude <= maxDistanceToSenseMonster)
                    {
                        MoveToNewHidingSpot();
                        return;
                    }
                }

                break;
            case CamperState.Moving:
                if (_canSeePlayer)
                {
                    var toPlayer = LevelManager.Instance.playerVisiblePoint.position - transform.position;
                    var toDestination = navMeshAgent.destination - transform.position;

                    if (Vector3.Angle(toPlayer, toDestination) <= maxVisionAngle)
                    {
                        var availableHidingSpots = CamperManager.Instance.hidingSpotsRandomizer.GetItems().Where(hidingSpot =>
                        {
                            var toHidingSpot = hidingSpot.transform.position - transform.position;
                            return Vector3.Angle(toPlayer, toHidingSpot) > maxVisionAngle;
                        }).ToList();

                        MoveToNewHidingSpot(availableHidingSpots.Count > 0 ? availableHidingSpots.GetRandom() : null);
                        return;
                    }
                }

                if (Vector3.Distance(transform.position, navMeshAgent.destination) <= navMeshAgent.stoppingDistance)
                {
                    SwitchState(curStateData.metaData.GetOrDefault("stateOnReachedTarget", CamperState.Hiding));
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
            case CamperState.Captured:
                transform.SetParent(null);
                navMeshAgent.enabled = true;
                animator.SetBool("captured", false);
                break;
            default:
                break;
        }

        navMeshAgent.ResetPath();
        curStateData = new CamperStateData();

        // On State Enter
        switch (_curState)
        {
            case CamperState.Hiding:
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
                curStateData.metaData["stateOnReachedTarget"] =
                    data.GetOrDefault("stateOnReachedTarget", CamperState.Hiding);

                navMeshAgent.SetDestination(data.GetOrDefault("target", navMeshAgent.transform.position));
                break;
            case CamperState.Captured:
                navMeshAgent.enabled = false;

                transform.SetParent(PlayerModel.Instance.camperCapturePivot);
                transform.localPosition = capturedLocalOffset;
                transform.localRotation = Quaternion.Euler(capturedLocalRotation);

                animator.SetBool("captured", true);

                break;
            case CamperState.Safe:
                animator.SetBool("hiding", true);
                break;
            case CamperState.Eaten:
                gameObject.SetActive(false);
                break;
            default:
                break;
        }
    }
}
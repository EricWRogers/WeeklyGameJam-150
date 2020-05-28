using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerModel : SingletonMonoBehaviour<PlayerModel>
{
    public Transform visibilityCheckPoint;
    public Transform camperCapturePivot;
    public Animator animator;

    public float maxAttackRadius = 3f;
    public float maxAttackAngle = 30f;

    [SerializeField] [ReadOnly] private int _campersEaten = 0;

    public PlayerMovement playerMovement { get; private set; }
    public QTEManager qteManager { get; private set; }
    public GameObject avatar => playerMovement.avatar;

    public int campersEaten => _campersEaten;
    public event Action<Camper> onCamperEaten;

    new void Awake()
    {
        base.Awake();
        playerMovement = GetComponent<PlayerMovement>();
        
        qteManager = GetComponent<QTEManager>();
        qteManager.enabled = false;

        animator.GetComponent<AnimatorStateHelper>().onStateExit += (stateIdentifier, animator1, info, index) =>
        {
            if (stateIdentifier.Equals("devour"))
            {
                if (qteManager.enabled)
                {
                    OnCamperEaten(qteManager.camperInPossession);
                    ChangeState(PlayerModel.PlayerState.Moving);

                    qteManager.OnEatingAnimationCompleted();
                }
            }
        };
    }

    public enum PlayerState
    {
        Moving,
        Attacking
    }

    public PlayerState playerState = PlayerState.Moving;

    void OnDrawGizmos()
    {
        DebugExtension.DebugWireSphere(transform.position, Color.red, maxAttackRadius);
    }

    private bool CanAttackCamper(Camper camper)
    {
        if (camper.curState == CamperState.Safe)
        {
            return false;
        }

        if (camper.isLineToPlayerBlocked)
        {
            return false;
        }

        var toCamper = camper.transform.position - transform.position;
        if (toCamper.magnitude <= maxAttackRadius)
        {
            var angleToCamper = Vector3.Angle(avatar.transform.forward.GetYLess(), toCamper.GetYLess().normalized);
            if (angleToCamper <= maxAttackAngle)
            {
                return true;
            }
        }

        return false;
    }

    public void OnCamperEaten(Camper camper)
    {
        _campersEaten++;
        onCamperEaten?.Invoke(camper);
    }

    void OnAttack(InputValue inputValue)
    {
        if (LevelManager.Instance.isGameEnded)
        {
            return;
        }

        animator.SetTrigger("attack");

        var camper = CamperManager.Instance.campers.Find(CanAttackCamper);
        if (camper == null)
        {
            return;
        }

        if (playerState == PlayerState.Moving)
        {
            qteManager.camperInPossession = camper;
            qteManager.camperInPossession.OnCaptured();

            ChangeState(PlayerState.Attacking);
        }
    }

    public void ChangeState(PlayerState state)
    {
        switch (state)
        {
            case PlayerState.Moving:
                playerMovement.canMove = true;
                
                break;
            case PlayerState.Attacking:
                qteManager.enabled = true;
                playerMovement.canMove = false;
                
                break;
        }

        playerState = state;
    }
}

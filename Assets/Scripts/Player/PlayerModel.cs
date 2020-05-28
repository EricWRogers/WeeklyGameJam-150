using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerModel : SingletonMonoBehaviour<PlayerModel>
{
    public Transform visibilityCheckPoint;

    public PlayerMovement playerMovement { get; private set; }
    public QTEManager qteManager { get; private set; }

    new void Awake()
    {
        base.Awake();
        playerMovement = GetComponent<PlayerMovement>();
        
        qteManager = GetComponent<QTEManager>();
        qteManager.enabled = false;
    }

    public enum PlayerState
    {
        Moving,
        Attacking
    }

    public PlayerState playerState = PlayerState.Moving;

    void OnAttack(InputValue inputValue)
    {
        if (playerState == PlayerState.Moving)
        {
            ChangeState(PlayerState.Attacking);
        }
        else if(playerState == PlayerState.Attacking)
        {
            ChangeState(PlayerState.Moving);   
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

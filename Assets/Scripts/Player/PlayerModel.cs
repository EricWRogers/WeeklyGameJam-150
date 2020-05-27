using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerModel : SingletonMonoBehaviour<PlayerModel>
{
    public PlayerMovement playerMovement { get; private set; }
    new void Awake()
    {
        base.Awake();
        playerMovement = GetComponent<PlayerMovement>();
    }

    public enum PlayerState
    {
        Moving,
        Attacking
    }

    public PlayerState playerState = PlayerState.Moving;

    private bool interactPressed = false;

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

    void ChangeState(PlayerState state)
    {
        switch (state)
        {
            case PlayerState.Moving:
                playerMovement.canMove = true;
                //Disable QTE
                Debug.Log("Moving");
                break;
            case PlayerState.Attacking:
                //Enable QTE
                playerMovement.canMove = false;
                Debug.Log("Attacking");
                break;
        }

        playerState = state;
    }
}

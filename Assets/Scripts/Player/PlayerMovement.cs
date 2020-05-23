using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    PlayerInputActions inputActions;
    CharacterController charController;

    Vector2 movementInput;

    public float speed;

    void Awake()
    {
        inputActions = new PlayerInputActions();
        inputActions.PlayerControls.Movement.performed += ctx => movementInput = ctx.ReadValue<Vector2>();

        charController = GetComponent<CharacterController>();
    }

    void OnEnable()
    {
        inputActions.Enable();
    }

    void Update()
    {
        float h = movementInput.x;
        float v = movementInput.y;
        Vector3 movementVector = new Vector3(h, 0, v);

        charController.Move(movementVector * speed * Time.deltaTime);

    }
}

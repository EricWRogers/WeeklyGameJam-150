using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    CharacterController charController;

    Vector2 movementInput;

    public float speed;

    void Awake()
    {

        charController = GetComponent<CharacterController>();
    }

    void OnMovement(InputValue inputValue)
    {
        movementInput = inputValue.Get<Vector2>();
    }

    void Update()
    {
        float h = movementInput.x;
        float v = movementInput.y;
        Vector3 movementVector = new Vector3(h, 0, v);

        charController.Move(movementVector * speed * Time.deltaTime);

    }
}

using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    CharacterController charController;
    
    public GameObject cinemachine;
    public CinemachineBrain cinemachineBrain;
    public GameObject avatar;

    Vector2 movementInput;
    Vector3 movementVector;

    public float speed = 5;
    public float rotationSpeed = 4;
    public float gravity = 10;

    [Range(0.01f, 1f)]
    public float minimumGravity = 0.01f;

    void Awake()
    {
        charController = GetComponent<CharacterController>();

        HelperUtilities.UpdateCursorLock(true);

        movementVector = Vector3.zero;
    }

    void OnMovement(InputValue inputValue)
    {
        movementInput = inputValue.Get<Vector2>();
    }

    Vector3 OrientPlayer(Vector3 baseVector)
    {
        Vector3 forwardDir = cinemachineBrain.gameObject.transform.forward * baseVector.z;
        forwardDir.y = 0;

        Vector3 rightDir = cinemachineBrain.gameObject.transform.right * baseVector.x;
        rightDir.y = 0;

        return (forwardDir + rightDir).normalized;
    }

    void Update()
    {
        float h = movementInput.x;
        float v = movementInput.y;
        movementVector = new Vector3(h, 0, v);
        if (movementVector != Vector3.zero)
        {
            movementVector = OrientPlayer(movementVector);
            avatar.transform.rotation = Quaternion.Slerp(avatar.transform.rotation, Quaternion.LookRotation(movementVector, transform.up), rotationSpeed * Time.deltaTime);
        }

        if (!charController.isGrounded)
        {
            movementVector.y -= gravity;
        }
        else
        {
            movementVector.y -= minimumGravity;
        }

        charController.Move(movementVector * speed * Time.deltaTime);
    }
}

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
    CinemachineFreeLook freeLookCam;

    Vector2 movementInput;

    public float speed;

    void Awake()
    {
        charController = GetComponent<CharacterController>();
        freeLookCam = cinemachine.GetComponent<CinemachineFreeLook>();

        HelperUtilities.UpdateCursorLock(true);
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
        Vector3 movementVector = new Vector3(h, 0, v);
        movementVector = OrientPlayer(movementVector);

        charController.Move(movementVector * speed * Time.deltaTime);
    }
}

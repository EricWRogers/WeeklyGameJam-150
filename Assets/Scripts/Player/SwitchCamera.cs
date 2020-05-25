using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SwitchCamera : MonoBehaviour
{
    public GameObject cinemachine;

    private CinemachineFreeLook freeLook;

    Vector2 lookDirection;

    void Start()
    {
        freeLook = cinemachine.GetComponent<CinemachineFreeLook>();
    }

    void OnMovement(InputValue inputValue)
    {
        lookDirection = inputValue.Get<Vector2>();

        freeLook.m_XAxis.m_InputAxisValue = lookDirection.x;
        freeLook.m_YAxis.m_InputAxisValue = lookDirection.y;
    }
}

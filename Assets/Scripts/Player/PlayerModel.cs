using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModel : MonoBehaviour
{
    public PlayerMovement playerMovement { get; private set; }
    void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }
}

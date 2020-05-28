using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WinPanel : MonoBehaviour
{
    void OnEnable()
    {
        GetComponent<TMP_Text>().text = "You Monster you ate " + PlayerModel.Instance.campersEaten + " campers!";
    }
}

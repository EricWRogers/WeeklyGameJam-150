using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WinPanel : MonoBehaviour
{
    [HideInInspector] public string reason = "";

    void OnEnable()
    {
        string winText = reason; 
        winText += "\n\nRank: " + LevelManager.Instance.GetRank();
        winText += "\n\nYou Monster! You ate " + PlayerModel.Instance.campersEaten +
                   $" camper{(PlayerModel.Instance.campersEaten == 1 ? "" : "s")}!";

        GetComponent<TMP_Text>().text = winText;
    }

    public void MainMenu()
    {
        GameManager.Instance.GoToMainMenu();
    }
}
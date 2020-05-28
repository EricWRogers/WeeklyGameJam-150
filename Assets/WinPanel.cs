using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WinPanel : MonoBehaviour
{
    void OnEnable()
    {
        string winText = "Rank: " + LevelManager.Instance.GetRank();
        winText += "\n\nYou Monster! You ate " + PlayerModel.Instance.campersEaten +
                   $" camper{(PlayerModel.Instance.campersEaten == 1 ? "" : "s")}!";

        GetComponent<TMP_Text>().text = winText;
    }

    public void MainMenu()
    {
        GameManager.Instance.GoToMainMenu();
    }
}
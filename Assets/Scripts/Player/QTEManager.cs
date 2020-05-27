using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class QTEManager : MonoBehaviour
{
    public enum QTEOptions
    {
        North,
        South,
        West,
        East,
        QTEOptionsSize
    }

    public QTEOptions currentKey;
    public RangeFloat maxQTEBuffer;
    public float QTEBuffer = 0;
    public float QTETimer;
    public RangeInt maxQuickTimeEvents;

    public TMPro.TextMeshProUGUI QTETextMesh;
    public UnityEngine.UI.Image QTETimerImage;

    private float maxQTETimer = 0;
    private int currentPasses = 0;

    private bool canGetNextKey = true;
    private bool checkQTETimer = false;

    void Awake()
    {
        QTEBuffer = maxQTEBuffer.GetRandom();
        maxQTETimer = QTETimer;
    }

    void OnEnable()
    {
        maxQuickTimeEvents.SelectRandom();

        canGetNextKey = true;
        checkQTETimer = false;

        QTEBuffer = maxQTEBuffer.GetRandom();
        QTETimer = maxQTETimer;

        currentPasses = 0;

        QTETextMesh.text = "";
        QTETimerImage.enabled = true;
        QTETimerImage.fillAmount = 0;
    }

    void OnDisable()
    {
        canGetNextKey = true;

        QTETextMesh.text = "";
        QTETimerImage.enabled = false;
    }

    void Update()
    {
        if (canGetNextKey)
        {
            GetNextKey();
        }

        if (checkQTETimer)
        {
            QTETimer -= Time.deltaTime;

            QTETimerImage.fillAmount = (((QTETimer - 0) * (1 - 0)) / (maxQTETimer - 0)) + 0;
            
            if (QTETimer <= 0)
            {
                Fail();
            }
        }
    }

    void GetNextKey()
    {
        CheckPass();
        
        QTEBuffer -= Time.deltaTime;
        if (QTEBuffer <= 0)
        {
            currentKey = (QTEOptions)Random.Range(0, (int)QTEOptions.QTEOptionsSize);
            Debug.Log(currentKey);

            switch (currentKey)
            {
                case QTEOptions.North:
                    QTETextMesh.text = "W";
                    break;
                case QTEOptions.South:
                    QTETextMesh.text = "S";
                    break;
                case QTEOptions.West:
                    QTETextMesh.text = "A";
                    break;
                case QTEOptions.East:
                    QTETextMesh.text = "D";
                    break;
            }

            checkQTETimer = true;
            QTETimer = maxQTETimer;
            QTETimerImage.fillAmount = 1;

            canGetNextKey = false;
        }
    }

    void Fail()
    {
        Debug.Log("Fail");
        PlayerModel.Instance.ChangeState(PlayerModel.PlayerState.Moving);
        this.enabled = false;
    }

    void CheckPass()
    {
        if (currentPasses == maxQuickTimeEvents.selected)
        {
            Debug.Log("Passed!");
            PlayerModel.Instance.ChangeState(PlayerModel.PlayerState.Moving);
            this.enabled = false;
        }

        QTETextMesh.text = "";
        QTETimerImage.fillAmount = 0;
    }

    void OnQuickTimeNorth(InputValue inputValue)
    {
        if (!canGetNextKey)
        {
            if (currentKey == QTEOptions.North)
            {
                currentPasses++;

                checkQTETimer = false;
                canGetNextKey = true;

                QTEBuffer = maxQTEBuffer.GetRandom();

                Debug.Log("Good!");
            }
            else
            {
                Fail();
            }
        }
    }

    void OnQuickTimeSouth(InputValue inputValue)
    {
        if (!canGetNextKey)
        {
            if (currentKey == QTEOptions.South)
            {
                currentPasses++;

                checkQTETimer = false;
                canGetNextKey = true;

                QTEBuffer = maxQTEBuffer.GetRandom();

                Debug.Log("Good!");
            }
            else
            {
                Fail();
            }
        }
    }

    void OnQuickTimeWest(InputValue inputValue)
    {
        if (!canGetNextKey)
        {
            if (currentKey == QTEOptions.West)
            {
                currentPasses++;

                checkQTETimer = false;
                canGetNextKey = true;

                QTEBuffer = maxQTEBuffer.GetRandom();

                Debug.Log("Good!");
            }
            else
            {
                Fail();
            }
        }
    }

    void OnQuickTimeEast(InputValue inputValue)
    {
        if (!canGetNextKey)
        {
            if (currentKey == QTEOptions.East)
            {
                currentPasses++;

                checkQTETimer = false;
                canGetNextKey = true;

                QTEBuffer = maxQTEBuffer.GetRandom();

                Debug.Log("Good!");
            }
            else
            {
                Fail();
            }
        }
    }
}

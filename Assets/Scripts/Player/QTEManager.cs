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
    public float QTEBuffer;
    public float QTETimer;
    public RangeInt maxQuickTimeEvents;

    private float maxQTEBuffer = 0;
    private float maxQTETimer = 0;
    private int currentPasses = 0;

    private bool canGetNextKey = true;
    private bool checkQTETimer = false;

    void Awake()
    {
        maxQTEBuffer = QTEBuffer;
        maxQTETimer = QTETimer;
    }

    void OnEnable()
    {
        maxQuickTimeEvents.SelectRandom();

        canGetNextKey = true;
        checkQTETimer = false;

        QTEBuffer = maxQTEBuffer;
        QTETimer = maxQTETimer;

        currentPasses = 0;
    }

    void OnDisable()
    {
        canGetNextKey = true;
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

            //Display Button

            checkQTETimer = true;
            QTETimer = maxQTETimer;

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
                QTEBuffer = maxQTEBuffer;

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
                QTEBuffer = maxQTEBuffer;

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
                QTEBuffer = maxQTEBuffer;

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
                QTEBuffer = maxQTEBuffer;

                Debug.Log("Good!");
            }
            else
            {
                Fail();
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public int waitingForKey;
    public int correctKey;
    public int countingDown;

    void Update()
    {
        if (waitingForKey == 0)
        {
            currentKey = (QTEOptions)Random.Range(0, (int)QTEOptions.QTEOptionsSize);
            countingDown = 1;

            //Display Key Here

            waitingForKey = 1;
        }


    }
}

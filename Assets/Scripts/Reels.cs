using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Reels : MonoBehaviour
{
    private int randomValue;
    public float timeInterval;

    public bool spinStopped;
    public static bool manualStopped;
    public string stoppedSlot;

    public string[] symbols;
    private float[] validYCoordinates;
    private float startY = -5.0f;
    private int[] result;
    private bool processedResult;
    private bool isSlowedDown;
    private float slowDownTimer;


    // Start is called before the first frame update
    void Start()
    {
        spinStopped = true;
        manualStopped = false;
        timeInterval = 3.0f;
        slowDownTimer = 3.0f;
        isSlowedDown = false;
        GameControl.ButtonPressed += StartRotating;
        CalculateValidYCoordinates();
        result = new int[3];
        RandomizeTimeInterval();
        processedResult = false;
    }

    private void RandomizeTimeInterval()
    {
        randomValue = UnityEngine.Random.Range(0,5);
        switch (randomValue % 3)
        {
            case 0:
                timeInterval = 3.0f;
                break;
            case 1:
                timeInterval = 3.5f;
                break;
            case 2:
                timeInterval = 4.0f;
                break;
            case 3:
                timeInterval = 4.5f;
                break;
            case 4:
                timeInterval = 5.0f;
                break;
            case 5:
                timeInterval = 5.5f;
                break;
        }
    }

    private void CalculateValidYCoordinates()
    {
        validYCoordinates = new float[symbols.Length-2];
        validYCoordinates[0] = startY;
        for(int i=1; i<validYCoordinates.Length; i++)
        {
            validYCoordinates[i] = validYCoordinates[i - 1] + 2.2f;
        }
    }

    public int[] GetResult()
    {
        return result;
    }

    /*public float[] GetValidYCoordinates()
    {
        return validYCoordinates;
    }*/

    private void StartRotating()
    {
        stoppedSlot = "";
        StartCoroutine("Rotate");
    }

    private IEnumerator Rotate()
    {
        spinStopped = false;
        timeInterval = 0.5f;
        
        for(int i=0;i<30;i++)
        {
            if (transform.position.y >= 6.0f)
                transform.position = new Vector2(transform.position.x, -5.0f);
            transform.position = new Vector2(transform.position.x, transform.position.y + 1.25f);
            yield return new WaitForSeconds(timeInterval);
        }

        randomValue = UnityEngine.Random.Range(60, 100);

        if (!manualStopped)
        {
            switch (randomValue % 3)
            {
                case 1:
                    randomValue += 2;
                    break;
                case 2:
                    randomValue += 1;
                    break;
            }

            for (int i = 0; i < randomValue; i++)
            {
                if (transform.position.y >= 6.0f)
                    transform.position = new Vector2(transform.position.x, -5.0f);
                transform.position = new Vector2(transform.position.x, transform.position.y + 0.2f);

                if (i > Mathf.RoundToInt(randomValue * 0.25f))
                    timeInterval = 0.05f;
                if (i > Mathf.RoundToInt(randomValue * 0.5f))
                    timeInterval = 0.1f;
                if (i > Mathf.RoundToInt(randomValue * 0.75f))
                    timeInterval = 0.15f;
                if (i > Mathf.RoundToInt(randomValue * 0.95f))
                    timeInterval = 0.2f;

                yield return new WaitForSeconds(timeInterval);
            }
        }

        //normalize y valyue for transform
        int index = 0;
        while(transform.position.y>validYCoordinates[index] && index<validYCoordinates.Length-1)
        {
            index++;
        }
        if(index==0)
            transform.position = new Vector2(transform.position.x, validYCoordinates[index]);
        else if ((validYCoordinates[index] - transform.position.y) <= (transform.position.y - validYCoordinates[index - 1]))
            transform.position = new Vector2(transform.position.x, validYCoordinates[index]);
        else
            transform.position = new Vector2(transform.position.x, validYCoordinates[index-1]);

        if (transform.position.y == -3.5f)
            stoppedSlot = "Diamond";
        else if (transform.position.y == -2.75f)
            stoppedSlot = "Crown";
        else if (transform.position.y == -2f)
            stoppedSlot = "Melon";
        else if (transform.position.y == -1.25f)
            stoppedSlot = "Bar";
        else if (transform.position.y == -0.5f)
            stoppedSlot = "Seven";
        else if (transform.position.y == 0.25f)
            stoppedSlot = "Cherry";
        else if (transform.position.y == 1f)
            stoppedSlot = "Lemon";
        else if (transform.position.y == 1 / 75f)
            stoppedSlot = "Diamond";

        spinStopped = true;
    }

    private void OnDestroy()
    {
        GameControl.ButtonPressed -= StartRotating;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (GameControl.isSpinning&&timeInterval>=0)
        {
            spinStopped = false;
            processedResult = false;
            isSlowedDown = false;
            timeInterval -= Time.deltaTime;
                if (transform.position.y >= 6.0f)
                    transform.position = new Vector2(transform.position.x, -5.0f);
                transform.position = new Vector2(transform.position.x, transform.position.y + 0.4f * timeInterval);
        }
        else
        {
            if (!spinStopped)
            {
                if (!isSlowedDown)
                {
                    randomValue = UnityEngine.Random.Range(60, 100);

                    switch (randomValue % 3)
                    {
                        case 1:
                            randomValue += 2;
                            break;
                        case 2:
                            randomValue += 1;
                            break;
                    }

                    for (int i = 0; i < randomValue; i++)
                    {
                        if (transform.position.y >= 6.0f)
                            transform.position = new Vector2(transform.position.x, -5.0f);
                        transform.position = new Vector2(transform.position.x, transform.position.y + 0.2f);

                        if (i > Mathf.RoundToInt(randomValue * 0.25f))
                            slowDownTimer = 0.05f;
                        if (i > Mathf.RoundToInt(randomValue * 0.5f))
                            slowDownTimer = 0.1f;
                        if (i > Mathf.RoundToInt(randomValue * 0.75f))
                            slowDownTimer = 0.15f;
                        if (i > Mathf.RoundToInt(randomValue * 0.95f))
                            slowDownTimer = 0.2f;
                    }
                    isSlowedDown = true;
                }
                if (slowDownTimer < 0)
                {
                    int index = 0;
                    while (transform.position.y > validYCoordinates[index] && index < validYCoordinates.Length - 1)
                    {
                        index++;
                    }
                    while (transform.position.y < validYCoordinates[index])
                    {
                        transform.position = new Vector2(transform.position.x, transform.position.y + 0.02f);
                    }
                    transform.position = new Vector2(transform.position.x, validYCoordinates[index]);
                    /*if (index==0)
                    {
                        transform.position = new Vector2(transform.position.x, validYCoordinates[index]);
                    }
                    else if ((validYCoordinates[index] - transform.position.y) <= (transform.position.y - validYCoordinates[index - 1]))
                    {
                        while (transform.position.y < validYCoordinates[index])
                        {
                            transform.position = new Vector2(transform.position.x, transform.position.y + 0.02f);
                        }
                        transform.position = new Vector2(transform.position.x, validYCoordinates[index]);
                    }
                    else
                    {
                        while (transform.position.y > validYCoordinates[index-1])
                        {
                            transform.position = new Vector2(transform.position.x, transform.position.y - 0.02f);
                        }
                        transform.position = new Vector2(transform.position.x, validYCoordinates[index]);
                    }*/
                    /*if (index == 0)
                        transform.position = new Vector2(transform.position.x, validYCoordinates[index]);
                    else if ((validYCoordinates[index] - transform.position.y) <= (transform.position.y - validYCoordinates[index - 1]))
                        transform.position = new Vector2(transform.position.x, validYCoordinates[index]);
                    else
                        transform.position = new Vector2(transform.position.x, validYCoordinates[index - 1]);*/
                    RandomizeTimeInterval();
                    if (!processedResult)
                    {
                        ProcessResult();
                        processedResult = true;
                    }
                    spinStopped = true;
                    GameControl.isSpinning = false;
                    GameControl.isStopping = false;
                }
                else
                {
                    slowDownTimer -= Time.deltaTime;
                }
            }
        }

    }

    private void ProcessResult()
    {
        for(int i=0; i<validYCoordinates.Length; i++)
        {
            if(transform.position.y == validYCoordinates[i])
            {
                result[0] = i;
                result[1] = i+1;
                result[2] = i+2;
                break;
            }
        }
    }

}

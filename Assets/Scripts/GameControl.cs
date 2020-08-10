using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UI;

public class GameControl : MonoBehaviour
{

    [SerializeField]
    private Text winningText;

    [SerializeField]
    private Reels[] reels;

    private int prizeValue;

    private bool resultsChecked = false;
    public static bool isSpinning;
    private static int[,] payoutValue;

    public int playerBalance;

    [SerializeField]
    private Text balanceText;

    private int currentBet;

    [SerializeField]
    private Text betText;

    public static bool isStopping;

    private static AudioSource audioSource;
    private static String[] symbols;
    public AudioClip buttonClip;
    public AudioClip errorClip;
    public AudioClip winClip;

    // Start is called before the first frame update
    void Start()
    {
        isSpinning = false;
        resultsChecked = true;
        playerBalance = int.Parse(balanceText.text);
        currentBet = 0;
        betText.text = "" + currentBet;
        isStopping = false;
        audioSource = GetComponent<AudioSource>();
        symbols = reels[0].symbols;
    }

    // Update is called once per frame
    void Update()
    {
        if (!reels[0].IsSpinStopped() && !reels[1].IsSpinStopped() && !reels[2].IsSpinStopped() && !reels[3].IsSpinStopped() && !reels[4].IsSpinStopped())
        {
            prizeValue = 0;
            winningText.text = "Winnings: "+prizeValue;
            resultsChecked = false;
        }

        if(reels[0].IsSpinStopped() && reels[1].IsSpinStopped() && reels[2].IsSpinStopped() && reels[3].IsSpinStopped() && reels[4].IsSpinStopped() && !resultsChecked)
        {
            CheckResults();
            winningText.enabled = true;
            winningText.text = "Winnings: " + prizeValue;
            playerBalance += prizeValue;
            balanceText.text = "" + playerBalance;
        }

    }

    public static void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }

    private void OnMouseDown()
    {
        currentBet = int.Parse(betText.text) * 20;
        playerBalance = int.Parse(balanceText.text);
        if (currentBet == 0)
            PlaySound(errorClip);
        if (!isStopping)
        {
            PlaySound(buttonClip);
            if (playerBalance >= currentBet && !isSpinning && currentBet != 0)
            {
                isSpinning = true;
                playerBalance -= currentBet;
                balanceText.text = "" + playerBalance;
            }
            else if (isSpinning)
            {
                isSpinning = false;
                isStopping = true;
            }
        }
        else
            PlaySound(errorClip);
    }

    private void SetPayoutValue()
    {
        payoutValue = new int[reels[0].symbols.Length, 3];
        payoutValue[0,0] = 1;
        payoutValue[0,1] = 5;
        payoutValue[0,2] = 10;

        for (int i=1; i<reels[0].symbols.Length; i++)
        {
            payoutValue[i, 0] = payoutValue[i-1,0]+1;
            payoutValue[i, 1] = payoutValue[i - 1, 1]+2;
            payoutValue[i, 2] = payoutValue[i - 1, 2]+3;
        }
    }

    public void CheckResults()
    {
        SetPayoutValue();
        //20 payout lines
        prizeValue += ComputePayout(new int[] {reels[0].GetResult()[0], reels[1].GetResult()[0], reels[2].GetResult()[0], reels[3].GetResult()[0], reels[4].GetResult()[0] });
        prizeValue += ComputePayout(new int[] { reels[0].GetResult()[1], reels[1].GetResult()[1], reels[2].GetResult()[1], reels[3].GetResult()[1], reels[4].GetResult()[1] });
        prizeValue += ComputePayout(new int[] { reels[0].GetResult()[2], reels[1].GetResult()[2], reels[2].GetResult()[2], reels[3].GetResult()[2], reels[4].GetResult()[2] });
        prizeValue += ComputePayout(new int[] { reels[0].GetResult()[0], reels[1].GetResult()[1], reels[2].GetResult()[2], reels[3].GetResult()[1], reels[4].GetResult()[0] });
        prizeValue += ComputePayout(new int[] { reels[0].GetResult()[2], reels[1].GetResult()[1], reels[2].GetResult()[0], reels[3].GetResult()[1], reels[4].GetResult()[2] });
        prizeValue += ComputePayout(new int[] { reels[0].GetResult()[1], reels[1].GetResult()[1], reels[2].GetResult()[0], reels[3].GetResult()[1], reels[4].GetResult()[1] });
        prizeValue += ComputePayout(new int[] { reels[0].GetResult()[1], reels[1].GetResult()[1], reels[2].GetResult()[2], reels[3].GetResult()[1], reels[4].GetResult()[1] });
        prizeValue += ComputePayout(new int[] { reels[0].GetResult()[1], reels[1].GetResult()[0], reels[2].GetResult()[1], reels[3].GetResult()[0], reels[4].GetResult()[1] });
        prizeValue += ComputePayout(new int[] { reels[0].GetResult()[1], reels[1].GetResult()[2], reels[2].GetResult()[1], reels[3].GetResult()[2], reels[4].GetResult()[1] });
        prizeValue += ComputePayout(new int[] { reels[0].GetResult()[0], reels[1].GetResult()[1], reels[2].GetResult()[0], reels[3].GetResult()[1], reels[4].GetResult()[0] });
        prizeValue += ComputePayout(new int[] { reels[0].GetResult()[2], reels[1].GetResult()[1], reels[2].GetResult()[2], reels[3].GetResult()[1], reels[4].GetResult()[2] });
        prizeValue += ComputePayout(new int[] { reels[0].GetResult()[1], reels[1].GetResult()[0], reels[2].GetResult()[1], reels[3].GetResult()[2], reels[4].GetResult()[1] });
        prizeValue += ComputePayout(new int[] { reels[0].GetResult()[1], reels[1].GetResult()[2], reels[2].GetResult()[1], reels[3].GetResult()[0], reels[4].GetResult()[1] });
        prizeValue += ComputePayout(new int[] { reels[0].GetResult()[2], reels[1].GetResult()[1], reels[2].GetResult()[0], reels[3].GetResult()[0], reels[4].GetResult()[0] });
        prizeValue += ComputePayout(new int[] { reels[0].GetResult()[2], reels[1].GetResult()[1], reels[2].GetResult()[1], reels[3].GetResult()[1], reels[4].GetResult()[1] });
        prizeValue += ComputePayout(new int[] { reels[0].GetResult()[1], reels[1].GetResult()[0], reels[2].GetResult()[0], reels[3].GetResult()[0], reels[4].GetResult()[0] });
        prizeValue += ComputePayout(new int[] { reels[0].GetResult()[0], reels[1].GetResult()[1], reels[2].GetResult()[1], reels[3].GetResult()[1], reels[4].GetResult()[1] });
        prizeValue += ComputePayout(new int[] { reels[0].GetResult()[1], reels[1].GetResult()[2], reels[2].GetResult()[2], reels[3].GetResult()[2], reels[4].GetResult()[2] });
        prizeValue += ComputePayout(new int[] { reels[0].GetResult()[0], reels[1].GetResult()[0], reels[2].GetResult()[1], reels[3].GetResult()[1], reels[4].GetResult()[1] });
        prizeValue += ComputePayout(new int[] { reels[0].GetResult()[0], reels[1].GetResult()[0], reels[2].GetResult()[0], reels[3].GetResult()[1], reels[4].GetResult()[1] });

        prizeValue *= currentBet;
        if (prizeValue > 0)
            PlaySound(winClip);

        currentBet = 0;
        betText.text = "" + currentBet;
        resultsChecked = true;
    }

    private int ComputePayout(int[] line)
    {
        int numMatches = 1;
        int indexMatch = 0;
        for (int i = 0; i < line.Length; i++)
        {
            for (int j = 0; j < line.Length; j++)
            {
                if (i != j)
                {
                    if (line[i] == line[j])
                        numMatches++;
                }
            }
            if (numMatches >= 3)
            {
                indexMatch = line[i];
                break;
            }
            numMatches = 1;
        }

        if (numMatches == 3)
            return payoutValue[indexMatch, 0];
        else if (numMatches == 4)
            return payoutValue[indexMatch, 1];
        else if (numMatches == 5)
            return payoutValue[indexMatch, 2];

        return 0;
    }
}

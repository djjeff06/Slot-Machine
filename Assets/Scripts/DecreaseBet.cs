using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DecreaseBet : MonoBehaviour
{

    [SerializeField]
    private Text betText;

    public AudioClip clip;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        if ((int.Parse(betText.text) - 1) >= 0)
        {
            betText.text = "" + (int.Parse(betText.text) - 1);
            GameControl.PlaySound(clip);
        }
    }
}

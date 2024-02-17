using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformShrink : MonoBehaviour
{
    [SerializeField]
    private float timeToShrink = 100f;
    [SerializeField]
    private float startSize = 10f;

    private float startTime;
    private float currentTime;

    private bool gameStarted = false;

    // Start is called before the first frame update
    void Start()
    {

        transform.localScale = new Vector3(startSize, 0.2f, startSize);
        startTime = Time.time;
        gameStarted = true;
    }

    // Update is called once per frame
    void Update()
    {
        currentTime = Time.time;
        if (gameStarted) //replace with LaserMicroGameController.gameStarted
        {
            float currentSize = (1 - ((currentTime - startTime) / timeToShrink)) * startSize;
            transform.localScale = new Vector3(currentSize, 0.2f, currentSize);
        }
    }
}

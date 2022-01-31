﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject BowlingBall;
    public GameObject Pins;
    public UiManager UIManager;
    private GameObject NextPins;
    private GameObject NextBall;

    private int _roundScore = 0;

    public float PinHeight = 4;

    private bool isSecondThrow = false;

    void Awake()
    {
        Instance = this;
    }

    public void PinFall()
    {
        _roundScore++;
        if (_roundScore == 10)
        {
            UIManager.SetSubText(isSecondThrow ? "spare" : "STRIKE!");
        }
        UIManager.SetScoreText(_roundScore);
    }

    void Start()
    {
        NextPins = GameObject.Instantiate(Pins);
        NextPins.SetActive(false);
        NextBall = GameObject.Instantiate(BowlingBall);
        NextBall.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (BowlingBall.transform.position.y < -20f)
        {
            if (isSecondThrow)
            {
                Reset();
            }
            else
            {
                FirstThrowDone();
            }
        }
    }

    private void FirstThrowDone()
    {

        if (_roundScore == 10)
        {
            // strike, finish round
            Reset();
            return;
        }

        isSecondThrow = true;

        ResetBall();

        GameObject[] pins = GameObject.FindGameObjectsWithTag("PIN");
        Debug.Log(pins.Length);
        foreach (var pin in pins)
        {
            if (pin.GetComponent<Pin>().hasFallen)
            {
                pin.GetComponent<Pin>().Destroy();
            }
        }
    }

    private void Reset()
    {
        ResetBall();
        ResetPins();
        _roundScore = 0;
        UIManager.Reset();
        isSecondThrow = false;
    }

    private void ResetBall()
    {
        GameObject.Destroy(BowlingBall);
        NextBall.SetActive(true);
        BowlingBall = NextBall;
        NextBall = GameObject.Instantiate(BowlingBall);
        NextBall.SetActive(false);
    }

    private void ResetPins()
    {
        GameObject.Destroy(Pins);
        NextPins.SetActive(true);
        Pins = NextPins;
        NextPins = GameObject.Instantiate(Pins);
        NextPins.SetActive(false);
    }
}

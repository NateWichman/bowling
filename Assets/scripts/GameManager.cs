
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public bool BallIsThrowing = false;
    public UnityEvent Resetting;
    public GameObject BowlingBall;
    public GameObject Pins;
    public UiManager UIManager;
    private GameObject NextPins;
    private GameObject NextBall;
    public CameraFollow cameraFollow;

    public GameObject Panel;

    private Score _score;

    private int _roundScore = 0;
    private int _shotScore = 0;

    public float PinHeight = 4;

    private bool isSecondThrow = false;

    void Awake()
    {
        Instance = this;
        _score = new Score();
    }

    public void PinFall()
    {
        _roundScore++;
        _shotScore++;

        if (_shotScore == 10)
        {
            UIManager.SetSubText(isSecondThrow ? "Spare" : "STRIKE!");
        }
    }

    void Start()
    {
        NextPins = GameObject.Instantiate(Pins);
        NextPins.SetActive(false);
        NextBall = GameObject.Instantiate(BowlingBall);
        NextBall.SetActive(false);
        Resetting = new UnityEvent();
    }

    // Update is called once per frame
    void Update()
    {
        if (BowlingBall.transform.position.y < -6f)
        {
            var a = BowlingBall.GetComponent<AudioSource>();
            if (a.isPlaying)
                a.Stop();
        }
        if (BowlingBall.transform.position.y < -100f)
        {
            RoundOver();
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
        _shotScore = 0;
        UIManager.Reset();
        isSecondThrow = false;
    }

    private void ResetBall()
    {
        if (_score.IsGameOver())
        {
            _score = new Score();
        }
        _score.OnShot(_roundScore);
        _roundScore = 0;
        UIManager.DisplayFrames(_score.GetFrames());


        GameObject.Destroy(BowlingBall);
        NextBall.SetActive(true);
        BowlingBall = NextBall;
        NextBall = GameObject.Instantiate(BowlingBall);
        NextBall.SetActive(false);
        Resetting.Invoke();
        BallIsThrowing = false;
        Panel.SetActive(true);
    }

    private void ResetPins()
    {
        GameObject.Destroy(Pins);
        NextPins.SetActive(true);
        Pins = NextPins;
        NextPins = GameObject.Instantiate(Pins);
        NextPins.SetActive(false);
        Panel.SetActive(false);
        BallIsThrowing = false;
        Panel.SetActive(true);
    }

    public void OnThrow()
    {
        Panel.SetActive(false);
        BallIsThrowing = true;
        cameraFollow.FollowBall();
        InputService.Instance.ClearAll();
    }

    private void RoundOver()
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

    public void ForceBallFall()
    {
        RoundOver();
    }
}

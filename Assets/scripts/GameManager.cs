
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

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
    public GameObject CustomizePanel;

    public GameObject EndGamePanel;

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
        EndGamePanel.SetActive(false);
        Panel.SetActive(true);
        CustomizePanel.SetActive(false);
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
        if (BowlingBall.transform.position.y < -150f)
        {
            RoundOver();
        }
    }


    public void OnCustomize()
    {
        CustomizePanel.SetActive(true);
        Panel.SetActive(false);
    }

    public void OnEndCustomize()
    {
        CustomizePanel.SetActive(false);
        Panel.SetActive(true);
        GameObject.FindGameObjectWithTag("CUSTOMIZE_BALL").GetComponent<CustomizeIcon>().UpdateColor();
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

        if (!_score.IsGameOver())
        {
            ResetPins();
        }
        _shotScore = 0;
        isSecondThrow = false;
    }

    private void EndGame()
    {
        EndGamePanel.SetActive(true);
        Panel.SetActive(false);
        int highscore = PlayerPrefs.GetInt("HIGH_SCORE");
        ;
        if (_score.GetTotal() > highscore)
        {
            UIManager.OnHighScore();
            UIManager.SetHighScore(_score.GetTotal());
            PlayerPrefs.SetInt("HIGH_SCORE", _score.GetTotal());
        }
        else
        {
            UIManager.SetHighScore(highscore);
        }

        UIManager.SetEndGameTotal(_score.GetTotal());
    }

    public void NewGame()
    {
        _score = new Score();
        ResetPins();
        _shotScore = 0;
        isSecondThrow = false;
        UIManager.HideHighscoreText();
        EndGamePanel.SetActive(false);
        Panel.SetActive(true);
    }

    private void ResetBall()
    {
        _score.OnShot(_roundScore);
        _roundScore = 0;


        if (_score.IsGameOver())
        {
            UnlockManager.Instance.UpdateUnlocks(_score);
            try
            {
                AdManager.Instance.ShowIntersitialAd();
            }
            catch (System.Exception e)
            {
                Debug.LogError(e.Message);
            }
        }

        var frames = _score.GetFrames();
        UIManager.DisplayFrames(_score.GetFrames());


        GameObject.Destroy(BowlingBall);
        NextBall.SetActive(true);
        BowlingBall = NextBall;
        NextBall = GameObject.Instantiate(BowlingBall);
        NextBall.SetActive(false);
        Resetting.Invoke();
        BallIsThrowing = false;

        if (!_score.IsGameOver())
        {
            Panel.SetActive(true);
        }
        else
        {
            // is game over
            EndGame();
        }

        // show ad after 5th frame
        if (frames.Count == 5 && (frames.Last().shots.Count == 2 || frames.Last().isStrike))
        {
            try
            {
                AdManager.Instance.ShowIntersitialAd();
            }
            catch (System.Exception e)
            {
                Debug.LogError(e.Message);
            }
        }

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

    public void OnRate()
    {
        PlayerPrefs.SetInt("HAS_RATED", 1);
        Application.OpenURL("https://play.google.com/store/apps/details?id=com.StoneBison.Bowling");
    }

    public void SetMaterial(Material material)
    {
        PlayerPrefs.SetString("BALL", material.name);
        GameObject.FindGameObjectWithTag("BALL").GetComponent<Renderer>().material = material;
    }

}

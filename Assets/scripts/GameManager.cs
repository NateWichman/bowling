
using UnityEngine;
using UnityEngine.Events;
using System.Linq;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public bool BallIsThrowing = false;
    public UnityEvent Resetting;
    public UnityEvent SkinChange;
    public GameObject BowlingBall;
    public GameObject Pins;
    public UiManager UIManager;
    private GameObject NextPins;
    private GameObject NextBall;
    public CameraFollow cameraFollow;
    public ParticleSystem StrikePartice;
    public GameObject explosionPoint;
    public ParticleSystem SpareParticle;

    public GameObject Panel;
    public GameObject CustomizePanel;

    public GameObject EndGamePanel;

    private Score _score;

    private int _roundScore = 0;
    private int _shotScore = 0;

    public float PinHeight = 4;

    private bool isSecondThrow = false;

    private int _strikesInARow = 0;

    void Awake()
    {
        Instance = this;
        _score = new Score();
        SkinChange = new UnityEvent();
    }

    public void PinFall()
    {
        _roundScore++;
        _shotScore++;

        if (_shotScore == 10)
        {
            if (!isSecondThrow) {
                _strikesInARow++;
                StrikeAnimation();
                var memory = PlayerPrefs.GetInt("STRIKES_ROW", 0);
                if (_strikesInARow > memory) {
                    PlayerPrefs.SetInt("STRIKES_ROW", _strikesInARow);
                }
            } else {
                StartCoroutine(SpareAnimation());
                _strikesInARow = 0;
            }
            UIManager.SetSubText(isSecondThrow ? "Spare" : "STRIKE!");
        } else {
            _strikesInARow = 0;
        }
    }

    IEnumerator SpareAnimation()
    {
        SpareParticle.Play(true);
        yield return new WaitForSeconds(2);
        SpareParticle.Stop();
    }

    private void StrikeAnimation()
    {
        StrikePartice.Play(true);
        var pins = GameObject.FindGameObjectsWithTag("PIN");
        foreach (var pin in pins)
        {
            Debug.Log(pin);
            pin.GetComponent<Rigidbody>().AddExplosionForce(300000f, explosionPoint.transform.position, 10f);
        }
    }

    void Start()
    {
        NextPins = GameObject.Instantiate(Pins);
        NextPins.SetActive(false);
        NextBall = GameObject.Instantiate(BowlingBall);
        NextBall.SetActive(false);
        Resetting = new UnityEvent();

        SkinChange.AddListener(Test);
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
        CustomizeService.Instance.InitUnlocks();
        CustomizePanel.SetActive(true);
        Panel.SetActive(false);
    }

    public void OnEndCustomize()
    {
        CustomizePanel.SetActive(false);
        Panel.SetActive(true);
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
        int highscore = PlayerPrefs.GetInt("HIGH_SCORE", 0);
        int numGamesPlayed = PlayerPrefs.GetInt("NUM_GAMES_PLAYED", 0);
        PlayerPrefs.SetInt("NUM_GAMES_PLAYED", numGamesPlayed + 1);

        if (_score.GetTotal() == 0)
        {
            PlayerPrefs.SetInt("IS_GHOST", 1);
        }
        if (_score.GetTotal() == 9)
        {
            PlayerPrefs.SetInt("IS_POOL_BALL", 1);
        }

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
        OnEndCustomize();
        SkinChange.Invoke();
    }

    private void Test()
    {
        Debug.Log("TESTING");
    }
}

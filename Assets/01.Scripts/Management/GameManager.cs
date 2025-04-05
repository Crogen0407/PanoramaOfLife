using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Cinemachine;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    //Managers
    UIManager _uiManager;
    PoolManager _poolManager;

    private Transform _camTrm;
    private CinemachineImpulseSource _cinemachineImpulseSource;

    public AudioClip stageSound;
    public AudioClip bossSound;

    AudioSource audioSource;
    [SerializeField] private AudioSource backgroundAudioSource;
    [SerializeField] private GameObject[] stages;

    //UI
    Slider _playerHpSlider;
    [SerializeField] private Slider _bossHpSlider;

    //InGame
    public bool islessdamagedMode;
    public bool isPowerUp;
    public bool isLastStage = false;
    private bool _isGameOver;
    private bool _isGameClear;

    private Image timeLineImage;
    private float timeLine = 0;

    public bool _isFlipY;
    private float hp=80;
    private float bossHp = 6000;
    private const float maxHp=80;
    private const float maxBossHp = 6000;

    GameObject currentStage;

    public Player _player;
    Boss _boss;

    #region Property
    public float TimeLine
	{
        get => timeLine;
		set
		{
            timeLine = value;
            timeLineImage.fillAmount = timeLine/120;
            if(timeLineImage.fillAmount >= 1)
			{
                isLastStage = true;
                _player.GetComponent<BoxCollider2D>().enabled = false;
                StartCoroutine(_uiManager.PanelfadeIn());
                StartCoroutine(GameClear());
            }
        }
	}

    public bool IsGameOver
	{
        get => _isGameOver;
        set
        {
            _isGameOver = value;
            if(value==true)
			{
                CameraShake(40);
                StartCoroutine(_uiManager.PanelfadeIn());
                _player.GetComponent<BoxCollider2D>().enabled = false;
                audioSource.Play();
            }
        }
	}

    public bool IsGameClear
    {
        get => _isGameClear;
        set
        {
            if (value == true)
            {
                CameraShake(40);
                StartCoroutine(_uiManager.PanelfadeIn());
                _boss.GetComponent<PolygonCollider2D>().enabled = false;
                StartCoroutine(_boss.GetComponent<BossPattern>().KillBoss());
                audioSource.Play();
            }
            _isGameClear = value;
        }
    }
    public float Hp
	{
        get => hp;
		set
		{
            hp = value;
            hp = Mathf.Clamp(hp, 0, 100);
            _playerHpSlider.value = hp / maxHp;
            if(hp <= 0.5f)
			{
                _player.HalfHp();
            }
            if(hp <= 0 && IsGameOver == false)
			{
                IsGameOver = true;
                DOTween.KillAll();
            }
        }
    }

    public float BossHp
	{
        get => bossHp;
		set
		{
            bossHp = value;
            _bossHpSlider.value = bossHp / maxBossHp;
            if (bossHp <= 0 && IsGameClear == false)
            {
                IsGameClear = true;
                DOTween.KillAll();
            }
        }
	}

	private void Awake()
	{
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else
            Destroy(gameObject);
    }

	#endregion
	private void OnEnable()
	{
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

	private void OnDisable()
	{
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
        islessdamagedMode = false;
        isPowerUp = false;
        Scene currScene = SceneManager.GetActiveScene();
        if (currScene.name == "GameScene")
        {
            backgroundAudioSource.volume = 0.75f * 1 * PlayerPrefs.GetFloat("Sound");

            backgroundAudioSource.clip = stageSound;
            backgroundAudioSource.Play();
            timeLine = 0;
            hp = maxHp;
            _uiManager = UIManager.Instance;
            _player = GameObject.Find("Player").GetComponent<Player>();
            _playerHpSlider = GameObject.Find("PlayerCanvas").transform.Find("PlayerPenel").transform.Find("HpSlider").GetComponent<Slider>();
            _camTrm = GameObject.Find("CMvcam").transform;
            _poolManager = GameObject.Find("PoolManager").GetComponent<PoolManager>();
            _player.GetComponent<BoxCollider2D>().enabled = true;
            _cinemachineImpulseSource = GameObject.Find("CinemachineImpulseSource").GetComponent<CinemachineImpulseSource>();
            timeLineImage = GameObject.Find("PlayerCanvas").transform.Find("TimeLineImage").GetComponent<Image>();
            _isFlipY = false;
            //Init
            _poolManager.Init();
            _player.Init();
            StartCoroutine(_uiManager.PanelfadeOut());
            GameObject.Find("Player").GetComponent<PlayerSkillCtrl>().Init();
            if (!isLastStage)
            {
                backgroundAudioSource.volume = 0.4f * 1 * PlayerPrefs.GetFloat("Sound");
                backgroundAudioSource.clip = stageSound;
                backgroundAudioSource.Play();
                currentStage = Instantiate(stages[0], transform.position, Quaternion.identity);
                currentStage.transform.Find("SpawnManager").GetComponent<SpawnManager>().Init();
            }
            else
            {
                backgroundAudioSource.volume = 0.4f * 1 * PlayerPrefs.GetFloat("Sound");
                backgroundAudioSource.clip = bossSound;
                backgroundAudioSource.Play();
                bossHp = maxBossHp;
                currentStage = Instantiate(stages[1], transform.position, Quaternion.identity);
                timeLineImage.gameObject.SetActive(false);
                _boss = currentStage.transform.Find("Boss").GetComponent<Boss>();
                _boss.Init();
                _bossHpSlider = currentStage.transform.Find("Canvas").Find("HpSlider").GetComponent<Slider>();
                _player._bossPattern = currentStage.transform.Find("Boss").GetComponent<BossPattern>();

            }

        }
		else
		{
            backgroundAudioSource.Pause();
        }

        audioSource = GetComponent<AudioSource>();
        audioSource.volume = 1 * PlayerPrefs.GetFloat("Sound"); ;

    }

    public void CameraShake(float power)
	{
        _cinemachineImpulseSource.GenerateImpulse(power);
    }

    public void FlipScreen()
	{
        Quaternion qua = _camTrm.transform.rotation;
        if (qua.z==0)
            qua = Quaternion.Euler(new Vector3(0, 0, -180));
        else
            qua = Quaternion.Euler(new Vector3(0, 0, 0));
        _camTrm.transform.rotation = qua;
        _isFlipY = !_isFlipY;
    }

	private void Update()
	{
        if(Input.GetMouseButtonDown(0))
		{
            Cursor.visible = false;
        }
        if(!isLastStage && Hp > 0 && SceneManager.GetActiveScene().name == "GameScene")
		{
            TimeLine += Time.deltaTime;
		}
	}

    public IEnumerator GameClear()
	{
        Sequence seq = DOTween.Sequence();
        Transform trm = _player.transform;
        yield return new WaitForSeconds(2);
        seq.Append(trm.DOMove(Vector2.zero, 2));
        yield return new WaitForSeconds(2.2f);
        seq.Append(trm.DOMove(new Vector3(10, 0), 1));
        seq.AppendCallback(() => trm.gameObject.SetActive(false));
    }
}

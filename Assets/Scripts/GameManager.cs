using System.Collections;
using EnemyBehavior;
using Inventory;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    #region Declare Variables
    public int totalPoint;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI scoreAddAnimation;
    [SerializeField] private GameObject warningText;
    [SerializeField] private GameObject YouWin;
    [SerializeField] private GameObject YouLose;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button continueButton;
    
    public Vector2 spawnPoint;
    public Transform player;
    public Transform playerBase;
    public Enemy lastBoss;
    public bool isNightWarning;
    
    [Header("Sound")]
    [SerializeField] private AudioClip[] nightAlertSound;
    [SerializeField] private AudioClip[] bossSpawnSound;
    
    public static GameManager Instance;
    #endregion
    
    private void Awake()
    {
        Instance = this;
        restartButton.onClick.AddListener(() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex));
        mainMenuButton.onClick.AddListener(() => SceneManager.LoadScene(0));
        continueButton.onClick.AddListener(() => SceneManager.LoadScene(2));
    }
    
    private void Update()
    {
        EndGameHandle();
            
        EvenWarningHandler();
    }
    
    
    #region Methods
    /// <summary>
    /// Warning player when the enemy wave are coming (17:00 - 19:00).
    /// </summary>
    private void EvenWarningHandler()
    {
        if (!TimeSystem.Instance.TimePeriodCheck(17, 18))
        {
            isNightWarning = false;
            return;
        }
        if (warningText.gameObject.activeSelf || isNightWarning) return;
        isNightWarning = true;
        SoundManager.Instance.RandomPlaySound(nightAlertSound);
        StartCoroutine(ToggleSetActiveRelateWithAnimation(warningText.gameObject));
    }

    
    /// <summary>
    /// Add point to the total point.
    /// </summary>
    /// <param name="n">Point amount.</param>
    public void AddPoint(int n)
    {
        totalPoint += n;
        scoreText.text = $"{totalPoint}";
        scoreAddAnimation.text = $"+ {n}";
        PlayerInventoryController.Instance.inventoryUI.totalPointText.text = $"{totalPoint}";
        StartCoroutine(ToggleSetActiveRelateWithAnimation(scoreAddAnimation.gameObject));
    }
    
    public void SpawnLastBoss()
    {
        lastBoss.gameObject.SetActive(true);
        SoundManager.Instance.RandomPlaySound(bossSpawnSound);
    }

    public void EndGameHandle()
    {
        if (!playerBase)
            YouLose.SetActive(true);
        if (!lastBoss)
            YouWin.SetActive(true);
    }
    
    /// <summary>
    /// Toggle set active game object relate with animation.
    /// Set active game object and wait until the animation is done.
    /// </summary>
    /// <param name="obj">Object to set.</param>
    /// <returns></returns>
    IEnumerator ToggleSetActiveRelateWithAnimation(GameObject obj)
    {
        if (!obj.TryGetComponent(out Animator animator)) yield break;
        yield return new WaitUntil(() => !obj.activeSelf);
        obj.SetActive(true);

        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1);
        obj.SetActive(false);
    }
    #endregion
}

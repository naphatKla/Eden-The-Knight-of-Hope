using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Declare Variables
    public int totalPoint;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI scoreAddAnimation;
    [SerializeField] private TextMeshProUGUI warningText;
    public Vector2 spawnPoint;
    public Transform player;
    public Transform playerBase;
    
    public static GameManager Instance;
    #endregion
    
    private void Start()
    {
        Instance = this;
    }
    
    private void Update()
    {
        if (!playerBase) SceneManager.LoadScene(0);
        EvenWarningHandler();
    }

    #region Methods
    /// <summary>
    /// Warning player when the enemy wave are coming (17:00 - 19:00).
    /// </summary>
    private void EvenWarningHandler()
    {
        if (!TimeSystem.Instance.TimePeriodCheck(17, 19)) return;
        if (warningText.gameObject.activeSelf) return;
        StartCoroutine(ToggleSetActiveRelateWithAnimation(warningText.gameObject));
    }

    
    /// <summary>
    /// Add point to the total point.
    /// </summary>
    /// <param name="n">Point amount.</param>
    public void AddPoint(int n)
    {
        totalPoint += n;
        scoreText.text = $"Score: {totalPoint}";
        scoreAddAnimation.text = $"+ {n}";
        StartCoroutine(ToggleSetActiveRelateWithAnimation(scoreAddAnimation.gameObject));
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

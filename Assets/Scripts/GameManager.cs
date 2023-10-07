using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int totalPoint = 0;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI scoreAddAnimation;
    [SerializeField] private TextMeshProUGUI warningText;
    public Vector2 spawnPoint;
    private bool _isWarning;
    public Transform player;
    public Transform playerBase;

    public static GameManager instance;
    void Start()
    {
        instance = this;
    }
    
    void Update()
    {
        if (playerBase.IsUnityNull()) SceneManager.LoadScene(0);
        if (TimeSystem.instance.TimePeriodCheck(17,18))
        {
            if(_isWarning) return;
            _isWarning = true;
            StartCoroutine(ToggleSetActiveRelateWithAnimation(warningText.gameObject));
            return;
        }

        _isWarning = false;
    }
    
    public void AddPoint(int n)
    {
        totalPoint += n;
        scoreText.text = $"Score: {totalPoint}";
        scoreAddAnimation.text = $"+ {n}";
        StartCoroutine(ToggleSetActiveRelateWithAnimation(scoreAddAnimation.gameObject));

    }
    
    IEnumerator ToggleSetActiveRelateWithAnimation(GameObject obj)
    {
        if (!obj.TryGetComponent(out Animator animator))
            yield break;
        
        yield return new WaitUntil(() => !obj.activeSelf);
        obj.SetActive(true);

        yield return new WaitUntil(() =>
            animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1);
        
        obj.SetActive(false);
    }
}

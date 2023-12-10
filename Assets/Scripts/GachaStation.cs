using System.Collections.Generic;
using Interaction;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GachaStation : InteractableObject
{
    [Header("Gacha Station Config")]
    [SerializeField] Vector2 gachaArea;
    [SerializeField] LayerMask playerLayer;
    [SerializeField] private List<PriorityObject<GachaSo>> gachaDataList;
    [SerializeField] private int cost;
    
    [Header("Gacha Station UI")]
    [SerializeField] private GameObject gachaStationUI;
    [SerializeField] private GameObject equipZoneUI;
    [SerializeField] private TextMeshProUGUI gachaNameText;
    [SerializeField] private TextMeshProUGUI gachaDescriptionText;
    [SerializeField] private TextMeshProUGUI equipZoneNameText;
    [SerializeField] private TextMeshProUGUI equipZoneDescriptionText;
    [SerializeField] private Button randomButton;
    [SerializeField] private Button selectButton;
    [SerializeField] private Button discardButton;
    [SerializeField] private Button closeButton;
    private GachaSo _currentGachaPick;
    private GachaSo _currentGachaSelect;
    private float _lastOpenTime;

    protected override void Start()
    {
        base.Start();
        randomButton.onClick.AddListener(RandomGacha);
        selectButton.onClick.AddListener(SelectGacha);
        discardButton.onClick.AddListener(OpenGachaUI);
        closeButton.onClick.AddListener(CloseGachaUI);
    }

    protected override void Update()
    {
        base.Update();
        if (Input.GetKeyDown(KeyCode.Escape))
            CloseGachaUI();
        if(_lastOpenTime + 1.5f > Time.time) return;
        if (Physics2D.OverlapBoxNonAlloc(transform.position, gachaArea, 0,new Collider2D[1],playerLayer) == 0)
            CloseGachaUI();
    }

    protected override void InteractAction()
    {
        OpenGachaUI();
    }

    private void OpenGachaUI()
    {
        if (UIManager.Instance.CheckIsAnyUIOpen()) return;
        ResetGachaUI();
        if (_currentGachaSelect) equipZoneUI.SetActive(true);
        gachaNameText.text = $"Feel lucky today? \nTry your luck for <color=red>{cost}</color> coins!";
        randomButton.GetComponentInChildren<TextMeshProUGUI>().text = $"Random ({cost} coins)";
        gachaStationUI.SetActive(true);
        _lastOpenTime = Time.time;
    }
    
    private void CloseGachaUI()
    {
        ResetGachaUI();
        gachaStationUI.SetActive(false);
    }

    private void ResetGachaUI()
    {
        _currentGachaPick = null;
        selectButton.gameObject.SetActive(false);
        discardButton.gameObject.SetActive(false);
        gachaDescriptionText.gameObject.SetActive(false);
    }

    private void RandomGacha()
    {
        if (GameManager.Instance.totalPoint < cost)
        {
            ResetGachaUI();
            gachaNameText.text = $"<color=red>You don't have enough coins</color>";
            return;
        }
        _currentGachaPick = ProjectExtensions.RandomPickOne(gachaDataList).obj;
        _currentGachaPick.InitializeName();
        gachaNameText.text = $"You got {_currentGachaPick.GachaNamWithHexColor}!";
        selectButton.gameObject.SetActive(true);
        discardButton.gameObject.SetActive(true);
        gachaDescriptionText.gameObject.SetActive(true);
        gachaDescriptionText.text = _currentGachaPick.GetDescription();
        GameManager.Instance.AddPoint(-cost);
    }

    private void SelectGacha()
    {
        _currentGachaSelect?.RemoveStat();
        _currentGachaPick?.AddStat();
        _currentGachaSelect = _currentGachaPick;
        gachaNameText.text = $"Selected {_currentGachaPick.GachaNamWithHexColor}!";
        equipZoneNameText.text = _currentGachaSelect.GachaNamWithHexColor;
        equipZoneDescriptionText.text = _currentGachaSelect.GetDescription();
        equipZoneUI.SetActive(true);
        ResetGachaUI();
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, gachaArea);
    }
}

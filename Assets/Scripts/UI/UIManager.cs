using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public PlayerStatBar playerStatBar;

    [Header("事件监听")]
    public CharacterEventSO healthEvent;
    public SceneLoadEventSO unloadedSceneEvent;
    public VoidEventSO loadDataEvent;
    public VoidEventSO gameOverEvent;
    public VoidEventSO backToMenuEvent;
    public FloatEventSO syncVolumeEvent;

    [Header("组件")]
    public GameObject gameOverPanel;
    public GameObject restartBtn;
    public Button settingsButton;
    public GameObject pausePanel;
    public Slider volumeSlider;

    [Header("广播")]
    public VoidEventSO pauseEvent;


    private void Awake()
    {
        settingsButton.onClick.AddListener(TogglePausePanel);
    }
    private void TogglePausePanel()
    {
        if(pausePanel.activeInHierarchy)
        {
            pausePanel.SetActive(false);
            Time.timeScale = 1;
        }
        else
        {
            pauseEvent.RaiseEvent();
            pausePanel.SetActive(true);
            Time.timeScale = 0;
        }
    }


    private void OnEnable()
    {
        healthEvent.OnEventRaised += OnHealthEvent;
        unloadedSceneEvent.LoadRequestEvent += OnLoadEvent;
        loadDataEvent.OnEventRaised += OnLoadDataEvent;
        gameOverEvent.OnEventRaised += OnGanmeOverEvent;
        backToMenuEvent.OnEventRaised += OnLoadDataEvent;
        syncVolumeEvent.OnEventRaised += OnsyncVolumeEvent;
    }
   private void OnDisable()
   {
        healthEvent.OnEventRaised -= OnHealthEvent;
        unloadedSceneEvent.LoadRequestEvent -= OnLoadEvent;
        loadDataEvent.OnEventRaised -= OnLoadDataEvent;
        gameOverEvent.OnEventRaised -= OnGanmeOverEvent;
        backToMenuEvent.OnEventRaised -= OnLoadDataEvent;
        syncVolumeEvent.OnEventRaised -= OnsyncVolumeEvent;
    }

    private void OnsyncVolumeEvent(float amount)
    {
        volumeSlider.value = (amount + 80) / 100;
    }
   private void OnLoadDataEvent()
    {
        gameOverPanel.SetActive(false);
    }
   private void OnGanmeOverEvent()
    {
        gameOverPanel.SetActive(true);
        EventSystem.current.SetSelectedGameObject(restartBtn);
    }
    private void OnHealthEvent(Character character)
    {
        var persentage = (float)character.currentHealth / character.maxHealth;
        playerStatBar.OnHealthChange(persentage);
    }

    private void OnLoadEvent(GameSceneSO sceneToLoad,Vector3 arg1,bool arg2)
    {
        var isMenu = sceneToLoad.sceneType == SceneType.Menu;
            playerStatBar.gameObject.SetActive(!isMenu);
    }



}

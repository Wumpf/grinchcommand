using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

[RequireComponent(typeof(SledSpawner))]
[RequireComponent(typeof(PlayerLifeCounter))]
public class GameState : MonoBehaviour
{
    public GameObject IntroScreen;
    public GameObject GameOverScreen;
    public GameObject StageScreen;
    public GameObject AdvanceCodeScreen;
    public GameObject BonusCodeScreen;

    private PlayerLifeCounter playerLifeCounter;
    private SledSpawner sledSpawner;

    private int currentStage = 0;

    public int AdvanceStage = 3;
    public int BonusStage = 6;


    public float BaseSledSpawnFrequency = 0.1f;
    public float SledSpawnFrequencyIncreasePerStage = 0.05f;
    public float CurrentSledSpawnFrequency => BaseSledSpawnFrequency + SledSpawnFrequencyIncreasePerStage * currentStage;

    public float BasePresentSpawnFrequency = 0.8f;
    public float PresentSpawnFrequencyIncreasePerStage = 0.1f;
    public float CurrentPresentSpawnFrequency => BasePresentSpawnFrequency + PresentSpawnFrequencyIncreasePerStage * currentStage;

    public int BaseNumSledsToSpawn = 3;
    public int NumSledsToSpawnIncreasePerStage = 2;
    public int CurrentNumSledsToSpawn => BaseNumSledsToSpawn + NumSledsToSpawnIncreasePerStage * currentStage;

    enum State
    {
        Intro,
        StageTransition,
        Game,
        GameOver
    };

    private State currentState;

    private void Start()
    {
        playerLifeCounter = GetComponent<PlayerLifeCounter>();
        sledSpawner = GetComponent<SledSpawner>();

        currentState = State.Intro;
        IntroScreen.SetActive(true);
    }

    public void OnContinueButtonPressed(InputAction.CallbackContext context)
    {
        if (currentState == State.Game || context.phase != InputActionPhase.Started)
            return;

        IntroScreen.SetActive(false);
        GameOverScreen.SetActive(false);
        StageScreen.SetActive(false);

        AdvanceCodeScreen.SetActive(false);
        BonusCodeScreen.SetActive(false);

        if (currentState != State.StageTransition)
            playerLifeCounter.Restart();
        currentState = State.Game;

        ClearPresentsAndSleds();

        sledSpawner.enabled = true;
    }

    public void ClearPresentsAndSleds()
    {
        foreach(var present in GameObject.FindGameObjectsWithTag("Present"))
            GameObject.Destroy(present);
        foreach(var sled in GameObject.FindGameObjectsWithTag("Sled"))
            GameObject.Destroy(sled);
    }

    public void OnLoose()
    {
        GameOverScreen.SetActive(true);
        sledSpawner.enabled = false;
        currentState = State.GameOver;
        currentStage = 0;
    }

    public void OnStageSurvived()
    {
        currentStage++;
        if (currentStage == AdvanceStage)
            AdvanceCodeScreen.SetActive(true);
        if (currentStage == BonusStage)
            BonusCodeScreen.SetActive(true);

        sledSpawner.enabled = false;
        StageScreen.SetActive(true);
        StageScreen.GetComponent<Text>().text = $"STAGE {currentStage+1}";

        currentState = State.StageTransition;
    }

    private void FixedUpdate()
    {
        if (currentState == State.Game && sledSpawner.NumSledsLeft == 0)
        {
            if (GameObject.FindObjectOfType<Present>() == null)
                OnStageSurvived();
        }
    }
}
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(SledSpawner))]
[RequireComponent(typeof(PlayerLifeCounter))]
public class GameState : MonoBehaviour
{
    public GameObject IntroScreen;
    public GameObject GameOverScreen;

    private PlayerLifeCounter playerLifeCounter;
    private SledSpawner sledSpawner;

    enum State
    {
        Intro,
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

    public void OnContinueButtonPressed()
    {
        if (currentState == State.Game)
            return;

        IntroScreen.SetActive(false);
        GameOverScreen.SetActive(false);
        playerLifeCounter.Restart();
        sledSpawner.enabled = true;
        currentState = State.Game;

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
    }
}
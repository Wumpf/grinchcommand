using UnityEngine;
using UnityEngine.InputSystem;

public class GameState : MonoBehaviour
{
    public GameObject IntroScreen;
    public SledSpawner SledSpawner;

    enum State
    {
        Intro,
        Game,
        GameOver
    };

    private State currentState;

    private void Start()
    {
        currentState = State.Intro;
        IntroScreen.SetActive(true);
    }

    private void Update()
    {
        switch(currentState)
        {
            case State.Intro:
                if (Keyboard.current.anyKey.wasPressedThisFrame)
                {
                    IntroScreen.SetActive(false);
                    SledSpawner.enabled = true;
                    currentState = State.Game;
                }
                break;
        }
    }

    public void OnLoose()
    {

    }
}
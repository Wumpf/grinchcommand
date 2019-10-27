using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(GameState))]
public class PlayerLifeCounter : MonoBehaviour
{
    public Text Text;
    public GameObject FirstLifeIcon;
    public uint NumLifesAtStart = 7;

    private GameObject[] lifeIcons;

    private uint numLifesLeft;

    private Building[] lifeBuildings;

    private void UpdateText()
    {
        Text.text = $"{numLifesLeft}x";
    }

    private void Start()
    {
        lifeIcons = new GameObject[NumLifesAtStart];
        lifeIcons[0] = FirstLifeIcon;
        for (int i=1; i<lifeIcons.Length; ++i)
            lifeIcons[i] = Instantiate(lifeIcons[i-1], lifeIcons[i-1].transform.position + new Vector3(0.28f, 0, 1), Quaternion.identity);

        lifeBuildings = Object.FindObjectsOfType<Building>();
        foreach (var building in lifeBuildings)
            building.OnLifeLostEvent += OnLifeLost;

        Restart();
    }

    public void OnLifeLost()
    {
        if (numLifesLeft == 0)
            return;

        --numLifesLeft;
        lifeIcons[numLifesLeft].SetActive(false);
        UpdateText();
        if (numLifesLeft == 0)
            GetComponent<GameState>().OnLoose();
    }

    public void Restart()
    {
        numLifesLeft = NumLifesAtStart;
        foreach (var icon in lifeIcons)
            icon.SetActive(true);
        foreach (var building in lifeBuildings)
            building.Restart();
        UpdateText();
    }
}

using UnityEngine;
using UnityEngine.UI;

public class PlayerLifeCounter : MonoBehaviour
{
    public GameState GameState;

    public Text Text;
    public GameObject FirstLifeIcon;
    public int NumLifesAtStart = 7;

    public static PlayerLifeCounter Instance;

    private GameObject[] lifeIcons;

    private int numLifesLeft;

    private void UpdateText()
    {
        Text.text = $"{numLifesLeft}x";
    }

    private void Start()
    {
        Debug.Assert(Instance == null);
        Instance = this;

        lifeIcons = new GameObject[NumLifesAtStart];
        lifeIcons[0] = FirstLifeIcon;
        for (int i=1; i<lifeIcons.Length; ++i)
            lifeIcons[i] = Instantiate(lifeIcons[i-1], lifeIcons[i-1].transform.position + new Vector3(0.28f, 0, 1), Quaternion.identity);
        Reset();
    }

    public void OnLifeLost()
    {
        if (numLifesLeft <= 0)
            return;

        --numLifesLeft;
        lifeIcons[numLifesLeft].SetActive(false);
        UpdateText();
    }

    public void Reset()
    {
        numLifesLeft = lifeIcons.Length;
        foreach (var icon in lifeIcons)
            icon.SetActive(true);
        UpdateText();
    }
}

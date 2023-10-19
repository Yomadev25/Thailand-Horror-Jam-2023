using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [SerializeField]
    private Button playButton;
    [SerializeField]
    private Button exitButton;

    private void Start()
    {
        playButton.onClick.AddListener(Play);
        exitButton.onClick.AddListener(Exit);
    }

    private void Play()
    {
        TransitionManager.Instance.SceneFadeIn(1, () =>
        {
            SceneManager.LoadScene("Game");
        });
    }

    private void Exit()
    {

    }
}

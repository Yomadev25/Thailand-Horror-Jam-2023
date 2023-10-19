using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class TransitionManager : MonoBehaviour
{
    public static TransitionManager Instance;

    [Header("Transition HUD")]
    [SerializeField]
    private CanvasGroup _sceneTransition;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void SceneFadeIn(float duration = 1f, UnityAction callback = null)
    {
        _sceneTransition.alpha = 0f;
        _sceneTransition.blocksRaycasts = true;

        _sceneTransition.LeanAlpha(1, duration).setOnComplete(() => callback?.Invoke());
    }

    public void SceneFadeOut(float duration = 1f, UnityAction callback = null)
    {
        _sceneTransition.alpha = 1f;
        _sceneTransition.blocksRaycasts = false;

        _sceneTransition.LeanAlpha(0, duration).setDelay(0.5f).setOnComplete(() => callback?.Invoke());
    }

    public void NormalFadeIn(float duration = 1f, UnityAction callback = null)
    {
        _sceneTransition.alpha = 0f;
        _sceneTransition.blocksRaycasts = true;

        _sceneTransition.LeanAlpha(1, duration).setOnComplete(() => callback?.Invoke());
    }

    public void NormalFadeOut(float duration = 1f, float delay = 0.5f, UnityAction callback = null)
    {
        _sceneTransition.alpha = 1f;
        _sceneTransition.blocksRaycasts = false;

        _sceneTransition.LeanAlpha(0, duration).setDelay(delay).setOnComplete(() => callback?.Invoke());
    }
}

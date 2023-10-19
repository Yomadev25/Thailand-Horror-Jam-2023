using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Outline))]
public class Item : MonoBehaviour
{
    [SerializeField]
    private Outline _outline;
    [SerializeField]
    private UnityEvent onInteract;

    private void Start()
    {
        OnUnfocus();

        switch (this.tag)
        {
            case "Candy":
                onInteract.AddListener(() =>
                {
                    GameManager.instance.GetCandy();
                    GameManager.instance.ActiveEButton(false);
                    Destroy(transform.parent.gameObject);
                });
                break;

            case "Mask":
                onInteract.AddListener(() =>
                {
                    GameManager.instance.ActiveEButton(false);
                    Destroy(transform.parent.gameObject);
                });
                break;
            default:
                break;
        }
    }

    public void Interact()
    {
        if (!enabled) return;
        onInteract?.Invoke();
    }

    public void OnFocus()
    {
        if (!enabled) return;
        _outline.enabled = true;
    }

    public void OnUnfocus()
    {
        if (!enabled) return;
        _outline.enabled = false;
    }
}

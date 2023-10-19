using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coffin : MonoBehaviour
{
    [SerializeField]
    private Item item;

    [SerializeField]
    private Transform hidePoint;
    [SerializeField]
    private Transform player;

    public void Hide()
    {
        player.parent = hidePoint;
        player.GetComponent<CharacterController>().enabled = false;
        item.OnUnfocus();
        item.enabled = false;

        player.LeanMoveLocal(Vector3.zero, 1f);
        TransitionManager.Instance.NormalFadeIn(0.5f, () =>
        {
            player.GetComponent<PlayerManager>().Hide(this);
            TransitionManager.Instance.NormalFadeOut(1, 1);
        });
    }

    public void Unhide()
    {
        player.parent = null;
        player.GetComponent<CharacterController>().enabled = true;
        item.enabled = true;
        Vector3 position = hidePoint.forward;
        position.y = 0;

        player.LeanMoveLocal(position, 1f);
        TransitionManager.Instance.NormalFadeOut(0.5f, 0.5f, () =>
        {
            
        });
    }
}

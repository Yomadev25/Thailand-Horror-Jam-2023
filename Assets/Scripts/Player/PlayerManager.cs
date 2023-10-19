using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [Header("Interaction System")]
    [SerializeField]
    private Transform _povTransform;
    [SerializeField]
    private float _interactRange;
    private Item _selectedItem;
    private Coffin _coffin;

    [Header("Skills")]
    [SerializeField]
    private bool _readyMasked;
    [SerializeField]
    private float _maskDuration;
    [SerializeField]
    private bool _readyDistract;

    [Header("Others")]
    [SerializeField]
    private Animator _animator;
    [SerializeField]
    private Transform _cameraRoot;
    [SerializeField]
    private CharacterController _characterController;

    #region PUBLIC VARIABLES
    public bool IsMasked { get; set; }
    public bool IsHide { get; set; }
    #endregion

    void Start()
    {
        IsMasked = false;
        GameManager.instance.UpdateMaskedSkillImage(_readyMasked);
        GameManager.instance.UpdateDistractSkillImage(_readyDistract);
    }

    void Update()
    {
        InteractDetect();
        InteractHandler();

        MaskedHandler();
        DistractHandler();

        if (IsHide)
        {
            if (Input.GetAxis("Horizontal") > 0 || Input.GetAxis("Vertical") > 0)
            {
                Unhide();
            }
        }
    }

    private void InteractDetect()
    {
        Ray ray = new Ray(_povTransform.position, _povTransform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, _interactRange))
        {
            if (hit.collider.TryGetComponent(out Item item))
            {
                _selectedItem = item;
                _selectedItem.OnFocus();

                if (!_selectedItem.enabled) return;
                GameManager.instance.ActiveEButton(true);
            }
            else
            {
                if (_selectedItem == null) return;

                _selectedItem.OnUnfocus();
                _selectedItem = null;

                GameManager.instance.ActiveEButton(false);
            }          
        }
        else
        {
            if (_selectedItem == null) return;

            _selectedItem.OnUnfocus();
            _selectedItem = null;

            GameManager.instance.ActiveEButton(false);
        }
    }

    private void InteractHandler()
    {
        if (_selectedItem == null) return;
        if (Input.GetKeyDown(KeyCode.E))
        {
            _selectedItem.Interact();
        }
    }

    private void MaskedHandler()
    {
        if (!_readyMasked || IsHide) return;

        if (Input.GetKeyDown(KeyCode.C))
        {
            StartCoroutine(MaskedCoroutine());

            _readyMasked = false;
            GameManager.instance.UpdateMaskedSkillImage(false);
        }
    }

    IEnumerator MaskedCoroutine()
    {
        IsMasked = true;
        _animator.SetTrigger("Masked");

        yield return new WaitForSeconds(1.1f);
        float duration = _maskDuration;

        while(duration > 0)
        {
            duration -= Time.deltaTime;
            GameManager.instance.UpdateMaskedFill(duration / _maskDuration);
            yield return null;
        }

        IsMasked = false;
        _animator.SetTrigger("Unmasked");
    }

    public void GetMask()
    {
        if (IsMasked) return;

        _readyMasked = true;
        GameManager.instance.UpdateMaskedSkillImage(true);
    }

    private void DistractHandler()
    {
        if (!_readyDistract) return;

        _readyDistract = false;
        GameManager.instance.UpdateDistractSkillImage(false);
    }

    public void Hide(Coffin coffin)
    {
        _cameraRoot.localEulerAngles = Vector3.zero;

        _coffin = coffin;
        IsHide = true;
    }

    private void Unhide()
    {
        _coffin.Unhide();
        _coffin = null;
        IsHide = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            var enemy = other.GetComponent<EnemyStateMachine>();
            if (enemy != null)
            {
                this.gameObject.SetActive(false);
                GameManager.instance.Gameover();
            }
        }
    }
}

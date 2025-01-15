using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

//------Require section------//
[RequireComponent(typeof(InputActionReference))]
[RequireComponent(typeof(GameObject))]
[RequireComponent(typeof(Collider2D))]

public class ObjectInteraction : MonoBehaviour
{
    [Header("References")]
    [SerializeField] InputActionReference _clickToDrag;
    [SerializeField] GameObject _target;
    [SerializeField] Collider2D _coll;
    [SerializeField] LayerMask _mask;

    [Header("Parameters")]
    [SerializeField] bool _isDragable = true;

    private Vector2 _initPos;
    private Vector2 _mousePos;
    private Coroutine _dragUpdateCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        _clickToDrag.action.performed += OnHold;
        _clickToDrag.action.canceled += OnReleased;
    }

    private void OnDestroy()
    {
        _clickToDrag.action.performed -= OnHold;
        _clickToDrag.action.canceled -= OnReleased;
    }

    void OnHold(InputAction.CallbackContext ctx)
    {
        //Debug.Log("test");
        if (_isDragable)
            _dragUpdateCoroutine = StartCoroutine(DragUpdate());
    }

    void OnReleased(InputAction.CallbackContext ctx)
    {
        var hit = Physics2D.OverlapBox(_target.transform.position, _target.transform.localScale, 0, _mask);

        if(hit != null ) 
        {
            //if (LayerMask.GetMask(_mask))
            _isDragable = false;
            _target.transform.position = _initPos;
            Debug.Log("Placement impossible");
        }
        else
            _target.transform.position = _mousePos;

        StopAllCoroutines();
        _isDragable = true;
    }

    IEnumerator DragUpdate()
    {
        while (_isDragable) 
        {
            var mousePos = Input.mousePosition;
            _mousePos = Camera.main.ScreenToWorldPoint(mousePos);
            _target.transform.position = _mousePos;
            yield return null;
        }
    }
}

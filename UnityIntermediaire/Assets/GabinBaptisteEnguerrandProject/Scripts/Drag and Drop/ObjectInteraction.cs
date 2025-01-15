using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

//------Require section------//
//[RequireComponent(typeof(InputActionReference))]
[RequireComponent(typeof(GameObject))]
//[RequireComponent(typeof(Collider2D))]

public class ObjectInteraction : MonoBehaviour, IPointerEnterHandler
{
    [Header("References")]
    [SerializeField] InputActionReference _clickToDrag;
    [SerializeField] GameObject _target;
    [SerializeField] Collider2D _coll;

    [Header("Parameters")]
    [SerializeField] bool _isDragable = true;
    [SerializeField] LayerMask _mask;
    [SerializeField] _mode mode;

    private Vector2 _initPos;
    private Vector2 _mousePos;
    private Camera _cam;
    private Coroutine _dragUpdateCoroutine;
    private PointerEventData _UImousPos;

    public enum _mode
    {
        Game,
        UI
    };

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
        switch (mode)
        {
            case _mode.Game:
                if (_isDragable && )
                    _dragUpdateCoroutine = StartCoroutine(DragUpdate());
                break;
            case _mode.UI:
                Debug.Log("test");
                if (_isDragable)
                    _dragUpdateCoroutine = StartCoroutine(DragUpdate());
                break;
        }
    }

    void OnReleased(InputAction.CallbackContext ctx)
    {
        switch (mode)
        {
            case _mode.Game:
                var hit = Physics2D.OverlapBox(_target.transform.position, _target.transform.localScale, 0, _mask);

                if (hit != null)
                {
                    //if (LayerMask.GetMask(_mask))
                    _isDragable = false;
                    _target.transform.position = _initPos;
                    Debug.Log("Placement impossible");
                }
                else
                    _target.transform.position = _mousePos;
                break;
            case _mode.UI:
                GraphicRaycaster UIhit = FindAnyObjectByType<GraphicRaycaster>();
                List<RaycastResult> results = new List<RaycastResult>();
                foreach(RaycastResult result in results)
                {
                    if (result.gameObject.CompareTag("InvalidPlacement"))
                    {
                        _isDragable = false;
                        _target.transform.position = _initPos;
                        Debug.Log("Placement impossible");
                    }
                    else
                        _target.transform.position = _mousePos;
                }
                
            break;
        }

        StopAllCoroutines();
        _isDragable = true;
    }

    IEnumerator DragUpdate()
    {
        while (_isDragable) 
        {
            switch (mode)
            {
                case _mode.Game:
                    var mousePos = Input.mousePosition;
                    _mousePos = Camera.main.ScreenToWorldPoint(mousePos);
                    _target.transform.position = _mousePos;
                    break;
                case _mode.UI:
                    var uiMousePos = Input.mousePosition;
                    var pos = new Vector2(Screen.width, Screen.height);
                    var newMousePos = Camera.main.ScreenToViewportPoint(uiMousePos)* pos;
                    newMousePos -= pos / 2;
                    _target.GetComponent<RectTransform>().localPosition = newMousePos;
                    //Debug.Log("tes la chakal");
                    break;
            }
            
            yield return null;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _target = eventData.pointerEnter;
    }
}

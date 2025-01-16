using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.UIElements;
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
    private Coroutine _dragUpdateCoroutine;
    private PointerEventData _UImousPos;

    public enum _mode
    {
        Game,
        UI
    };

    void Start()
    {
        _mousePos = Input.mousePosition;
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
        
                if ( _isDragable)
                {
                    _dragUpdateCoroutine = StartCoroutine(DragUpdate());
                }
                break;
            case _mode.UI:
                //Debug.Log("test");
                if (  _isDragable)
                {
                    _dragUpdateCoroutine = StartCoroutine(DragUpdate());
                }
                break;
        }
    }

    void OnReleased(InputAction.CallbackContext ctx)
    {
        switch (mode)
        {
            case _mode.Game:
                //check if obj position is valid or not
                var hit = Physics2D.OverlapBox(_target.transform.position, _target.transform.localScale, 0);

                if (hit.tag != "InvalidPlacement")
                {
                    _isDragable = false;
                    _target.transform.position = _initPos;
                    Debug.Log("Placement impossible");
                }
                else
                {
                    _target.transform.position = _mousePos;
                    _isDragable = true;
                }
                break;
            case _mode.UI:
                //check if obj is under
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
                    {
                        _target.transform.position = _mousePos;
                        _isDragable = true;
                    }
                }

                break;
        }

        StopAllCoroutines();
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
        //FindTarget(Input.mousePosition, out _target);
        _target = eventData.pointerEnter;
    }

    /*private void FindTarget()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(_mousePos);
        RaycastHit2D hitInfo = Physics2D.Raycast(mousePos, Vector2.zero);
        if(hitInfo.collider != null)
        {
            ObjectInteraction script = hitInfo.collider.GetComponent<ObjectInteraction>();
            if (script == null)
                return;
        }
    }*/
}

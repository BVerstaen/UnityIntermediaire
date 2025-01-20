using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//------Require section------//
//[RequireComponent(typeof(InputActionReference))]
[RequireComponent(typeof(GameObject))]
//[RequireComponent(typeof(Collider2D))]

public class ObjectInteraction : MonoBehaviour, IPointerEnterHandler
{
    [Header("References")]
    [SerializeField] GameObject _target;
    [SerializeField] Collider2D _coll;
    public EventSystem _eventSystem;

    [Header("Parameters")]
    [SerializeField] bool _isDragable = true;
    [SerializeField] LayerMask _mask;
    [SerializeField] _mode mode;
    [SerializeField] public int _money;

    private bool _isPressed = false;
    private bool invalidator = false;
    private Vector2 _initPos;
    private Vector2 _mousePosUI;
    private Vector3 _mousePosGame;
    private Coroutine _dragUpdateCoroutine;
    private PointerEventData _UImousPos;
    

    public enum _mode
    {
        Game,
        UI
    };

    void Start()
    {
        _mousePosUI = Input.mousePosition;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !_isPressed)
        {
            _isPressed = true;
            OnHold();
        }
        else if (Input.GetMouseButtonUp(0) && _isPressed)
        {
            OnReleased();
            _isPressed = false;
        }
    }

    void OnHold()
    {
        switch (mode)
        {
            case _mode.Game:

                if (IsTarget() && _isDragable)
                {
                    var newMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    _initPos = newMousePos;

                    _dragUpdateCoroutine = StartCoroutine(DragUpdate());
                }
                break;
            case _mode.UI:
                if (IsTarget() && _isDragable)
                {
                    _initPos = Input.mousePosition;
                    //Debug.Log("test");
                    _dragUpdateCoroutine = StartCoroutine(DragUpdate());
                }
                break;
        }
    }

    void OnReleased()
    {
        switch (mode)
        {
            case _mode.Game:
                //check if obj position is valid or not
                if (invalidator)
                {
                    _isDragable = true;
                    Vector3 defaultPos = _initPos;
                    defaultPos.z = _target.transform.position.z;
                    _target.transform.position = defaultPos;
                    Debug.Log("Placement impossible");
                }
                else
                {
                    _isDragable = true;
                   /* _initPos = _target.transform.position;
                    Debug.Log("aled");*/
                }
                break;
            case _mode.UI:
                //check if obj is under
                GraphicRaycaster UIhit = FindAnyObjectByType<GraphicRaycaster>();
                List<RaycastResult> results = new List<RaycastResult>();
                _UImousPos = new PointerEventData(_eventSystem);
                _UImousPos.position = Input.mousePosition;
                UIhit.Raycast(_UImousPos, results);
                foreach (RaycastResult result in results)
                {
                    if (result.gameObject.CompareTag("InvalidPlacement"))
                    {
                        _isDragable = false;
                        _target.transform.position = _initPos;
                        Debug.Log("Placement impossible");
                    }
                    else
                        _isDragable = true;
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
                    _mousePosGame = Camera.main.ScreenToWorldPoint(mousePos);
                    _mousePosGame.z = _target.transform.position.z;
                    _target.transform.position = _mousePosGame;
                    break;

                case _mode.UI:
                    var uiMousePos = Input.mousePosition;
                    var pos = new Vector2(Screen.width, Screen.height);
                    var newMousePos = Camera.main.ScreenToViewportPoint(uiMousePos) * pos;
                   // newMousePos -= pos / 2;
                    _mousePosUI = newMousePos;
                    _target.transform.position = _mousePosUI;
                    break;
            }
            yield return null;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _target = eventData.pointerEnter;
    }

    private bool IsTarget()
    {
        switch (mode)
        {
            case _mode.Game:
                RaycastHit hit;
                var newMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Debug.DrawRay(newMousePos, Vector3.forward * 200, Color.red, 2000f);
                if (Physics.Raycast(newMousePos, Vector3.forward * 200, out hit, Mathf.Infinity))
                {
                    if(hit.collider.CompareTag("Card"))
                    {
                        print("touch�");
                        return true;
                    }
                }
                break;

            case _mode.UI:
                GraphicRaycaster UIhit = FindAnyObjectByType<GraphicRaycaster>();
                List<RaycastResult> results = new List<RaycastResult>();
                _UImousPos = new PointerEventData(_eventSystem);
                _UImousPos.position = Input.mousePosition;
                UIhit.Raycast(_UImousPos, results);

                foreach (var result in results)
                {
                    if (result.gameObject.GetComponent<ObjectInteraction>() == this)
                        return true;
                }
                break;
        }
        return false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("InvalidPlacement"))
        {
            invalidator = true;
            print("enter ici");
        }

    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("InvalidPlacement"))
        {
            invalidator = false;
            //print("exit");
        }
    }
}


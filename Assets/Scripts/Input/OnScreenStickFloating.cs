using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Services.Analytics.Internal;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.OnScreen;

public class OnScreenStickFloating : MonoBehaviour
{
    [SerializeField] private RectTransform m_stickArea;
    [SerializeField] private RectTransform m_stickOuter;
    [SerializeField] private OnScreenStickExternalExtension m_stickHandle;

    [SerializeField] GameObject m_instruction;


    [SerializeField]
    [Tooltip("The action that will be used to detect pointer down events on the stick control. Note that if no bindings " +
            "are set, default ones will be provided.")]
    private InputAction m_PointerDownAction;

    [SerializeField]
    [Tooltip("The action that will be used to detect pointer movement on the stick control. Note that if no bindings " +
        "are set, default ones will be provided.")]
    private InputAction m_PointerMoveAction;
    [NonSerialized]
    private List<RaycastResult> m_RaycastResults;
    [NonSerialized]
    private PointerEventData m_PointerEventData;

    private void Awake()
    {
        m_stickArea = GetComponent<RectTransform>();
        m_stickOuter= GetComponentsInChildren<RectTransform>().First(c => c.gameObject != gameObject);
        
        m_stickHandle = m_stickOuter.GetComponentInChildren<OnScreenStickExternalExtension>();

        m_stickOuter.gameObject.SetActive(false);
    }
    
    private void Start()
    {
        m_RaycastResults = new List<RaycastResult>();
        m_PointerEventData = new PointerEventData(EventSystem.current);

        // if the pointer actions have no bindings (the default), add some
        if (m_PointerDownAction == null || m_PointerDownAction.bindings.Count == 0)
        {
            if (m_PointerDownAction == null)
                m_PointerDownAction = new InputAction();

            m_PointerDownAction.AddBinding("<Mouse>/leftButton");
            m_PointerDownAction.AddBinding("<Pen>/tip");
            m_PointerDownAction.AddBinding("<Touchscreen>/touch*/press");
            m_PointerDownAction.AddBinding("<XRController>/trigger");
        }

        if (m_PointerMoveAction == null || m_PointerMoveAction.bindings.Count == 0)
        {
            if (m_PointerMoveAction == null)
                m_PointerMoveAction = new InputAction();

            m_PointerMoveAction.AddBinding("<Mouse>/position");
            m_PointerMoveAction.AddBinding("<Pen>/position");
            m_PointerMoveAction.AddBinding("<Touchscreen>/touch*/position");
        }

        m_PointerDownAction.started += OnPointerDown;
        m_PointerDownAction.started += OnPointerDownRemoveInstruction;
        m_PointerDownAction.canceled += OnPointerUp;
        m_PointerDownAction.Enable();
        m_PointerMoveAction.Enable();
    }

    private void OnPointerDownRemoveInstruction(InputAction.CallbackContext ctx)
    {
        m_instruction.SetActive(false);
        m_PointerDownAction.started -= OnPointerDownRemoveInstruction;
    }
    
    private void OnDestroy()
    {
        m_PointerDownAction.started -= OnPointerDown;
        m_PointerDownAction.started -= OnPointerDownRemoveInstruction;
        m_PointerDownAction.canceled -= OnPointerUp;
        m_PointerDownAction.Disable();
        m_PointerMoveAction.Disable();
    }

    private void OnPointerDown(InputAction.CallbackContext ctx)
    {
        
        Debug.Assert(EventSystem.current != null);

        var screenPosition = Vector2.zero;
        if (ctx.control?.device is Pointer pointer)
            screenPosition = pointer.position.ReadValue();

        m_PointerEventData.position = screenPosition;
        EventSystem.current.RaycastAll(m_PointerEventData, m_RaycastResults);
        if (m_RaycastResults.Count == 0)
            return;

        if (m_RaycastResults[0].gameObject != gameObject)
            return;


        var stickAreaSelected = false;
        foreach (var result in m_RaycastResults)
        {
            
            if (result.gameObject != gameObject) continue;

            stickAreaSelected = true;
            break;
        }

        if (!stickAreaSelected)
            return;

        m_stickOuter.gameObject.SetActive(true);
        Vector2 localPosition;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(m_stickArea, screenPosition, null, out localPosition);

        m_stickOuter.localPosition = localPosition;

        m_stickHandle.BeginInteraction(screenPosition, GetCameraFromCanvas());
        m_PointerMoveAction.performed += OnPointerMove;
    }

    private void OnPointerMove(InputAction.CallbackContext ctx)
    {
        // only pointer devices are allowed
        Debug.Assert(ctx.control?.device is Pointer);

        var screenPosition = ((Pointer)ctx.control.device).position.ReadValue();

        m_stickHandle.MoveStick(screenPosition, GetCameraFromCanvas());
    }

    private void OnPointerUp(InputAction.CallbackContext ctx)
    {
        m_stickHandle.EndInteraction();
        m_PointerMoveAction.performed -= OnPointerMove;
        m_stickOuter.gameObject.SetActive(false);
    }
    private Camera GetCameraFromCanvas()
    {
        var canvas = GetComponentInParent<Canvas>();
        var renderMode = canvas?.renderMode;
        if (renderMode == RenderMode.ScreenSpaceOverlay
            || (renderMode == RenderMode.ScreenSpaceCamera && canvas?.worldCamera == null))
            return null;

        return canvas?.worldCamera ?? Camera.main;
    }

}

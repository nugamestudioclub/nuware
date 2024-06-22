using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour, IInputHandler
{
    [SerializeField]
    private float m_inputVectorMagnitudeCutoff = 0.1f;

    private readonly ButtonType[] types = { ButtonType.North, ButtonType.South, ButtonType.East, ButtonType.West };

    public Vector2 CurrentLateral {  get; private set; }
    public IDictionary<ButtonType, InputContextType> CurrentButtons { get; private set; }

    public delegate void OnEvent<T>(T eventData);
    public event OnEvent<(Vector2 Data, InputContextType ContextType)> LateralEvent;
    public event OnEvent<IDictionary<ButtonType, InputContextType>> ButtonsEvent;

    private IAvatar m_currentPossession;

    private void Awake()
    {
        CurrentButtons = new Dictionary<ButtonType, InputContextType>
        {
            { ButtonType.North, InputContextType.None },
            { ButtonType.South, InputContextType.None },
            { ButtonType.East, InputContextType.None },
            { ButtonType.West, InputContextType.None }
        };
    }

    private void LateUpdate()
    {
        foreach (ButtonType button in types)
        if (CurrentButtons[button] == InputContextType.Canceled)
            CurrentButtons[button] = InputContextType.None;
    }

    private InputContextType ParseType(InputAction.CallbackContext context)
    {
        if (context.started) return InputContextType.Started;
        if (context.performed) return InputContextType.Performed;

        return InputContextType.Canceled;
    }

    public void OnLateral(InputAction.CallbackContext context)
    {
        var vector = context.ReadValue<Vector2>();
        var context_type = InputContextType.None;

        bool is_input_present = vector.sqrMagnitude > m_inputVectorMagnitudeCutoff;
        bool is_current_present = CurrentLateral.sqrMagnitude > m_inputVectorMagnitudeCutoff;

        if (is_input_present && !is_current_present)
        {
            context_type = InputContextType.Started;
        } 
        else if (!is_input_present && is_current_present)
        {
            context_type = InputContextType.Canceled;
        } 
        else if (is_input_present && is_current_present)
        {
            context_type = InputContextType.Performed;
        }

        CurrentLateral = vector;

        LateralEvent?.Invoke((CurrentLateral, context_type));
    }

    public void OnNorth(InputAction.CallbackContext context)
    {
        CurrentButtons[ButtonType.North] = ParseType(context);

        ButtonsEvent?.Invoke(CurrentButtons);
    }

    public void OnSouth(InputAction.CallbackContext context)
    {
        CurrentButtons[ButtonType.South] = ParseType(context);

        ButtonsEvent?.Invoke(CurrentButtons);
    }

    public void OnEast(InputAction.CallbackContext context)
    {
        CurrentButtons[ButtonType.East] = ParseType(context);

        ButtonsEvent?.Invoke(CurrentButtons);
    }

    public void OnWest(InputAction.CallbackContext context)
    {
        CurrentButtons[ButtonType.West] = ParseType(context);

        ButtonsEvent?.Invoke(CurrentButtons);
    }

    public void Possess(IAvatar target)
    {
        if (m_currentPossession != null)
        {
            Debug.LogWarning($"{gameObject.name} is already possessing another object!");
            return;
        }

        ButtonsEvent += target.OnButtonEvent;
        LateralEvent += target.OnLateralEvent;

        m_currentPossession = target;
    }

    public void Free()
    {
        ButtonsEvent -= m_currentPossession.OnButtonEvent;
        LateralEvent -= m_currentPossession.OnLateralEvent;

        m_currentPossession = null;
    }
}

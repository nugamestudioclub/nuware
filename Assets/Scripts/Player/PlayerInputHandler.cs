using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// A class that handles player input distribution.
/// </summary>
public class PlayerInputHandler : MonoBehaviour, IInputHandler
{
    // since enums can't have members for whatever reason
    private readonly ButtonType[] types = { ButtonType.North, ButtonType.South, ButtonType.East, ButtonType.West };

    [SerializeField, Tooltip("The magnitude of input at which lower values should be regarded as \"not input\".")]
    private float m_inputVectorMagnitudeCutoff = 0.1f;

    /// <summary>
    /// The current vector2 input currently held.
    /// </summary>
    public Vector2 CurrentLateral {  get; private set; }

    /// <summary>
    /// The current map of button statuses.
    /// </summary>
    public IDictionary<ButtonType, InputContextType> CurrentButtons { get; private set; }

    /// <summary>
    /// Generic input event delegate declaration. Listeners must take in the same type argument.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="eventData"></param>
    public delegate void OnInputEvent<T>(T eventData);

    /// <summary>
    /// The event to invoke when lateral inputs (joystick, WASD) are processed.
    /// </summary>
    public event OnInputEvent<(Vector2 Data, InputContextType ContextType)> LateralEvent;

    /// <summary>
    /// The event to invoke when button input are processed.
    /// </summary>
    public event OnInputEvent<IDictionary<ButtonType, InputContextType>> ButtonsEvent;

    /// <summary>
    /// The current bound avatar that input signals are sent to.
    /// </summary>
    private IAvatar m_currentAvatar;

    /// <summary>
    /// Fills the current button map with each of the four cardinal buttons.
    /// </summary>
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

    /// <summary>
    /// After input has been processed and signaled, if any button map states are canceled, they are
    /// mapped to None. This is because an input can only be "canceled" for a frame.
    /// </summary>
    private void LateUpdate()
    {
        foreach (ButtonType button in types)
        if (CurrentButtons[button] == InputContextType.Canceled)
            CurrentButtons[button] = InputContextType.None;
    }

    /// <summary>
    /// Given an inputaction context, returns Started if the input was started, Performed if performed, and 
    /// Canceled otherwise.
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    private InputContextType ParseType(InputAction.CallbackContext context)
    {
        if (context.started) return InputContextType.Started;
        if (context.performed) return InputContextType.Performed;

        return InputContextType.Canceled;
    }

    /// <summary>
    /// Parses the current lateral input. Rather than using the input context to determine the input
    /// state, it is derived from the prior input compared with the current input.
    /// </summary>
    /// <param name="context"></param>
    public void OnLateral(InputAction.CallbackContext context)
    {
        var vector = context.ReadValue<Vector2>();
        var context_type = InputContextType.None;

        // if input is too weak, it's not to be used
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

    /// <summary>
    /// A button callback takes in a context and invokes its respective button.
    /// </summary>
    #region Button Callbacks
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
    #endregion

    /// <summary>
    /// Given an avatar, binds the callbacks to the two input events.
    /// </summary>
    /// <param name="target"></param>
    /// <exception cref="InvalidOperationException">Thrown if an avatar is already being possessed.</exception>
    public void Possess(IAvatar target)
    {
        if (m_currentAvatar != null)
        {
            throw new InvalidOperationException($"{gameObject.name} is already possessing another avatar!");
        }

        ButtonsEvent += target.OnButtonEvent;
        LateralEvent += target.OnLateralEvent;

        m_currentAvatar = target;
    }

    /// <summary>
    /// Frees the current avatar from the player and destroys if it request.
    /// </summary>
    /// <param name="destroy_possessed"></param>
    /// <exception cref="InvalidOperationException">Thrown if no avatar is being possessed.</exception>
    public void Free(bool destroy_possessed)
    {
        if (m_currentAvatar == null)
        {
            throw new InvalidOperationException($"{gameObject.name} isn't possessing an avatar!");
        }

        ButtonsEvent -= m_currentAvatar.OnButtonEvent;
        LateralEvent -= m_currentAvatar.OnLateralEvent;

        if (destroy_possessed) m_currentAvatar.DestroyAvatar();

        m_currentAvatar = null;
    }
}

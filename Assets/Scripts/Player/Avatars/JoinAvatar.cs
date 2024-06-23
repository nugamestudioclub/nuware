using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An avatar script for the join avatar. Provides behavior to leave a party and change the player's color.
/// </summary>
public class JoinAvatar : AbstractAvatar
{
    // serialized members
    [Header("Player Color Settings")]

    [SerializeField, Tooltip("The gradient of colors players can choose from.")]
    private Gradient m_playerColorGradient;

    [SerializeField, Tooltip("How much each hue change impulse should change the hue.")]
    private float m_hueChangeAmountScalar; // = 0.25f

    [SerializeField, Tooltip("How often the hue should be updated depending on hue difference."), Range(0f, 1f)]
    private float m_hueUpdateLimit; // = 0.05f

    [Header("Player Leave Settings")]
    
    [SerializeField, Tooltip("The button binding for leaving the game.")]
    private ButtonType m_leaveButton;

    [SerializeField, Tooltip("The impulse vector the avatar will be launched by when destroy is invoked.")]
    private Vector3 m_launchVector;

    [SerializeField, Tooltip("How long it take for the avatar to be deleted when DestroyAvatar is invoked.")]
    private float m_deathDuration; // = 4f

    // monobehavior members
    private MeshRenderer m_renderer; // cache
    private Rigidbody m_rigidbody; // cache

    // primitive data members
    /// <summary>
    /// The current hue value from 0-1.
    /// </summary>
    private float m_hue;

    /// <summary>
    /// The previous hue value at which we updated the color. Only changes
    /// when the color is updated, not when the hue value is updated.
    /// </summary>
    private float m_prevHue;

    /// <summary>
    /// How much we're changing the hue by in the Update loop.
    /// </summary>
    private float m_hueChangeAmount;

    /// <summary>
    /// It's possible for the player to join by pressing the leave button. Without this boolean to block leave attempt
    /// before the first tick, the player would join and instantly leave, allowing them to spawn a zillion avatars.
    /// 
    /// It's funny, though.
    /// </summary>
    private bool m_doIgnoreLeaveInput;

    /// <summary>
    /// Called upon creation. Caches components and sets up hue fields so that the first call to UpdateColor
    /// actually updates the color of the avatar.
    /// </summary>
    private void Awake()
    {
        m_doIgnoreLeaveInput = true;

        m_renderer = GetComponent<MeshRenderer>();
        m_rigidbody = GetComponent<Rigidbody>();

        m_hue = 0;
        m_prevHue = m_hueChangeAmount + 1f; // ensures that the first call to UpdateColor actually updates it.
    }

    /// <summary>
    /// Scales the hue by the input changeamount. Hue wraps if over/underflowing. If hue has changed enough,
    /// the visible color of the avatar changes.
    /// </summary>
    private void Update()
    {
        // if this ticks once, then we should stop ignoring input bc that means the player didn't join by pressing
        // the "leave" button.
        m_doIgnoreLeaveInput = false;

        if (Mathf.Approximately(m_hueChangeAmount, 0f)) return;

        m_hue += m_hueChangeAmount * Time.deltaTime;
        if (m_hue < 0f) m_hue = 0.99f;
        else if (m_hue > 1f) m_hue = 0.01f;

        UpdateColor();
    }

    /// <summary>
    /// Initializes the avatar to get the starting hue and updates it to be visible.
    /// </summary>
    protected override void InitAvatar()
    {
        Color.RGBToHSV(_playerData.Color, out m_hue, out _, out _);
        m_hue = 1 - m_hue; // invert the hue because im lazy and did the gradient the wrong way

        UpdateColor();
    }

    /// <summary>
    /// If the leave button is pressed, unbinds the player from this avatar, destroys this avatar, and removes the
    /// player from the party. This behavior is duplicated in the PlayerJoinManager because I don't want to have to
    /// get a reference to the class just for this method. Oh well.
    /// </summary>
    /// <param name="map"></param>
    public override void OnButtonEvent(IDictionary<ButtonType, InputContextType> map)
    {
        if (!m_doIgnoreLeaveInput && map[m_leaveButton] == InputContextType.Performed)
        {
            // free the player from this binding, invoking the DestroyAvatar method
            _partyManager.UnbindPlayer(_boundPlayerNumber);

            // then remove them from the party.
            _partyManager.RemovePlayerFromParty(_boundPlayerNumber);
        }
    }

    /// <summary>
    /// Resets the hue change amount when invoked. If performed, the hue change amount is read to be the x value of the input
    /// scaled by the hue change amount scalar value.
    /// </summary>
    /// <param name="data_tuple"></param>
    public override void OnLateralEvent((Vector2 Data, InputContextType ContextType) data_tuple)
    {
        // this being here is okay bc the current inputsystem settings are on the Update cycle.
        // if they weren't, this would get a bit strange.
        m_hueChangeAmount = 0f;

        if (data_tuple.ContextType != InputContextType.Performed) return;

        m_hueChangeAmount = data_tuple.Data.x * m_hueChangeAmountScalar; 
    }

    /// <summary>
    /// Launches the avatar away, destroying it a few seconds later decided by the death duration variable.
    /// </summary>
    public override void DestroyAvatar()
    {
        m_rigidbody.AddForce(m_launchVector, ForceMode.Impulse);

        Destroy(this.gameObject, m_deathDuration);
    }

    /// <summary>
    /// A private helper method to change the color of the player data and the avatar once it has changed enough.
    /// Changing the color every tick would be too expensive.
    /// </summary>
    private void UpdateColor()
    {
        // only update if the value has been changed enough.
        if (Mathf.Abs(m_prevHue - m_hue) < m_hueUpdateLimit) return;

        Color color = m_playerColorGradient.Evaluate(m_hue);

        m_renderer.material.color = color;
        _playerData.Color = color;

        m_prevHue = m_hue;
    }
}
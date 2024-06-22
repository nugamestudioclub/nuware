using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class JoinAvatar : AbstractAvatar
{
    /// <summary>
    /// The gradient of colors players can choose from.
    /// </summary>
    [SerializeField]
    private Gradient m_playerColorGradient;

    [SerializeField]
    private float m_hueChangeAmountScalar = 10f;

    [SerializeField, Range(0f, 1f)]
    private float m_hueUpdateLimit = 0.05f;

    private MeshRenderer m_renderer;

    private PlayerData m_playerData;

    private float m_hue;
    private float m_prevHue;
    private float m_hueChangeAmount;

    private void Awake()
    {
        m_renderer = GetComponent<MeshRenderer>();
    }

    private void Update()
    {
        m_hue += m_hueChangeAmount * Time.deltaTime;
        if (m_hue < 0f) m_hue = 0.99f;
        else if (m_hue > 1f) m_hue = 0.01f;

        UpdateColor();
    }

    public override void OnButtonEvent(IDictionary<ButtonType, InputContextType> map)
    {
        Debug.Log(PrintDictionary(map));
    }

    public override void OnLateralEvent((Vector2 Data, InputContextType ContextType) data_tuple)
    {
        m_hueChangeAmount = 0f;

        if (data_tuple.ContextType != InputContextType.Performed) return;

        m_hueChangeAmount = data_tuple.Data.x * m_hueChangeAmountScalar; 
    }

    protected override void InitAvatar()
    {
        m_playerData = _partyManager.GetPlayerData(_boundPlayerNumber);

        Debug.Log($"Hello hello, player {_boundPlayerNumber} of color {m_playerData.Color}");

        Color.RGBToHSV(m_playerData.Color, out m_hue, out _, out _);
        m_hue = 1 - m_hue; // invert the hue because im lazy and did the gradient the wrong way
    }

    private string PrintDictionary(IDictionary<ButtonType, InputContextType> map)
    {
        StringBuilder sb = new StringBuilder();

        foreach (var item in map)
        {
            sb.Append(item.ToString() + "\n");
        }

        return sb.ToString();
    }

    private void UpdateColor()
    {
        // only update if the value has been changed enough.
        if (Mathf.Abs(m_prevHue - m_hue) < m_hueUpdateLimit) return;

        m_renderer.material.color = m_playerColorGradient.Evaluate(m_hue);

        m_prevHue = m_hue;
    }
}

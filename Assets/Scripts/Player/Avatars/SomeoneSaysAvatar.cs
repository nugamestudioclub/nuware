using System.Collections.Generic;
using UnityEngine;

public class SomeoneSaysAvatar : AbstractAvatar
{
    [SerializeField] private MeshRenderer m_renderer;
    [SerializeField] private CodeType m_codeType;

    [SerializeField] private ButtonType m_binding = ButtonType.South;

    public override void OnButtonEvent(IDictionary<ButtonType, InputContextType> map)
    {
        if (map[m_binding] == InputContextType.Started)
        {

        }
    }

    protected override void InitAvatar()
    {
        m_renderer.material.color = _partyManager.GetPlayerData(_boundPlayerNumber).Color;
    }
}

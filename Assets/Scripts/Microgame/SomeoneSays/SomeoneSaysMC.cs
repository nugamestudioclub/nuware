using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SomeoneSaysMC : AMicrogame
{
    [SerializeField] private MicrogameData m_data;
    [SerializeField] private float m_time;
    [SerializeField] private SerializeablePair<DifficultyType, int>[] m_codeLengths = new SerializeablePair<DifficultyType, int>[5];

    [Space(10)]

    [SerializeField] private List<SerializeablePair<CodeType, Transform>> m_buttonAssociations;

    private int m_codeLen;
    private Queue<CodeType> m_code;

    public override MicrogameData GetData()
    {
        return m_data;
    }

    protected override float CalculateGameDuration(DifficultyType difficulty)
    {
        // game duration doesn't scale with difficulty; the lengths of the codes do.
        return m_time;
    }

    public override void StartGame()
    {
        // generate code
    }

    public void PressButton(CodeType code) 
    {
        
    }

    private void IEAnimateButtonPress(Transform transform)
    {

    }
}

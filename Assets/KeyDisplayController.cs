using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyDisplayController : MonoBehaviour
{
    private GameObject key;
    private List<string> buttons = new List<string> { "A", "B", "X", "Y" };
    private string selectedButton;

    // Start is called before the first frame update
    void Start()
    {
        key = transform.GetChild(0).gameObject;
        selectedButton = buttons[Random.Range(0, buttons.Count)];
        StartCoroutine(UpdateButton());
    }

    IEnumerator UpdateButton()
    {
        TMPro.TMP_Text text = key.GetComponent<TMPro.TMP_Text>();
        while (true)
        {
            //text.SetText(counter.ToString());
            text.SetText(selectedButton);
            RandomizeButton();
            yield return new WaitForSeconds(1);
        }
    }

    private void RandomizeButton()
    {
        string newButton;
        do
        {
            newButton = buttons[Random.Range(0, buttons.Count)];
        } while (newButton == selectedButton);
        selectedButton = newButton;
    }
}
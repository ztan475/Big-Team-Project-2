using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextDisplay : MonoBehaviour
{
    [Header("UI Reference")]
    [SerializeField] private TextMeshProUGUI textDisplay;

    [Header("Text Sequence")]
    [TextArea(2, 5)]
    [SerializeField] private List<string> textSequence = new List<string>();

    private int currentIndex = 0;

    void Start()
    {
        if (textDisplay == null)
        {
            Debug.LogError("TextDisplay is not assigned!");
            return;
        }

        if (textSequence.Count > 0)
        {
            textDisplay.text = textSequence[0];
        }
        else
        {
            textDisplay.gameObject.SetActive(false); // Nothing to show
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            ShowNextText();
        }
    }

    void ShowNextText()
    {
        currentIndex++;

        if (currentIndex < textSequence.Count)
        {
            textDisplay.text = textSequence[currentIndex];
        }
        else
        {
            textDisplay.gameObject.SetActive(false); // Hide when finished
            enabled = false; // Optionally stop checking input
        }
    }
}

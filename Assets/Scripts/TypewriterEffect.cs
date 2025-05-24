using TMPro;
using UnityEngine;
using System.Collections;

public class TypewriterEffect : MonoBehaviour
{
    public TextMeshProUGUI textMesh;
    public string fullText = "You’re Alex — a tired indie game developer working late at the office during a game jam crunch.";
    public float delay = 0.04f;

    void Start() => StartCoroutine(ShowText());

    IEnumerator ShowText()
    {
        textMesh.text = "";
        for (int i = 0; i <= fullText.Length; i++)
        {
            textMesh.text = fullText.Substring(0, i);
            yield return new WaitForSeconds(delay);
        }
    }
}

using TMPro;
using UnityEngine;
using System.Collections;

public class TypewriterEffect : MonoBehaviour
{
    public TextMeshProUGUI textMesh;
    [TextArea] public string fullText;
    public float delay = 0.04f;

    private Coroutine typingCoroutine;

    public void StartTyping()
    {
        // Reset text and stop any previous coroutine
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeText());
    }

    IEnumerator TypeText()
    {
        textMesh.text = "";
        for (int i = 0; i <= fullText.Length; i++)
        {
            textMesh.text = fullText.Substring(0, i);
            yield return new WaitForSeconds(delay);
        }
    }
}

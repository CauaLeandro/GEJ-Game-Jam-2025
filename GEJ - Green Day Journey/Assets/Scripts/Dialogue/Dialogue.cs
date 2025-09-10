using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class SimpleDialogue : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;
    public string[] messages;
    public Button finalButton;
    public float typingSpeed = 0.05f;

    private int index = 0;
    private bool isTyping = false;
    private Coroutine typingCoroutine;

    void Start()
    {
        if (messages.Length > 0)
            StartTypingMessage(messages[index]);

        if (finalButton != null)
            finalButton.gameObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isTyping)
            {
                SkipTyping();
            }
            else
            {
                NextMessage();
            }
        }
    }

    private void StartTypingMessage(string message)
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeText(message));
    }

    IEnumerator TypeText(string message)
    {
        isTyping = true;
        dialogueText.text = "";

        foreach (char c in message)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }

    private void SkipTyping()
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        dialogueText.text = messages[index];
        isTyping = false;
    }

    public void NextMessage()
    {
        index++;

        if (index < messages.Length)
        {
            StartTypingMessage(messages[index]);
        }
        else
        {
            if (finalButton != null)
                finalButton.gameObject.SetActive(true);
        }
    }
}

using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public GameObject dialogueCanvas;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            dialogueCanvas.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            dialogueCanvas.SetActive(false);
        }
    }
}
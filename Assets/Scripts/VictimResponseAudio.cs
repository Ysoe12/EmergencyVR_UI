using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class VictimResponseAudio : MonoBehaviour
{
    private AudioSource audioSource;
    private XRSimpleInteractable interactable;
    private bool reproduciendo = false;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        interactable = GetComponent<XRSimpleInteractable>();
    }

    private void OnEnable()
    {
        reproduciendo = false;
        if (audioSource != null)
        {
            audioSource.Stop();
        }

        if (interactable == null)
            interactable = GetComponent<XRSimpleInteractable>();

        if (interactable != null)
        {
            interactable.selectEntered.AddListener(OnSelectEntered);
        }
    }

    private void OnDisable()
    {
        if (interactable != null)
        {
            interactable.selectEntered.RemoveListener(OnSelectEntered);
        }
    }

    private void OnSelectEntered(SelectEnterEventArgs args)
    {
        if (!TrainingManager.EntrenamientoIniciado) return;

        if (audioSource == null) return;

        if (!reproduciendo)
        {
            audioSource.Play();
            reproduciendo = true;
        }
        else
        {
            audioSource.Stop();
            reproduciendo = false;
        }
    }
}

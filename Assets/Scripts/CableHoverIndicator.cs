using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class CableHoverIndicator : MonoBehaviour
{
    [Header("Indicador")]
    [SerializeField] private GameObject indicador;
    [SerializeField] private Vector3 offset = new Vector3(0f, 0.35f, 0f);

    private XRSimpleInteractable interactable;
    private Renderer cableRenderer;

    private void Awake()
    {
        interactable = GetComponent<XRSimpleInteractable>();
        cableRenderer = GetComponent<Renderer>();
    }

    private void OnEnable()
    {
        if (interactable == null)
            interactable = GetComponent<XRSimpleInteractable>();

        if (cableRenderer == null)
            cableRenderer = GetComponent<Renderer>();

        if (interactable != null)
        {
            interactable.hoverEntered.AddListener(OnHoverEntered);
            interactable.hoverExited.AddListener(OnHoverExited);
            interactable.selectEntered.AddListener(OnSelectEntered);
        }

        if (indicador != null)
        {
            indicador.SetActive(false);
        }
    }

    private void OnDisable()
    {
        if (interactable != null)
        {
            interactable.hoverEntered.RemoveListener(OnHoverEntered);
            interactable.hoverExited.RemoveListener(OnHoverExited);
            interactable.selectEntered.RemoveListener(OnSelectEntered);
        }

        if (indicador != null)
        {
            indicador.SetActive(false);
        }
    }

    private void OnHoverEntered(HoverEnterEventArgs args)
    {
        if (!TrainingManager.EntrenamientoIniciado) return;
        if (indicador == null) return;

        Vector3 targetPosition;
        if (cableRenderer != null)
        {
            targetPosition = cableRenderer.bounds.center + offset;
        }
        else
        {
            targetPosition = transform.position + offset;
        }

        indicador.transform.position = targetPosition;
        indicador.SetActive(true);
    }

    private void OnHoverExited(HoverExitEventArgs args)
    {
        if (indicador != null)
        {
            indicador.SetActive(false);
        }
    }

    private void OnSelectEntered(SelectEnterEventArgs args)
    {
        if (!TrainingManager.EntrenamientoIniciado) return;

        if (indicador != null)
        {
            indicador.SetActive(false);
        }

        gameObject.SetActive(false);
    }
}

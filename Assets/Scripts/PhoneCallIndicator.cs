using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class PhoneCallIndicator : MonoBehaviour
{
    [Header("Indicadores")]
    [SerializeField] private GameObject indicadorLlamando;
    [SerializeField] private GameObject indicadorPanelNumerico;
    [SerializeField] private GameObject indicadorEnCamino;

    private XRSimpleInteractable interactable;
    private int estadoActual = 0;

    private void Awake()
    {
        interactable = GetComponent<XRSimpleInteractable>();
    }

    private void OnEnable()
    {
        estadoActual = 0;
        ActualizarIndicadores();

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

        estadoActual = (estadoActual + 1) % 4;
        ActualizarIndicadores();
    }

    private void ActualizarIndicadores()
    {
        if (indicadorLlamando != null)
            indicadorLlamando.SetActive(estadoActual == 1);

        if (indicadorPanelNumerico != null)
            indicadorPanelNumerico.SetActive(estadoActual == 2);

        if (indicadorEnCamino != null)
            indicadorEnCamino.SetActive(estadoActual == 3);
    }
}

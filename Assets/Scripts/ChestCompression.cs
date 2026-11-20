using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class ChestCompression : MonoBehaviour
{
    [Header("Bones")]
    [SerializeField] private Transform hipBone;
    [SerializeField] private Transform[] pinnedBones;

    [Header("Compression Settings")]
    [SerializeField] private float maxDepth = 0.06f;
    [SerializeField] private float pressSpeed = 0.30f;
    [SerializeField] private float recoilSpeed = 0.40f;

    private float depth;
    private Vector3 hipBoneRestLocalPosition;
    private Vector3[] savedWorldPositions;
    private bool isPressing;
    private XRSimpleInteractable interactable;

    private void Awake()
    {
        interactable = GetComponent<XRSimpleInteractable>();
    }

    private void Start()
    {
        if (hipBone != null)
        {
            hipBoneRestLocalPosition = hipBone.localPosition;
        }

        if (pinnedBones != null && pinnedBones.Length > 0)
        {
            savedWorldPositions = new Vector3[pinnedBones.Length];
        }
    }

    private void OnEnable()
    {
        isPressing = false;

        if (interactable != null)
        {
            interactable.selectEntered.AddListener(OnSelectEntered);
            interactable.selectExited.AddListener(OnSelectExited);
        }
    }

    private void OnDisable()
    {
        if (interactable != null)
        {
            interactable.selectEntered.RemoveListener(OnSelectEntered);
            interactable.selectExited.RemoveListener(OnSelectExited);
        }
    }

    private void OnSelectEntered(SelectEnterEventArgs args)
    {
        if (!TrainingManager.EntrenamientoIniciado)
        {
            isPressing = false;
            return;
        }
        isPressing = true;
    }

    private void OnSelectExited(SelectExitEventArgs args)
    {
        if (!TrainingManager.EntrenamientoIniciado)
        {
            isPressing = false;
            return;
        }
        isPressing = false;
    }

    private void Update()
    {
        float target = isPressing ? maxDepth : 0f;
        float speed = isPressing ? pressSpeed : recoilSpeed;

        depth = Mathf.MoveTowards(depth, target, speed * Time.deltaTime);
    }

    private void LateUpdate()
    {
        if (hipBone == null) return;

        // 1. Guardar la posición WORLD actual de cada hueso anclado
        if (pinnedBones != null)
        {
            if (savedWorldPositions == null || savedWorldPositions.Length != pinnedBones.Length)
            {
                savedWorldPositions = new Vector3[pinnedBones.Length];
            }

            for (int i = 0; i < pinnedBones.Length; i++)
            {
                if (pinnedBones[i] != null)
                {
                    savedWorldPositions[i] = pinnedBones[i].position;
                }
            }
        }

        // 2. Calcular el offset hacia abajo en espacio mundo
        Vector3 offset = Vector3.down * depth;

        // 3. Mover hipBone aplicando el offset en su espacio local
        if (hipBone.parent != null)
        {
            hipBone.localPosition = hipBoneRestLocalPosition + hipBone.parent.InverseTransformVector(offset);
        }
        else
        {
            hipBone.position += offset;
        }

        // 4. Restaurar la posición WORLD de los huesos anclados
        if (pinnedBones != null)
        {
            for (int i = 0; i < pinnedBones.Length; i++)
            {
                if (pinnedBones[i] != null)
                {
                    pinnedBones[i].position = savedWorldPositions[i];
                }
            }
        }
    }
}

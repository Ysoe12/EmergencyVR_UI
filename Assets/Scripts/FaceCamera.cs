using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    [Header("Configuración de Cámara")]
    [Tooltip("Cámara objetivo. Si no se asigna, buscará automáticamente Camera.main.")]
    [SerializeField] private Camera targetCamera;

    [Tooltip("Si es true, iguala la rotación exacta de la cámara. Si es false, orienta desde la posición de la cámara.")]
    [SerializeField] private bool matchCameraRotation = false;

    private void Awake()
    {
        FindCamera();
    }

    private void OnEnable()
    {
        FindCamera();
    }

    private void LateUpdate()
    {
        if (targetCamera == null || !targetCamera.isActiveAndEnabled)
        {
            FindCamera();
            if (targetCamera == null) return;
        }

        if (matchCameraRotation)
        {
            transform.rotation = targetCamera.transform.rotation;
        }
        else
        {
            Vector3 dirFromCamera = transform.position - targetCamera.transform.position;
            if (dirFromCamera.sqrMagnitude > 0.0001f)
            {
                // En un World Space Canvas de Unity, el contenido se dibuja en el plano XY
                // y su cara frontal mira en la dirección de su eje local +Z.
                // Apuntando +Z en la dirección (posición - cámara), el vector apunta alejándose
                // de la cámara, por lo que el jugador mira de frente el Canvas y no de espaldas (evitando el efecto espejo).
                transform.rotation = Quaternion.LookRotation(dirFromCamera, targetCamera.transform.up);
            }
        }
    }

    private void FindCamera()
    {
        if (Camera.main != null && Camera.main.isActiveAndEnabled)
        {
            targetCamera = Camera.main;
        }
    }
}

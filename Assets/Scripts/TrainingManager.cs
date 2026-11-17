using UnityEngine;

public class TrainingManager : MonoBehaviour
{
    public static bool EntrenamientoIniciado { get; private set; } = false;

    public void StartTraining()
    {
        EntrenamientoIniciado = true;
        Debug.Log("[TrainingManager] El entrenamiento ha iniciado.");
    }
}

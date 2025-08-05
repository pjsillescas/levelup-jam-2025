using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Splines; // Asegúrate de importar el namespace correcto

public class NenufarDistributor : MonoBehaviour
{
    // Lista de nenúfares a distribuir
    public List<GameObject> nenufares;

    // Referencia al spline
    public SplineContainer spline; // Usando el tipo correcto para Unity Splines

    public float duration = 1f; // Nuevo campo para asignar duración

    void Start()
    {
        if (spline == null || nenufares == null || nenufares.Count == 0)
            return;

        int count = nenufares.Count;
        for (int i = 0; i < count; i++)
        {
            // Distribuye equitativamente usando t de 0 a 1
            float t = (float)i / count;
            var animate = nenufares[i].GetComponent<SplineAnimate>();
            if (animate != null)
            {
                animate.Container = spline;
                animate.StartOffset = t;
                animate.Duration = duration; // Asigna la duración
            }
        }
    }
}

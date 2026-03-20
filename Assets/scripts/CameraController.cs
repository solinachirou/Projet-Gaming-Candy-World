using UnityEngine;

public class CameraController : MonoBehaviour
{
    // On glissera le Player ici dans l'Inspector
    public Transform target; 

    // Réglage de la distance (souvent 0, 0, -10)
    public Vector3 offset = new Vector3(0, 0, -10);

    // Vitesse du lissage (plus c'est petit, plus c'est fluide)
    public float smoothTime = 0.25f;
    
    private Vector3 velocity = Vector3.zero;

    // LateUpdate s'exécute APRÈS le mouvement du joueur
    void LateUpdate()
    {
        if (target != null)
        {
            Vector3 targetPosition = target.position + offset;
            // Cette ligne crée le mouvement "élastique" et fluide
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        }
    }
}
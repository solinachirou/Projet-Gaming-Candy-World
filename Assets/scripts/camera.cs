using UnityEngine;

public class CameraSuiveuse : MonoBehaviour
{
    public Transform cible; // Le joueur à suivre
    public Vector3 decalage = new Vector3(0f, 0f, -10f); // Le -10 empêche l'écran noir en 2D
    public float fluidite = 5f; // La vitesse de suivi

    void LateUpdate()
    {
        if (cible != null)
        {
            // Calcule la position idéale où la caméra doit aller
            Vector3 positionFinale = cible.position + decalage;
            
            // Déplace la caméra doucement vers cette position
            transform.position = Vector3.Lerp(transform.position, positionFinale, fluidite * Time.deltaTime);
        }
    }
}
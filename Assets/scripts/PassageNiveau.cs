using UnityEngine;
using UnityEngine.SceneManagement; 

public class PassageNiveau : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // On vérifie que c'est bien le joueur qui entre dans le château
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Niveau terminé !");
            
            // On demande à Unity de charger la scène qui a le numéro suivant
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
using UnityEngine;

public class MonstreVolant : MonoBehaviour
{
    [Header("Chemin de vol")]
    public Transform pointA;
    public Transform pointB;
    public float vitesse = 2f;

    private Vector3 cibleActuelle;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        // On détache les points du monstre pour ne pas qu'ils bougent avec lui
        pointA.parent = null;
        pointB.parent = null;

        // Il commence par se diriger vers le Point B
        cibleActuelle = pointB.position;
    }

    void Update()
    {
        // Déplace le monstre doucement vers sa cible
        transform.position = Vector3.MoveTowards(transform.position, cibleActuelle, vitesse * Time.deltaTime);

        // S'il est arrivé très près de sa cible, il change de direction
        if (Vector3.Distance(transform.position, cibleActuelle) < 0.1f)
        {
            if (cibleActuelle == pointA.position)
            {
                cibleActuelle = pointB.position;
                spriteRenderer.flipX = false; // Regarde vers la droite
            }
            else
            {
                cibleActuelle = pointA.position;
                spriteRenderer.flipX = true; // Regarde vers la gauche
            }
        }
    }
}
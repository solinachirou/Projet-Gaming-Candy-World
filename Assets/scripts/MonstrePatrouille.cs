using UnityEngine;

public class MonstrePatrouille : MonoBehaviour
{
    public float vitesse = 1f; 
    private int direction = -1; // -1 = gauche, 1 = droite

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Animator anim;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        // CORRECTION DU MOONWALK AU DÉMARRAGE :
        // Comme le dessin regarde à droite, s'il va à gauche on doit le retourner direct !
        if (direction == -1) spriteRenderer.flipX = true;
    }

    void Update()
    {
        rb.linearVelocity = new Vector2(direction * vitesse, rb.linearVelocity.y);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Tourne"))
        {
            FaireDemiTour();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // On vérifie qu'il touche bien le joueur (N'oublie pas de mettre le Tag Player à ton personnage !)
        if (collision.gameObject.CompareTag("Player"))
        {
            anim.SetTrigger("Frappe");
            Debug.Log("BANG ! Le monstre donne un coup !");
        }
    }

    void FaireDemiTour()
    {
        direction *= -1; 
        
        // CORRECTION DU MOONWALK QUAND IL TOURNE :
        // S'il va à droite (1), on remet l'image normale. S'il va à gauche (-1), on la retourne.
        if (direction == 1) spriteRenderer.flipX = false;
        else spriteRenderer.flipX = true;
    }
}
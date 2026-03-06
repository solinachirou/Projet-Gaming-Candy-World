using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Variables que tu pourras modifier dans Unity
    public float vitesse = 5f;
    public float forceSaut = 7f;

    private Rigidbody2D rb;
    private bool toucheLeSol;

    void Start()
    {
        // On récupère le composant physique de ton carré au lancement du jeu
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // 1. DÉPLACEMENT HORIZONTAL (Flèches gauche/droite ou touches Q/D)
        float mouvement = Input.GetAxis("Horizontal");
        rb.linearVelocity = new Vector2(mouvement * vitesse, rb.linearVelocity.y);

        // 2. SAUT (Barre Espace)
        // On vérifie qu'on appuie sur Espace ET qu'on touche le sol
        if (Input.GetButtonDown("Jump") && toucheLeSol)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, forceSaut);
        }
    }

    // --- VÉRIFICATION DU SOL ---
    // Quand le carré touche tes biscuits
    private void OnCollisionEnter2D(Collision2D collision)
    {
        toucheLeSol = true;
    }

    // Quand le carré quitte tes biscuits (pendant le saut)
    private void OnCollisionExit2D(Collision2D collision)
    {
        toucheLeSol = false;
    }
}
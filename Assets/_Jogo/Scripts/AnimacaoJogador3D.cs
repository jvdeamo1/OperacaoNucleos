using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class AnimacaoJogador3D : MonoBehaviour
{
    [SerializeField] private Animator animator;

    private CharacterController controlador;

    private void Awake()
    {
        controlador = GetComponent<CharacterController>();

        if (animator == null)
        {
            Debug.LogError(
                "Arraste o Animator do FBX_Player para o campo Animator.", this);
            enabled = false;
        }
    }

    private void LateUpdate()
    {
        // Jogador3D já chamou Move no Update deste quadro.
        Vector3 velocidade = controlador.velocity;
        float velocidadeHorizontal =
            new Vector2(velocidade.x, velocidade.z).magnitude;

        animator.SetFloat("Velocidade", velocidadeHorizontal);
        animator.SetBool("NoChao", controlador.isGrounded);
        animator.SetBool("Subindo", velocidade.y > 0.1f);
    }
}
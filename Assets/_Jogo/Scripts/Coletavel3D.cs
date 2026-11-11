using UnityEngine;

[RequireComponent(typeof(SphereCollider), typeof(Rigidbody))]
public class Coletavel3D : MonoBehaviour
{
    [SerializeField] private Transform visual;
    [SerializeField] private float giro = 90f;
    [SerializeField] private float amplitude = 0.15f;
    [SerializeField] private float frequencia = 2f;
    private Vector3 posicaoInicial;
    private bool coletado;

    private void Awake()
    {
        if (visual != null) posicaoInicial = visual.localPosition;
    }

    private void Update()
    {
        if (visual == null) return;
        visual.Rotate(Vector3.up, giro * Time.deltaTime, Space.Self);
        visual.localPosition = posicaoInicial
            + Vector3.up * Mathf.Sin(Time.time * frequencia) * amplitude;
    }

    private void OnTriggerEnter(Collider other)
    {
        Jogador3D jogador = other.GetComponentInParent<Jogador3D>();
        if (coletado || jogador == null) return;
        if (!jogador.Jogo.RegistrarColeta()) return;

        coletado = true; // Impede contagem dupla antes do Destroy.
        GetComponent<SphereCollider>().enabled = false;
        Destroy(gameObject);
    }
}
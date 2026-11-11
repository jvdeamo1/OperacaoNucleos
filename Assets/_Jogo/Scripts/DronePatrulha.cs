using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class DronePatrulha : MonoBehaviour
{
    [SerializeField] private Transform[] pontos;
    [SerializeField] private float velocidade = 6f;
    private Rigidbody corpo;
    private int indice = 1;

    private void Awake()
    {
        corpo = GetComponent<Rigidbody>();
        if (pontos == null || pontos.Length < 2)
        {
            Debug.LogError("O drone precisa de pelo menos dois pontos.", this);
            enabled = false;
        }
    }

    private void FixedUpdate()
    {
        if (pontos[indice] == null) return;
        Vector3 destino = pontos[indice].position;
        Vector3 proxima = Vector3.MoveTowards(corpo.position, destino,
            velocidade * Time.fixedDeltaTime);
        corpo.MovePosition(proxima);

        if ((proxima - destino).sqrMagnitude < 0.0025f)
            indice = (indice + 1) % pontos.Length;
    }
}
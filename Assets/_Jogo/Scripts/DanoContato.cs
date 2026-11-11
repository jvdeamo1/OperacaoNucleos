using UnityEngine;

[RequireComponent(typeof(Collider))]
public class DanoContato : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) => TentarDano(other);
    private void OnTriggerStay(Collider other) => TentarDano(other);

    private void TentarDano(Collider other)
    {
        Jogador3D jogador = other.GetComponentInParent<Jogador3D>();
        if (jogador != null) jogador.Jogo.ReceberDano();
    }
}
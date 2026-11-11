using UnityEngine;

public class TerminalSaida : MonoBehaviour
{
    [SerializeField] private Jogador3D jogador;
    [SerializeField] private Transform pontoInteracao;
    [SerializeField] private float alcance = 2f;

    private void Update()
    {
        if (jogador == null || pontoInteracao == null) return;
        GerenciadorJogo jogo = jogador.Jogo;
        if (!jogo.EmAndamento) return;

        float distancia = Vector3.Distance(jogador.transform.position,
            pontoInteracao.position);
        if (distancia > alcance) return;

        jogo.MostrarAviso(jogo.TodosColetados
            ? "E / botão oeste: ativar terminal"
            : $"Faltam {jogo.ItensRestantes} núcleos. Explore a instalação!", 0.15f);

        if (jogador.QuerInteragir) jogo.TentarVencer();
    }
}
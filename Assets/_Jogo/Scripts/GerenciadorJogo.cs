using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GerenciadorJogo : MonoBehaviour
{
    [Header("Regras")]
    [SerializeField, Min(1)] private int totalItens = 6;
    [SerializeField, Min(1)] private int vidasIniciais = 3;
    [SerializeField, Min(1f)] private float duracao = 180f;
    [SerializeField, Min(0.1f)] private float intervaloDano = 1.25f;

    [Header("Interface")]
    [SerializeField] private TMP_Text textoItens;
    [SerializeField] private TMP_Text textoVidas;
    [SerializeField] private TMP_Text textoTempo;
    [SerializeField] private TMP_Text textoAviso;
    [SerializeField] private TMP_Text textoResultado;
    [SerializeField] private GameObject painelResultado;
    [SerializeField] private GameObject botaoReiniciar;

    [Header("Audio opcional")]
    [SerializeField] private AudioSource fonteEfeitos;
    [SerializeField] private AudioClip somColeta;
    [SerializeField] private AudioClip somDano;
    [SerializeField] private AudioClip somVitoria;
    [SerializeField] private AudioClip somDerrota;

    private int coletados;
    private int vidas;
    private float tempoRestante;
    private float proximoDano;
    private float fimAviso;

    public bool EmAndamento { get; private set; } = true;
    public bool TodosColetados => coletados >= totalItens;
    public int ItensRestantes => Mathf.Max(0, totalItens - coletados);

    private void Awake()
    {
        // Reinicia tambem o tempo global ao recarregar a cena.
        Time.timeScale = 1f;
        coletados = 0;
        vidas = vidasIniciais;
        tempoRestante = duracao;
        EmAndamento = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Start()
    {
        if (painelResultado != null) painelResultado.SetActive(false);
        if (textoAviso != null) textoAviso.text = "";
        AtualizarHUD();
    }

    private void Update()
    {
        if (!EmAndamento) return;

        tempoRestante = Mathf.Max(0f, tempoRestante - Time.deltaTime);
        if (textoAviso != null && Time.time >= fimAviso)
            textoAviso.text = "";

        AtualizarHUD();
        if (tempoRestante <= 0f)
            Encerrar(false, "O tempo acabou!");
    }

    public bool RegistrarColeta()
    {
        if (!EmAndamento || TodosColetados) return false;

        coletados++;
        Tocar(somColeta);
        AtualizarHUD();
        MostrarAviso(TodosColetados
            ? "Todos os núcleos! Volte ao terminal."
            : "+1 núcleo!", 2f);
        return true;
    }

    public void ReceberDano()
    {
        // A imunidade vale para todos os drones, evitando dano por quadro.
        if (!EmAndamento || Time.time < proximoDano) return;

        proximoDano = Time.time + intervaloDano;
        vidas--;
        Tocar(somDano);
        AtualizarHUD();

        if (vidas <= 0) Encerrar(false, "Suas vidas acabaram!");
        else MostrarAviso("Dano! Afaste-se do drone.", 1.5f);
    }

    public void TentarVencer()
    {
        if (!EmAndamento) return;
        if (TodosColetados) Encerrar(true, "Terminal ativado!");
        else MostrarAviso($"Faltam {ItensRestantes} núcleos.", 2f);
    }

    public void DerrotaPorQueda()
    {
        Encerrar(false, "Você caiu para fora da instalação!");
    }

    public void MostrarAviso(string mensagem, float segundos)
    {
        if (!EmAndamento) return;
        if (textoAviso != null) textoAviso.text = mensagem;
        fimAviso = Time.time + segundos;
    }

    private void AtualizarHUD()
    {
        if (textoItens != null) textoItens.text = $"Núcleos: {coletados}/{totalItens}";
        if (textoVidas != null) textoVidas.text = $"Vidas: {vidas}";
        if (textoTempo != null)
            textoTempo.text = $"Tempo: {Mathf.CeilToInt(tempoRestante)} s";
    }

    private void Encerrar(bool venceu, string mensagem)
    {
        if (!EmAndamento) return;

        EmAndamento = false;
        if (textoAviso != null) textoAviso.text = "";
        if (textoResultado != null)
            textoResultado.text = (venceu ? "VITÓRIA!\n" : "FIM DE JOGO\n") + mensagem;
        if (painelResultado != null) painelResultado.SetActive(true);
        if (EventSystem.current != null && botaoReiniciar != null)
            EventSystem.current.SetSelectedGameObject(botaoReiniciar);

        Tocar(venceu ? somVitoria : somDerrota);
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void Tocar(AudioClip clipe)
    {
        if (fonteEfeitos != null && clipe != null)
            fonteEfeitos.PlayOneShot(clipe);
    }

    public void Reiniciar()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void VoltarAoMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MenuPrincipal");
    }
    public void Sair()
    {
        Application.Quit(); // Fecha o executavel; nao encerra o Play no Editor.
    }
}
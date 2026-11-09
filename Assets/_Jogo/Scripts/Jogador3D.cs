using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class Jogador3D : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private GerenciadorJogo jogo;
    [SerializeField] private Transform visual;
    [SerializeField] private Transform alvoCamera;
    [SerializeField] private InputActionAsset controles;

    [Header("Movimento")]
    [SerializeField] private float velocidade = 5f;
    [SerializeField] private float alturaPulo = 1.6f;
    [SerializeField] private float gravidade = -20f;
    [SerializeField] private float velocidadeGiro = 720f;

    [Header("Camera")]
    [SerializeField] private float sensibilidadeMouse = 0.12f;
    [SerializeField] private float velocidadeOlharControle = 120f;
    [SerializeField] private float inclinacaoMinima = -25f;
    [SerializeField] private float inclinacaoMaxima = 65f;

    private CharacterController controller;
    private InputActionAsset controlesAtivos;
    private InputActionMap mapa;
    private InputAction mover, olharMouse, olharControle;
    private InputAction pular, interagir, alternarCursor, capturarCursor;
    private float velocidadeVertical;
    private float anguloHorizontal;
    private float anguloVertical = 15f;
    private bool cursorCapturado = true;

    public GerenciadorJogo Jogo => jogo;
    public bool QuerInteragir => jogo.EmAndamento
        && Cursor.lockState == CursorLockMode.Locked
        && interagir.WasPressedThisFrame();

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        if (jogo == null || visual == null || alvoCamera == null || controles == null)
        {
            Debug.LogError("Preencha as quatro referências do Jogador3D.", this);
            enabled = false;
            return;
        }

        // A copia pertence a este jogador; nao alteramos o asset original.
        controlesAtivos = Instantiate(controles);
        mapa = controlesAtivos.FindActionMap("Jogador", true);
        mover = mapa.FindAction("Mover", true);
        olharMouse = mapa.FindAction("OlharMouse", true);
        olharControle = mapa.FindAction("OlharControle", true);
        pular = mapa.FindAction("Pular", true);
        interagir = mapa.FindAction("Interagir", true);
        alternarCursor = mapa.FindAction("AlternarCursor", true);
        capturarCursor = mapa.FindAction("CapturarCursor", true);
        anguloHorizontal = alvoCamera.eulerAngles.y;
        alvoCamera.rotation = Quaternion.Euler(anguloVertical, anguloHorizontal, 0f);
    }

    private void OnEnable() => mapa?.Enable();
    private void OnDisable() => mapa?.Disable();

    private void OnDestroy()
    {
        if (controlesAtivos != null) Destroy(controlesAtivos);
    }

    private void Update()
    {
        if (!jogo.EmAndamento) return;

        if (alternarCursor.WasPressedThisFrame())
            DefinirCursor(!cursorCapturado);

        if (capturarCursor.WasPressedThisFrame()
            && Cursor.lockState != CursorLockMode.Locked)
        {
            DefinirCursor(true);
            return; // Descarta o movimento do mouse do clique de recaptura.
        }

        bool entradaAtiva = Cursor.lockState == CursorLockMode.Locked;
        if (entradaAtiva)
        {
            Vector2 mouse = olharMouse.ReadValue<Vector2>();
            Vector2 controle = olharControle.ReadValue<Vector2>();

            // Mouse: deslocamento por quadro. Stick: velocidade por segundo.
            anguloHorizontal += mouse.x * sensibilidadeMouse
                + controle.x * velocidadeOlharControle * Time.deltaTime;
            anguloVertical -= mouse.y * sensibilidadeMouse
                + controle.y * velocidadeOlharControle * Time.deltaTime;
            anguloVertical = Mathf.Clamp(anguloVertical, inclinacaoMinima, inclinacaoMaxima);
        }

        // O Cinemachine acompanha este alvo, sem ser movimentado diretamente.
        alvoCamera.rotation = Quaternion.Euler(anguloVertical, anguloHorizontal, 0f);

        Vector2 entrada = entradaAtiva ? mover.ReadValue<Vector2>() : Vector2.zero;
        Vector3 direcao = Quaternion.Euler(0f, anguloHorizontal, 0f)
            * new Vector3(entrada.x, 0f, entrada.y);
        direcao = Vector3.ClampMagnitude(direcao, 1f);

        // Gira somente o modelo; a raiz e o alvo da camera ficam independentes.
        if (direcao.sqrMagnitude > 0.001f)
            visual.rotation = Quaternion.RotateTowards(visual.rotation,
                Quaternion.LookRotation(direcao), velocidadeGiro * Time.deltaTime);

        if (controller.isGrounded && velocidadeVertical < 0f)
            velocidadeVertical = -2f;

        if (entradaAtiva && controller.isGrounded && pular.WasPressedThisFrame())
            velocidadeVertical = Mathf.Sqrt(-2f * gravidade * alturaPulo);

        velocidadeVertical += gravidade * Time.deltaTime;
        Vector3 movimento = direcao * velocidade + Vector3.up * velocidadeVertical;
        CollisionFlags colisoes = controller.Move(movimento * Time.deltaTime);

        if ((colisoes & CollisionFlags.Above) != 0 && velocidadeVertical > 0f)
            velocidadeVertical = 0f;

        if (transform.position.y < -5f) jogo.DerrotaPorQueda();
    }

    private void DefinirCursor(bool capturar)
    {
        cursorCapturado = capturar;
        Cursor.lockState = capturar ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !capturar;
    }
}
# Operação Núcleos

**Jogo 3D de exploração e coleta em terceira pessoa**, desenvolvido em Unity para Windows.

Explore uma instalação, suba nas plataformas e desvie dos drones de patrulha para recuperar **seis núcleos de energia**. Depois de coletar todos, volte ao terminal e interaja com ele para concluir a missão.

O projeto nasceu como exercício de desenvolvimento 3D para quem já conhece a interface da Unity e está começando a programar em C#. A primeira arena reúne movimento, câmera, colisão, coleta e interface em uma partida completa.

## Como jogar

1. Explore a arena e passe pelos núcleos para coletá-los.
2. Use o pulo para chegar às plataformas e observe a rota dos drones antes de atravessar.
3. Com o contador em **6/6**, aproxime-se do terminal e interaja para vencer.

Na configuração padrão, você começa com **3 vidas e 180 segundos**. Tocar em um drone causa dano. A partida termina em derrota quando as vidas ou o tempo acabam, ou se o personagem cair para fora da instalação. A tela de resultado permite jogar novamente.

### Controles na partida

| Ação | Teclado e mouse | Controle Xbox ou equivalente |
| --- | --- | --- |
| Mover | W, A, S, D | Analógico esquerdo |
| Olhar ao redor | Mover o mouse | Analógico direito |
| Pular | Espaço | A, botão sul |
| Interagir com o terminal | E | X, botão oeste |
| Liberar ou prender o cursor | Esc | Start |
| Prender o cursor novamente | Clique esquerdo na janela do jogo | Start |

## Abrir na Unity

Use o **Unity Hub** e instale a versão do Editor indicada em `ProjectSettings/ProjectVersion.txt` do projeto completo. O jogo usa **Unity 6**, **C#**, **URP**, **Input System**, **Cinemachine**, **TextMeshPro** e **ProBuilder**. As dependências do projeto são registradas em `Packages/manifest.json`.

1. Clone o repositório pelo botão **Code** do GitHub ou baixe o ZIP e extraia.
2. No Unity Hub, selecione **Add → Add project from disk** e escolha a pasta que contém `Assets`, `Packages` e `ProjectSettings`.
3. Aguarde a importação dos recursos. Para começar pelo menu, abra `Assets/_Jogo/Scenes/MenuPrincipal.unity` e clique em **Play → Jogar**. Para testar a fase diretamente, abra `Assets/_Jogo/Scenes/Instalacao.unity` e clique em **Play**.

## Gerar o executável para Windows

1. Abra **File → Build Profiles** e selecione o perfil **Windows**. Instale o módulo de suporte pelo Unity Hub caso o Editor solicite.
2. Na **Scene List**, habilite `MenuPrincipal` como primeira cena e `Instalacao` como segunda. Assim, o executável abre no menu e a fase pode ser carregada e reiniciada.
3. Use **Build And Run** e selecione uma pasta de saída, como `Builds/Windows`. Para distribuir a build, compacte a pasta inteira produzida pela Unity, incluindo o executável e os arquivos que o acompanham.

[Documentação da Unity sobre a lista de cenas da build](https://docs.unity.com/en-us/engine/6000.5/manual/building-and-publishing/build-settings/build-profile-scene-list)

## Organização do projeto

Os recursos do jogo ficam em `Assets/_Jogo`. A pasta `Scenes` reúne o menu e a instalação. `Prefabs` guarda os objetos reutilizáveis, `Input` contém as ações de controle e `Scripts` reúne o código C#.

| Script | Responsabilidade |
| --- | --- |
| `Jogador3D.cs` | Movimento relativo à câmera, pulo e interação. |
| `Coletavel3D.cs` | Animação visual dos núcleos e registro da coleta. |
| `DronePatrulha.cs` e `DanoContato.cs` | Patrulha por pontos no espaço 3D e dano por contato. |
| `TerminalSaida.cs` | Verificação de proximidade e interação com o terminal. |
| `GerenciadorJogo.cs` | Contadores, cronômetro, vitória, derrota e reinício. |

## Próximos passos

- Criar uma segunda fase com galpão central, depósito, oficina e passarela.
- Adicionar rotas alternativas e desafios de salto com diferentes alturas.
- Usar variações de altura na patrulha dos drones para criar janelas de passagem.

## Créditos

**Desenvolvimento e material didático** — João Victor Martins Deamo.

**Recursos de terceiros** — [Kenney Platformer Kit](https://kenney.nl/assets/platformer-kit) para modelos 3D. O tutorial também indica [Kenney UI Audio](https://kenney.nl/assets/ui-audio) para efeitos sonoros. Os pacotes da Kenney são distribuídos sob CC0.

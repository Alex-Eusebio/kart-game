using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;  // Necessário para VideoPlayer
using UnityEngine.SceneManagement;  // Necessário para carregar cenas
using System.Collections;

public class StageSelectController : MonoBehaviour
{
    [Header("Flag Images")]
    public Image[] flagImages;  // Imagens das bandeiras
    public Vector2 defaultSize = new Vector2(100f, 100f);  // Tamanho padrão
    public Vector2 enlargedSize = new Vector2(150f, 150f);  // Tamanho aumentado

    [Header("Videos")]
    public VideoPlayer[] videos;  // Array de VideoPlayers (5 vídeos)
    public RawImage videoDisplay;  // RawImage para exibir o vídeo

    private int currentFlagIndex = 0; // Índice da bandeira atual
    private Coroutine blinkCoroutine;  // Corrotina de piscar para a bandeira atual

    void Start()
    {
        // Configura todos os vídeos para ficarem em loop
        foreach (VideoPlayer video in videos)
        {
            video.isLooping = true; // Ativa o looping para cada vídeo
        }

        UpdateFlagSizes(); // Atualiza as bandeiras
        PlayVideo(currentFlagIndex); // Reproduz o vídeo correspondente à bandeira inicial

        // Inicializa a animação da bandeira atual, para garantir que o primeiro efeito de aumento aconteça
        StartCoroutine(AnimateFlagSize(flagImages[currentFlagIndex], enlargedSize));
        // Inicia o efeito de piscar para a primeira bandeira
        blinkCoroutine = StartCoroutine(BlinkFlag(flagImages[currentFlagIndex]));
    }

    void Update()
    {
        // Controle de navegação usando teclas A/D ou setas
        if ((Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) && currentFlagIndex > 0)
        {
            ChangeFlag(-1);  // Muda para a bandeira anterior
        }
        else if ((Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) && currentFlagIndex < flagImages.Length - 1)
        {
            ChangeFlag(1);  // Muda para a próxima bandeira
        }

        // Quando o jogador pressiona Enter ou Space, vai para a cena da pista correspondente
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
        {
            // Aqui você já está na bandeira certa e vai para a cena correspondente
            LoadTrackScene(currentFlagIndex);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(3);
        }
    }

    // Muda a bandeira com base na direção
    private void ChangeFlag(int direction)
    {
        // Para o vídeo atual antes de mudar de bandeira
        StopCurrentVideo();

        // Para a animação de piscar da bandeira anterior (se existir)
        if (blinkCoroutine != null)
        {
            StopCoroutine(blinkCoroutine);
        }

        // Inicia animação de diminuição do tamanho para a bandeira anterior
        StartCoroutine(AnimateFlagSize(flagImages[currentFlagIndex], defaultSize));

        currentFlagIndex += direction;  // Altera o índice

        // Atualiza os tamanhos das bandeiras e anima a nova bandeira
        UpdateFlagSizes();  
        PlayVideo(currentFlagIndex);  // Reproduz o vídeo correspondente

        // Inicia animação de aumento do tamanho para a nova bandeira
        StartCoroutine(AnimateFlagSize(flagImages[currentFlagIndex], enlargedSize));

        // Inicia o efeito de piscar para a nova bandeira
        blinkCoroutine = StartCoroutine(BlinkFlag(flagImages[currentFlagIndex]));
    }

    // Atualiza os tamanhos das imagens das bandeiras
    private void UpdateFlagSizes()
    {
        for (int i = 0; i < flagImages.Length; i++)
        {
            // Altera o tamanho da bandeira ativa e mantém o tamanho padrão para as outras
            if (i != currentFlagIndex)
            {
                flagImages[i].rectTransform.sizeDelta = defaultSize;
            }
        }
    }

    // Reproduz o vídeo correspondente à bandeira selecionada
    private void PlayVideo(int index)
    {
        // Verifica se o índice é válido antes de tentar reproduzir o vídeo
        if (index >= 0 && index < videos.Length)
        {
            // Para todos os vídeos (isso é importante para garantir que só um vídeo seja reproduzido de cada vez)
            foreach (VideoPlayer video in videos)
            {
                video.Stop();  // Para todos os vídeos
            }

            // Começa o vídeo correspondente ao índice da bandeira
            videos[index].Play();
            videoDisplay.texture = videos[index].texture;  // Atribui o vídeo ao RawImage para exibir na tela
        }
    }

    // Para o vídeo atual (caso haja) quando mudar de bandeira
    private void StopCurrentVideo()
    {
        if (currentFlagIndex >= 0 && currentFlagIndex < videos.Length)
        {
            videos[currentFlagIndex].Stop();
        }
    }

    // Faz a bandeira ativa piscar (aumentando e diminuindo)
    private IEnumerator BlinkFlag(Image flag)
    {
        while (true)
        {
            // Aumenta o tamanho
            yield return StartCoroutine(AnimateFlagSize(flag, enlargedSize));

            // Diminui o tamanho
            yield return StartCoroutine(AnimateFlagSize(flag, defaultSize));
        }
    }

    // Anima o aumento ou diminuição do tamanho da bandeira
    private IEnumerator AnimateFlagSize(Image flag, Vector2 targetSize)
    {
        Vector2 initialSize = flag.rectTransform.sizeDelta;
        float time = 0f;
        float duration = 0.5f; // Duração da animação

        while (time < duration)
        {
            flag.rectTransform.sizeDelta = Vector2.Lerp(initialSize, targetSize, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        flag.rectTransform.sizeDelta = targetSize; // Garante que o tamanho final é exatamente o target
    }

    // Carrega a cena da pista com base na bandeira selecionada
    private void LoadTrackScene(int flagIndex)
    {
        string sceneName = "Pista " + (flagIndex + 1);  // A cena é "Pista 1", "Pista 2", etc.
        Debug.Log("Carregando a cena: " + sceneName);
        SceneManager.LoadScene(sceneName);  // Carrega a cena correspondente à bandeira
    }
}












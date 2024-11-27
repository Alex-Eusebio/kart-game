using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using System.Collections; // Adicione esta linha

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
        // Recupera a personagem escolhida na cena anterior
        string selectedCharacter = PlayerPrefs.GetString("SelectedCharacter", "Ben");  // "Ben" é o valor padrão caso não tenha sido salvo nada
        Debug.Log("Personagem escolhida na cena anterior: " + selectedCharacter);

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

        // Controle de avanço para a próxima cena
        if (Input.GetKeyDown(KeyCode.Return))
        {
            string selectedCharacter = PlayerPrefs.GetString("SelectedCharacter", "Ben");
            Debug.Log("Personagem escolhida: " + selectedCharacter);
            UnityEngine.SceneManagement.SceneManager.LoadScene("NextScene");  // Troque "NextScene" pela cena de destino
        }
    }

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
        if (index >= 0 && index < videos.Length)
        {
            foreach (VideoPlayer video in videos)
            {
                video.Stop();
            }

            videos[index].Play();
            videoDisplay.texture = videos[index].texture;
        }
    }

    // Para o vídeo atual
    private void StopCurrentVideo()
    {
        if (currentFlagIndex >= 0 && currentFlagIndex < videos.Length)
        {
            videos[currentFlagIndex].Stop();
        }
    }

    private IEnumerator BlinkFlag(Image flag)
    {
        while (true)
        {
            yield return StartCoroutine(AnimateFlagSize(flag, enlargedSize));
            yield return StartCoroutine(AnimateFlagSize(flag, defaultSize));
        }
    }

    private IEnumerator AnimateFlagSize(Image flag, Vector2 targetSize)
    {
        Vector2 initialSize = flag.rectTransform.sizeDelta;
        float time = 0f;
        float duration = 0.5f;

        while (time < duration)
        {
            flag.rectTransform.sizeDelta = Vector2.Lerp(initialSize, targetSize, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        flag.rectTransform.sizeDelta = targetSize;
    }
}











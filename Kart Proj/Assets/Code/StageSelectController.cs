using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;  // Necessário para VideoPlayer
using UnityEngine.SceneManagement;  // Necessário para carregar cenas
using System.Collections;
using System;
using TMPro;
using UnityEngine.TextCore.Text;

public class StageSelectController : MonoBehaviour
{
    public Track[] tracks;

    [Header("Flag Images")]
    public Vector2 defaultSize = new Vector2(100f, 100f);  // Tamanho padrão
    public Vector2 enlargedSize = new Vector2(150f, 150f);  // Tamanho aumentado

    private int currentFlagIndex = 0; // Índice da bandeira atual
    private Coroutine blinkCoroutine;  // Corrotina de piscar para a bandeira atual

    [Header("Text Field")]
    public TextMeshProUGUI titleTxt;
    public TextMeshProUGUI descTxt;

    void Start()
    {
        // Configura todos os vídeos para ficarem em loop
        foreach (Track t in tracks)
        {
            t.player.isLooping = true; // Ativa o looping para cada vídeo
        }

        ChangeFlag(0);

        UpdateFlagSizes(); // Atualiza as bandeiras
        PlayVideo(currentFlagIndex); // Reproduz o vídeo correspondente à bandeira inicial
        titleTxt.text = tracks[currentFlagIndex].title;
        descTxt.text = tracks[currentFlagIndex].description;

        // Inicializa a animação da bandeira atual, para garantir que o primeiro efeito de aumento aconteça
        StartCoroutine(AnimateFlagSize(tracks[currentFlagIndex].flagImage, enlargedSize));
        // Inicia o efeito de piscar para a primeira bandeira
        blinkCoroutine = StartCoroutine(BlinkFlag(tracks[currentFlagIndex].flagImage));
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            ChangeFlag(-1);
        else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            ChangeFlag(1);

        // Quando o jogador pressiona Enter ou Space, vai para a cena da pista correspondente
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
            // Aqui você já está na bandeira certa e vai para a cena correspondente
            LoadTrackScene(currentFlagIndex);

        if (Input.GetKeyDown(KeyCode.Escape))
            SceneManager.LoadScene(3);
    }

    // Muda a bandeira com base na direção
    private void ChangeFlag(int direction)
    {
        if (direction != 0)
            AudioManager.Instance.PlaySfx("menuClick");

        // Para o vídeo atual antes de mudar de bandeira
        StopCurrentVideo();

        // Para a animação de piscar da bandeira anterior (se existir)
        if (blinkCoroutine != null)
        {
            StopCoroutine(blinkCoroutine);
        }

        // Inicia animação de diminuição do tamanho para a bandeira anterior
        StartCoroutine(AnimateFlagSize(tracks[currentFlagIndex].flagImage, defaultSize));

        currentFlagIndex += direction;  // Altera o índice

        if (currentFlagIndex < 0)
            currentFlagIndex = tracks.Length - 1;
        else if (currentFlagIndex > tracks.Length - 1)
            currentFlagIndex = 0;

        titleTxt.text = tracks[currentFlagIndex].title;
        descTxt.text = tracks[currentFlagIndex].description;

        for (int i = 0; i < tracks.Length; i++)
        {
            if (i != currentFlagIndex)
            {
                tracks[i].flagImageUnselect.gameObject.SetActive(true);
                tracks[i].flagImage.gameObject.SetActive(false);
            }
        }

        tracks[currentFlagIndex].flagImageUnselect.gameObject.SetActive(false);
        tracks[currentFlagIndex].flagImage.gameObject.SetActive(true);

        // Atualiza os tamanhos das bandeiras e anima a nova bandeira
        UpdateFlagSizes();  
        PlayVideo(currentFlagIndex);  // Reproduz o vídeo correspondente

        // Inicia animação de aumento do tamanho para a nova bandeira
        StartCoroutine(AnimateFlagSize(tracks[currentFlagIndex].flagImage, enlargedSize));

        // Inicia o efeito de piscar para a nova bandeira
        blinkCoroutine = StartCoroutine(BlinkFlag(tracks[currentFlagIndex].flagImage));
    }

    // Atualiza os tamanhos das imagens das bandeiras
    private void UpdateFlagSizes()
    {
        for (int i = 0; i < tracks.Length; i++)
        {
            // Altera o tamanho da bandeira ativa e mantém o tamanho padrão para as outras
            if (i != currentFlagIndex)
            {
                tracks[i].flagImage.rectTransform.sizeDelta = defaultSize;
            }
        }
    }

    // Reproduz o vídeo correspondente à bandeira selecionada
    private void PlayVideo(int index)
    {
        // Verifica se o índice é válido antes de tentar reproduzir o vídeo
        if (index >= 0 && index < tracks.Length)
        {
            // Para todos os vídeos (isso é importante para garantir que só um vídeo seja reproduzido de cada vez)
            foreach (Track t in tracks)
            {
                t.player.Stop();  // Para todos os vídeos
            }

            // Começa o vídeo correspondente ao índice da bandeira
            tracks[index].player.Play();
        }
    }

    // Para o vídeo atual (caso haja) quando mudar de bandeira
    private void StopCurrentVideo()
    {
        if (currentFlagIndex >= 0 && currentFlagIndex < tracks.Length)
        {
            tracks[currentFlagIndex].player.Stop();
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
        AudioManager.Instance.PlaySfx("menuSelect");
        string sceneName = "Pista " + (flagIndex + 1);  // A cena é "Pista 1", "Pista 2", etc.
        Debug.Log("Carregando a cena: " + sceneName);
        SceneManager.LoadScene(sceneName);  // Carrega a cena correspondente à bandeira
    }
}

[Serializable]
public struct Track
{
    public string title;
    public string description;
    public Image flagImage;
    public Image flagImageUnselect;
    public VideoPlayer player;
}
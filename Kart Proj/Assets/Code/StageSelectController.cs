using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

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

    void Start()
    {
        UpdateFlagSizes(); // Atualiza as bandeiras
        PlayVideo(currentFlagIndex); // Reproduz o vídeo correspondente à bandeira inicial
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
    }

    // Muda a bandeira com base na direção
    private void ChangeFlag(int direction)
    {
        // Para o vídeo atual antes de mudar de bandeira
        StopCurrentVideo();
        
        currentFlagIndex += direction;  // Altera o índice
        UpdateFlagSizes();  // Atualiza os tamanhos das bandeiras
        PlayVideo(currentFlagIndex);  // Reproduz o vídeo correspondente
    }

    // Atualiza os tamanhos das imagens das bandeiras
    private void UpdateFlagSizes()
    {
        for (int i = 0; i < flagImages.Length; i++)
        {
            // Altera o tamanho da bandeira ativa e mantém o tamanho padrão para as outras
            flagImages[i].rectTransform.sizeDelta = (i == currentFlagIndex) ? enlargedSize : defaultSize;
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
}





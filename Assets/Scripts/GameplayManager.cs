using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class GameplayManager : MonoBehaviour
{
    #region Singleton
    private static GameplayManager instance;

    public static GameplayManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameplayManager>();
            }
            return instance;
        }
    }
    #endregion
    // Boleano que indica si el jugador ya subio la palanca
    [SerializeField]
    private bool isLeverUp;
    public bool IsLeverUP
    {
        get
        {
            return isLeverUp;
        }
    }
    // Vidas del jugador, ya que simplemente es un solo player y todos los enemigos tienen una sola vida
    // no hay necesidad de contar mas que eso
    [SerializeField]
    private int playerLife;
    // Lista de imagenes de los corazones de la UI
    [SerializeField]
    private List<Image> lifeImages;
    // Imagen de la llave de la puerta
    [SerializeField]
    private Image keyImage;
    // Imagen de corazon gris
    [SerializeField]
    private Sprite heartOffSprite;
    // Imagen de llave iluminada
    [SerializeField]
    private Sprite keyOnSprite;
    // Pantalla animada de muerte
    [SerializeField]
    private GameObject dieScreen;
    [SerializeField]
    private GameObject dieMenu;
    [SerializeField]
    private GameObject pauseMenu;
    [SerializeField]
    private CinemachineVirtualCamera cm1;
    [SerializeField]
    private CinemachineVirtualCamera cm2;
    [SerializeField]
    private CinemachineVirtualCamera cm3;
    // Efecto de brillo en la casa
    [SerializeField]
    private GameObject glowFX;
    // Game Object del player
    [SerializeField]
    private GameObject playerGO;
    // Booleano auxiliar para mostrar no poder poner pausa
    // Durante la pantalla de win
    private bool cantPause;
    public bool CantPause
    {
        get { return cantPause; }
        set { cantPause = value; }
    }

    public bool HasLifes
    {
        get
        {
            if (playerLife >= 1)
            {
                return true;
            }
            return false;
        }
    }
    [SerializeField]
    private bool pauseActive;
    // Pausa duranta las cinematicas
    private bool cinematicPause;
    public bool CinematicPause => cinematicPause;

    private void Awake()
    {
        cantPause = false;
        pauseActive = false;
        playerGO = GameObject.FindGameObjectWithTag("Player");
        Time.timeScale = 1;
        // Si hay un checkpoint Guardado cuando se resetea el lvl
        if (PlayerPrefs.HasKey("CheckpointX") && PlayerPrefs.HasKey("CheckpointY") && PlayerPrefs.HasKey("Lifes"))
        {
            // Setea la palanca
            isLeverUp = true;
            SetKeyUI();
            // Activa el efecto de brillo de la casa
            glowFX.SetActive(true);
            // Setea la pos
            playerGO.transform.position = new Vector3(PlayerPrefs.GetFloat("CheckpointX"), PlayerPrefs.GetFloat("CheckpointY"), 0);
            // Y las vidas con su UI
            playerLife = PlayerPrefs.GetInt("Lifes");
            UpdateHeartUI();
        }
    }
    private void Update()
    {
        if (InputManager.Instance.EscapeKeyDown)
        {
            // Si el menu de muerte no esta activo y si puede pausar
            if(!cantPause)
            {
                PauseMenu();
            }
        }
    }

    // Descuenta una vida del jugador 
    public void DisccountLife(int dmgAmmount)
    {
        playerLife -= dmgAmmount;
        // No puede tener menos de 0 vidas
        if (playerLife < 0) playerLife = 0;
        UpdateHeartUI();
        if (!HasLifes)
        {
            PlayerDie();
        }
    }

    // Cambia la imagen de la vida por una gris
    private void UpdateHeartUI()
    {
        for (int i = 2; i > playerLife - 1; i--)
        {
            lifeImages[i].sprite = heartOffSprite;
        }
        //lifeImages[playerLife].sprite = heartOffSprite;
    }

    private void PlayerDie()
    {
        // Animacion de muerte
        dieScreen.SetActive(true);
        cantPause = true;
        // Se invoca el menu despues de la animacion
        Invoke("DieMenu", 1f);
    }

    private void DieMenu()
    {
        dieMenu.SetActive(true);
    }

    // Lo que pasa cuando comunican que la Palanca se levanto
    public void LeverUp(Vector2 pPos)
    {
        isLeverUp = true;
        SetKeyUI();
        // Se guarda todo en player prefs
        PlayerPrefs.SetFloat("CheckpointX", pPos.x);
        PlayerPrefs.SetFloat("CheckpointY", pPos.y);
        PlayerPrefs.SetInt("Lifes", playerLife);
        PlayerPrefs.Save();
        // Se establece un booleano para pausar solo al jugador
        cinematicPause = true;
        // Se invoca una serie de animaciones con las camaras virtuales 
        // de CineMachine
        StartCoroutine(CamAnimation());
    }

    private IEnumerator CamAnimation()
    {
        yield return new WaitForSeconds(0.3f);
        // Activa el efecto de brillo de la casa
        glowFX.SetActive(true);
        cm1.gameObject.SetActive(false);
        cm2.gameObject.SetActive(true);
        cm3.gameObject.SetActive(false);
        yield return new WaitForSeconds(3f);

        // Se iguala la posicion de la camara 3 y la camara 1
        cm3.transform.position = cm1.transform.position;
        cm1.gameObject.SetActive(false);
        cm2.gameObject.SetActive(false);
        cm3.gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);

        cm1.gameObject.SetActive(true);
        cm2.gameObject.SetActive(false);
        cm3.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.2f);
        ReturnControl();
    }

    // Se devuelve el control al jugador para que puede seguir
    private void ReturnControl()
    {
        cinematicPause = false;
    }

    // Cambia la imagen de la vida por una iluminada
    private void SetKeyUI()
    {
        keyImage.sprite = keyOnSprite;
    }

    // Si no esta pausado pausa
    public void PauseMenu()
    {
        pauseActive = !pauseActive;
        pauseMenu.SetActive(pauseActive);
        PauseGame(pauseActive);
    }
    // 0 para pausar; 1 para sacar la pausa
    private void PauseGame(bool pPausa)
    {
        int pY = 1;
        if (pPausa == true) pY = 0;
        Time.timeScale = pY;
    }

}

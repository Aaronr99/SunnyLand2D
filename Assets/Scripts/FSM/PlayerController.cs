using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerController : StateMachine
{
    // Velocidad de movimiento del player
    [HideInInspector]
    public float moveSpeed;
    public float moveAcceleration;
    public float maxMoveSpeed;
    // Cantidad de aumento de gravidad en la caida
    [SerializeField]
    private float fallGravity;
    [HideInInspector]
    public float jumpTimeCounter;
    // Cantidad de tiempo que se puede mantener apretado el boton de salto para saltar mas alto
    public float jumpTime;
    // Fuerza con la que salta
    public float jumpForce;
    [HideInInspector]
    public bool jumping;
    [HideInInspector]
    // Booleano que indica si se puede hacer daño al jugador
    private bool invulnerability;
    [SerializeField]
    private SpriteRenderer spritePlayer;
    [HideInInspector]
    private float inicialGravity;
    public Rigidbody2D rBody;
    public Animator anim;
    [HideInInspector]
    // Indica si todavia es posible realizar el salto
    public bool allowedAirDelay;
    // "Buffer" de salto que guarda las veces que se apreto el boton
    private int jumpBuffer;
    [HideInInspector]
    public bool canJump;
    [HideInInspector]
    // Indica si el jugador ha sido dañado
    public bool damaged;
    [HideInInspector]
    // Direccion de donde viene el golpe
    public float dmgDir;
    // Virtual camera para el efecto de Shake
    [SerializeField]
    private CinemachineVirtualCamera machineCamera;

    [Header("Debug")]
    [HideInInspector]
    public FSMDebugger fSMDebugger;
    [HideInInspector]
    public BoxCollider2D boxCollider;
    [SerializeField]
    private GameObject jumpFX;

    [Header("Sounds")]
    private AudioSource soundSource;
    [SerializeField]
    private List<AudioClip> runClips;
    [SerializeField]
    private AudioClip jumpClip;
    [SerializeField]
    private AudioClip fallClip;
    [SerializeField]
    private AudioClip hurtClip;
    [SerializeField]
    private AudioClip dieClip;
    [SerializeField]
    private AudioClip enemyHurtClip;
    private bool hurtSound;


    private void Start()
    {
        // Se asigna el Sound Source
        soundSource = gameObject.GetComponent<AudioSource>();
        // Asigna como estado base al estado Iddle pasandole
        // esta misma clase como parametro
        SetState(new IddleState(this));
        inicialGravity = rBody.gravityScale;
        // Opciones de Debug
        fSMDebugger = gameObject.GetComponent<FSMDebugger>();
        boxCollider = gameObject.GetComponent<BoxCollider2D>();
    }

    // En Update se gestionan los aspectos mas relacionados al input
    private void Update()
    {
        // Para prevenir que no se mueva durante la cinematica
        if (GameplayManager.Instance.CinematicPause)
            return;
        JumpAirDelay();
        JumpBuffer();
    }

    // Se utiliza Fixed Update para evitar problemas raros con el movimiento
    private void FixedUpdate()
    {
        // Para prevenir que no se mueva durante la cinematica
        if (GameplayManager.Instance.CinematicPause)
            return;
        actualState.DoState();
        JumpCorrection();
    }

    #region Jump
    // Se encarga de corregir las esquinas en los saltos
    private void JumpCorrection()
    {
        RaycastHit2D leftRaycast =
            Physics2D.Raycast(transform.position + new Vector3(-0.275f, 0.275f), Vector2.up, 0.3f, Utility.groundLayer);
        RaycastHit2D rightRaycast =
            Physics2D.Raycast(transform.position + new Vector3(0.275f, 0.275f), Vector2.up, 0.3f, Utility.groundLayer);

        // Si esta tocando el  raycast de la izquierda pero no derecha
        if (leftRaycast && !rightRaycast)
        {
            transform.position += new Vector3(0.15f, 0);
        }
        // Si esta tocando el  raycast de la derecha pero no izquierda
        else if (!leftRaycast && rightRaycast)
        {
            transform.position -= new Vector3(0.15f, 0);
        }
    }

    // Comprueba si esta tocando el suelo y que esta tocando
    public GameObject InGround()
    {
        // Comprueba que no este saltando en el momento
        if (!jumping)
        {
            // Castea un cuadrado a los pies del pj para saber si toco algo
            RaycastHit2D hit;
            // Primero comprueba si choco con un enemigo
            hit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0f, Vector2.down, 0.6f, Utility.enemyLayer);
            if (hit)
            {
                return hit.collider.gameObject;
            }
            // Luego el suelo que tiene un boxcast mas chico para evitar problemas con paredes y el terreno en general
            hit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size - new Vector3(0.08f, 0), 0f, Vector2.down, 0.3f, Utility.groundLayer);
            if (hit)
            {
                return hit.collider.gameObject; 
            }
            return null;
        }
        return null;
    }

    // Guarda los saltos que se realizaron antes de tiempo con el fin de
    // hacer parecer a los controles mas responsivos 
    private void JumpBuffer()
    {
        // Si se apreta uno de estos botones se guarda en la lista
        if (InputManager.Instance.JumpKeyDown)
        {
            // Añade la accion a la lista
            jumpBuffer += 1;
            // Remueve del buffer cada cierto tiempo para no estar saltando constantemente
            Invoke("undoJump", 0.65f);
        }

        // Si esta en el suelo o en el tiempo permitido para saltar
        if (allowedAirDelay)
        {
            // Permite el salto y descuenta uno de los saltos del buffer
            if (jumpBuffer >= 1)
            {
                canJump = true;
                jumpBuffer -= 1;
            }
        }

    }

    // Borra del buffer un salto
    private void undoJump()
    {
        if (jumpBuffer > 0)
        {
            jumpBuffer -= 1;
        }
    }

    private float airTime = 0;
    // Checkea si es posible saltar con un tiempo de delay en el aire
    private void JumpAirDelay()
    {
        if (InGround())
        {
            airTime = 0;
        }
        // Suma tiempo si esta en el aire
        else
        {
            airTime += Time.deltaTime;
        }

        // Si no estuvo suficiente tiempo en el aire todavia puede saltar
        if (airTime < 0.2f)
        {
            allowedAirDelay = true;
        }
        else
        {
            allowedAirDelay = false;
        }
    }

    // Acelera la caida para poder dar un poco mas de manejo al Player
    // en el aire
    public void AccelerateFall()
    {
        // Solo acelerar la caida si se apreto el boton para bajar y la gravedad todavia no se cambio
        if (InputManager.Instance.Down == -1 && rBody.gravityScale == inicialGravity)
        {
            rBody.gravityScale *= fallGravity;
        }

        // Se resetea la gravedad
        if (InGround())
        {
            rBody.gravityScale = inicialGravity;
        }
    }


    // Instancia unas particulas solo si se cae muy rapido
    public void InstantiateJumpFX()
    {
        // Solo lo hace si cae muy fuerte
        if (rBody.velocity.y < -19f)
        {
            GameObject tempGO = Instantiate(jumpFX, transform.position + Vector3.down * 0.25f, Quaternion.identity);
            Destroy(tempGO, 0.5f);
        }
    }

    #endregion

    #region Movement

    public void Move()
    {
        // Rotar el player segun la direccion en la que esta mirando
        if (InputManager.Instance.Horizontal == 1)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (InputManager.Instance.Horizontal == -1)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }

        // Asigna la aceleracion inicial
        if (moveSpeed < maxMoveSpeed)
        {
            moveSpeed += moveAcceleration * Time.deltaTime;
        }

        float horizontal = InputManager.Instance.Horizontal;

        // Asigna el movimiento y su velocidad
        transform.position += new Vector3(horizontal, 0, 0) * moveSpeed * Time.deltaTime;

        // Se resetea la speed para generar resistencia al giro
        if (InputManager.Instance.Horizontal == 0)
        {
            moveSpeed = 0;
        }
    }

    #endregion

    #region Damage
    // Metodo para tomar daño
    public void TakeDmg(float pDir, int dmgAmmount)
    {
        // Si paso suficiente tiempo desde el ultimo ataque
        if (!invulnerability)
        {
            damaged = true;
            GameplayManager.Instance.DisccountLife(dmgAmmount);
            // Se guarda la direccion del golpe
            dmgDir = pDir;
            TurnOnInvulnerability();
        }
    }
    private void TurnOnInvulnerability()
    {
        invulnerability = true;
        // Cambia el alpha del player para que sea mas claro
        // que es invulnerable
        Color tmp = spritePlayer.color;
        tmp.a = 0.5f;
        spritePlayer.color = tmp;
        // Espera un segundo para sacarle la invulnerabilidad
        Invoke("TurnOffInvulnerability", 1);
    }

    private void TurnOffInvulnerability()
    {
        // Vuelve el color a la normailidad
        invulnerability = false;
        Color tmp = spritePlayer.color;
        tmp.a = 1f;
        spritePlayer.color = tmp;
    }
    #endregion

    #region Audio

    private int cont = 0;
    // Reproduce todos los clips de audio para el sonido de corer
    public void RunAudio()
    {
        soundSource.clip = runClips[cont];
        soundSource.volume = 0.6f;
        soundSource.pitch = 2;
        soundSource.Play();
        cont++;
        if (cont == runClips.Count)
            cont = 0;
    }
    // Se retocan los pitchs y volumes para intentar que todos los sonidos
    // esten en el mismo tono
    public void JumpSound()
    {
        soundSource.pitch = 0.8f;
        soundSource.volume = 0.25f;
        soundSource.clip = jumpClip;
        soundSource.Play();
    }
    public void FallSound()
    {
        soundSource.pitch = 1;
        soundSource.volume = 1f;
        soundSource.clip = fallClip;
        soundSource.Play();
    }
    public void HurtSound()
    {
        soundSource.pitch = 1f;
        soundSource.volume = 0.5f;
        soundSource.clip = hurtClip;
        soundSource.Play();
    }
    public void DieSound()
    {
        soundSource.pitch = 1;
        soundSource.volume = 0.8f;
        soundSource.clip = dieClip;
        soundSource.Play();
    }
    public void EnemyHurtSound()
    {
        soundSource.pitch = 0.7f;
        soundSource.volume = 0.6f;
        soundSource.clip = enemyHurtClip;
        hurtSound = true;
        soundSource.Play();
    }
    // Comprueba si se esta reproduciendo el sonido de daño en ese momento
    // para evitar que choque con el audio de salto
    public bool HurtSoundPlaying()
    {
        if (hurtSound)
        {
            hurtSound = false;
            return true;
        }
        return false;
    }
    #endregion

    #region CameraShake
    // Metodo para iniciar el Camera Shake
    // Ocupa el ruido integrado en Cinemachine
    public void CameraShake()
    {
        CinemachineBasicMultiChannelPerlin cinemachineBasic = machineCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        if (cinemachineBasic.m_AmplitudeGain == 0)
        {
            cinemachineBasic.m_AmplitudeGain = 3;
            Invoke("UnnableShake", 0.1f);
        }
    }
    private void UnnableShake()
    {
        CinemachineBasicMultiChannelPerlin cinemachineBasic = machineCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        cinemachineBasic.m_AmplitudeGain = 0;
    }
    #endregion
}

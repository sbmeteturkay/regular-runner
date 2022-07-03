using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPathMovement : MonoBehaviour
{
    private Transform player;
    private Vector3 startMousePos, startPlayerPosition;
    private bool movePlayer;
    [Range(0f, 1f)] public float maxSpeed;
    [Range(0f, 50f)] public float pathSpeed;
    public float velocity;
    Camera cam;
    public Transform path;
    string tags;
    public GameManager gameManager;
    public static Transform defaultLevelPosition,camDefaultPosition;
    public AudioClip[] audioClips;
    public AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        player = transform;
        defaultLevelPosition = path;
        cam = Camera.main;
        Transform transformCam = Camera.main.transform;
        camDefaultPosition = transformCam;
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0)&&MenuManager.MenuManagerInstance.GameState)
        {
            movePlayer = true;
            Plane newPlane = new Plane(Vector3.up, 0f);
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if(newPlane.Raycast(ray,out var distance))
            {
                startMousePos = ray.GetPoint(distance);
                startPlayerPosition = player.position;
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            movePlayer = false;
        }
        if (movePlayer && MenuManager.MenuManagerInstance.GameState)
        {
            movePlayer = true;
            Plane newPlane = new Plane(Vector3.up, 0f);
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if (newPlane.Raycast(ray, out var distance))
            {
                Vector3 mousePos = ray.GetPoint(distance);
                Vector3 mouseNewPos = mousePos - startMousePos;
                Vector3 desirePlayerPosition = mouseNewPos + startPlayerPosition;
                desirePlayerPosition.x = Mathf.Clamp(desirePlayerPosition.x, -6.25f, 6.25f);
                player.position = new Vector3(Mathf.SmoothDamp(player.position.x, desirePlayerPosition.x, ref velocity,maxSpeed), player.position.y, player.position.z);
            }
        }
        if (MenuManager.MenuManagerInstance.GameState)
        {
            var pathNewPosition = path.position;
            path.position = new Vector3(pathNewPosition.x, pathNewPosition.y, Mathf.MoveTowards(pathNewPosition.z, -50, pathSpeed * Time.deltaTime));
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        tags = other.tag;
        switch (tags)
        {
            case "Finish":
                if (MenuManager.MenuManagerInstance.GameState)
                {
                    gameManager.playerAnimator.SetTrigger("finish");
                    MenuManager.MenuManagerInstance.GameState = false;
                    gameManager.animationWeight = 0;
                    gameManager.currentCollectedCurrency = 0;
                    MenuManager.MenuManagerInstance.FinishGame();
                    gameManager.playerAnimator.SetBool("running", false);
                    audioSource.clip = audioClips[0];
                    audioSource.Play();
                }
                break;

            case "Gold":
                gameManager.IncreaseCurrencyAmount(1);
                other.gameObject.SetActive(false);
                audioSource.clip = audioClips[1];
                audioSource.Play();
                break;

            case "Obstacle":
                gameManager.DecreaseStack(1);
                other.gameObject.SetActive(false);
                audioSource.clip = audioClips[2];
                audioSource.Play();
                break;

            case "Stack":
                gameManager.IncreaseStack(1);
                other.gameObject.SetActive(false);
                audioSource.clip = audioClips[3];
                audioSource.Play();
                break;

            default:
                break;

        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class HookGrabable : MonoBehaviour {
    //When adding a hook to the level the forward direction of the object must be set appropriatly(facing towards where the player
    //will dismount when going up)

    public System.Action OnMastReached;
    public System.Action OnShipReached;
    public System.Action OnGrab;
    public System.Action OnRelease;

    [Header("Translation")]
    [SerializeField] [Tooltip("Target for moving up must be placed first and then the target for moving down")] Transform[] targetPosition;
    [SerializeField] [Tooltip("Target for moving up must be placed first and then the target for moving down")] Transform[] teleportPosition;
    [SerializeField] bool isMovingUp;
    [SerializeField] [Tooltip("Speed in meters / second")] float speed;
    [SerializeField] float coroutineUpdateTime;

    private XRSimpleInteractable interactable;
    private CharacterController playerController;
    private Player player;

    private ContinuousMoveProviderBase moveProvider;
    private ContinuousTurnProviderBase turnProvider;

    Vector3[] forwardDirection = new Vector3[2];
    Transform playerTransform = null;
    bool isCoroutineRunning = false;
    bool wasReleased = false;

    int currentTargetIndex { get { return (isMovingUp) ? 0 : 1; } }

    #region Unity Events
    private void Awake() {
        interactable = GetComponent<XRSimpleInteractable>();

        interactable.selectEntered.AddListener(Grab);
        interactable.selectExited.AddListener(Release);

        interactable.hoverExited.AddListener(OnHoverExit);

        forwardDirection[0] = transform.forward;
        forwardDirection[1] = -transform.forward;
    }

    private void Start() {
        playerController = SceneManager.instance.playerGameObject.GetComponent<CharacterController>();
        player = playerController.GetComponent<Player>();

        moveProvider = player.GetComponent<ContinuousMoveProviderBase>();
        turnProvider = player.GetComponent<ContinuousTurnProviderBase>();

        playerTransform = SceneManager.instance.playerGameObject.transform;
    }
    #endregion

    #region Grab
    void Grab(SelectEnterEventArgs pArgs) {
        if (isCoroutineRunning)
            return;

        wasReleased = false;

        moveProvider.useGravity = false;
        moveProvider.enabled = false;
        turnProvider.enabled = false;

        OnGrab?.Invoke();

        StartCoroutine(TranslateCoroutine());
    }

    void Release(SelectExitEventArgs pArgs) {
        if (wasReleased)
            return;

        playerTransform = null;
        wasReleased = true;

        moveProvider.useGravity = true;
        moveProvider.enabled = true;
        turnProvider.enabled = true;

        OnRelease?.Invoke();

        if (!isCoroutineRunning)
            return;

        if (isMovingUp)
            isMovingUp = !isMovingUp;
    }

    void OnHoverExit(HoverExitEventArgs pArgs) {
        //OnRelease(null);
    }

    IEnumerator TranslateCoroutine(float startDelay = 0) {
        float epsilon = 0.025f;

        if (startDelay != 0)
            yield return new WaitForSeconds(0);

        isCoroutineRunning = true;

        float distanceToTarget = (targetPosition[currentTargetIndex].position - transform.position).magnitude;

        while (distanceToTarget >= epsilon) {

            Vector3 directionToTarget = (targetPosition[currentTargetIndex].position - transform.position).normalized;
            distanceToTarget = (targetPosition[currentTargetIndex].position - transform.position).magnitude;
            float distanceToTravel = speed * Time.deltaTime;
            distanceToTravel = Mathf.Clamp(distanceToTravel, 0, distanceToTarget);

            transform.Translate(distanceToTravel * directionToTarget, Space.World);

            if (playerTransform != null)
                playerTransform.Translate(distanceToTravel * directionToTarget, Space.World);

            yield return new WaitForSeconds(coroutineUpdateTime);
        }

        if (playerTransform != null) {
            playerTransform.transform.position = teleportPosition[currentTargetIndex].position;
        }

        isCoroutineRunning = false;

        if (isMovingUp)
            OnMastReached?.Invoke();
        else
            OnShipReached?.Invoke();

        isMovingUp = !isMovingUp;

        if (playerTransform != null && isMovingUp)
            Release(null);
    }
    #endregion
}
using System.Collections;
using UnityEngine;


public class MagnetorItem : MonoBehaviour
{
    FillBar coinMagnetFill;

    Transform ball;

    bool isCheck;

    float distance = 0;

    [SerializeField]
    float maxTriggerDistance = 5f;

    [SerializeField]
    float speed;

    [SerializeField]
    AnimationCurve animationCurve;

    private void Start()
    {
        var playerCollider = PlayerController.Instance.PlayerMovement.GetComponent<PlayerCollider>();


        if (playerCollider == null)
            return;

        coinMagnetFill = FillController.Instance.CoinMagnetFill;

        ball = playerCollider.gameObject.transform;


        coinMagnetFill.OnDecreaseStarted += CoinMagnetFill_OnDecreaseStarted;

        coinMagnetFill.OnDecreaseFinished += CoinMagnetFill_OnDecreaseFinished;
    }



    private void Update()
    {
        // TODO: you might decide to limit the amount of calls here if coins is farther away from player.

        // ...
        if (!isCheck)
            return;

        if (ball.position.z > transform.position.z + maxTriggerDistance)
            return;

        distance = (transform.position - ball.position).z;

        if (distance <= maxTriggerDistance)
            StartMagnet();
    }


    public void StartMagnet()
    {
        transform.position = Vector3.Lerp(transform.position, ball.position, animationCurve.Evaluate(speed * Time.deltaTime));
    }


    private void CoinMagnetFill_OnDecreaseStarted()
    {
        isCheck = true;
    }

    private void CoinMagnetFill_OnDecreaseFinished()
    {
        isCheck = false;
    }

    private void OnDisable()
    {
        if (coinMagnetFill == null)
            return;

        coinMagnetFill.OnDecreaseStarted += CoinMagnetFill_OnDecreaseStarted;

        coinMagnetFill.OnDecreaseFinished += CoinMagnetFill_OnDecreaseFinished;
    }
}


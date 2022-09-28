using UnityEngine;


internal class MagnetorItem : MonoBehaviour
{
    private FillBar _coinMagnetFill;

    private Transform _ball;

    private bool _isCheck;

    private float _distance;

    [SerializeField] private float maxTriggerDistance = 5f;

    [SerializeField] private float speed;

    [SerializeField] private AnimationCurve animationCurve;

    private void Start()
    {
        var playerCollider = PlayerController.Instance.PlayerCollider;

        if (playerCollider == null)
            return;

        _coinMagnetFill = FillController.Instance.CoinMagnetFill;

        _ball = playerCollider.gameObject.transform;


        _coinMagnetFill.OnDecreaseStarted += CoinMagnetFill_OnDecreaseStarted;

        _coinMagnetFill.OnDecreaseFinished += CoinMagnetFill_OnDecreaseFinished;
    }



    private void Update()
    {
        // TODO: you might decide to limit the amount of calls here if coins is farther away from player.

        // ...
        if (!_isCheck)
            return;

        if (_ball.position.z > transform.position.z + maxTriggerDistance)
            return;

        _distance = (transform.position - _ball.position).z;

        if (_distance <= maxTriggerDistance)
            StartMagnet();
    }


    public void StartMagnet()
    {
        transform.position = Vector3.Lerp(transform.position, _ball.position, animationCurve.Evaluate(speed * Time.deltaTime));
    }


    private void CoinMagnetFill_OnDecreaseStarted()
    {
        _isCheck = true;
    }

    private void CoinMagnetFill_OnDecreaseFinished()
    {
        _isCheck = false;
    }

    private void OnDisable()
    {
        if (_coinMagnetFill == null)
            return;

        _coinMagnetFill.OnDecreaseStarted -= CoinMagnetFill_OnDecreaseStarted;

        _coinMagnetFill.OnDecreaseFinished -= CoinMagnetFill_OnDecreaseFinished;
    }
}


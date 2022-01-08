using UnityEngine;

public class BallDestroyer : MonoBehaviour
{
    [SerializeField] BallsHolder _ballsHolder;

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.TryGetComponent(out Ball ball))
        {
            _ballsHolder.RemoveBall(ball);
            Destroy(ball.gameObject);
        }
        else if (collider.gameObject.TryGetComponent(out CueBall cueBall))
        {
            cueBall.transform.position = new Vector3(5, 3, 2);
        }
    }
}

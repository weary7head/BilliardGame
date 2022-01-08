using System;
using System.Collections.Generic;
using UnityEngine;

public class BallsHolder : MonoBehaviour
{
    public event Action BallsEnded;

    [SerializeField] private List<Ball> _balls;

    public void RemoveBall(Ball ball)
    {
        _balls.Remove(ball);
        if (_balls.Count == 0)
        {
            BallsEnded.Invoke();
        }
    }
}

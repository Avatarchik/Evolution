using UnityEngine;
using System.Collections;

public class BackgroundPentagon : MovingAI {
    
    [System.Serializable]
    public class Twitching
    {
        public bool enabled = false;
        public float impact = 1f;
        public float speed = 1f;

        public float Value
        {
            get
            {
                return impact * Time.deltaTime * (int)Random.Range((int)-1, (int)2);
            }
        }
    }

    public Twitching twitchingNoise;

    private int _framesCount = 0;

    public override void Update()
    {
        base.Update();
        if (twitchingNoise.enabled && _framesCount > 0)
            transform.localPosition = Vector3.Lerp(
                transform.localPosition, 
                new Vector3(
                    transform.localPosition.x + twitchingNoise.Value,
                    transform.localPosition.y + twitchingNoise.Value,
                    0
                ), 
                twitchingNoise.speed * Time.deltaTime);
        _framesCount++;
    }
}

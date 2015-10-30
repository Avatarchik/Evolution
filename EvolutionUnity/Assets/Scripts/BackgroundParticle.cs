using UnityEngine;
using System.Collections;

public class BackgroundParticle : MovingAI {

    public AnimatedFloat grow;
    public TwitchingFloat twitchingNoise;

    private Vector3 startedScale;

    public override void Start()
    {
        base.Start();
        startedScale = transform.localScale;
        grow.OnUpperCap += (float value) => grow.enabled = false;
    }

    public override void Update()
    {
        base.Update();

        if (twitchingNoise.enabled)
            transform.localPosition = Vector3.Lerp(
                transform.localPosition,
                new Vector3(
                    transform.localPosition.x + twitchingNoise.Value,
                    transform.localPosition.y + twitchingNoise.Value,
                    0
                ),
                twitchingNoise.speed * Time.deltaTime);

        if (grow.enabled)
        {
            float value = grow.Value;

            transform.localScale = new Vector3(
                startedScale.x * value,
                startedScale.y * value,
                transform.localScale.z
                );
        }
    }
}

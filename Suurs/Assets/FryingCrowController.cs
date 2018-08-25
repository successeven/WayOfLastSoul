using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FryingCrowController : MonoBehaviour
{

    enum CrowsSounds
    {
        None,
        Move
    }

    bool _FlyOn = false;
    float flyTime;
    public GameObject EffectObject;
    public GameObject SpriteObject;
    AudioManager audioManager;

    private void Start()
    {
        audioManager = transform.root.GetComponent<AudioManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && !_FlyOn)
        {
            if (!audioManager.IsPlaying(CrowsSounds.Move.ToString()))
                audioManager.Play(CrowsSounds.Move.ToString());

            SpriteObject.SetActive(false);
            EffectObject.SetActive(true);
            _FlyOn = true;
            flyTime = Time.fixedTime;
        }
    }

    private void Update()
    {
        if (_FlyOn && Time.fixedTime - flyTime > 5f)
        {
            Destroy(transform.gameObject);
        }
    }
}

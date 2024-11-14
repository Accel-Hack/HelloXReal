using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationSelecterDemo : AnimationSelecter
{
    [SerializeField] StickmanCreater stickmanCreater;
    private List<string> animations = null;
    private int index = 0;

    void Awake() {
        StartCoroutine(stickmanLoader.LoadAnimationList());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow)) {
            this.index++;
            if (this.index >= this.animations.Count) {
                this.index = 0;
            }
            string url = "http://192.168.50.110:8000/download_animation/" + this.animations[this.index];
            StartCoroutine(stickmanLoader.LoadAnimation(url));
        }
        if (Input.GetKeyDown(KeyCode.Space)) {
            stickmanCreater.Replay();
        }
    }

    public override void SetAnimations(List<string> animations)
    {
        Debug.Log("!");
        this.animations = animations;
        if (this.index >= animations.Count) {
            this.index = Mathf.Max(0, animations.Count - 1);
        }
    }
}

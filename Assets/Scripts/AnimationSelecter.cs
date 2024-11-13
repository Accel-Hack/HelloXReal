using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimationSelecter : MonoBehaviour
{
    private readonly Vector3 positionOfFirstButton = new Vector3(0, -10, 0);
    private readonly Vector3 delta = new Vector3(0, -15, 0);

    // List of Button. Each button corresponds to a animation file (output of mediapipe).
    private List<GameObject> buttons;
    [SerializeField] StickmanLoader stickmanLoader;
    [SerializeField] GameObject animationButtonPrefab;

    private void Awake()
    {
        StartCoroutine(stickmanLoader.LoadAnimationList());
        this.buttons = new List<GameObject>();
    }

    public void SetAnimations(List<string> animations)
    {
        // If there are already buttons, clean up them.
        foreach (GameObject button in this.buttons) {
            Destroy(button);
        }
        this.buttons.Clear();

        // Create buttons for each animations on the server.
        for (int i = 0; i < animations.Count; i++) {
            GameObject button = Instantiate(
                animationButtonPrefab,
                Vector3.zero,
                transform.rotation,
                transform
            );
            this.buttons.Add(button);
            button.GetComponent<AnimationButton>().Initialize(animations[i], stickmanLoader);

            // Place button at a relative position from AnimationSelecter.
            // You can't set relative position with the 2nd argment of Instantiate()...
            button.transform.localPosition = this.positionOfFirstButton + this.delta * i;
        }
    }
}

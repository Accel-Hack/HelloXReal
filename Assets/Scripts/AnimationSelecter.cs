using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimationSelecter : MonoBehaviour
{
    private readonly Vector3 positionOfFirstButton = new Vector3(0, 15, 0);
    private readonly Vector3 delta = new Vector3(0, -15, 0);

    // List of Button. Each button corresponds to a animation file (output of mediapipe).
    private List<Button> buttons;
    [SerializeField] StickmanLoader stickmanLoader;
    [SerializeField] GameObject animationButtonPrefab;

    private void Awake()
    {
        StartCoroutine(stickmanLoader.LoadAnimationList());
        this.buttons = new List<Button>();
    }

    public void SetAnimations(List<string> animations)
    {
        // If there are already buttons, clean up them.
        foreach (Button button in this.buttons) {
            Destroy(button.gameObject);
        }
        this.buttons.Clear();

        // Create buttons for each animations on the server.
        for (int i = 0; i < animations.Count; i++) {
            GameObject button = Instantiate(
                animationButtonPrefab,
                Vector3.zero,
                Quaternion.identity,
                transform
            );
            button.GetComponent<AnimationButton>().Initialize(animations[i], stickmanLoader);

            // Place button at a relative position from AnimationSelecter.
            // You can't set relative position with the 2nd argment of Instantiate()...
            button.transform.localPosition = this.positionOfFirstButton + this.delta * i;
        }
    }
}

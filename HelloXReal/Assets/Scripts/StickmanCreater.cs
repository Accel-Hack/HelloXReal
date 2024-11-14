using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickmanCreater : MonoBehaviour
{
    // Size of the stickman.
    public const float MAGNIFICATION = 0.6f;

    // When MAGNIFICATION is 10, the scale of sphere and cube should be 0.2f.
    public const float PREFAB_SCALE_FOR_MAGNIFICATION = 0.02f;

    // The speed to play posing animation.
    private const int TIMESCALE = 2;

    // The location of stickman.
    public static readonly Vector3 stickmanPosition = new Vector3(0, 0, 3);
    
    // output from mediapipe.
    private string sequence = null;

    // Origin of mediapipe-output space. Calculated on the first frame.
    private Vector3 origin;

    // Stop update until loading model completed.
    private bool initialized = false;

    private int animationFrame = 0;
    // Animation is playing or not.
    private bool isPlaying = false;
    
    // Parent object of all joints and bones.
    [SerializeField] GameObject stickman;

    StickmanNode[] nodes;
    List<List<Vector3>> frames;

    [SerializeField] GameObject sphere;
    [SerializeField] GameObject cube;

    private void Start()
    {
        this.nodes = ConstructSkeleton();
        foreach (StickmanNode node in this.nodes) {
            node.Instantiate(this.sphere, this.cube, this.stickman.transform);
        }
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (!this.initialized || !this.isPlaying) {
            return;
        }
        foreach (StickmanNode node in this.nodes) {
            // node.Pose(frames[animationFrame / TIMESCALE]);
            node.Pose(this.LeapFrames((float) this.animationFrame / TIMESCALE));
        }
        if (this.animationFrame < (this.frames.Count - 2) * TIMESCALE) {
            this.animationFrame++;
        } else {
            this.animationFrame = 0;    // TODO: Remove not to loop.
        }
    }

    public void InitializeWithSequence(string sequence)
    {
        frames = MediaPipeReceiver.ReadSequence(sequence);
        for (int i = 0; i < frames.Count; i++) {
            for (int j = 0; j < frames[i].Count; j++) {
                frames[i][j] *= MAGNIFICATION;
            }
        }

        this.initialized = true;
        this.sequence = sequence;
        this.isPlaying = false;
        this.animationFrame = 0;
    }

    public void Replay()
    {
        this.isPlaying = true;
        this.animationFrame = 0;
    }

    // Make move of joints and bones smooth.
    private List<Vector3> LeapFrames(float floatIndex)
    {
        List<Vector3> leapedFrame = new List<Vector3>();
        float rate = floatIndex % 1;
        for (int i = 0; i < this.frames[0].Count; i++) {
            Vector3 joint = Vector3.Lerp(this.frames[(int)Mathf.Floor(floatIndex)][i], this.frames[(int)Mathf.Ceil(floatIndex)][i], rate);
            leapedFrame.Add(joint);
        }
        return leapedFrame;
    }

    // Construct network relation of each joints.
    private StickmanNode[] ConstructSkeleton()
    {
        StickmanNode nose = new StickmanNode("nose", 0, new StickmanNode[]{});
        StickmanNode left_eye_inner = new StickmanNode("left eye (inner)", 1, new StickmanNode[]{nose});
        StickmanNode left_eye = new StickmanNode("left eye", 2, new StickmanNode[]{left_eye_inner});
        StickmanNode left_eye_outer = new StickmanNode("left eye (outer)", 3, new StickmanNode[]{left_eye});
        StickmanNode right_eye_inner = new StickmanNode("right eye (inner)", 4, new StickmanNode[]{nose});
        StickmanNode right_eye = new StickmanNode("right eye", 5, new StickmanNode[]{right_eye_inner});
        StickmanNode right_eye_outer = new StickmanNode("right eye (outer)", 6, new StickmanNode[]{right_eye});
        StickmanNode left_ear = new StickmanNode("left ear", 7, new StickmanNode[]{left_eye_outer});
        StickmanNode right_ear = new StickmanNode("right ear", 8, new StickmanNode[]{right_eye_outer});
        StickmanNode mouth_left = new StickmanNode("mouth (left)", 9, new StickmanNode[]{});
        StickmanNode mouth_right = new StickmanNode("mouth (right)", 10, new StickmanNode[]{mouth_left});
        StickmanNode left_shoulder = new StickmanNode("left shoulder", 11, new StickmanNode[]{});
        StickmanNode right_shoulder = new StickmanNode("right shoulder", 12, new StickmanNode[]{left_shoulder});
        StickmanNode left_elbow = new StickmanNode("left elbow", 13, new StickmanNode[]{left_shoulder});
        StickmanNode right_elbow = new StickmanNode("right elbow", 14, new StickmanNode[]{right_shoulder});
        StickmanNode left_wrist = new StickmanNode("left wrist", 15, new StickmanNode[]{left_elbow});
        StickmanNode right_wrist = new StickmanNode("right wrist", 16, new StickmanNode[]{right_elbow});
        StickmanNode left_pinky = new StickmanNode("left pinky", 17, new StickmanNode[]{left_wrist});
        StickmanNode right_pinky = new StickmanNode("right pinky", 18, new StickmanNode[]{right_wrist});
        StickmanNode left_index = new StickmanNode("left index", 19, new StickmanNode[]{left_wrist, left_pinky});
        StickmanNode right_index = new StickmanNode("right index", 20, new StickmanNode[]{right_wrist, right_pinky});
        StickmanNode left_thumb = new StickmanNode("left thumb", 21, new StickmanNode[]{left_wrist});
        StickmanNode right_thumb = new StickmanNode("right thumb", 22, new StickmanNode[]{right_wrist});
        StickmanNode left_hip = new StickmanNode("left hip", 23, new StickmanNode[]{left_shoulder});
        StickmanNode right_hip = new StickmanNode("right hip", 24, new StickmanNode[]{right_shoulder, left_hip});
        StickmanNode left_knee = new StickmanNode("left knee", 25, new StickmanNode[]{left_hip});
        StickmanNode right_knee = new StickmanNode("right knee", 26, new StickmanNode[]{right_hip});
        StickmanNode left_ankle = new StickmanNode("left ankle", 27, new StickmanNode[]{left_knee});
        StickmanNode right_ankle = new StickmanNode("right ankle", 28, new StickmanNode[]{right_knee});
        StickmanNode left_heel = new StickmanNode("left heel", 29, new StickmanNode[]{left_ankle});
        StickmanNode right_heel = new StickmanNode("right heel", 30, new StickmanNode[]{right_ankle});
        StickmanNode left_foot_index = new StickmanNode("left foot index", 31, new StickmanNode[]{left_ankle, left_heel});
        StickmanNode right_foot_index = new StickmanNode("right foot index", 32, new StickmanNode[]{right_ankle, right_heel});


        return new StickmanNode[] {
            nose,
            left_eye_inner,
            left_eye,
            left_eye_outer, 
            right_eye_inner,
            right_eye,
            right_eye_outer,
            left_ear,
            right_ear,
            mouth_left,
            mouth_right,
            left_shoulder,
            right_shoulder,
            left_elbow,
            right_elbow,
            left_wrist,
            right_wrist,
            left_pinky,
            right_pinky,
            left_index,
            right_index,
            left_thumb,
            right_thumb,
            left_hip,
            right_hip,
            left_knee,
            right_knee,
            left_ankle,
            right_ankle,
            left_heel,
            right_heel,
            left_foot_index,
            right_foot_index
        };
    }
}

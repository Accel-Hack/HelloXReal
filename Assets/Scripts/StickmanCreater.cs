using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Globalization;
using System;

public class StickmanCreater : MonoBehaviour
{
    private const float MAGNIFICATION = 10;

    // Contain output of MediaPipe as a string
    private string positions = "[(-0.10755845904350281, -0.566332995891571, -0.17337942123413086), (-0.10199901461601257, -0.606780469417572, -0.17168641090393066), (-0.10171599686145782, -0.6070476174354553, -0.1708533763885498), (-0.10196453332901001, -0.607175886631012, -0.17069387435913086), (-0.12774784862995148, -0.6001722812652588, -0.15709590911865234), (-0.12790951132774353, -0.6008838415145874, -0.15790700912475586), (-0.1282593011856079, -0.602125883102417, -0.1573042869567871), (-0.013760147616267204, -0.6200494766235352, -0.10658049583435059), (-0.1388438642024994, -0.602886974811554, -0.050353050231933594), (-0.06614112854003906, -0.5525497198104858, -0.14666032791137695), (-0.1023990586400032, -0.5450075268745422, -0.12749600410461426), (0.1468842476606369, -0.47658395767211914, -0.029406724497675896), (-0.19353780150413513, -0.4618590772151947, -0.0064144134521484375), (0.24338525533676147, -0.2852853536605835, -0.08939969539642334), (-0.3664446473121643, -0.2701388895511627, -0.005773961544036865), (0.23226884007453918, -0.18100324273109436, -0.2542600631713867), (-0.49040162563323975, -0.13705164194107056, -0.11932206153869629), (0.23264752328395844, -0.14604002237319946, -0.2890293598175049), (-0.5259864330291748, -0.10318729281425476, -0.13482451438903809), (0.200709730386734, -0.1630423218011856, -0.2988412380218506), (-0.5157558917999268, -0.11307214200496674, -0.16433978080749512), (0.21744896471500397, -0.17938275635242462, -0.2606377601623535), (-0.4874730706214905, -0.1289059817790985, -0.13547611236572266), (0.11736413836479187, 0.0058386740274727345, -0.0022706985473632812), (-0.1177339181303978, -0.006278082262724638, 0.0031251907348632812), (0.3591022193431854, 0.13185732066631317, -0.2219095230102539), (-0.4189647138118744, 0.09201347827911377, -0.11996400356292725), (0.4239124059677124, 0.4016002416610718, -0.0023698806762695312), (-0.40427669882774353, 0.40096476674079895, 0.059227943420410156), (0.4410778880119324, 0.4397982358932495, 0.016841888427734375), (-0.40634793043136597, 0.445121169090271, 0.0764617919921875), (0.5483031272888184, 0.4968748390674591, -0.06334924697875977), (-0.5222113728523254, 0.5222684144973755, 0.017089366912841797)]";
    
    // Origin of mediapipe-output space. Calculated on the first frame.
    private Vector3 origin;
    
    // Parent object of all joints and bones.
    [SerializeField] GameObject stickman;

    [SerializeField] GameObject sphere;
    [SerializeField] GameObject cube;

    // Start is called before the first frame update
    void Start()
    {
        List<Vector3> joints = ReadJoints(positions);
        for (int i = 0; i < joints.Count; i++) {
            joints[i] *= MAGNIFICATION;
            joints[i] = Vector3.Scale(joints[i], new Vector3(1, -1, 1));
        }
        origin = (joints[23] + joints[24]) / 2;
        for (int i = 0; i < joints.Count; i++) {
            joints[i] -= origin;
        }
        StickmanNode[] nodes = ConstructSkeleton(joints);
        foreach (StickmanNode node in nodes) {
            node.Instantiate(sphere, cube, stickman.transform);
        }
        stickman.transform.position = new Vector3(0, 0, 5);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Read string from mediapipe and create List of positions of each joint.
    private List<Vector3> ReadJoints(string positions)
    {
        // 不要な文字を削除（括弧やスペースなど）
        positions = positions.Trim('[', ']', ' ');

        // 各タプルの文字列を分割
        string[] tupleStrings = positions.Split(new[] { "), (" }, StringSplitOptions.None);

        // タプルのリスト
        List<Vector3> joints = new List<Vector3>();

        // 各タプルの文字列を解析してタプルに変換
        foreach (string tupleString in tupleStrings)
        {
            string cleanedTuple = tupleString.Trim('(', ')');
            string[] parts = cleanedTuple.Split(',');

            // floatに変換してタプルに追加
            float item1 = float.Parse(parts[0], CultureInfo.InvariantCulture);
            float item2 = float.Parse(parts[1], CultureInfo.InvariantCulture);
            float item3 = float.Parse(parts[2], CultureInfo.InvariantCulture);
            joints.Add(new Vector3(item1, item2, item3));
        }

        return joints;
    }

    private StickmanNode[] ConstructSkeleton(List<Vector3> joints)
    {
        StickmanNode nose = new StickmanNode("nose", joints[0], new StickmanNode[]{});
        StickmanNode left_eye_inner = new StickmanNode("left eye (inner)", joints[1], new StickmanNode[]{nose});
        StickmanNode left_eye = new StickmanNode("left eye", joints[2], new StickmanNode[]{left_eye_inner});
        StickmanNode left_eye_outer = new StickmanNode("left eye (outer)", joints[3], new StickmanNode[]{left_eye});
        StickmanNode right_eye_inner = new StickmanNode("right eye (inner)", joints[4], new StickmanNode[]{nose});
        StickmanNode right_eye = new StickmanNode("right eye", joints[5], new StickmanNode[]{right_eye_inner});
        StickmanNode right_eye_outer = new StickmanNode("right eye (outer)", joints[6], new StickmanNode[]{right_eye});
        StickmanNode left_ear = new StickmanNode("left ear", joints[7], new StickmanNode[]{left_eye_outer});
        StickmanNode right_ear = new StickmanNode("right ear", joints[8], new StickmanNode[]{right_eye_outer});
        StickmanNode mouth_left = new StickmanNode("mouth (left)", joints[9], new StickmanNode[]{});
        StickmanNode mouth_right = new StickmanNode("mouth (right)", joints[10], new StickmanNode[]{mouth_left});
        StickmanNode left_shoulder = new StickmanNode("left shoulder", joints[11], new StickmanNode[]{});
        StickmanNode right_shoulder = new StickmanNode("right shoulder", joints[12], new StickmanNode[]{left_shoulder});
        StickmanNode left_elbow = new StickmanNode("left elbow", joints[13], new StickmanNode[]{left_shoulder});
        StickmanNode right_elbow = new StickmanNode("right elbow", joints[14], new StickmanNode[]{right_shoulder});
        StickmanNode left_wrist = new StickmanNode("left wrist", joints[15], new StickmanNode[]{left_elbow});
        StickmanNode right_wrist = new StickmanNode("right wrist", joints[16], new StickmanNode[]{right_elbow});
        StickmanNode left_pinky = new StickmanNode("left pinky", joints[17], new StickmanNode[]{left_wrist});
        StickmanNode right_pinky = new StickmanNode("right pinky", joints[18], new StickmanNode[]{right_wrist});
        StickmanNode left_index = new StickmanNode("left index", joints[19], new StickmanNode[]{left_wrist, left_pinky});
        StickmanNode right_index = new StickmanNode("right index", joints[20], new StickmanNode[]{right_wrist, right_pinky});
        StickmanNode left_thumb = new StickmanNode("left thumb", joints[21], new StickmanNode[]{left_wrist});
        StickmanNode right_thumb = new StickmanNode("right thumb", joints[22], new StickmanNode[]{right_wrist});
        StickmanNode left_hip = new StickmanNode("left hip", joints[23], new StickmanNode[]{left_shoulder});
        StickmanNode right_hip = new StickmanNode("right hip", joints[24], new StickmanNode[]{right_shoulder, left_hip});
        StickmanNode left_knee = new StickmanNode("left knee", joints[25], new StickmanNode[]{left_hip});
        StickmanNode right_knee = new StickmanNode("right knee", joints[26], new StickmanNode[]{right_hip});
        StickmanNode left_ankle = new StickmanNode("left ankle", joints[27], new StickmanNode[]{left_knee});
        StickmanNode right_ankle = new StickmanNode("right ankle", joints[28], new StickmanNode[]{right_knee});
        StickmanNode left_heel = new StickmanNode("left heel", joints[29], new StickmanNode[]{left_ankle});
        StickmanNode right_heel = new StickmanNode("right heel", joints[30], new StickmanNode[]{right_ankle});
        StickmanNode left_foot_index = new StickmanNode("left foot index", joints[31], new StickmanNode[]{left_ankle, left_heel});
        StickmanNode right_foot_index = new StickmanNode("right foot index", joints[32], new StickmanNode[]{right_ankle, right_heel});
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

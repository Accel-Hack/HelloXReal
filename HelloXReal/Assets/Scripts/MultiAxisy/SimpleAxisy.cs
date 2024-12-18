using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleAxisy : MonoBehaviour
{
    [SerializeField] GameObject torsoPrefab;
    [SerializeField] GameObject limbPrefab;
    [SerializeField] GameObject fingerNeckPrefab;   // Prefab of finger and neck.
    [SerializeField] GameObject feetPrefab;
    [SerializeField] GameObject headPrefab;

    private AxisymmetryManTorso torso;
    private AxisymmetryManBone leftUpperArm;
    private AxisymmetryManBone rightUpperArm;
    private AxisymmetryManBone leftLowerArm;
    private AxisymmetryManBone rightLowerArm;
    private AxisymmetryManBone leftUpperLeg;
    private AxisymmetryManBone rightUpperLeg;
    private AxisymmetryManBone leftLowerLeg;
    private AxisymmetryManBone rightLowerLeg;
    private AxisymmetryManBone leftThumb;
    private AxisymmetryManBone rightThumb;
    private AxisymmetryManBone leftIndex;
    private AxisymmetryManBone rightIndex;
    private AxisymmetryManBone leftPinky;
    private AxisymmetryManBone rightPinky;
    private AxisymmetryManFeet leftFeet;
    private AxisymmetryManFeet rightFeet;
    private AxisymmetryManHead head;

    // Start is called before the first frame update
    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        this.torso = Instantiate(torsoPrefab, transform).GetComponent<AxisymmetryManTorso>();
        this.leftUpperArm  = Instantiate(limbPrefab, transform).GetComponent<AxisymmetryManBone>();
        this.rightUpperArm = Instantiate(limbPrefab, transform).GetComponent<AxisymmetryManBone>();
        this.leftLowerArm  = Instantiate(limbPrefab, transform).GetComponent<AxisymmetryManBone>();
        this.rightLowerArm = Instantiate(limbPrefab, transform).GetComponent<AxisymmetryManBone>();
        this.leftUpperLeg  = Instantiate(limbPrefab, transform).GetComponent<AxisymmetryManBone>();
        this.rightUpperLeg = Instantiate(limbPrefab, transform).GetComponent<AxisymmetryManBone>();
        this.leftLowerLeg  = Instantiate(limbPrefab, transform).GetComponent<AxisymmetryManBone>();
        this.rightLowerLeg = Instantiate(limbPrefab, transform).GetComponent<AxisymmetryManBone>();
        this.leftThumb  = Instantiate(fingerNeckPrefab, transform).GetComponent<AxisymmetryManBone>();
        this.rightThumb = Instantiate(fingerNeckPrefab, transform).GetComponent<AxisymmetryManBone>();
        this.leftIndex  = Instantiate(fingerNeckPrefab, transform).GetComponent<AxisymmetryManBone>();
        this.rightIndex = Instantiate(fingerNeckPrefab, transform).GetComponent<AxisymmetryManBone>();
        this.leftPinky  = Instantiate(fingerNeckPrefab, transform).GetComponent<AxisymmetryManBone>();
        this.rightPinky = Instantiate(fingerNeckPrefab, transform).GetComponent<AxisymmetryManBone>();
        this.leftFeet  = Instantiate(feetPrefab, transform).GetComponent<AxisymmetryManFeet>();
        this.rightFeet = Instantiate(feetPrefab, transform).GetComponent<AxisymmetryManFeet>();
        this.head = Instantiate(headPrefab, transform).GetComponent<AxisymmetryManHead>();
    }

    public void Pose(List<Vector3> frame)
    {
        this.torso.Place(frame[12], frame[11], (frame[23] + frame[24]) / 2);
        this.leftUpperArm .Place(frame[11], frame[13]);
        this.rightUpperArm.Place(frame[12], frame[14]);
        this.leftLowerArm .Place(frame[13], frame[15]);
        this.rightLowerArm.Place(frame[14], frame[16]);
        this.leftUpperLeg .Place(frame[23], frame[25]);
        this.rightUpperLeg.Place(frame[24], frame[26]);
        this.leftLowerLeg .Place(frame[25], frame[27]);
        this.rightLowerLeg.Place(frame[26], frame[28]);
        this.leftThumb .Place(frame[15], frame[21]);
        this.rightThumb.Place(frame[16], frame[22]);
        this.leftIndex .Place(frame[15], frame[19]);
        this.rightIndex.Place(frame[16], frame[20]);
        this.leftPinky .Place(frame[15], frame[17]);
        this.rightPinky.Place(frame[16], frame[18]);
        this.leftFeet .Place(frame[27], frame[31], frame[29]);
        this.rightFeet.Place(frame[28], frame[32], frame[30]);
        this.head.Place(frame[7], frame[8], frame[0]);
    }
}

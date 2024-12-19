using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Data of pose of a person at a frame.
// Dummy PersonFrameData can exist. It is filled with 1 binary.
public class PersonFrameData
{
    public const int JOINT_NUM = 33;
    private const int RIGHT_HIP_IDX = 23;
    private const int LEFT_HIP_IDX = 24;
    public const int SIZE = 2 * (4 + 3 * JOINT_NUM);

    private ushort x0;
    private ushort y0;
    private ushort x1;
    private ushort y1;
    private Vector3[] joints = new Vector3[JOINT_NUM];

    private float size = 0;

    private bool isSeparator = true;

    public PersonFrameData(byte[] bytes, int startIndex)
    {
        this.x0 = BitConverter.ToUInt16(bytes, startIndex);
        this.y0 = BitConverter.ToUInt16(bytes, startIndex + 2);
        this.x1 = BitConverter.ToUInt16(bytes, startIndex + 4);
        this.y1 = BitConverter.ToUInt16(bytes, startIndex + 6);
        if (this.x0 != ushort.MaxValue) this.isSeparator = false;
        if (this.y0 != ushort.MaxValue) this.isSeparator = false;
        if (this.x1 != ushort.MaxValue) this.isSeparator = false;
        if (this.y1 != ushort.MaxValue) this.isSeparator = false;

        for (int i = 0; i < JOINT_NUM; i++)
        {
            short x = BitConverter.ToInt16(bytes, startIndex + 8 + 2 * (i * 3));
            short y = BitConverter.ToInt16(bytes, startIndex + 8 + 2 * (i * 3 + 1));
            short z = BitConverter.ToInt16(bytes, startIndex + 8 + 2 * (i * 3 + 2));
            if (x != -1) this.isSeparator = false;
            if (y != -1) this.isSeparator = false;
            if (z != -1) this.isSeparator = false;

            // Output of Mediapipe is upside down...
            this.joints[i] = new Vector3(x, -y, z);
        }

        Vector3 localCenter = (this.joints[RIGHT_HIP_IDX] + this.joints[LEFT_HIP_IDX]) / 2;
        for (int i = 0; i < JOINT_NUM; i++) {
            // Standardize joints' positions for center of hip.
            this.joints[i] -= localCenter;
        }
    }

    public bool IsSeparator()
    {
        return this.isSeparator;
    }

    public List<Vector3> GetPose()
    {
        return new List<Vector3>(this.joints);
    }

    public Vector3 GetCenterInImage()
    {
        return new Vector3((this.x0 + this.x1) / 2 / this.size, (this.y0 + this.y1) / 2 / this.size, 0);
    }

    public float GetSize()
    {
        return (this.joints[RIGHT_HIP_IDX] - this.joints[LEFT_HIP_IDX]).sqrMagnitude;
    }

    public void Normalize(float size)
    {
        for (int i = 0; i < this.joints.Length; i++) {
            this.joints[i] /= size;
        }
        this.size = size;
    }
}

public class FrameData
{
    private List<PersonFrameData> personFrameDatas = new List<PersonFrameData>();

    public FrameData(List<PersonFrameData> personFrameDatas)
    {
        this.personFrameDatas = personFrameDatas;
    }

    public int GetPersonNum()
    {
        return this.personFrameDatas.Count;
    }

    public List<Vector3> GetPose(int personIdx)
    {
        if (personIdx >= this.GetPersonNum()) {
            return null;
        }
        return this.personFrameDatas[personIdx].GetPose();
    }

    public Vector3? GetCenterInImage(int personIdx)
    {
        if (personIdx >= this.GetPersonNum()) {
            return null;
        }
        return this.personFrameDatas[personIdx].GetCenterInImage();
    }

    public Vector3 GetPeopleCenter()
    {
        Vector3 sum = Vector3.zero;
        foreach (PersonFrameData personFrameData in this.personFrameDatas) {
            sum += personFrameData.GetCenterInImage();
        }
        return sum / personFrameDatas.Count;
    }

    public void Normalize(float size)
    {
        foreach (PersonFrameData personFrameData in this.personFrameDatas) {
            personFrameData.Normalize(size);
        }
    }
}

public class VideoData
{
    private List<FrameData> frameDatas = new List<FrameData>();

    // Size of first person.
    private float size = 0;

    public VideoData(byte[] bytes)
    {
        // Encode binary data to VideoData.
        List<PersonFrameData> personFrameDatas = new List<PersonFrameData>();
        for (int i = 0; i < bytes.Length / PersonFrameData.SIZE; i++)
        {
            int personStartIndex = i * PersonFrameData.SIZE;
            PersonFrameData personFrameData = new PersonFrameData(bytes, personStartIndex);
            if (personFrameData.IsSeparator())
            {
                this.frameDatas.Add(new FrameData(personFrameDatas));
                personFrameDatas = new List<PersonFrameData>();
            } else {
                personFrameDatas.Add(personFrameData);
                if (this.size == 0) {
                    this.size = personFrameData.GetSize();
                }
            }
        }
        this.Normalize();
    }

    public int GetMaxPersonNum()
    {
        int max = 0;
        foreach (FrameData frameData in this.frameDatas) {
            int personNum = frameData.GetPersonNum();
            if (max < personNum) {
                max = personNum;
            }
        }
        return max;
    }

    public List<Vector3> GetPose(int personIdx, int animationFrameCount)
    {
        return this.frameDatas[animationFrameCount].GetPose(personIdx);
    }

    public Vector3? GetCenterInImage(int personIdx, int animationFrameCount)
    {
        return this.frameDatas[animationFrameCount].GetCenterInImage(personIdx);
    }

    // Called for first frame.
    public Vector3 GetFirstPeopleCenter()
    {
        return this.frameDatas[0].GetPeopleCenter();
    }

    private void Normalize()
    {
        foreach (FrameData frameData in this.frameDatas) {
            frameData.Normalize(this.size);
        }
    }
}
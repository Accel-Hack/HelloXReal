using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Data of rect of a person at a frame.
// Dummy YoloPersonFrameData can exist. It is filled with 1 binary.
public class YoloPersonFrameData
{
    public const int SIZE = 2 * 4;

    private ushort x0;
    private ushort y0;
    private ushort x1;
    private ushort y1;

    private bool isSeparator = true;

    public YoloPersonFrameData(byte[] bytes, int startIndex)
    {
        this.x0 = BitConverter.ToUInt16(bytes, startIndex);
        this.y0 = BitConverter.ToUInt16(bytes, startIndex + 2);
        this.x1 = BitConverter.ToUInt16(bytes, startIndex + 4);
        this.y1 = BitConverter.ToUInt16(bytes, startIndex + 6);
        if (this.x0 != ushort.MaxValue) this.isSeparator = false;
        if (this.y0 != ushort.MaxValue) this.isSeparator = false;
        if (this.x1 != ushort.MaxValue) this.isSeparator = false;
        if (this.y1 != ushort.MaxValue) this.isSeparator = false;
    }

    public bool IsSeparator()
    {
        return this.isSeparator;
    }

    public Vector3 GetCenterInImage()
    {
        return new Vector3((this.x0 + this.x1) / 2, (this.y0 + this.y1) / 2, 0);
    }
}

public class YoloFrameData
{
    private List<YoloPersonFrameData> personFrameDatas = new List<YoloPersonFrameData>();

    public YoloFrameData(List<YoloPersonFrameData> personFrameDatas)
    {
        this.personFrameDatas = personFrameDatas;
    }

    public int GetPersonNum()
    {
        return this.personFrameDatas.Count;
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
        foreach (YoloPersonFrameData personFrameData in this.personFrameDatas) {
            sum += personFrameData.GetCenterInImage();
        }
        return sum / personFrameDatas.Count;
    }
}

public class YoloVideoData
{
    private List<YoloFrameData> frameDatas = new List<YoloFrameData>();
    private ushort videoWidth = 0;
    private ushort videoHeight = 0;

    // Encode binary data to YoloVideoData.
    public YoloVideoData(byte[] bytes)
    {
        this.videoWidth = BitConverter.ToUInt16(bytes, 0);
        this.videoHeight = BitConverter.ToUInt16(bytes, 2);
        List<YoloPersonFrameData> personFrameDatas = new List<YoloPersonFrameData>();
        for (int i = 0; i < bytes.Length / YoloPersonFrameData.SIZE; i++)
        {
            int personStartIndex = i * YoloPersonFrameData.SIZE + 4;
            YoloPersonFrameData personFrameData = new YoloPersonFrameData(bytes, personStartIndex);
            if (personFrameData.IsSeparator())
            {
                this.frameDatas.Add(new YoloFrameData(personFrameDatas));
                personFrameDatas = new List<YoloPersonFrameData>();
            } else {
                personFrameDatas.Add(personFrameData);
            }
        }
    }

    public int GetMaxPersonNum()
    {
        int max = 0;
        foreach (YoloFrameData frameData in this.frameDatas) {
            int personNum = frameData.GetPersonNum();
            if (max < personNum) {
                max = personNum;
            }
        }
        return max;
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

    public (int, int) GetVideoSize()
    {
        return ((int) this.videoWidth, (int) this.videoHeight);
    }
}
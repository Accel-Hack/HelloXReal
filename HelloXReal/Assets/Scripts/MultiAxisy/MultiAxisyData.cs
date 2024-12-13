using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonFrameData
{
    public const int JOINT_NUM = 33;
    public const int SIZE = 2 * (4 + 3 * JOINT_NUM);

    private ushort x0;
    private ushort y0;
    private ushort x1;
    private ushort y1;
    private Vector3[] joints = new Vector3[JOINT_NUM];

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
            this.joints[i] = new Vector3(x, y, z);
        }     
    }

    public bool IsSeparator()
    {
        return this.isSeparator;
    }
}

public class FrameData
{
    private List<PersonFrameData> personFrameDatas = new List<PersonFrameData>();

    public FrameData(List<PersonFrameData> personFrameDatas)
    {
        this.personFrameDatas = personFrameDatas;
    }
}

public class VideoData
{
    private List<FrameData> frameDatas = new List<FrameData>();

    public VideoData(byte[] bytes)
    {
        List<PersonFrameData> personFrameDatas = new List<PersonFrameData>();
        for (int i = 0; i < bytes.Length / PersonFrameData.SIZE; i++)
        {
            int personStartIndex = i * PersonFrameData.SIZE;
            PersonFrameData personFrameData = PersonFrameData(bytes, personStartIndex);
            if (personFrameData.IsSeparator())
            {
                this.frameDatas.Add(FrameData(personFrameDatas));
                personFrameDatas = new List<PersonFrameData>();
            } else {
                personFrameDatas.Add(personFrameData);
            }
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Globalization;
using System;

public class MediaPipeReceiver
{
    // Read string from mediapipe and create List of positions of each joint.
    public static List<Vector3> ReadJoints(string positions)
    {
        // 前後の不要な文字を削除（括弧やスペースなど）
        positions = positions.Trim('[', ']', ' ', '\n');

        // 各タプルの文字列を分割
        string[] tupleStrings = positions.Split(new[] { "), (" }, StringSplitOptions.None);

        // 座標のリスト
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

    // Create joints' positions sequence as List<List<Vector3>> from string
    public static List<List<Vector3>> ReadSequence(string sequence)
    {
        string[] frameStrings = sequence.Split(new[] {"], ["}, StringSplitOptions.None);
        List<List<Vector3>> frames = new List<List<Vector3>>();
        foreach (string frameString in frameStrings) {
            if (frameString != "") {    // TODO: Hundle empty frame
                frames.Add(ReadJoints(frameString));
            }
        }
        for (int i = 0; i < frames.Count; i++) {
            for (int j = 0; j < frames[i].Count; j++) {
                frames[i][j] = Vector3.Scale(frames[i][j], new Vector3(1, -1, 1));
            }
        }
        Vector3 origin = (frames[0][23] + frames[0][24]) / 2;
        for (int i = 0; i < frames.Count; i++) {
            for (int j = 0; j < frames[i].Count; j++) {
                frames[i][j] -= origin;
            }
        }
        return frames;
    }
}

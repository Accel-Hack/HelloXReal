using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickmanNode
{
    class NeighborInformation
    {
        public bool boneExists;
        public StickmanNode neighbor;

        public NeighborInformation(StickmanNode neighbor)
        {
            this.boneExists = false;
            this.neighbor = neighbor;
        }
    }

    private const float BONE_THICKNESS = 0.1f;
    private string name;
    private Vector3 position;
    private List<NeighborInformation> neighbors;

    // Constructor for each node.
    public StickmanNode(string name, Vector3 position, StickmanNode[] neighbors)
    {
        this.name = name;
        this.position = position;
        this.neighbors = new List<NeighborInformation>();
        foreach (StickmanNode neighbor in neighbors) {
            this.neighbors.Add(new NeighborInformation(neighbor));
            neighbor.neighbors.Add(new NeighborInformation(this));
        }
    }

    // Instantiate sphere and cube with Node information.
    public void Instantiate(GameObject sphere, GameObject cube, Transform parent)
    {
        GameObject obj = GameObject.Instantiate(sphere, this.position, Quaternion.identity, parent);
        obj.name = this.name;
        foreach (NeighborInformation info in this.neighbors) {
            if (!info.boneExists) {
                this.InstantiateBone(cube, this.position, info.neighbor.position, parent);
                info.boneExists = true;
                foreach (NeighborInformation neighborInfo in info.neighbor.neighbors) {
                    if (neighborInfo.neighbor == this) {
                        neighborInfo.boneExists = true;
                    }
                }
            }
        }
    }

    // Instantiate bone whose head and tail locates at given vector3.
    public void InstantiateBone(GameObject bonePrefab, Vector3 head, Vector3 tail, Transform parent)
    {
        GameObject bone = GameObject.Instantiate(bonePrefab, (head + tail) / 2, Quaternion.identity, parent);
        float length = (tail - head).magnitude;
        bone.transform.localScale = new Vector3(BONE_THICKNESS, BONE_THICKNESS, length);
        bone.transform.LookAt(tail);
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Information each joint contains.
public class StickmanNode
{
    // Information between neighbor joint.
    // Neighbor joint itself and the bone sharing with it.
    class NeighborInformation
    {
        public GameObject bone;
        public StickmanNode neighbor;

        public NeighborInformation(StickmanNode neighbor)
        {
            this.bone = null;
            this.neighbor = neighbor;
        }
    }

    private const float BONE_THICKNESS = 0.5f;
    private string name;
    private int id;
    private GameObject sphere;
    private Vector3 position;
    private List<NeighborInformation> neighbors;

    // Constructor for each node.
    public StickmanNode(string name, int id, StickmanNode[] neighbors)
    {
        this.name = name;
        this.id = id;
        this.sphere = null;
        this.position = Vector3.zero;
        this.neighbors = new List<NeighborInformation>();
        foreach (StickmanNode neighbor in neighbors) {
            this.neighbors.Add(new NeighborInformation(neighbor));
            neighbor.neighbors.Add(new NeighborInformation(this));
        }
    }

    // Instantiate sphere and cube with Node information.
    public void Instantiate(GameObject sphere, GameObject cube, Transform parent)
    {
        this.sphere = GameObject.Instantiate(sphere, this.position, Quaternion.identity, parent);
        this.sphere.name = this.name;
        this.sphere.transform.localScale = StickmanCreater.MAGNIFICATION * StickmanCreater.PREFAB_SCALE_FOR_MAGNIFICATION * Vector3.one;
        foreach (NeighborInformation info in this.neighbors) {
            if (info.bone == null) {
                info.bone = this.InstantiateBone(cube, this.position, info.neighbor.position, parent);
                foreach (NeighborInformation neighborInfo in info.neighbor.neighbors) {
                    if (neighborInfo.neighbor == this) {
                        neighborInfo.bone = info.bone;
                    }
                }
            }
        }
    }

    // Instantiate bone whose head and tail locates at given vector3.
    public GameObject InstantiateBone(GameObject bonePrefab, Vector3 head, Vector3 tail, Transform parent)
    {
        GameObject bone = GameObject.Instantiate(bonePrefab, Vector3.zero, Quaternion.identity, parent);
        this.ReplaceBone(bone, head, tail);
        return bone;
    }

    // Place a bone at coordinates of its head and tail.
    private void ReplaceBone(GameObject bone, Vector3 head, Vector3 tail)
    {
        bone.transform.position = (head + tail) / 2;
        float length = (tail - head).magnitude;
        bone.transform.localScale = new Vector3(
            BONE_THICKNESS * StickmanCreater.MAGNIFICATION * StickmanCreater.PREFAB_SCALE_FOR_MAGNIFICATION, 
            BONE_THICKNESS * StickmanCreater.MAGNIFICATION * StickmanCreater.PREFAB_SCALE_FOR_MAGNIFICATION, 
            length
        );
        bone.transform.LookAt(tail);
        bone.transform.position += StickmanCreater.stickmanPosition;
    }

    // Place a joint at a coordinate. This also call ReplaceBone to make the bones follow the joints.
    // TODO: Not optimized. Each bone moves twice...
    public void Pose(List<Vector3> joints)
    {
        this.position = joints[this.id];
        this.sphere.transform.position = joints[this.id] + StickmanCreater.stickmanPosition;
        foreach (NeighborInformation info in this.neighbors) {
            this.ReplaceBone(info.bone, this.position, info.neighbor.position);
        }
    }
}
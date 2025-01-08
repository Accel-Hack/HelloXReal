import cv2  # type: ignore

from data import FrameData  # type: ignore

# Visualize rects of person areas and circles of their joints.
class Visualizer:
    def __init__(self):
        pass

    def visualized_frame(self, frame ,frame_data: FrameData):
        for person_frame_data in frame_data:
            cv2.rectangle(frame, (person_frame_data.rect[0], person_frame_data.rect[1]), (person_frame_data.rect[2], person_frame_data.rect[3]), (255, 255, 255), thickness=8)
            if person_frame_data.joint_positions != None:
                for position in person_frame_data.joint_positions:
                    cv2.circle(frame, (person_frame_data.rect[0] + position[0], person_frame_data.rect[1] + position[1]), 5, (255, 0, 0), 2)
                
        return frame
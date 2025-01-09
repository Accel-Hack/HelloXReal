import cv2                  # type: ignore
from loguru import logger   # type: ignore
from tqdm import tqdm       # type: ignore

import sys

from yolopredictor import YoloPredictor as YoloPredictor    # type: ignore
from mppredictor import MPPredictor as MPPredictor          # type: ignore
from visualizer import Visualizer           # type: ignore
from data import PersonFrameData, FrameData, VideoData      # type: ignore

MERGIN = 20

def estimate(device_name, input_path, output_path):
    global MERGIN

    yolopredictor = YoloPredictor(device_name)
    mppredictor = MPPredictor()
    visualizer = Visualizer()
    
    cap = cv2.VideoCapture(input_path)
    width = cap.get(cv2.CAP_PROP_FRAME_WIDTH)  # float
    height = cap.get(cv2.CAP_PROP_FRAME_HEIGHT)  # float
    fps = cap.get(cv2.CAP_PROP_FPS)

    bar = tqdm(total=cap.get(cv2.CAP_PROP_FRAME_COUNT))

    logger.info(f"video save_path is {output_path}")
    vid_writer = cv2.VideoWriter(
        output_path, cv2.VideoWriter_fourcc(*"mp4v"), fps, (int(width), int(height))
    )

    video_data = VideoData()

    while True:
        ret_val, frame = cap.read()
        bar.update(1)
        if ret_val:
            outputs, img_info = yolopredictor.inference(frame)
            assert len(outputs) == 1

            frame_data = FrameData()

            frame = img_info["raw_img"].copy()
            for object in outputs[0]:
                if object[4] * object[5] < 0.5:
                    continue    # confidence is too low...
                if object[6] != 0:
                    continue    # not a person
                rect = object[0:4] / img_info["ratio"]
                x0 = int(max(rect[0] - MERGIN, 0))
                y0 = int(max(rect[1] - MERGIN, 0))
                x1 = int(min(rect[2] + MERGIN, width))
                y1 = int(min(rect[3] + MERGIN, height))
                person_clip = frame[y0:y1, x0:x1]

                if person_clip.shape[0] < 1 or person_clip.shape[1] < 1:
                    continue    # rect is empty

                joint_positions = mppredictor.inference(person_clip)

                if joint_positions != None:
                    frame_data.append(PersonFrameData((x0, y0, x1, y1), joint_positions))

            frame = visualizer.visualized_frame(frame, frame_data)
            video_data.append(frame_data)
            vid_writer.write(frame)
        else:
            # end of video
            break

    encoded_bytes = video_data.encode()

    return encoded_bytes

if __name__ == "__main__":
    estimate("mps", sys.argv[1], "output.mov")
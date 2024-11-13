import mediapipe as mp  # type: ignore
import cv2              # type: ignore
import sys

def detect_joints_from_video(video_path):
    # Load trained model
    model_path = "pose_landmarker_full.task"

    # Setting based on
    # https://ai.google.dev/edge/mediapipe/solutions/vision/pose_landmarker/python?hl=ja#video
    BaseOptions = mp.tasks.BaseOptions
    PoseLandmarker = mp.tasks.vision.PoseLandmarker
    PoseLandmarkerOptions = mp.tasks.vision.PoseLandmarkerOptions
    VisionRunningMode = mp.tasks.vision.RunningMode

    options = PoseLandmarkerOptions(
        base_options=BaseOptions(model_asset_path=model_path),
        running_mode=VisionRunningMode.VIDEO)

    with PoseLandmarker.create_from_options(options) as landmarker:
        cap = cv2.VideoCapture(video_path)
        if not cap.isOpened():
            print("Reading file failed...")
            sys.exit(1)

        positions_sequence = []
        while True: # Loop for each frame
            ret, frame = cap.read() # Read 1 frame
            if not ret:
                # End of file
                break

            frame_rgb = cv2.cvtColor(frame, cv2.COLOR_BGR2RGB)
            timestamp_ms = int(cap.get(cv2.CAP_PROP_POS_MSEC))
            mp_image = mp.Image(image_format=mp.ImageFormat.SRGB, data=frame_rgb)

            result = landmarker.detect_for_video(mp_image, timestamp_ms=timestamp_ms)   # Result of detection
            if result is None or result.pose_world_landmarks is None:
                positions_sequence.append([])   # Failed frame
            else:
                positions = [(position.x, position.y, position.z) for position in result.pose_world_landmarks[0]]
                positions_sequence.append(positions)

        # release resources
        cap.release()

        return positions_sequence
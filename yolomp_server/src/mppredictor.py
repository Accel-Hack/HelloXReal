import mediapipe as mp  # type: ignore
import cv2              # type: ignore

class MPPredictor:
    def __init__(self):
        model_path = "resource/pose_landmarker_full.task"

        # Setting based on
        # https://ai.google.dev/edge/mediapipe/solutions/vision/pose_landmarker/python?hl=ja#video
        BaseOptions = mp.tasks.BaseOptions
        self.PoseLandmarker = mp.tasks.vision.PoseLandmarker
        PoseLandmarkerOptions = mp.tasks.vision.PoseLandmarkerOptions
        # VisionRunningMode = mp.tasks.vision.RunningMode

        self.options = PoseLandmarkerOptions(
            base_options=BaseOptions(model_asset_path=model_path),
            # running_mode=VisionRunningMode.VIDEO,
            num_poses=1
            )
        
        self.landmarker = self.PoseLandmarker.create_from_options(self.options)
        
    def inference(self, frame):
        frame_rgb = cv2.cvtColor(frame, cv2.COLOR_BGR2RGB)
        width = frame_rgb.shape[1]
        height = frame_rgb.shape[0]
        mp_image = mp.Image(image_format=mp.ImageFormat.SRGB, data=frame_rgb)

        # result = self.landmarker.detect_for_video(mp_image, timestamp_ms=self.timestamp)   # Result of detection
        # self.timestamp += 1
        result = self.landmarker.detect(mp_image)
        if result is None or result.pose_landmarks is None or len(result.pose_landmarks) < 1:
            return None # Failed frame
        else:
            # delta_x = result.pose_landmarks[0][24].x - result.pose_landmarks[0][23].x
            # delta_z = result.pose_landmarks[0][24].z - result.pose_landmarks[0][23].z
            # z_rate = abs(delta_z / delta_x)
            # z_rate = max(z_rate, 0.5)
            # z_rate = min(z_rate, 2)
            positions = [(
                int(position.x * width), 
                int(position.y * height), 
                int(position.z * width / 3)) 
                            for position in result.pose_landmarks[0]]
            return positions
        

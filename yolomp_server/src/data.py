MEDIAPIPE_JOINT_NUM = 33

# Data of a person on a frame.
class PersonFrameData:
    def __init__(self, rect: tuple[int, int, int, int], joint_positions: list[tuple[int, int, int]]):
        assert joint_positions != None
        assert len(joint_positions) == MEDIAPIPE_JOINT_NUM
        self.rect = rect
        self.joint_positions = joint_positions

    def encode(self) -> bytes:
        result = bytes()
        for item in self.rect:
            result += item.to_bytes(2, 'little')
        for position in self.joint_positions:
            for item in position:
                result += item.to_bytes(2, 'little', signed=True)
        return result
    
    def dummy() -> bytes:
        return b'\xff\xff' * (4 + 3 * MEDIAPIPE_JOINT_NUM)
        
# Data of a frame.
class FrameData:
    def __init__(self):
        self.person_datas: list[PersonFrameData] = []

    def append(self, person_data: PersonFrameData):
        self.person_datas.append(person_data)

    def encode(self) -> bytes:
        result = bytes()
        for item in self.person_datas:
            result += item.encode()
        return result

    def __iter__(self):
        return iter(self.person_datas)

# Data of a video (frames).
class VideoData:
    def __init__(self):
        self.frame_datas: list[FrameData] = []

    def append(self, frame_data: FrameData):
        self.frame_datas.append(frame_data)

    def encode(self) -> str:
        result = bytes()
        for frame_data in self.frame_datas:
            result += frame_data.encode()
            result += PersonFrameData.dummy()
        return result

    def __iter__(self):
        return iter(self.frame_datas)
    

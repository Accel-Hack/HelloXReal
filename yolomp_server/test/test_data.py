from data import PersonFrameData    # type: ignore

def test_data():
    person_frame_data = PersonFrameData((1, 1, 1, 0), [(0, 0, 0)] * 33)
    correct = b'\x00\x01\x00\x01\x00\x01\x00\x00' + b'\x00\x00' * 33 * 3
    assert len(correct) == len(person_frame_data.encode())
    assert correct == person_frame_data.encode()
import os

import cv2                  # type: ignore
import torch                # type: ignore

from yolox.data.data_augment import ValTransform  # type: ignore
from yolox.data.datasets import COCO_CLASSES      # type: ignore
from yolox.exp import get_exp                     # type: ignore
from yolox.utils import postprocess, vis          # type: ignore

class YoloPredictor(object):
    def __init__(self, device_name):
        exp = get_exp(None, "yolox-s")
        exp.test_conf = 0.25
        exp.nmsthre = 0.45
        exp.test_size = (640, 640)
        model = exp.get_model().to(device_name)
        model.eval()
        ckpt_file = "resource/yolox_s.pth"
        ckpt = torch.load(ckpt_file, map_location="cpu", weights_only=True)
        # load the model state dict
        model.load_state_dict(ckpt["model"])

        self.model = model
        self.cls_names = COCO_CLASSES
        self.num_classes = exp.num_classes
        self.nmsthre = exp.nmsthre
        self.test_size = exp.test_size
        self.device = device_name
        self.preproc = ValTransform(legacy=False)
        self.confthre = exp.test_conf

    def inference(self, img):
        img_info = {"id": 0}
        if isinstance(img, str):
            img_info["file_name"] = os.path.basename(img)
            img = cv2.imread(img)
        else:
            img_info["file_name"] = None

        height, width = img.shape[:2]
        img_info["height"] = height
        img_info["width"] = width
        img_info["raw_img"] = img

        ratio = min(self.test_size[0] / img.shape[0], self.test_size[1] / img.shape[1])
        img_info["ratio"] = ratio

        img, _ = self.preproc(img, None, self.test_size)
        img = torch.from_numpy(img).unsqueeze(0)
        img = img.float()
        img = img.to(self.device)

        with torch.no_grad():
            outputs = self.model(img)
            outputs = postprocess(
                outputs, self.num_classes, self.confthre,
                self.nmsthre, class_agnostic=True
            )
        return outputs, img_info
#
# This file is part of the Buildings and Habitats object Model (BHoM)
# Copyright (c) 2015 - 2020, the respective contributors. All rights reserved.
#
# Each contributor holds copyright over their respective contributions.
# The project versioning (Git) records all such contribution source information.
#
#
# The BHoM is free software: you can redistribute it and/or modify
# it under the terms of the GNU Lesser General Public License as published by
# the Free Software Foundation, either version 3.0 of the License, or
# (at your option) any later version.
#
# The BHoM is distributed in the hope that it will be useful,
# but WITHOUT ANY WARRANTY; without even the implied warranty of
# MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
# GNU Lesser General Public License for more details.
#
# You should have received a copy of the GNU Lesser General Public License
# along with this code. If not, see <https://www.gnu.org/licenses/lgpl-3.0.html>.
#

import os
import PIL
import torch
import torchvision


model: torch.nn.Module = torchvision.models.detection.fasterrcnn_resnet50_fpn(pretrained=True, pretrained_backbone=True)
model.eval()

def detect_objects(image_path: str, gpu=False):
    if not (os.path.isfile(image_path)):
        raise FileNotFoundError(image_path)

    pil_image: PIL.Image.Image = PIL.Image.open(image_path)
    tensor_image: torch.Tensor = torchvision.transforms.functional.to_tensor(pil_image)

    if gpu:
        tensor_image = tensor_image.cuda()
        model = model.cuda()

    with torch.no_grad():
        detection: List[Dict[str, torch.Tensor]] = model(tensor_image.unsqueeze(0))
    return detection

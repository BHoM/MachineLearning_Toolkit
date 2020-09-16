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
import numpy as np
import torch
import torchvision


@torch.no_grad()
def infer(image_path: str, gpu: bool=False):
	if not (os.path.isfile(image_path)):
		raise FileNotFoundError(image_path)

	# intantiace the model
	global model
	if model is None:
		model = torch.hub.load('pytorch/vision:v0.6.0', 'deeplabv3_resnet101', pretrained=True)
	model.eval()

	# preprocess image and transform into tensor
	pil_image: PIL.Image.Image = PIL.Image.open(image_path)
	preprocess = torchvision.transforms.Compose([
		torchvision.transforms.ToTensor(),
		torchvision.transforms.Normalize(mean=[0.485, 0.456, 0.406], std=[0.229, 0.224, 0.225]),
	])
	tensor_image: torch.Tensor = preprocess(pil_image).unsqueeze(0)

	# set accelerators
	if gpu:
		tensor_image = tensor_image.cuda()
		model = model.cuda()
	else:
		tensor_image = tensor_image.cpu()
		model = model.cpu()

	# run prediction
	output = model(tensor_image).get("out")
	return torch.argmax(output, dim=1).squeeze()


model = None

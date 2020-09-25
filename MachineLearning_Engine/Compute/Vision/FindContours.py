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

import numpy as np
import matplotlib.pyplot as plt
import skimage.measure
import skimage.color
import torch


def infer(im: np.ndarray, level: int):
	if torch.is_tensor(im):
		im = im.detach().cpu().numpy()

	im = im.squeeze()
	ndim = im.ndim

	if ndim == 2:
		return skimage.measure.find_contours(im, level)

	if ndim == 3:
		# case: RGB image, channel first
		if im.size(0) == 3:
			im = np.transpose(im, (2, 1, 0))
			return skimage.measure.find_contours(im, level)
		# case: RGB image, channel last
		elif im.size(2) == 3:
			mask = skimage.color.rgb2gray(im)
			return skimage.measure.find_contours(im, level)
		# case: list of grayscale images
		elif 3 not in im.shape:
			return [infer(x, level) for x in im]

	if ndim == 4:
		# the only case is a list of images, rgb or grayscale with channels 1
		return [infer(x, level) for x in im]

	raise ValueError("I really don't know what you want to do with a tensor {}".format(im.shape))
	return

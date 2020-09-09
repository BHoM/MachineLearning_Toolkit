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

import PIL
import torch
import matplotlib
import matplotlib.pyplot as plt
import os
import subprocess


def plot_pil_image(image: PIL.Image, height: int, width: int, grayscale: bool):
	fig = plt.figure(figsize=(height, width))

	if grayscale:
		cmap = "gray"
	else:
		cmap = None
	plt.imshow(image, cmap=cmap)

	plt.show()
	return
	

def plot_tensor_image(image: torch.Tensor, height: int, width: int, grayscale: bool):
	fig = plt.figure(figsize=(height, width))

	if grayscale:
		cmap = "gray"
	else:
		cmap = None
	plt.imshow(image.cpu(), cmap=cmap)
	plt.tight_layout()

	home = r"C:\ProgramData\BHoM\Extensions\Python\temp"
	os.makedirs(home)

	path = os.path.join(home, "current_plot.png")
	plt.savefig(path)
	
	subprocess.run(["explorer", path])
	return
	
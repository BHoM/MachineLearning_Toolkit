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

from sklearn.preprocessing import MinMaxScaler
import numpy as np


def fit(x: np.ndarray):
	# instantiace a min-max scaler
	scaler = MinMaxScaler()

	# make sure the input is at least bidimensinal
	if x.ndim == 1:
		x = x.reshape(-1, 1)
	return scaler.fit(x)

def infer(scaler: MinMaxScaler, x: np.ndarray):
	# make sure the input is at least bidimensinal
	if x.ndim == 1:
		x = x.reshape(-1, 1)
	return scaler.transform(x)

def infer_inverse(scaler: MinMaxScaler, x: np.ndarray):
	# make sure the input is at least bidimensinal
	if x.ndim == 1:
		x = x.reshape(-1, 1)
	return scaler.inverse_transform(x)
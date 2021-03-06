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

from sklearn.linear_model import LinearRegression
import numpy as np


def fit(x: np.ndarray, y: np.ndarray):
	assert len(x) == len(y), "Input data and target data have different length {} and {}".format(
								len(x), len(y))
	
	# instantiace linear regression model
	model = LinearRegression()
	
	# make sure the input is at least bidimensinal
	if x.ndim == 1:
		x = x.reshape(-1, 1)
	if y.ndim == 1:
		y = y.reshape(-1, 1)

	# fit the training data
	model.fit(x, y)
	return model

def infer(model: LinearRegression, x: np.ndarray):
	# make sure the input is at least bidimensinal
	if x.ndim == 1:
		x = x.reshape(-1, 1)
	return model.predict(x)

def score(model: LinearRegression, x: np.ndarray, y: np.ndarray):
	# make sure the input is at least bidimensinal
	if x.ndim == 1:
		x = x.reshape(-1, 1)
	if y.ndim == 1:
		y = y.reshape(-1, 1)
	return model.score(x, y)
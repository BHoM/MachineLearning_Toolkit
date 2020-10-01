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

import torch
from transformers import pipeline


def infer(text: str):
    global SENTIMENT_ANALYSIS_PIPELINE
    if SENTIMENT_ANALYSIS_PIPELINE is None:
        SENTIMENT_ANALYSIS_PIPELINE = pipeline('sentiment-analysis')

    output = SENTIMENT_ANALYSIS_PIPELINE(text)[0]
    sentiment = 1 if output.get("label") == "POSITIVE" else -1
    score = output.get("score")

    return score * sentiment

SENTIMENT_ANALYSIS_PIPELINE = None

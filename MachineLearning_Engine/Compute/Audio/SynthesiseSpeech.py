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
import numpy as np


def infer(text: str, gpu: bool):
    # accelerate on gpu if possible
    device = "cuda" if gpu else "cpu"

    # load models if necessary
    global WAVEGLOW
    if WAVEGLOW is None:
        WAVEGLOW = torch.hub.load('nvidia/DeepLearningExamples:torchhub', 'nvidia_waveglow')
        WAVEGLOW = WAVEGLOW.remove_weightnorm(WAVEGLOW)
        WAVEGLOW.eval()

    global TACOTRON2
    if TACOTRON2 is None:
        TACOTRON2 = torch.hub.load('nvidia/DeepLearningExamples:torchhub', 'nvidia_tacotron2')
        TACOTRON2.eval()

    WAVEGLOW = WAVEGLOW.to(device)
    TACOTRON2 = TACOTRON2.to(device)

    # preprocess text sequence
    sequence = np.array(TACOTRON2.text_to_sequence(text, ['english_cleaners']))[None, :]
    sequence = torch.as_tensor(sequence, device=device, dtype=torch.int64)

    # compute output
    with torch.no_grad():
        _, mel, _, _ = TACOTRON2.infer(sequence)
        audio = WAVEGLOW.infer(mel)
    audio_numpy = audio[0].data.cpu().numpy()

    return audio_numpy

TACOTRON2 = None
WAVEGLOW = None

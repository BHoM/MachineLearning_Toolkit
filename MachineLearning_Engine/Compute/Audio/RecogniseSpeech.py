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
import sys
import at16k
import at16k.api


def infer(wav_file: str, run: bool):
    if not run:
        return ""

    os.environ['AT16K_RESOURCES_DIR'] = HOME

    # load models if necessary
    global SPEECH_RECOGNITION_MODEL
    if SPEECH_RECOGNITION_MODEL is None:
        _download()
        SPEECH_RECOGNITION_MODEL = at16k.api.SpeechToText(os.path.normpath("en_16k"))



    return audio_numpy


def _download():
    os.makedirs(HOME, exist_ok=True)

    sys.argv = ['download', 'en_16k']
    exec(open(at16k.download.__file__).read())
    return

HOME = "C:\\ProgramData\\BHoM\\Extensions\\Python\\models\\audio\\at_16k\\"
SPEECH_RECOGNITION_MODEL = None

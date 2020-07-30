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
import numpy as np
import matplotlib as mpl
matplotlib.use("agg")
import matplotlib.pyplot as plt

def rose(
    directions: np.ndarray,
    values: np.ndarray,
    save_path: str,
    ndirections: int = 16,
    bins: np.ndarray = None,
    barwidth: float = 0.8,
    normed: bool = True,
    title: str = None,
    unit: str = None,
    cmap: str = 'GnBu',
    tone_color: str = "black",
    transparency: bool = False,
    ):
    """ Generate a Rose from a set of directions and values

    Parameters
    ----------
    directions : np.ndarray
        List of n values < 360.
    values : np.ndarray
        List of n values.
    save_path : str
        The full path where the plot will be saved.
    ndirections : int
        The number of bins into which "directions" will be sorted
    bins : int or None
        The bin edges into which "values" will be sorted
    barwidth : float
        The proportion of sector to opening in the polar plot
    normed : bool
        Normalise the values in each bin (frequency vs. count)
    title : str or None
        Adds a title to the plot.
    unit : str or None
        Adds a unit string to the color-bar associated with the heatmap.
    cmap : str or None
        A Matplotlib valid colormap name. An up-to-date list of values is available from https://github.com/matplotlib/matplotlib/blob/master/lib/matplotlib/_color_data.py.
    tone_color : str or None
        Text and border colours throughout the plot.
    invert_y : bool
        Reverse the y-axis so that 0-24 hours runs from top to bottom.
    transparency : bool
        Sets transparency of saved plot.

    Returns
    -------
    imagePath : str
        Path to the saved image
    """

    ################
    # Input checks #
    ################

    assert len(directions) == len(values), \
        "Length of \"directions\" and \"values\" inputs do not match"

    assert max(directions) <= 360, \
        "\"directions\" contains values greater than 360"

    if np.isnan(directions).any():
        warnings.warn("\"directions\" contains non-numeric data. The corresponding index for \"values\" will be removed")

    if np.isnan(values).any():
        warnings.warn("\"values\" contains non-numeric data. The corresponding index for \"directions\" will be removed")

    mask = ~np.isnan(np.stack([directions, values]).T).any(axis=1)
    direction = directions[mask]
    value = values[mask]

    if bins is None:
        bins = np.linspace(values.min(), values.max(), 11)

    ###################
    # Data processing #
    ###################

    dir_bins = np.linspace(0, 360, (ndirections * 2) + 1)
    var_bins = np.array(bins).tolist()

    # Create 2d histogram of values
    table = np.histogram2d(x=values, y=directions, bins=[var_bins, dir_bins], normed=normed)[0]

    # Add last value to first value (for North angle winds)
    table[:, 0] = table[:, 0] + table[:, -1]
    table = table[:, :-1]

    # Roll the sum of pairs of winds to get total for each direction angle
    a = table[:, 0].reshape(len(bins) - 1, 1)
    b = table[:, 1:][:, ::2] + table[:, 1:][:, 1::2]
    actual_table = np.hstack([a, b])

    print(actual_table.sum().sum())

    # Determine bar ends for plotting in polar axes
    bottoms = actual_table.cumsum(axis=0) - actual_table
    tops = actual_table.cumsum(axis=0)

    ###############################
    # Associated data preparation #
    ###############################

    # Create colors to apply to bins
    colors = np.array([mpl.cm.get_cmap(cmap)(i) for i in np.linspace(0.0, 1.0, len(bins) - 1)])

    # Create bar theta angle width
    dtheta = barwidth * (np.radians(360) / ndirections)

    # Create angles for each direction to be plotted
    thetas = np.arange(0, np.radians(360), np.radians(360) / ndirections)

    ###################
    # Plot population #
    ###################

    fig, ax = plt.subplots(1, 1, figsize=(8, 8), subplot_kw={'projection': "polar"})

    labels = []
    handles = []
    for _bin in range(len(bins) - 1):
        for n, theta in enumerate(thetas):
            ax.bar(theta, actual_table[_bin][n], width=dtheta, bottom=bottoms[_bin][n], color=colors[_bin], alpha=1)
        labels.append("{0:0.2f} - {1:0.2f}".format(bins[_bin], bins[_bin + 1]))
        handles.append(mpl.patches.Rectangle([1, 1], 1, 1, facecolor=colors[_bin], edgecolor=None))

    ###################
    # Plot formatting #
    ###################

    # axes
    ax.set_theta_zero_location("N")
    ax.set_theta_direction(-1)
    ax.spines['polar'].set_visible(False)
    ax.grid(color=tone_color, alpha=0.5, linestyle=":")
    ax.tick_params(axis='both', colors=tone_color)
    if normed:
        ax.yaxis.set_major_formatter(mpl.ticker.PercentFormatter(0.1))
    ax.set_thetagrids(angles=np.arange(0, 360, 45), labels=["N", "NE", "E", "SE", "S", "SW", "W", "NW"], color=tone_color)

    # legend
    lgd = ax.legend(handles, labels, bbox_to_anchor=(1.1, 0.5), loc='center left', frameon=False, title=unit)
    lgd.get_frame().set_facecolor((1, 1, 1, 0))
    [plt.setp(text, color=tone_color) for text in lgd.get_texts()]
    plt.setp(lgd.get_title(), color=tone_color)

    # Add title if provided
    if title is not None:
        plt.title(title, color=tone_color, loc="center", ha="center", va="bottom")

    # Tidy plot
    plt.tight_layout()

    # Save figure
    fig.savefig(save_path, bbox_inches="tight", dpi=300, transparent=transparency)

    return save_path

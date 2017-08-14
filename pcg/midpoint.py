""" A python implementation of midpoint displacement algorithms

.. moduleauthor:: Richard Hallett

This module offers the ability to create fractals based upon the midpoint
displacement algorithm either in 1 or 2 dimensions.

"""

import random


def gen_1d_fractal(size, roughness, seed):
    """ Generate data for a 1d random fractal using midpoint displacement

    size - Power of 2 size for the data
    roughness - As name suggests determine how rough the data is,
    higher will be smoother
    seed - Seed for the random number generator

    """
    data = [0.0] * \
        (size + 1)  # Create the data array for 2 segments per vertice

    random.seed(seed)

    # This is the ratio used to calculate the scale at which the random number
    # range is reduced by
    ratio = pow(2.0, -roughness)
    # The scale is what we use to reduce the random number range
    scale = 1.0 * ratio

    step = size // 2
    while step:
        # Each point between two segments calculate the midpoint and displace
        i = step
        while i < size:
            start = i - step  # Start of the segment
            end = i + step  # End of the segment

            # The midpoint of the two segment values (the average)
            midpoint = (data[start] + data[end]) * 0.5
            displacement = random.random() * scale

            # Set the midpoint modified by the displacement
            data[i] = displacement + midpoint

            # Skip the next midpoint as it was done in the previous step
            i += step * 2

        # Reduce random number range
        scale *= ratio

        # After we've calculated (and created new segments) we half the step
        # as there will now be more segments
        step //= 2

    return data


def gen_2d_fractal(size, roughness, seed):
    """ Generated 2d fractal using diamond square

    size - Power of 2 size for the data
    roughness - As name suggests determine how rough the data is,
    higher will be smoother
    seed - Seed for the random number generator

    """
    vertices_size = size + 1
    # Create the data array for 2 segments per vertice
    data = [0.0] * (vertices_size * vertices_size)

    data[0] = 0.5
    data[vertices_size * size] = 0.5
    data[(vertices_size * size) + size] = 0.5
    data[size] = 0.5

    random.seed(seed)

    # This is the ratio used to calculate the scale at which the random number
    # range is reduced by
    ratio = pow(2.0, -roughness)
    scale = ratio  # The scale is what we use to reduce the random number range

    step = size // 2
    while step:

        # Diamond step
        i = step
        while i < size:
            j = step
            while j < size:

                index = (i * vertices_size) + j

                # Calculate the 4 corners of the square
                bl_corner = ((i - step) * vertices_size) + j - step
                br_corner = ((i - step) * vertices_size) + j + step
                tl_corner = ((i + step) * vertices_size) + j - step
                tr_corner = ((i + step) * vertices_size) + j + step

                # Calculate the average of the 4 corners
                midpoint = (data[bl_corner] + data[br_corner] +
                            data[tl_corner] + data[tr_corner]) * 0.25

                # The displacement to modify the midpoint by
                displacement = random.uniform(-1.0, 1.0) * scale

                # Set the midpoint modified by the displacement

                if data[index] + (displacement + midpoint) > 1:
                    data[index] = 1
                elif data[index] + (displacement + midpoint) < 0:
                    data[index] = 0
                else:
                    data[index] = displacement + midpoint

                j += step * 2

            i += step * 2

        # Square Step
        oddline = 0
        i = 0
        while i < size:
            j = 0
            oddline = (oddline == 0)
            while j < size:
                if((oddline) and not j):
                    j += step
                # Index we are changing
                index = (i * vertices_size) + j

                # Calculate the average of the 4 corners
                left, top, right, bottom = 0, 0, 0, 0
                if i == 0:
                    left = (i * vertices_size) + j - step
                    right = (i * vertices_size) + j + step
                    top = ((size - step) * vertices_size) + j
                    bottom = ((i + step) * vertices_size) + j
                elif j == 0:
                    top = (i - step) * vertices_size + j
                    bottom = (i + step) * vertices_size + j
                    right = (i * vertices_size) + j + step
                    left = (i * vertices_size) + size - step
                else:
                    top = ((i - step) * vertices_size) + j
                    bottom = ((i + step) * vertices_size) + j
                    right = (i * vertices_size) + j - step
                    left = (i * vertices_size) + j + step

                midpoint = (data[left] + data[right] +
                            data[top] + data[bottom]) * 0.25

                # The displacement to modify the midpoint by
                displacement = random.uniform(-1.0, 1.0) * scale

                if data[index] + (displacement + midpoint) > 1:
                    data[index] = 1
                elif data[index] + (displacement + midpoint) < 0:
                    data[index] = 0
                else:
                    data[index] = displacement + midpoint

                if i == 0:
                    data[(size * vertices_size) + j] = data[index]
                if j == 0:
                    data[(i * vertices_size) + size] = data[index]

                j += step * 2

            i += step

        # Reduce random number range
        scale *= ratio

        # After we've calculated (and created new segments) we half the step
        # as there will now be more segments
        step //= 2

    return data


def create_1d_image(image_name, roughness):
    from PIL import Image
    w = 512
    h = 180

    # Generate the fractal data
    height_data = gen_1d_fractal(w, roughness, 1234)

    image = Image.new('L', (w + 1, h), 255)

    # Lets fill in each pixel of the image
    data = []
    for y in range(h):
        for x in range(w + 1):
            # We normalise to the size of the image and the roughness so it
            # doesn't go out of bounds.
            dy = int(h - (height_data[x] * h * roughness))
            if y < dy:
                p = 255
            else:
                p = 128

            data.append(p)

    # Save the image
    image.putdata(data)
    image.save(image_name + '.png', "PNG")


def create_2d_image(image_name, roughness):
    from PIL import Image
    w = 512
    h = 512

    # Generate the fractal data
    fractal_data = gen_2d_fractal(w, roughness, 1234)

    image = Image.new('L', (w + 1, h + 1), 255)

    # Build image data
    data = []
    for x in range(w + 1):
        for z in range(h + 1):
            height = fractal_data[(x * (w + 1)) + z]
            y = abs(height * 255)

            data.append(y)

    # Save the image out
    image.putdata(data)
    image.save(image_name + '.png', "PNG")


if __name__ == '__main__':
    print("Generating example 1d fractals...")

    create_1d_image('1d-midpoint-smooth', 1.0)
    create_1d_image('1d-midpoint-rough', 0.5)
    create_1d_image('1d-midpoint-jaggy', 0.3)

    create_2d_image('2d-midpoint-smooth', 1.0)
    create_2d_image('2d-midpoint-rough', 0.5)
    create_2d_image('2d-midpoint-jaggy', 0.3)

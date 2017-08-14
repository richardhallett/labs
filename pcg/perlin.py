""" A python implementation of Ken Perlin's classic noise

.. moduleauthor:: Richard Hallett

This module offers the ability to create perlin noise in
both 2 and 3 dimensions.
The code differs slightly to the original reference implementation,
mostly for readability.

Useful links to which I based this implementation from:
http://mrl.nyu.edu/~perlin/paper445.pdf - Ken's improved perlin paper
http://webstaff.itn.liu.se/~stegu/simplexnoise/simplexnoise.pdf - An excellent
paper that is mostly about simplex noise but it also explains perlin noise
fairly well.

"""
import random
import math


class PerlinNoise(object):
    def __init__(self, seed):
        # This allows us to randomise the result but always get exactly the
        # same expected outcome (assuming the same random number generator
        # under the hood)
        random.seed(seed)

        # 12 vectors define the directions from the centre point of a cube
        # to the edges, however we actually expand this to 16 gradients
        # to make a regular tetrahedron.
        # This is because in perlins improved noise he says that being able to
        # directly hash into a 4 bit value will be faster on certain
        # hardware implementations.
        self.gradients = [
                         [1, 1, 0], [-1, 1, 0], [1, -1, 0], [-1, -1, 0],
                         [1, 0, 1], [-1, 0, 1], [1, 0, -1], [-1, 0, -1],
                         [0, 1, 1], [0, -1, 1], [0, 1, -1], [0, -1, -1],
                         [1, 1, 0], [-1, 1, 0], [0, -1, 1], [0, -1, -1]
        ]

        # The permutation table is indexed to lookup what gradient to choose
        # at the lattice point of a grid.
        # Perlin described using a permutation table to limit the memory
        # requirements of having a larger grid of points for
        # increasing dimensions i.e. 3d would be 256^3
        # This works by the fact we always ensure our input to noise can map
        # to one value in this table.
        # Also 256 is a suitable good number for pleasant randomness in the
        # gradients we get, it could be less or more but this works out
        # pretty good.
        # We double the table at the end to avoid us having to write code to
        # deal with index wrapping i.e. 256 being 0 and so on.
        self.p = list(range(0, 256))
        random.shuffle(self.p)
        self.p *= 2

    def lerp(self, a, b, t):
        """ Linear interpolate between a and b over t """
        return (1 - t) * a + t * b

    def fade(self, t):
        """ Fifth degree polynomial curve over t"""
        return t * t * t * (t * (t * 6 - 15) + 10)

    def grad2(self, x, y, i, j):
        """ Calculate the scalar product from contributions of the
        relative coordiates with the selected gradient vector from
        the containing point"""
        # We hash our coordinate from the permutation table to get our index
        # into the gradient vectors
        gi = self.p[i + self.p[j]] & 15

        # With the gradient vector now found, combine with the relative
        # coordiates to get the total contribution (dot product)
        p = self.gradients[gi][0] * x + self.gradients[gi][1] * y
        return p

    def grad3(self, x, y, z, i, j, k):
        """ Calculate the scalar product from contributions of the relative
        # coordiates with the selected gradient vector from the
        containing point"""
        # We hash our coordinate from the permutation table to get our index
        # into the gradient vectors
        gi = self.p[i + self.p[j + self.p[k]]] & 15

        # With the gradient vector now found, combine with the relative
        # coordiates to get the total contribution (dot product)
        p = self.gradients[gi][0] * x + \
            self.gradients[gi][1] * y + self.gradients[gi][2] * z
        return p

    def noise2(self, x, y):
        """ Generate 2d noise for coordinates x,y """
        # For the input values get which point on the grid we're in.
        i = int(math.floor(x))
        j = int(math.floor(y))

        # Normalise the input values so we get coordinate pair between
        # lattice points
        x = x - i
        y = y - j

        # Apply smoothing to the coordinates
        u = self.fade(x)
        v = self.fade(y)

        # Calculate scalar product from our gradients and relative grid
        # coordinates
        n00 = self.grad2(x, y, i, j)
        n10 = self.grad2(x - 1, y, i + 1, j)
        n01 = self.grad2(x, y - 1, i, j + 1)
        n11 = self.grad2(x - 1, y - 1, i + 1, j + 1)

        # Linearly interpolate along the x axis
        nx00 = self.lerp(n00, n10, u)
        nx01 = self.lerp(n01, n11, u)

        # Linearly interpolate along the y axis
        # This is our result
        return self.lerp(nx00, nx01, v)

    def noise3(self, x, y, z):
        """ Generate 2d noise for coordinates x,y,z """

        # For the input values get which point on the grid we're in.
        i = int(math.floor(x))
        j = int(math.floor(y))
        k = int(math.floor(z))

        # Normalise the input values so we get coordinate pair between
        # lattice points
        x = x - i
        y = y - j
        z = z - k

        # Apply smoothing to the coordinates
        u = self.fade(x)
        v = self.fade(y)
        w = self.fade(z)

        # Calculate scalar product from our gradients and relative cube
        # coordinates
        n000 = self.grad3(x, y, z, i, j, k)
        n100 = self.grad3(x - 1, y, z, i + 1, j, k)
        n010 = self.grad3(x, y - 1, z, i, j + 1, k)
        n110 = self.grad3(x - 1, y - 1, z, i + 1, j + 1, k)
        n001 = self.grad3(x, y, z - 1, i, j, k + 1)
        n101 = self.grad3(x - 1, y, z - 1, i + 1, j, k + 1)
        n011 = self.grad3(x, y - 1, z - 1, i, j + 1, k + 1)
        n111 = self.grad3(x - 1, y - 1, z - 1, i + 1, j + 1, k + 1)

        # Linearly interpolate along the x axis
        nx00 = self.lerp(n000, n100, u)
        nx01 = self.lerp(n001, n101, u)
        nx10 = self.lerp(n010, n110, u)
        nx11 = self.lerp(n011, n111, u)
        # Linearly interpolate along the y axis
        nxy0 = self.lerp(nx00, nx10, v)
        nxy1 = self.lerp(nx01, nx11, v)
        # Linearly interpolate along the z axis
        # Result found
        return self.lerp(nxy0, nxy1, w)


#
# Following is some examples of how to use the noise
#

def simple_noise(n, x, y):
    return n.noise2(x, y)


def fbm(n, x, y):
    """ Produce a fractal sum of noise, sum(1/f(noise)) """
    frequency = 1.0
    amp = 1.0
    value = 0.0  # reset the value for the next point
    # Octaves, the number of times we should sum(higher is more detail)
    for i in range(6):
        value += n.noise2(x * frequency, y * frequency) * amp
        amp *= 0.5
        frequency *= 2.0

    return value


def turbulence(n, x, y):
    """ Produce a fractal sum of absolute noise, sum(1/f(|noise|)) """
    frequency = 1.0
    amp = 1.0
    value = 0.0  # reset the value for the next point
    maxValue = 0.0
    # Octaves, the number of times we should sum (higher is more detail)
    for i in range(6):
        maxValue += amp
        value += math.fabs(n.noise2(x * frequency, y * frequency)) * amp
        amp *= 0.5
        frequency *= 2.0

    return value


def marble(n, x, y):
    """ Perlin function to produce marble like effect """
    return math.sin(x + turbulence(n, x, y))


def marble2(n, x, y):
    """ Perlin function to produce marble like effect """
    return (math.sin(x * 4 + fbm(n, x * 0.5, y * 0.5) * 4) + 1) * 0.5


def wood(n, x, y):
    """ Perlin function to produce wood like effect """
    g = n.noise2(x, y) * 10
    return g - int(g)


def create_noise_image(image_name, noise_func):
    """ Create a 2d image representing the perlin noise """
    from PIL import Image

    size = 256  # Size we want the image to be
    # This is the number we want to scale our coordinates by.
    scale = (1.0 / size) * 8.0
    n = PerlinNoise(232)  # We seed the permutation table

    image = Image.new('L', (size, size), 255)
    data = []
    for px in range(size):
        for py in range(size):
            x = (px * scale)
            y = (py * scale)

            value = noise_func(n, x, y)

            # Put it in the range 0-1 (mostly just for putting into the image)
            value = (1 + value) / 2
            data.append(value)

    # Take the values and put them in the 0-255 range for colours
    # then throw at the image.
    image.putdata([int(v * 255) for v in data])

    image.save(image_name + '.png', "PNG")


if __name__ == '__main__':
    print("Making some noise...")

    create_noise_image("straightPerlinNoise", simple_noise)
    create_noise_image("fbm", fbm)
    create_noise_image("turbulence", turbulence)
    create_noise_image("marble", marble)
    create_noise_image("marble2", marble2)
    create_noise_image("wood", wood)

import pyglet
import random
import csv

class Grid(object):
    """ A 2 dimensional grid of cells that have a state"""

    def __init__(self, width, height, states = [0, 1], default_state_index = 0):
        self.width = width
        self.height = height
        self.states = states
        self.default_state_index = default_state_index
        self.cells = [states[default_state_index]] * (width*height) 

    def save(self, filename):        
        """Write out grid state to a file"""
        with open(filename, 'w') as f:
            writer = csv.writer(f, delimiter=',', quoting=csv.QUOTE_MINIMAL)
            writer.writerow(self.cells)

    def read(self, filename):
        """Read in grid state from a file"""
        with open(filename, 'r') as f:
            reader = csv.reader(f, delimiter=',', quoting=csv.QUOTE_MINIMAL)

            row = next(reader)
            self.cells = [int(s) for s in row]

    def randomise(self):
        """ Fill the game board with random state """
        for x in range(self.width):
            for y in range(self.height):
                self.set_cell(x, y, random.choice(self.states))

    def get_cell(self, x, y):        
        """ Get the state of a cell from the grid """
        x = x % self.width
        y = y % self.height

        index = x + y * self.width
        return self.cells[index]

    def set_cell(self, x, y, state):
        """ Set the cell of the grid to a new state """
        index = int(x + y * self.width)
        self.cells[index] = state
     
    def get_neighbours(self, x, y):
        """ Get all the neighbours for a grid cell according 
            This implementation is a Moore Neighborhood, i.e. eight cells around a central cell.
        """
        neighbours = [self.get_cell(x+1, y),
                      self.get_cell(x-1, y),
                      self.get_cell(x, y+1),
                      self.get_cell(x, y-1),
                      self.get_cell(x+1, y+1),
                      self.get_cell(x-1, y+1),
                      self.get_cell(x+1, y-1),
                      self.get_cell(x-1, y-1)]

        return neighbours

    def clear(self):
        self.cells = [self.states[self.default_state_index] for c in self.cells]

    def __getitem__(self, index):
        """ Return a specific cell from an index """
        return self.cells[index]

    def __setitem__(self, index, value):
        """ Set an item of the grid to a specific value, can be slices"""
        self.cells[index] = value

    def __len__(self):
        """ The total number of cells in the grid """
        return len(self.cells)

class GridRenderer():
    """ Renders a 2d grid with it's cells to a specific colour as defined by the colour map"""

    def __init__(self, width, height, cell_colour_map = {0: (0,0,0,255), 1: (255, 255, 255, 255)}, line_colour = (96, 96, 96, 255), grid_size=16):                
        self.width = width
        self.height= height
        self.cell_colour_map = cell_colour_map
        
        self.draw_grid_lines = True
        self.height_vert = self.height + 1
        self.width_vert = self.width + 1
        
        # Grid vertices
        grid_vertices = []                
        for y in range(self.height_vert):
            for x in range(self.width_vert):                                              
                px = x * grid_size
                py = y * grid_size                
                grid_vertices += [px, py]   
        
        grid_indices = []                  
        cell_vertices = []
        for y in range(self.height):
            for x in range(self.width):                  
                px = x * grid_size
                py = y * grid_size              
                
                # Lots of vertices for the triangles       
                cell_vertices += [px, py, px, py + grid_size, px + grid_size, py]   
                cell_vertices += [px + grid_size, py, px, py + grid_size, px + grid_size, py + grid_size]

                # Indices to make the grid lines
                vindex = x + y * self.width_vert                
                grid_indices += [vindex, vindex + 1]       
                grid_indices += [vindex + self.width_vert, vindex + self.width_vert + 1]
                grid_indices += [vindex, vindex + self.width_vert]
                grid_indices += [vindex + 1, vindex + self.width_vert + 1]  

        half_grid_vertices = int(len(grid_vertices) / 2)
        half_cell_vertices = int(len(cell_vertices) / 2)
        self.grid_vbo = pyglet.graphics.vertex_list_indexed(half_grid_vertices, grid_indices, ('v2i/static', grid_vertices), ('c4B/dynamic', line_colour * half_grid_vertices))
        self.cells_vbo = pyglet.graphics.vertex_list(half_cell_vertices, ('v2i/dynamic', cell_vertices), ('c4B/dynamic', [255, 255, 255, 0] * half_cell_vertices))              

    def update(self, grid): 
        """ This updates the colours of internal grid representation with the grid passed in, either making the cells visible or not """        
        # We update the colours of the cells to effectivly make them visible or not.
        new_colours = []
        for i, cell_state in enumerate(grid):      
            vert_colours = self.cell_colour_map[int(cell_state)] * 6

            start = i * 24
            end = start + 24
            new_colours[start:end] = vert_colours

        self.cells_vbo.colors = new_colours

    def draw(self):         
        """ Draw the grid and it's cells """     
        self.cells_vbo.draw(pyglet.gl.GL_TRIANGLES)        
        if self.draw_grid_lines:   
            self.grid_vbo.draw(pyglet.gl.GL_LINES)    
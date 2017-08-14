""" A python implementation of John Conway's Game of life and other cellular automaton

This is not designed to be highly performant, more as a demonstration of the basic logic.

.. moduleauthor:: Richard Hallett

"""

from grid import Grid, GridRenderer

import pyglet
pyglet.options['shadow_window'] = False # Pyglet decides for (imo) silly reasons to create the opengl context in a seperate shadow window 
                                        # so it's available before you actually create a window, this causes some crashes on certain cards                                       
import random

class CellularAutomata():
    """ Base class for Cellular Automaton """
    def tick(self):        
        new_cells = self.grid.cells[:] # We make a copy of the grid cells that we are going to change

        for x in range(self.grid.width):
            for y in range(self.grid.height):    
                new_state = self.rule(x, y) # Using the rules from the simulation get a new state for this cell
                index = x + y * self.grid.width
                new_cells[index] = new_state

        self.grid.cells = new_cells # Old state becomes the new state

class Conways(CellularAutomata):
    """ An implementation of Conways game of life """

    def __init__(self, width, height):
        self.active = True # Is the game running or not.
        self.grid = Grid(width, height)
        self.grid.randomise() # Start with some randomness

    def rule(self, x, y):
        cell_state = self.grid.get_cell(x, y) # The cell we are currently looking at
        new_state = cell_state # This will become our new state depending on below logic, we'll start it with the same state as it was
        neighbours = self.grid.get_neighbours(x, y) # The neighbours for this cell
        live_neighbours = len([n for n in neighbours if n == 1]) # The amount of live neighbours for this cell
                            
        if cell_state == 1:
            # Live cells
            if live_neighbours < 2:
                # Any live cell with fewer than two live neighbours dies, as if caused by under-population.
                new_state = 0
            elif live_neighbours == 2 or live_neighbours == 3:
                # Any live cell with two or three live neighbours lives on to the next generation.
                new_state = 1
            elif live_neighbours > 3:
                # Any live cell with more than three live neighbours dies, as if by overcrowding.
                new_state = 0
        else:
            # Dead cells
            if live_neighbours == 3:
                # Any dead cell with exactly three live neighbours becomes a live cell, as if by reproduction.
                new_state = 1

        return new_state
  
class Seeds(CellularAutomata):
    """ An implementation of the cellular automaton named Seeds """
    def __init__(self, width, height):
        self.width = width
        self.height = height
        self.grid = Grid(width, height)        
        
        centre_x, centre_y = width / 2, height / 2
        self.grid.set_cell(centre_x, centre_y, True)
        self.grid.set_cell(centre_x + 1, centre_y, True)
        self.grid.set_cell(centre_x, centre_y + 1, True)

    def rule(self, x, y):
        cell_state = self.grid.get_cell(x, y) # The cell we are currently looking at        
        neighbours = self.grid.get_neighbours(x, y) # The neighbours for this cell          

        new_state = cell_state # This will become our new state depending on below logic, we'll start it with the same state as it was
        live_neighbours = len([n for n in neighbours if n == 1]) # The amount of live (i.e. boolean true) neighbours for this cell
                                    
        if cell_state == 0: # If the cell is dead
            if live_neighbours == 2: # And it had two living neighbours
                new_state = 1 # Then bring it to life (born)
        else:
            # All other cells are off (dead)
            new_state = 0

        return new_state

class BriansBrain(CellularAutomata):
    """ An implementation of the cellular automata Brian's Brain """
    def __init__(self, width, height):
        self.width = width
        self.height = height
        states = [0, 1, 2] # 0 = Dead, 1 = Alive, 2 = Dying
        self.grid = Grid(width, height, states = states)                
        self.grid.randomise() # Start with some randomness

    def rule(self, x, y):
        cell_state = self.grid.get_cell(x, y) # The cell we are currently looking at        
        neighbours = self.grid.get_neighbours(x, y) # The neighbours for this cell          

        new_state = cell_state # This will become our new state depending on below logic, we'll start it with the same state as it was
        live_neighbours = len([n for n in neighbours if n == 1]) # The amount of live neighbours for this cell
                          
        if cell_state == 0: # If the cell is dead
            if live_neighbours == 2: # And it had two living neighbours
                new_state = 1 # Then bring it to life (born)
        elif cell_state == 1:
            # All currently alive cells start to die
            new_state = 2
        elif cell_state == 2:
            # All dying cells become dead.
            new_state = 0

        return new_state

class CaveGeneration(CellularAutomata):
    """ An example of using cellular automata to generate cave systems """
    def __init__(self, width, height):
        self.width = width
        self.height = height
        self.grid = Grid(width, height)                
        self.grid.randomise() # Start with some randomness

    def rule(self, x, y):
        cell_state = self.grid.get_cell(x, y)
        neighbours = self.grid.get_neighbours(x, y)

        new_state = cell_state
        live_neighbours = len([n for n in neighbours if  n == 1])
             
        # B678/S345678      
        if cell_state == 0:
            if live_neighbours in [6,7,8]:
                new_state = 1
        else:
            if not live_neighbours in [3,4,5,6,7,8]:
                new_state = 0

        return new_state

class Game():
    """ General handler for running cellular automata games """
    def __init__(self, width, height, grid_size=8):
        self.width = width
        self.height = height
        self.active = True
        self.simulation = None
        self.grid_renderer = GridRenderer(width, height, grid_size=grid_size)

    def tick(self):
        """ Main update function for the game """
        if self.simulation:
            self.simulation.tick()

    def draw(self):
        """ Draw the current game grid """
        if self.simulation:
            self.grid_renderer.update(self.simulation.grid)
            self.grid_renderer.draw()

    def run_sim(self, name):
        """ Choose a simulation based upon a name to run """
        self.simulation = None

        if name == 'conways':
            self.simulation = Conways(self.width, self.height)
            self.grid_renderer.cell_colour_map = {
                                                  0: (3,80,150,255), 
                                                  1: (125, 249, 255, 255)
                                                  }
        elif name == 'seeds':
            self.simulation = Seeds(self.width, self.height)
            self.grid_renderer.cell_colour_map = {
                                                  0: (3,80,150,255), 
                                                  1: (125, 249, 255, 255)
                                                  }
        elif name == 'briansbrain':
            self.simulation = BriansBrain(self.width, self.height)
            self.grid_renderer.cell_colour_map = {
                                                  0: (3,80,150,255), 
                                                  1: (255, 255, 255, 255),
                                                  2: (125, 249, 255, 255),
                                                  }
        elif name == 'cave':
            self.simulation = CaveGeneration(self.width, self.height)
            self.grid_renderer.cell_colour_map = {
                                                  0: (3,80,150,255), 
                                                  1: (125, 249, 255, 255)
                                                  }

def main():       
    width = 800
    height = 600
    window = pyglet.window.Window(width, height, vsync=False)

    scale = 8
    grid_width = int(width / scale)
    grid_height = int(height / scale)

    game = Game(grid_width, grid_height, grid_size=scale)
    game.run_sim('conways')

    @window.event
    def on_key_press(symbol, modifiers):
        if symbol == pyglet.window.key.F1:
            game.active = not game.active

        if symbol == pyglet.window.key.F2:
            game.simulation.grid.clear()

        if symbol == pyglet.window.key.F3:
            game.simulation.grid.randomise()

        if symbol == pyglet.window.key.F4:
            game.grid_renderer.draw_grid_lines = not game.grid_renderer.draw_grid_lines

        if symbol == pyglet.window.key.F5:
            game.simulation.grid.save('saved_state.txt')

        if symbol == pyglet.window.key.F6:
            game.simulation.grid.read('saved_state.txt')

        if symbol == pyglet.window.key._1:
            game.run_sim('conways')

        if symbol == pyglet.window.key._2:
            game.run_sim('seeds')
        
        if symbol == pyglet.window.key._3:
            game.run_sim('briansbrain')

        if symbol == pyglet.window.key._4:
            game.run_sim('cave')

        if symbol == pyglet.window.key.G:
            game.simulation.grid.read('glider_gun.txt')

    @window.event
    def on_mouse_press(x, y, button, modifiers):        
        # If the game isn't running allow user to manually fill the grid.
        if not game.active:
            cell_x = x / scale
            cell_y = y / scale
            states = game.simulation.grid.states[:]
            current_state = game.simulation.grid.get_cell(cell_x, cell_y)
            current_index = states.index(current_state) # Get the current index in the grid states

            states = states[current_index+1:] + states[:current_index+1] # Rotate the list of states starting from the next one on from the current state

            # Set the cell to be whatever the next state index would be after we've rotated.
            game.simulation.grid.set_cell(cell_x, cell_y, states[0])

    @window.event
    def on_draw():
        window.clear()                
        game.draw()
    
    def update(dt):         
        # Step the game on  
        if game.active:
            game.tick()

    # Call main update every 1000th of a second so we can see things progressing easier.
    pyglet.clock.schedule_interval(update, 0.1)
    pyglet.app.run()

if __name__ == '__main__':
    main()
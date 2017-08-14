gameoflife
==========

This is primarily an implementation in python of John Conway's Game of Life see: http://en.wikipedia.org/wiki/Conway's_Game_of_Life
In addition it includes other celular automata that are also interesting.

## Requirements

	* Python 3 or newer
	* Pyglet 1.2

Run 'pip install -r requirements.txt'

## Controls
	* F1 - Pauses the update tick for the game.
	* F2 - Clears the grid.
	* F3 - Randomises the grid.
	* F4 - Toggle grid lines.
	* F5 - Save grid state.
	* F6 - Read grid state.
	* 1 - Switch to Conway's Game of life simulation.
	* 2 - Switch to the Seeds simulation.
	* 3 - Switch to the Brian's Brain simulation.
	* 4 - Switch to the Cave generation simulation.
	* G - Load the premade glider gun pattern.
	* Mouse click - Toggles through the various states for the current simulations on target cell, only works when the game is paused.
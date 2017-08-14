# Generates random names using markov chains

import random


class RandomWordGenerator():
    """Generates random words based upon a dataset."""

    def __init__(self, dataset, chain_length=2):
        """Setup the generator based upon the passed in dataset.

        Keyword arguments:
        chain_length -- the length of letters that are chained together when
        looking for their suffixes, as it increases the amount of different
        possibilities will decrease.
        """

        # This class is based upon the markov chain algorithm, it matches up
        # chained letters (of a certain specified size)
        # against possible suffixes that they can have, this gets done by
        # analyzing the dataset.

        self.chain_length = chain_length  # The size to group letters by.
        # Holds a list of potential suffixes against a chained letters
        self._markov_table = {}

        for word in dataset:
            # Each chain of letters that occur we add their following value.
            # Effectivly the list stored against one value is what is most
            # likely to occur after that value.
            for i in range(len(word) - self.chain_length):
                key = word[i:i + chain_length]
                if key not in self._markov_table:
                    self._markov_table[key] = []
                self._markov_table[key].append(word[i + chain_length])

    def generate(self, max_length=8):
        """Generate a random word

        Keyword arguments:
        max_length -- The maximum length a word can be.

        """
        word = random.choice(list(self._markov_table.keys()))

        # Build the word a suffix at a time until either there is no suggestion
        # for a suffix
        # or the word length has been reached.
        while True:
            key = word[-self.chain_length:]
            if key not in self._markov_table or len(word) >= max_length:
                break
            else:
                # A random suffix based that can occur after the letter chain
                suffix = random.choice(self._markov_table[key])
                word += suffix

        return word


if __name__ == '__main__':
    # Dataset from http://www.ruf.rice.edu/~pound/#sfng
    female_firstnames = "Aimee Aleksandra Alice Alicia Allison Alyssa Amy Andrea Angel Angela \
    Ann Anna Anne Anne Marie Annie Ashley Barbara Beatrice Beth Betty \
    Brenda Brooke Candace Cara Caren Carol Caroline Carolyn Carrie \
    Cassandra Catherine Charlotte Chrissy Christen Christina Christine \
    Christy Claire Claudia Courtney Crystal Cynthia Dana Danielle Deanne \
    Deborah Deirdre Denise Diane Dianne Dorothy Eileen Elena Elizabeth \
    Emily Erica Erin Frances Gina Giulietta Heather Helen Jane Janet Janice \
    Jenna Jennifer Jessica Joanna Joyce Julia Juliana Julie Justine Kara \
    Karen Katharine Katherine Kathleen Kathryn Katrina Kelly Kerry Kim \
    Kimberly Kristen Kristina Kristine Laura Laurel Lauren Laurie Leah \
    Linda Lisa Lori Marcia Margaret Maria Marina Marisa Martha Mary Mary \
    Ann Maya Melanie Melissa Michelle Monica Nancy Natalie Nicole Nina \
    Pamela Patricia Rachel Rebecca Renee Sandra Sara Sharon Sheri Shirley \
    Sonia Stefanie Stephanie Susan Suzanne Sylvia Tamara Tara Tatiana Terri \
    Theresa Tiffany Tracy Valerie Veronica Vicky Vivian Wendy"

    rnd_names = RandomWordGenerator(
        female_firstnames.split(' '),
        chain_length=3)

    names = []
    for i in range(500):
        names.append(rnd_names.generate(random.randint(4, 8)).capitalize())

    # Unique Names
    print(set(names))

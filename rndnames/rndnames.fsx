let femaleFirstnames = 
    "Aimee Aleksandra Alice Alicia Allison Alyssa Amy Andrea Angel Angela \
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
    Theresa Tiffany Tracy Valerie Veronica Vicky Vivian Wendy".Split(' ')

let rnd = System.Random()

let rndChoice (arr: 'a[]) =    
    arr.[rnd.Next(arr.Length)]

// Construct a markov table for a given chain length from sequence of words
let toMarkov markovLength (words:string seq) =
    words
    |> Seq.map(fun word -> word.ToLower()) // Lowercase all the words so we don't end up with duplicates due to case
    |> Seq.collect(fun word -> // Each word
        [0..word.Length-markovLength-1] // For all letters in the word up to the letter chain length.
        |> Seq.map (fun i -> (word.Substring(i, markovLength), word.Substring(i+markovLength, 1))) // Map each letter in the word to it's next possible letter chain.
    )
    |> Seq.groupBy(fst) // Group the possible chains by letter.
    |> Seq.map(fun (key, values) -> key, values // Map to new key, values
                                         |> Seq.map(snd) // where we remove the key from the letter chains for each key
                                         |> Seq.distinct // Ensure only unique letter chains    
                                         |> Seq.toArray)
    |> Map.ofSeq // Return a map collection for lookup


// Make a random word
let makeWord (markovTable: Map<string,string[]>) wordLength =   
    
    (* Function to construct a word recursively *) 
    let rec build (wordLength:int) (prevKey:string) (newWord:string) = 
        match newWord.Length = wordLength with // Up to desired length only
        | true ->
            newWord
        | false ->
            let key = newWord.Substring(newWord.Length-prevKey.Length)
            let markov = markovTable.TryFind key // Key is the next letter in the sequence
            match markov with
            | Some suffix -> // Have a key, return the possible suffix
                suffix
                |> rndChoice // Random suffix
                |> (+) newWord // Append to the word
                |> build wordLength key // Keep going
            | None ->
                newWord // No key found, lets stop and return what we got

    let chainStart = markovTable |> Map.toArray |> rndChoice |> fst
    build wordLength chainStart chainStart

let testMarkov = toMarkov 3 femaleFirstnames

let randomNames = 
    [0..500]
    |> List.map(fun i -> makeWord testMarkov 8 |> fun x -> System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(x))    

printfn "%A" randomNames
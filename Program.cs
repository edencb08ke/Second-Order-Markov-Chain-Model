using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Second_Order_Markov_Chain_Model;

class Program
{
    static void Main(string[] args)
    {
        var midiProcessor = new MidiProcessor();
        var model = new MarkovChain();

        // load training data
        var corpus = new List<List<NoteTuple>>();
        string[] choraleFiles = Directory.GetFiles("ChoralesData/", "*.mid");

        foreach (var file in choraleFiles) 
        {
            var sequence = midiProcessor.LoadChorale(file);
            if (sequence.Count > 0) corpus.Add(sequence);
        }

        //train model
        model.Train(corpus);

        // generate new seq from seed
        if (corpus.Count > 0 && corpus[0].Count >= 2)
        {
            var seed1 = corpus[0][0];
            var seed2 = corpus[0][1];

            var outputSequence = model.Generate(seed1, seed2, length: 32);

            // save output
            midiProcessor.ExportToMidi(outputSequence, "GeneratedChorale.mid");
            Console.WriteLine("Generation complete: saved to GeneratedChorale.mid");
        }
    }
}
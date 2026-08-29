using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Second_Order_Markov_Chain_Model;

namespace Second_Order_Markov_Chain_Model;

public class MarkovChain
{
    private readonly Dictionary<KeyState2ndOrder, List<NoteTuple>> transitions = new();
    private readonly Random rng = new();

    public void Train(IEnumerable<List<NoteTuple>> corpus)
    {
        foreach (var sequence in corpus)
        {
            if (sequence.Count < 3) continue;

            for (int i = 0; i < sequence.Count - 2; i++)
            {
                var key = new KeyState2ndOrder(sequence[i], sequence[i + 1]);
                var nextState = sequence[i + 2];

                if (!transitions.TryGetValue(key, out var list))
                {
                    list = new List<NoteTuple>();
                    transitions[key] = list;
                }
                list.Add(nextState);
            }
        }
    }
    public List<NoteTuple> Generate(NoteTuple seed1, NoteTuple seed2, int length)
    {
        var result = new List<NoteTuple> { seed1, seed2 };

        for (int i = 2; i < length; i++)
        {
            var currentKey = new KeyState2ndOrder(result[i - 2], result[i - 1]);

            if (!transitions.TryGetValue(currentKey, out var options) || options.Count == 0)
            {
                // Dead end reached: no matching sequence found in training data
                break;
            }

            var nextChoice = options[rng.Next(options.Count)];
            result.Add(nextChoice);
        }
        return result;
    }
}

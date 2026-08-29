using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Second_Order_Markov_Chain_Model;

public class Models
{
    public readonly record struct NoteTuple(int Soprano, int Alto, int Tenor, int Bass); //SATB
    public readonly record struct KeyState2ndOrder(NoteTuple State1, NoteTuple State2); //Key for chain
}

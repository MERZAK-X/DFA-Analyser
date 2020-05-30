using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace DFA_Analyzer.Entities
{
    public class DFSA
    {
        #region Variables
        private int _startState, _statesNumber;
        private int []_finalStates;
        private string _alphabet;
        private Dictionary<int, Dictionary<char, int>> _transitionTable;
        #endregion

        #region Contructor & Factory
        public static DFSA CreateInstance(string filename)
        {
            var automaton = new DFSA(filename);
            return automaton;
        }
        
        private DFSA(string filename)
        {
            var lines = File.ReadAllLines(filename);
            _statesNumber = int.Parse(lines[0]);
            _alphabet = lines[1];
            _startState = int.Parse(lines[2]); // Get states from file
            var finalStatesAsString = lines[4].Split(' ');
            _finalStates = Array.ConvertAll(finalStatesAsString, int.Parse);
            _transitionTable = new Dictionary<int, Dictionary<char, int>>();
            for (var i = 5; i <= lines.Length -1; i++)
            {
                var state = int.Parse(lines[i].Split(' ')[0]);
                var symbol = char.Parse(lines[i].Split(' ')[1]);
                var nextState = int.Parse(lines[i].Split(' ')[2]);

                if (_transitionTable.ContainsKey(state)) {
                    Dictionary<char, int> res;   
                    _transitionTable.TryGetValue(state, out res);
                    res?.Add(symbol, nextState);
                    _transitionTable[state] = res;
                } else {
                    var dict = new Dictionary<char, int>();
                    dict.Add(symbol, nextState);
                    _transitionTable.Add(state, dict);
                }
            }
        }

        #endregion

        #region Functions & Methods

        public override string ToString()
        {
            #region Variables
            
            var automaton = "";
            var states = new List<int>();
            
            #endregion

            #region Fill States List

            foreach (var keyValuePair in this._transitionTable) // fill states list
                foreach (var pair in keyValuePair.Value)
                {
                    if (states.IndexOf(pair.Value) == -1)
                        states.Add(pair.Value);
                    if (states.IndexOf(keyValuePair.Key) == -1)
                        states.Add(keyValuePair.Key);
                }

            #endregion
        
            #region States {E}
            
            var iteration = 1;
            automaton += "E = {";
            foreach (var state in states)
            {
                automaton += state;
                if (iteration >= states.Count) continue;
                automaton += ", ";
                iteration++;
            }
            automaton += "}\n";
            
            #endregion
            
            #region Alphabet {A}
            
            iteration = 1;
            automaton += "A = {";
            foreach (var c in _alphabet)
            {
                automaton += c;
                if (iteration >= _alphabet.Length) continue;
                automaton += ", ";
                iteration++;
            }
            
            #endregion

            #region Transitions {σ}
            
            automaton += "}\nTransitions:";
            automaton = this._transitionTable
                .Aggregate(automaton, (current1, keyValuePair) 
                    => keyValuePair.Value
                    .Aggregate(current1, (current, pair) 
                        => current + $"\nδ({keyValuePair.Key}, {pair.Key}) = {pair.Value}"));

            #endregion

            #region Initial States {q₀}
            automaton += $"\nq₀ = {this._startState}\n";
            #endregion

            #region Final States {F}

            iteration = 1;
            automaton += "F = {";
            foreach (var finalState in _finalStates)
            {
                automaton += finalState;
                if (iteration >= _finalStates.Length) continue;
                automaton += ", ";
                iteration++;
            }
            automaton += "}\n";
            return automaton;
            
            #endregion
        }
        
        public void Print()
        {
            Console.WriteLine(this);
        }
        
        public bool Accept(string word)
        {
            var letters = word.ToCharArray();
            var state = this._startState;
            foreach (var a in letters)
            {
                var nextState = this.Σ(state, a);
                state = nextState;
            }
            return ((IList) this._finalStates).Contains(state);
        }
        
        private int Σ(int state, char symbol)
        {
            if (!_transitionTable.TryGetValue(state, out var transition)) return 0;
            if (!transition.ContainsKey(symbol)) return 0;
            var nextState = _transitionTable[state][symbol];
            return nextState;
        }

        #endregion
        
    }
}
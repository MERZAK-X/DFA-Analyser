﻿using System;
using DFA_Analyzer.Entities;

namespace DFA_Analyzer
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                DFSA automaton = DFSA.CreateInstance(args[0]);
                automaton.Print();
                var word = "0";
                while(word != "-1"){
                    Console.WriteLine("Enter a word (-1 to exit) : ");
                    word = Console.ReadLine();
                    var accepted = automaton.Accept(word);
                    Console.WriteLine($@"{((accepted) ? '\u2713' : '\u2717')} The word `{word}` is {((accepted) ? String.Empty : "NOT " )}accepted by the automaton's described language !");
                }
            }
            catch (Exception e)
            {
                // File not found or error occured
                Console.WriteLine(e.Message);
            }
        }
    }
}
using System;
using System.Collections.Generic;
//using System.Linq;

namespace TuringMachine
{
    class TuringImplementation
    {
        public string State { get; private set; }
        public int Position { get; private set; }
        public string Tape { get; private set; }
        public string NextCommand { get; private set; }
        private HashSet<string> States = new HashSet<string>();
        private HashSet<char> Alphabet = new HashSet<char>();
        private Dictionary<string, string> Commands = new Dictionary<string, string>();
        private string InternalState;
        public TuringImplementation(HashSet<string> states, string startState, HashSet<char> alphabet, string initialTape, string commands)
        {
            Position = 0;
            States = states;
            State = startState;
            Alphabet = alphabet;
            Tape = initialTape.Trim('0');
            string[] coms = commands.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var item in coms)
            {
                if (item.StartsWith("//"))
                {
                    continue;
                }
                string[] temp = item.Split(new string[] { "->" }, StringSplitOptions.None);
                Commands.Add(temp[0], temp[1]);
            }
            InternalState = Tape[Position] + startState;
        }
        public void Step()
        {
            string command = Commands[InternalState];
            if (!Alphabet.Contains(command[0]))
            {
                throw new Exception("I don't know that symbol");
            }
            Tape = Tape.Substring(0, Position) + command[0] + Tape.Substring(Position + 1, Tape.Length - Position - 1);
            command = command.Substring(1);

            string newState = "";
            foreach (var state in States)
            {
                if (command.StartsWith(state))
                {
                    newState = state;
                    break;
                }
            }
            if (string.IsNullOrEmpty(newState))
            {
                throw new Exception("Bad new state");
            }
            else
            {
                State = newState;
            }
            command = command.Remove(0, newState.Length);
            if (!string.IsNullOrEmpty(command))
            {
                if (command.ToUpper().Equals("R"))
                {
                    Position++;
                }
                else if (command.ToUpper().Equals("L"))
                {
                    Position--;
                }
                else
                {
                    throw new Exception("Пойди туда не знаю куда");
                }
            }
            if (Position == Tape.Length)
            {
                Tape += "B";
            }
            else if (Position < 0)
            {
                Tape = "B" + Tape;
                Position++;
            }
            InternalState = Tape[Position] + State;
            if (State == "q0")
            {
                throw new DivideByZeroException();
            }
        }
        public string GetNextCommand()
        {
            return InternalState + "->" + Commands[InternalState];
        }
    }
}

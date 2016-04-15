using System;
using System.Collections.Generic;
using System.Windows;


namespace TuringMachine
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        TuringImplementation turing;
        public MainWindow()
        {
            InitializeComponent();
            States.Text = "q0\r\nq1\r\nq2\r\nq3";
            Alphabet.Text = "0\r\n1\r\nB";
            StartState.Text = "q1";
            StartTape.Text = "10111";
            CodeBox.Text =
@"// Прибавление единицы
// к двоичному числу
1q1->1q1R
0q1->0q1R
Bq1->Bq2L
1q2->0q2L
0q2->1q3L
Bq2->1q0
0q3->0q3L
1q3->1q3L
Bq3->Bq0R";

        }

        private void RunButton_Click(object sender, RoutedEventArgs e)
        {
         
            HashSet<string> states = new HashSet<string>();
            string[] statesArray = States.Text.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var item in statesArray)
            {
                states.Add(item);
            }
            HashSet<char> alphabet = new HashSet<char>();
            string[] symbolsArray = Alphabet.Text.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var item in symbolsArray)
            {
                alphabet.Add(item[0]);
            }
            turing = new TuringImplementation(states, StartState.Text, alphabet, StartTape.Text, CodeBox.Text);
            StepButton.IsEnabled = true;
        }

        private void StepButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                turing.Step();
                NextCommand.Text = turing.GetNextCommand();
            }
            catch (DivideByZeroException)
            {
                MessageBox.Show("That's all, folks!");
                StepButton.IsEnabled = false;
            }
            Tape.Text = turing.Tape;
            State.Text = turing.State;
            Tape.Focus();
            Tape.Select(turing.Position, 1);

        }

        private void AddState_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}

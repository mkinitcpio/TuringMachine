using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

namespace TuringMachine
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    /// 
    
    public partial class MainWindow : Window
    {
        DispatcherTimer timer;
        TuringImplementation turing;
        public MainWindow()
        {
            InitializeComponent();
            States.Text = "q0\r\nq1\r\nq2\r\nq3";
            Alphabet.Text = "0\r\n1\r\nB";
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
            AddDefaultStatesStartStatesAndAlphabet();
            AddLR.Items.Add("L");
            AddLR.Items.Add("R");
            States.LostFocus += new RoutedEventHandler(UpdateStateOfStates);
            Alphabet.LostFocus += new RoutedEventHandler(UpdateStateOfAlphabet);
        }

        private void AddDefaultStatesStartStatesAndAlphabet() {
            string[] alphabetArray = Alphabet.Text.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var item in alphabetArray)
            {
                AddAlphabetLeft.Items.Add(item);
                AddAlphabetRight.Items.Add(item);
            }
            string[] statesArray = States.Text.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var item in statesArray)
            {
                AddStateLeft.Items.Add(item);
                AddStateRight.Items.Add(item);
            }
            foreach (var item in statesArray)
            {
                StartState.Items.Add(item);
            }
            AddStateLeft.SelectedIndex = 0;
            AddStateRight.SelectedIndex = 0;
            AddAlphabetLeft.SelectedIndex = 0;
            AddAlphabetRight.SelectedIndex = 0;
            AddLR.SelectedIndex = 1;
            StartState.SelectedIndex = 1;
        }

        private void UpdateStateOfAlphabet(object sender, EventArgs e)
        {
            string[] alphabetArray = Alphabet.Text.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            AddAlphabetLeft.Items.Clear();
            AddAlphabetRight.Items.Clear();
            foreach (var item in alphabetArray)
            {
                AddAlphabetLeft.Items.Add(item);
                AddAlphabetRight.Items.Add(item);
            }
        }

        private void UpdateStateOfStates(object sender, EventArgs e)
        {
            string[] statesArray = States.Text.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            AddStateLeft.Items.Clear();
            AddStateRight.Items.Clear();
            foreach (var item in statesArray)
            {
                AddStateLeft.Items.Add(item);
                AddStateRight.Items.Add(item);

            }
        }

        private void RunButton_Click(object sender, RoutedEventArgs e)
        {
            timer = new DispatcherTimer();
            timer.Tick += new EventHandler(StartTuringMachine);
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Start();
        }

        private void StartTuringMachine(object sender, EventArgs e)
        {
            bool isEnd = false;
                try
                {
                    turing.Step();
                    NextCommand.Text = turing.GetNextCommand();
                }
                catch (DivideByZeroException)
                {
                    MessageBox.Show("That's all, folks!");
                    StepButton.IsEnabled = false;
                    isEnd = true;
                }
                Tape.Text = turing.Tape;
                State.Text = turing.State;
                Tape.Focus();
                Tape.Select(turing.Position, 1);
            if (isEnd)
            {
                timer.Stop();
            }
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

        private void AddStartData_Click(object sender, RoutedEventArgs e)
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
            Tape.Text = StartTape.Text;
            turing = new TuringImplementation(states, StartState.SelectedItem.ToString(), alphabet, StartTape.Text, CodeBox.Text);
            StepButton.IsEnabled = true;
            CreateButton.IsEnabled = true;
            Tape.Focus();
            Tape.Select(turing.Position, 1);
        }

        private void AddCommand_Click(object sender, RoutedEventArgs e)
        {
            if (IsAnyComboBoxSelectedItemNullOrEmpty())
            {
                MessageBox.Show("Test");
                return;
            }
            CodeBox.Text += AddAlphabetLeft.SelectedItem.ToString() + AddStateLeft.SelectedItem.ToString() + "->" + AddAlphabetRight.SelectedItem.ToString() + AddStateRight.SelectedItem.ToString() + AddLR.SelectedItem.ToString() + "\r\n"; 
        }
        private bool IsAnyComboBoxSelectedItemNullOrEmpty()
        {
            return null == AddStateLeft.SelectedItem || null == AddStateRight.SelectedItem || null == AddAlphabetLeft.SelectedItem || null == AddAlphabetRight.SelectedItem || null == AddLR.SelectedItem;
        }
    }
}

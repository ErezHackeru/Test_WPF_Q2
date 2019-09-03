using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Threading;

namespace Test_Q2_MathStudyWpfApp
{
    public class MainWinViewModel
    {
        public TimerCount TimerCountDown { get; set; }
        public DelegateCommand MyCommand_OptionWrong { get; set; }
        public DelegateCommand MyCommand_OptionRight { get; set; }        
        
        private Task timerRunTask;
        public bool RightAnswer;
        public bool WrongAnswer;
        private CancellationTokenSource tokenSource;
        public MainWinViewModel()
        {
            RightAnswer = true;
            WrongAnswer = true;

            TimerCountDown = new TimerCount()
            {
                Result = "30",
                WinColor = new SolidColorBrush(Colors.WhiteSmoke)
            };

            tokenSource = new CancellationTokenSource();
            RunTimerBackwords();
            
            MyCommand_OptionWrong = new DelegateCommand(ExecuteMethod_Wrong, CanExecuteMethod_Wrong);
            MyCommand_OptionRight = new DelegateCommand(ExecuteMethod_Right, CanExecuteMethod_Right);

        }        
        
        private void RunTimerBackwords()
        {
            timerRunTask = Task.Run(() =>
            {
                if (!tokenSource.IsCancellationRequested)
                {
                    for (int i = 30; i >= 0; i--)
                    {
                        TimerCountDown.Result = i.ToString();
                        Thread.Sleep(1000);
                        if (tokenSource.IsCancellationRequested)
                            break;
                    }
                    if (!tokenSource.IsCancellationRequested)
                    {
                        RightAnswer = false;
                        WrongAnswer = false;
                        MyCommand_OptionWrong.RaiseCanExecuteChanged();
                        MyCommand_OptionRight.RaiseCanExecuteChanged();                        
                    }
                }
            }, tokenSource.Token);

            
        }

        private bool CanExecuteMethod_Right()
        {            
            return RightAnswer;
        }

        private void ExecuteMethod_Right()
        {
            tokenSource.Cancel();
            //disable buttons
            RightAnswer = false;
            WrongAnswer = false;
            MyCommand_OptionWrong.RaiseCanExecuteChanged();
            MyCommand_OptionRight.RaiseCanExecuteChanged();
            TimerCountDown.WinColor = new SolidColorBrush(Colors.LightGreen);
        }

        private bool CanExecuteMethod_Wrong()
        {
            if (TimerCountDown.Result == "0")
            {
                TimerCountDown.WinColor = new SolidColorBrush(Colors.LightPink);
            }
            return WrongAnswer;
        }

        public void ExecuteMethod_Wrong()
        {
            tokenSource.Cancel();
            //disable buttons
            RightAnswer = false;
            WrongAnswer = false;
            MyCommand_OptionWrong.RaiseCanExecuteChanged();
            MyCommand_OptionRight.RaiseCanExecuteChanged();
            TimerCountDown.WinColor = new SolidColorBrush(Colors.LightPink);
        }
        
    }
}

using System.Collections.Generic;
using Ticketer.ASR;
using Ticketer.Database.Domain;
using Ticketer.Database.Repositories;
using Ticketer.Helpers;
using Ticketer.TTS;
using Ticketer.GUI;
using System.Threading;
using System;

namespace Ticketer
{
    public class Main
    {
        private readonly Speaker speaker = new Speaker();
        private readonly Repository repository = new Repository();
        private Reservation reservation;
        private TicketCounter ticketCounter;
        private MyWindow window;
        private List<string> spectacles;
        private Thread mainConvThread;
        private Thread waitForStartThread;

        public Main()
        {
            StartApplication();
        }

        private void StartApplication()
        {
            Thread windowThread = new Thread(new ThreadStart(() => {
                window = new MyWindow(this);
                window.Show();
                System.Windows.Threading.Dispatcher.Run();
            }));
            windowThread.SetApartmentState(ApartmentState.STA);
            windowThread.Start();
            SetUpInterruptAndEndListener();
            Thread.Sleep(2500);
            GetRepetitoires();
            waitForStartThread = new Thread(AskForStart);
            waitForStartThread.Start();
        }

        public void AskForStart()
        {
            try
            {
                string initMessage = "Powiedz \"Rozpocznij\", aby rozpocząć.";
                window.AddTextToConversation(speaker.SystemSpeak(initMessage));
                InitEngine initEngine = new InitEngine();
                initEngine.GetAnswer();
                window.EnableStartButton();
                StartMainConversation();
            }
            catch (ThreadInterruptedException thr) { }
        }

        public void StartMainConversation()
        {
            if (waitForStartThread.IsAlive)
            {
                mainConvThread = new Thread(StartReservation);
                mainConvThread.Start();
                window.DisableStartButton();
                waitForStartThread.Interrupt();          
            }
        }

        public void InterruptConversation()
        {
            if (mainConvThread.IsAlive)
            {
                mainConvThread.Interrupt();
                speaker.SayGoodbye();
                waitForStartThread = new Thread(AskForStart);
                waitForStartThread.Start();
                window.EnableStartButton();
                window.HideStopButton();
            }
        }

        public void StartReservation()
        {
            try
            {
                window.ShowStopButton();
                string welcomeMessage = "Dzień dobry, czy chcesz złożyć zamówienie?";
                string options = "[TAK/NIE]";
                window.AddTextToConversation(speaker.EmployeeSpeak(welcomeMessage, options));
                YesNoQuestion yesNoQuestion = new YesNoQuestion();
                string result = yesNoQuestion.GetAnswer();
                window.AddTextToConversation(speaker.CustomerSpeak(result));
                if (result.Equals("Tak"))
                {
                    AskForSpectacleName();
                }
                else
                {
                    speaker.SayGoodbye();
                    window.ClearConversation();
                    AskForStart();
                }
            } catch (ThreadInterruptedException thr) { }
        }

        private void SetUpInterruptAndEndListener()
        {
            Thread thread = new Thread(() =>
            {
                InterruptAndEndListener listener = new InterruptAndEndListener();

                while (true)
                {
                    string interrupt = listener.GetAnswer();
                    if (interrupt.Equals("Przerwij"))
                    {
                        InterruptConversation();
                    } else if (interrupt.Equals("Zakończ"))
                    {
                        Environment.Exit(0);
                    }
                }
            });
            thread.IsBackground = true;
            thread.Start();
        }

        private void GetRepetitoires()
        {
            Thread thread = new Thread(() =>
            {
                spectacles = repository.GetSpectacles();
                string repetitoire = "Spektakle grane są każdego dnia w godzinach:\n-13:00\n-16:00\n-19:00\n\nAktualnie grane spektakle to:\n";
                foreach (string spectacle in spectacles)
                {
                    repetitoire += "-" + spectacle + "\n";
                }
                repetitoire += "\n" + "CENNIK:\n-Dziecięcy (do lat 5) - bezpłatnie\n-Ulgowy - 25 złotych\n-Normalny 50 złotych";
                window.SetRepertoire(repetitoire);
            });
            thread.Start();
        }

        private void AskForSpectacleName()
        {
            reservation = new Reservation();
            ticketCounter = new TicketCounter();
            
            string spectacleMessage = "Na jaki spektakl chcesz zamówić bilety? ";
            string options = "[";
            foreach(string spectacle in spectacles)
            {
                options += spectacle.ToUpper() + "/";
            }
            spectacleMessage = spectacleMessage.Remove(spectacleMessage.Length - 2, 2);
            options = options.Remove(options.Length - 1, 1);
            options += "]";
            window.AddTextToConversation(speaker.EmployeeSpeak(spectacleMessage, options));
            SpectacleNameQuestion spectacleNameQuestion = new SpectacleNameQuestion();
            string spectacleName = spectacleNameQuestion.GetAnswer();
            window.AddTextToConversation(speaker.CustomerSpeak(spectacleName));
            reservation.Spectacle = repository.FindSpectacleByName(spectacleName);
            AskForDayOfSpectacle();
        }

        private void AskForDayOfSpectacle()
        {
            string dateMessage = "Na jaki dzień tygodnia mają być bilety? ";
            string options = "[PONIEDZIAŁEK/WTOREK/ŚRODA/CZWARTEK/PIĄTEK/SOBOTA/NIEDZIELA]";
            window.AddTextToConversation(speaker.EmployeeSpeak(dateMessage, options));
            DateQuestion dateQuestion = new DateQuestion();
            string spectacleDate = dateQuestion.GetAnswer();
            window.AddTextToConversation(speaker.CustomerSpeak(spectacleDate));
            reservation.Day = spectacleDate;
            AskForTimeOfSpectacle();
        }

        private void AskForTimeOfSpectacle()
        {
            string timeMessage = "Na którą godzinę ma być spektakl? ";
            string options = "[13:00/16:00/19:00]";
            window.AddTextToConversation(speaker.EmployeeSpeak(timeMessage, options));
            TimeQuestion timeQuestion = new TimeQuestion();
            string spectacleTime = timeQuestion.GetAnswer();
            window.AddTextToConversation(speaker.CustomerSpeak(spectacleTime));
            reservation.Time = spectacleTime;
            AskForTickets();
        }

        private void AskForTickets()
        {
            AskForTicketType();
            AskIfMoreTickets();
        }

        private void AskForTicketType()
        {
            string ticketTypeMessage = "Jaki typ biletów chcesz zamówić?";
            string options = "[DZIECIĘCY/ULGOWY/NORMALNY]";
            window.AddTextToConversation(speaker.EmployeeSpeak(ticketTypeMessage, options));
            TicketTypeQuestion ticketTypeQuestion = new TicketTypeQuestion();
            string ticketType = ticketTypeQuestion.GetAnswer();
            window.AddTextToConversation(speaker.CustomerSpeak(ticketType));
            string numberOfTickets = AskForNumberOfTickets();
            window.AddTextToConversation(speaker.CustomerSpeak(numberOfTickets));
            ticketCounter.AddTickets(ticketType, int.Parse(numberOfTickets));
        }

        private string AskForNumberOfTickets()
        {
            string numberOfTicketsMessage = "Ile ma być biletów?";
            if (ticketCounter.NoTicketsOrdered())
            {
                numberOfTicketsMessage += " Jednorazowo można wybrać maksymalnie 9 biletów jednego typu.";
            }
            window.AddTextToConversation(speaker.EmployeeSpeak(numberOfTicketsMessage, ""));
            NumberOfTicketsQuestion numberOfTicketsQuestion = new NumberOfTicketsQuestion();
            return numberOfTicketsQuestion.GetAnswer();
        }

        private void AskIfMoreTickets()
        {
            string moreTicketsMessage = "Czy życzysz sobie jeszcze jakieś bilety?";
            string options = "[TAK/NIE]";
            window.AddTextToConversation(speaker.EmployeeSpeak(moreTicketsMessage, options));
            YesNoQuestion yesNoQuestion = new YesNoQuestion();
            string result = yesNoQuestion.GetAnswer();
            window.AddTextToConversation(speaker.CustomerSpeak(result));
            if (result.Equals("Tak"))
            {
                AskForTickets();
            } else
            {
                reservation.Order = ticketCounter.GetOrderedTickets();
                reservation.Price = ticketCounter.GetTotalCost();
                AskForReservationCorrectness();
            }
        }

        private void AskForReservationCorrectness()
        {
            string correctnessMessage = "Czy wyświetlone zamówienie się zgadza?";
            string options = "[TAK/NIE]";
            string order = "========================\n" + "=====  ZAMÓWIENIE  =====\n" + "========================\n"
                + "Spektakl: " + reservation.Spectacle.Name
                + "\nData: " + reservation.Day + ", godz. " + reservation.Time
                + "\nBilety: " + reservation.Order
                + "\nŁączny koszt: " + reservation.Price + " złotych";
            window.AddTextToConversation(order);
            window.AddTextToConversation(speaker.EmployeeSpeak(correctnessMessage, options));
            YesNoQuestion yesNoQuestion = new YesNoQuestion();
            string result = yesNoQuestion.GetAnswer();
            if (result.Equals("Nie"))
            {
                StartReservationAgain();
            }
            else
            {
                AskForCustomerName();
            }
        }

        private void StartReservationAgain()
        {
            string failReservationMessage = "Niestety. W takim razie musimy zacząć zamówienie od początku.";
            window.AddTextToConversation(speaker.EmployeeSpeak(failReservationMessage, ""));
            reservation = new Reservation();
            AskForSpectacleName();
        }

        private void AskForCustomerName()
        {
            string customerNameMessage = "W takim razie proszę o imie i nazwisko, na które mam zapisać zamówienie.";
            window.AddTextToConversation(speaker.EmployeeSpeak(customerNameMessage, ""));
            CustomerNameQuestion customerNameQuestion = new CustomerNameQuestion();
            string customerName = customerNameQuestion.GetAnswer();
            window.AddTextToConversation(speaker.CustomerSpeak(customerName));
            reservation.Client = customerName;
            AskCustomerToConfirmReservation();
        }

        private void AskCustomerToConfirmReservation()
        {
            string confirmationMessage = reservation.Client + " czy zatwierdzasz swoje zamówienie?";
            string options = "[TAK/NIE]";
            window.AddTextToConversation(speaker.EmployeeSpeak(confirmationMessage, options));
            YesNoQuestion yesNoQuestion = new YesNoQuestion();
            string result = yesNoQuestion.GetAnswer();
            if (result.Equals("Nie"))
            {
                StartReservationAgain();
            } else
            {
                ConfirmReservation();
            }
        }

        private void ConfirmReservation()
        {
            repository.SaveReservation(reservation);
            string message = "Dziękujemy za złożenie zamówienia. Zapraszamy do kasy najpóźniej 30 minut przed spektaklem w celu opłacenia zamówienia. Pozdrawiam i zapraszam ponownie";
            window.AddTextToConversation(speaker.EmployeeSpeak(message, ""));
            AfterReservation();
        }

        private void AfterReservation()
        {
            string afterMessage = "Chcesz zakończyć działanie aplikacji czy rozpocząć od nowa?";
            string options = "[ZAKOŃCZ/POWTÓRZ]";
            window.AddTextToConversation(speaker.EmployeeSpeak(afterMessage, options));
            AfterReservationQuestion afterReservationQuestion = new AfterReservationQuestion();
            string decision = afterReservationQuestion.GetAnswer();
            if (decision.Equals("Powtórz"))
            {
                window.ClearConversation();
                StartReservation();
            } else
            {
                return;
            }
        }
    }
}

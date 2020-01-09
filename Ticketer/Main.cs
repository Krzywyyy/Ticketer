using System;
using System.Collections.Generic;
using Ticketer.ASR;
using Ticketer.Database.Domain;
using Ticketer.Database.Repositories;
using Ticketer.Helpers;
using Ticketer.TTS;

namespace Ticketer
{
    class Main
    {
        private readonly Speaker speaker = new Speaker();
        private readonly Repository repository = new Repository();
        private Reservation reservation;
        private TicketCounter ticketCounter;

        public Main()
        {
            AskForStart();
        }

        private void AskForStart()
        {
            string initMessage = "Powiedz \"Rozpocznij\", aby rozpocząć.";
            speaker.SystemSpeak(initMessage);
            InitEngine initEngine = new InitEngine();
            initEngine.GetAnswer();
            StartReservation();
        }

        private void StartReservation()
        {
            string welcomeMessage = "Dzień dobry, czy chcesz złożyć zamówienie?";
            string options = "[TAK/NIE]";
            speaker.EmployeeSpeak(welcomeMessage, options);
            YesNoQuestion yesNoQuestion = new YesNoQuestion();
            string result = yesNoQuestion.GetAnswer();
            speaker.CustomerSpeak(result);
            if (result.Equals("Tak"))
            {
                AskForSpectacleName();
            } else
            {
                speaker.SayGoodbye();
                AskForStart();
            }
        }

        private void AskForSpectacleName()
        {
            reservation = new Reservation();
            ticketCounter = new TicketCounter();
            List<string> spectacles = repository.GetSpectacles();
            string spectacleMessage = "Na jaki spektakl chcesz zamówić bilety? ";
            string options = "[";
            foreach(string spectacle in spectacles)
            {
                options += spectacle.ToUpper() + "/";
            }
            spectacleMessage = spectacleMessage.Remove(spectacleMessage.Length - 2, 2);
            options = options.Remove(options.Length - 1, 1);
            options += "]";
            speaker.EmployeeSpeak(spectacleMessage, options);
            SpectacleNameQuestion spectacleNameQuestion = new SpectacleNameQuestion();
            string spectacleName = spectacleNameQuestion.GetAnswer();
            speaker.CustomerSpeak(spectacleName);
            reservation.Spectacle = repository.FindSpectacleByName(spectacleName);
            AskForDayOfSpectacle();
        }

        private void AskForDayOfSpectacle()
        {
            string dateMessage = "Na jaki dzień tygodnia mają być bilety? ";
            string options = "[PONIEDZIAŁEK/WTOREK/ŚRODA/CZWARTEK/PIĄTEK/SOBOTA/NIEDZIELA]";
            speaker.EmployeeSpeak(dateMessage, options);
            DateQuestion dateQuestion = new DateQuestion();
            string spectacleDate = dateQuestion.GetAnswer();
            speaker.CustomerSpeak(spectacleDate);
            reservation.Day = spectacleDate;
            AskForTimeOfSpectacle();
        }

        private void AskForTimeOfSpectacle()
        {
            string timeMessage = "Na którą godzinę ma być spektakl? ";
            string options = "[13:00/16:00/19:00]";
            string spectaclesTimes = "Dostępne godziny to:\n-13\n-16\n-19";
            speaker.EmployeeSpeak(timeMessage, options);
            Console.WriteLine(spectaclesTimes);
            TimeQuestion timeQuestion = new TimeQuestion();
            string spectacleTime = timeQuestion.GetAnswer();
            speaker.CustomerSpeak(spectacleTime);
            reservation.Time = int.Parse(spectacleTime);
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
            if (ticketCounter.NoTicketsOrdered())
            {
                ticketTypeMessage += "Poniżej znajduje się cennik biletów.";
                string priceList = "CENNIK:\n-Dziecięcy (do lat 5) - bezpłatnie\n-Ulgowy - 25 złotych\n-Normalny 50 złotych";
                speaker.EmployeeSpeak(ticketTypeMessage, options);
                Console.WriteLine(priceList);
            } else
            {
                speaker.EmployeeSpeak(ticketTypeMessage, options);
            }
            TicketTypeQuestion ticketTypeQuestion = new TicketTypeQuestion();
            string ticketType = ticketTypeQuestion.GetAnswer();
            speaker.CustomerSpeak(ticketType);
            string numberOfTickets = AskForNumberOfTickets();
            speaker.CustomerSpeak(numberOfTickets);
            ticketCounter.AddTickets(ticketType, int.Parse(numberOfTickets));
        }

        private string AskForNumberOfTickets()
        {
            string numberOfTicketsMessage = "Ile ma być biletów?";
            if (ticketCounter.NoTicketsOrdered())
            {
                numberOfTicketsMessage += " Jednorazowo można wybrać maksymalnie 9 biletów jednego typu.";
            }
            speaker.EmployeeSpeak(numberOfTicketsMessage, "");
            NumberOfTicketsQuestion numberOfTicketsQuestion = new NumberOfTicketsQuestion();
            return numberOfTicketsQuestion.GetAnswer();
        }

        private void AskIfMoreTickets()
        {
            string moreTicketsMessage = "Czy życzysz sobie jeszcze jakieś bilety?";
            string options = "[TAK/NIE]";
            speaker.EmployeeSpeak(moreTicketsMessage, options);
            YesNoQuestion yesNoQuestion = new YesNoQuestion();
            string result = yesNoQuestion.GetAnswer();
            speaker.CustomerSpeak(result);
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
            Console.WriteLine(order);
            speaker.EmployeeSpeak(correctnessMessage, options);
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
            speaker.EmployeeSpeak(failReservationMessage, "");
            reservation = new Reservation();
            AskForSpectacleName();
        }

        private void AskForCustomerName()
        {
            string customerNameMessage = "W takim razie proszę o imie i nazwisko, na które mam zapisać zamówienie.";
            speaker.EmployeeSpeak(customerNameMessage, "");
            CustomerNameQuestion customerNameQuestion = new CustomerNameQuestion();
            string customerName = customerNameQuestion.GetAnswer();
            speaker.CustomerSpeak(customerName);
            reservation.Client = customerName;
            AskCustomerToConfirmReservation();
        }

        private void AskCustomerToConfirmReservation()
        {
            string confirmationMessage = reservation.Client + " czy zatwierdzasz swoje zamówienie?";
            string options = "[TAK/NIE]";
            speaker.EmployeeSpeak(confirmationMessage, options);
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
            speaker.EmployeeSpeak(message, "");
            AfterReservation();
        }

        private void AfterReservation()
        {
            string afterMessage = "Chcesz zakończyć działanie aplikacji czy rozpocząć od nowa?";
            string options = "[ZAKOŃCZ/POWTÓRZ]";
            speaker.EmployeeSpeak(afterMessage, options);
            AfterReservationQuestion afterReservationQuestion = new AfterReservationQuestion();
            string decision = afterReservationQuestion.GetAnswer();
            if (decision.Equals("Powtórz"))
            {
                Console.Clear();
                StartReservation();
            } else
            {
                return;
            }
        }
    }
}

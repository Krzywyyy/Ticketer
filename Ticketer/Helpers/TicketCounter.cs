namespace Ticketer.Helpers
{
    class TicketCounter
    {
        private int childrenTicketCount;
        private int reducedTicketCount;
        private int normalTicketCount;

        public TicketCounter()
        {
            childrenTicketCount = 0;
            reducedTicketCount = 0;
            normalTicketCount = 0;
        }

        public void AddTickets(string ticketType, int count)
        {
            if (ticketType.Equals("Dziecięcy"))
            {
                childrenTicketCount += count;
            } else if (ticketType.Equals("Ulgowy"))
            {
                reducedTicketCount += count;
            } else if(ticketType.Equals("Normalny"))
            {
                normalTicketCount += count;
            }
        }

        public bool NoTicketsOrdered()
        {
            return childrenTicketCount + reducedTicketCount + normalTicketCount == 0;
        }

        public string GetOrderedTickets()
        {
            return "Bilety:\n "
                + childrenTicketCount + "x Dziecięcy, \n"
                + reducedTicketCount + "x Ulgowy, \n"
                + normalTicketCount + "x Normalny";
        }

        public int GetTotalCost()
        {
            return reducedTicketCount * 25 + normalTicketCount * 50;
        }
    }
}

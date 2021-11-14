using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Entities.Helpers;
using NHibernate.Mapping;

namespace Services
{
    public class BusinessReportService
    {
        public BusinessSummary GetBusinessSummary(IEnumerable<Reservation> reservations, IEnumerable<Mark> marks, int backPeriod)
        {
            var result = new BusinessSummary();

            result.AverageMark = CalculateAverageMark(marks);
            result.Weekly = GetWeekly(backPeriod);
            result.Monthly = GetMonthly(backPeriod);
            result.Yearly = GetYearly(backPeriod);

            Calculate(result.Weekly, reservations);
            Calculate(result.Monthly, reservations);
            Calculate(result.Yearly, reservations);

            return result;
        }

        private double CalculateAverageMark(IEnumerable<Mark> marks)
        {
            if (!marks.Any())
            {
                return 0.0;
            }

            return marks.Select(x => x.GivenMark).Average();
        }

        private IEnumerable<SummaryItem> GetWeekly(int backPeriod)
        {
            var result = new List<SummaryItem>();

            for (int i = 1; i <= backPeriod; i++)
            {
                var startDate = DateTime.Now.Subtract(new TimeSpan(i * 7 + 1, 0, 0, 0));
                var endDate = startDate.AddDays(6);

                result.Add(new SummaryItem()
                {
                    StartDateTime = startDate,
                    EndDateTime = endDate
                });
            }

            return result;
        }

        private IEnumerable<SummaryItem> GetMonthly(int backPeriod)
        {
            var result = new List<SummaryItem>();

            for (int i = 1; i <= backPeriod; i++)
            {
                var startDate = DateTime.Now.AddMonths(-1 * i);
                var endDate = startDate.AddMonths(1).Subtract(new TimeSpan(1,0,0,0));

                result.Add(new SummaryItem()
                {
                    StartDateTime = startDate,
                    EndDateTime = endDate,
                });
            }

            return result;
        }

        private IEnumerable<SummaryItem> GetYearly(int backPeriod)
        {
            var result = new List<SummaryItem>();

            for (int i = 1; i <= backPeriod; i++)
            {
                var startDate = DateTime.Now.AddYears(i * -1);
                var endDate = startDate.AddYears(1).Subtract(new TimeSpan(1,0,0,0));

                result.Add(new SummaryItem()
                {
                    StartDateTime = startDate,
                    EndDateTime = endDate
                });
            }

            return result;
        }

        private void Calculate(IEnumerable<SummaryItem> summaries, IEnumerable<Reservation> reservations)
        {
            foreach (var summary in summaries)
            {
                var relevantReservations = reservations.Where(x =>
                    x.StartDateTime < summary.EndDateTime && x.StartDateTime >= summary.StartDateTime);

                summary.NumberOfReservations = relevantReservations.Count();
                summary.MoneyMade = CalculateMoneyMade(relevantReservations);
                summary.Name = CreateNameForSummaryItem(summary);
            }
        }

        private double CalculateMoneyMade(IEnumerable<Reservation> reservations)
        {
            double result = 0;

            foreach (var reservation in reservations)
            {
                result += CalculateMoneyMadeForReservation(reservation);
            }

            return result;
        }

        private double CalculateMoneyMadeForReservation(Reservation reservation)
        {
            var totalDays = (reservation.EndDateTime - reservation.StartDateTime).TotalDays;

            return totalDays * reservation.Price;
        }

        private string CreateNameForSummaryItem(SummaryItem summaryItem)
        {
            var stringBuilder = new StringBuilder();

            stringBuilder.Append(summaryItem.StartDateTime.ToString("d"))
                .Append(" - ")
                .Append(summaryItem.EndDateTime.ToString("d"));

            return stringBuilder.ToString();
        }
    }
}

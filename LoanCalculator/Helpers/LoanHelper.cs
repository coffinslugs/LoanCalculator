using LoanCalculator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoanCalculator.Helpers
{
    public class LoanHelper
    {
        public Loan GetPayments(Loan loan)
        {
            // Calculate Monthly Payment
            loan.MonthlyPayment = CalcPayment(loan.FullAmount, loan.InterestRate, loan.TermInMonths);

            // Create Loop From 1 to Term Length
            var balance = loan.FullAmount;
            var totalInterest = 0.0m;
            var monthlyInterest = 0.0m;
            var monthlyPrincipal = 0.0m;
            var monthlyRate = CalcMonthyRate(loan.InterestRate);

            for (int month = 1; month <= loan.TermInMonths; month++)
            {
                monthlyInterest = CalcMonthlyInterest(balance, monthlyRate);
                totalInterest += monthlyInterest;
                monthlyPrincipal = loan.MonthlyPayment - monthlyInterest;
                balance -= monthlyPrincipal;

                LoanPayment loanPayment = new();

                loanPayment.Month = month;
                loanPayment.Payment = loan.MonthlyPayment;
                loanPayment.MonthlyPrincipal = monthlyPrincipal;
                loanPayment.MonthlyInterest = monthlyInterest;
                loanPayment.TotalInterest = totalInterest;
                loanPayment.Balance = balance;

                // Push Object into Loan Model
                loan.Payments.Add(loanPayment);

            }

            loan.TotalInterest = totalInterest;
            loan.TotalCost = loan.FullAmount + totalInterest;

            return loan;
            // Calculate Payment Schedule


            // Push Payments To The Loan


            // Return Loan To View
            return loan;
        }

        private decimal CalcPayment(decimal FullAmount, decimal InterestRate, int TermInMonths)
        {

            decimal MonthlyRate = CalcMonthyRate(InterestRate);
            var amountD = Convert.ToDouble(FullAmount);
            var rateD = Convert.ToDouble(MonthlyRate);

            var paymentD = (amountD * rateD) / (1 - Math.Pow(1 + rateD, -TermInMonths));

            return Convert.ToDecimal(paymentD);
        }

        private decimal CalcMonthyRate(decimal InterestRate)
        {
            return InterestRate / 1200;
        }

        private decimal CalcMonthlyInterest(decimal balance, decimal monthlyRate)
        {
            return balance * monthlyRate;
        }
    }

}

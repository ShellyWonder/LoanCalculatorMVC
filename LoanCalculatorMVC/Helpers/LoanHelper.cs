using LoanCalculatorMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;




namespace LoanCalculatorMVC.Helpers
{
    public class LoanHelper
    {
        //Generate Amorization schedule
        public Loan GetPayments(Loan loan)
        {
            //calc monthly payment
            loan.Payment = CalcPayment(loan.Amount, loan.Rate, loan.Term);
            
            var balance = loan.Amount;
            var totalInterest = 0.0m;
            var monthlyInterest = 0.0m;
            var monthlyPrincipal = 0.0m;
            var MonthlyRate = CalcMonthlyRate(loan.Rate);
            //loop over each month until term reached
            for (int month = 1; month <= loan.Term; month++)
            {
                monthlyInterest = CalcMonthlyInterest(balance, MonthlyRate);
                totalInterest += monthlyInterest;
                monthlyPrincipal = loan.Payment - monthlyInterest;
                balance -= monthlyPrincipal;

                LoanPayment loanPayment = new();

                loanPayment.Month = month;
                loanPayment.Payment = loan.Payment;
                loanPayment.MonthlyPrincipal = monthlyPrincipal;
                loanPayment.MonthlyInterest = monthlyInterest;
                loanPayment.TotalInterest = totalInterest;
                loanPayment.Balance = balance;


            //push object into loan Model
              loan.Payments.Add(loanPayment);  
            }
            
            loan.TotalInterest = totalInterest;
            loan.TotalCost = loan.Amount + totalInterest;


            return loan;

        }
        //Calculate monthly payment
        private decimal CalcPayment(decimal amount, decimal rate, int term)
        {
           
            //rateD is annual
           var monthlyRate = CalcMonthlyRate(rate);
            var rateD = Convert.ToDouble(monthlyRate);
            var amountD = Convert.ToDouble(amount);

            var paymentD = (amountD * rateD) /(1 - Math.Pow(1+rateD, -term));
            
            return Convert.ToDecimal(paymentD);
        }
        //take annual rateD & convert to monthly rate
        private decimal CalcMonthlyRate(decimal rate)
        {
            return rate / 1200;
        }
        private decimal CalcMonthlyInterest(decimal balance, decimal monthlyRate)
        {
            return balance * monthlyRate;
        }
    }
}

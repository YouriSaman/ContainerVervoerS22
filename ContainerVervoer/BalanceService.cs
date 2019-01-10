using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContainerVervoer
{
    public class BalanceService
    {
        public decimal CalculateBalance(Ship ship)
        {
            var right = ship.WeightRight;
            var left = ship.WeightLeft;
            decimal balance;

            if (right >= left)
            {
                balance = left - right;
                balance = balance / right * 100;
            }
            else
            {
                balance = right - left;
                balance = balance / left * 100;
            }

            balance = Math.Round(balance, 2);

            return balance;
        }
    }
}

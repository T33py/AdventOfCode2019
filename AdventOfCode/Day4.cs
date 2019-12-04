using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode
{
    class Day4
    {
        static void Main(string[] args)
        {
            NumberAnalyzer na = new NumberAnalyzer();
            int low = 307237;
            int high = 769058;

            var number = 111122;

            Console.WriteLine("Low: " + low + " High: " + high);

            // part 1
            //Console.WriteLine(na.CheckCriteria(number));
            //Console.WriteLine(na.FindValidInRange(low, high).Count);

            // part 2
            //Console.WriteLine(na.Exactly2Adjacent(number));
            var candidates = na.FindValidInRange(low, high);
            var valid = new List<int>();
            foreach(int i in candidates)
            {
                if (na.Exactly2Adjacent(i))
                {
                    valid.Add(i);
                }
            }
            Console.WriteLine("Valid candidates: " + valid.Count);
        }
    }

    class NumberAnalyzer
    {
        public List<int> FindValidInRange(int low, int high)
        {
            List<int> validNumbers = new List<int>();

            for(int i = low; i <= high; i++)
            {
                if (CheckCriteria(i))
                {
                    validNumbers.Add(i);
                }
            }

            return validNumbers;
        }

        public bool CheckCriteria(int number)
        {
            Console.WriteLine(number);

            var digits = IntToDigits(number);

            //Console.Write("Digits: ");
            //foreach (int i in digits)
            //{
            //    Console.Write(i + ", ");
            //}
            //Console.WriteLine();

            var condition1 = false;
            var condition2 = true;

            // Condition 1
            // Criteria: Two adjacent digits are the same (like 22 in 122345)
            var digit = digits[0];
            for(int i = 1; i < digits.Length; i++)
            {
                if(digit == digits[i]) // if next digit is the same as the previous
                {
                    condition1 = true;
                    break;
                }

                // check for next number
                digit = digits[i];
            }

            if (!condition1)
            {
                //Console.WriteLine("failed condition 1");
                return false;
            }


            // Condition 2
            // Criteria: Going from left to right, the digits never decrease; they only ever increase or stay the same (like 111123 or 135679)
            digit = digits[0];
            for (int i = 1; i < digits.Length; i++)
            {
                if (digits[i] < digit) // if digit is larger than the next one
                {
                    condition2 = false;
                    break;
                    //Console.WriteLine(digits[i] + " < " + digit);
                }

                // check for next number
                digit = digits[i];
            }

            if (!condition2)
            {
                //Console.WriteLine("failed condition 2");
            }

            return condition1 && condition2;
        }

        public bool Exactly2Adjacent(int number)
        {
            //Console.WriteLine("Number: " + number);
            bool cond = false;
            var digits = IntToDigits(number);
            List<List<int>> adjacencies = new List<List<int>>();

            var pr = -1;
            var currList = new List<int>();
            foreach(int digit in digits)
            {
                //Console.WriteLine("curr: " + digit + " pr: " + pr);
                if(digit == pr)
                {
                    currList.Add(digit);
                }
                else
                {
                    adjacencies.Add(currList);
                    currList = new List<int>();
                    currList.Add(digit);
                }
                pr = digit;
            }
            adjacencies.Add(currList); // remember to put the final list into the adjacencies

            foreach(List<int> adjacency in adjacencies)
            {
                //Console.WriteLine(adjacency.Count);
                if(adjacency.Count == 2)
                {
                    cond = true;
                }
            }

            return cond;
        }

        public int[] IntToDigits(int number)
        {
            var num = number.ToString().ToCharArray();
            int[] digits = new int[num.Length];
            for(int i = 0; i < digits.Length; i++)
            {
                digits[i] = int.Parse(num[i].ToString());
            }

            return digits;
        }
    }
}

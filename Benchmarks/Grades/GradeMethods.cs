using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benchmarks.Grades
{
    public static class GradeMethods
    {
        public static string Original(int score)
        {
            if (score >= 90)
            {
                return "A";
            }
            else
            {
                if (score >= 80)
                {
                    return "B";
                }
                else
                {
                    if (score >= 70)
                    {
                        return "C";
                    }
                    else
                    {
                        if (score >= 60)
                        {
                            return "D";
                        }
                        else
                        {
                            return "F";
                        }
                    }
                }
            }
        }

        public static char AgileJebrim1(int score)
        {
            var grade = 'A';
            grade = score < 90 ? 'B' : grade;
            grade = score < 80 ? 'C' : grade;
            grade = score < 70 ? 'D' : grade;
            grade = score < 60 ? 'F' : grade;

            return grade;
        }

        public static char AgileJebrim2(int score)
        {
            var grade = 'A';
            if (score < 90) grade = 'B';
            if (score < 80) grade = 'C';
            if (score < 70) grade = 'D';
            if (score < 60) grade = 'F';

            return grade;
        }

        public static char AgileJebrim3(int score)
        {
            if (score < 60) return 'F';
            if (score < 70) return 'D';
            if (score < 80) return 'C';
            if (score < 90) return 'B';

            return 'A';
        }

        public static char amohr1(int score)
        {
            return "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFDDDDDDDDDDCCCCCCCCCCBBBBBBBBBBAAAAAAAAAAA"[score];
        }

        public static char amohr2(int score)
        {
            return "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFDDDDDCCCCCBBBBBAAAAAAF"[score >> 1];
        }

        public static char FreyaHolmer(int score)
        {
            return "FFFFFFDCBAA"[score / 10];
        }

        public static char iquilezles(int score)
        {
            return "FFFFFFDCBA"[((score << 4) + (score << 3) + score + 54) >> 8];
        }

        public static char nthnblair(int score)
        {
            int x = (((score << 4) + (score << 3) + score + 54) >> 8) - 6;
            return (char)(68 - (x & (~(x >> 4))) + ((x >> 4) & 2));
        }

        public static char xsphi(int score)
        {
            return (char)((((74 - score / 10) | 1179009280) >> (((score - 60) >> 2) & 24)) % 128);
        }

        public static string SwitchExpression(int score) => score switch
        {
            >= 90 => "A",
            >= 80 => "B",
            >= 70 => "C",
            >= 60 => "D",
            _ => "F"
        };
    }
}

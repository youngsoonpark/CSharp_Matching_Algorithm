using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Data.SqlClient;

using System.Windows.Forms.DataVisualization.Charting;

namespace Matching_Algorithm
{
    public partial class Form1 : Form
    {
        private List<Stock> sList = new List<Stock>();
        private List<UserStock> usList = new List<UserStock>();
        private List<Stock> tempsList = new List<Stock>();
        private List<int> matchList = new List<int>();

        private List<string> subcodeList = new List<string>();

        public Form1()
        {
            InitializeComponent();

            #region Input_User_Data
            UserStock us1 = new UserStock();
            us1.Semester = 1;
            us1.Day = 0;
            us1.Price = 100;
            usList.Add(us1);

            UserStock us2 = new UserStock();
            us2.Semester = 1;
            us2.Day = 4;
            us2.Price = 105;
            usList.Add(us2);

            UserStock us3 = new UserStock();
            us3.Semester = 2;
            us3.Day = 8;
            us3.Price = 94;
            usList.Add(us3);

            UserStock us4 = new UserStock();
            us4.Semester = 3;
            us4.Day = 12;
            us4.Price = 104;
            usList.Add(us4);

            UserStock us5 = new UserStock();
            us5.Semester = 4;
            us5.Day = 19;
            us5.Price = 99;
            usList.Add(us5);
            #endregion

            LoadSubjectCode();

            #region Chart_Data
            chart1.Series.Clear();

            Series data = new Series();

            data.ChartType = SeriesChartType.Line;
            data.Name = "Price";

            #endregion


            textBox1.Text = string.Format("{0}", trackBar1.Value);
            textBox2.Text = string.Format("{0}", trackBar2.Value);
            textBox3.Text = string.Format("{0}", trackBar3.Value);
        }

        void UserPatternMatch()
        {
            #region UserPattern_Analysis

            int hidx = 0, lidx = 0;
            for (int i = 0; i < usList.Count(); i++)
            {
                if (usList[hidx].Price < usList[i].Price)
                {
                    hidx = i;
                }
                if (usList[lidx].Price > usList[i].Price)
                {
                    lidx = i;
                }
            }
            usList[hidx].Highpt = true;
            usList[lidx].Lowpt = true;

            for (int i = 0; i < usList.Count(); i++)
            {
                if (usList[hidx].Price * 1.03 >= usList[i].Price && usList[hidx].Price * 0.97 <= usList[i].Price)
                {
                    usList[i].Highpt = true;
                }
                if (usList[lidx].Price * 1.03 >= usList[i].Price && usList[lidx].Price * 0.97 <= usList[i].Price)
                {
                    usList[i].Lowpt = true;
                }
            }
            #endregion

            ////////////////////////////////////////////////////

            #region Matching_Succed_Algorithm_High&Low

            for (int allScan = 0; allScan < sList.Count() - 20; allScan++)
            {
                for (int i = allScan; i < allScan + 20; i++)
                    tempsList.Add(sList[i]);
                
                int hhidx = 0, llidx = 0; ;
                for (int i = 0; i < tempsList.Count(); i++)
                {
                    if (tempsList[hhidx].StPrice < tempsList[i].StPrice)
                        hhidx = i;
                    if (tempsList[llidx].StPrice > tempsList[i].StPrice)
                        llidx = i;
                }
                tempsList[hhidx].Highpt = true;
                tempsList[llidx].Lowpt = true;

                for (int i = 0; i < tempsList.Count(); i++)
                {
                    if (tempsList[hhidx].StPrice * 1.0002 >= tempsList[i].StPrice && tempsList[hhidx].StPrice * 0.9998 <= tempsList[i].StPrice)
                        tempsList[i].Highpt = true;
                    if (tempsList[llidx].StPrice * 1.0002 >= tempsList[i].StPrice && tempsList[llidx].StPrice * 0.9998 <= tempsList[i].StPrice)
                        tempsList[i].Lowpt = true;
                }

                for (int i = 0; i < tempsList.Count() - 1; i++)
                {
                    if (tempsList[i].StPrice < tempsList[i + 1].StPrice)
                        tempsList[i + 1].Up_down = "Up";
                    else if (tempsList[i].StPrice > tempsList[i + 1].StPrice)
                        tempsList[i + 1].Up_down = "Down";
                    else
                        tempsList[i + 1].Up_down = "Same";
                }

                ///////////// user 고저점과 arr에 분기 넣어오기
                List<int> harr = new List<int>();
                List<int> larr = new List<int>();

                for (int i = 0; i < usList.Count(); i++)
                {
                    if (usList[i].Highpt == true)
                        harr.Add(usList[i].Semester);
                    if (usList[i].Lowpt == true)
                        larr.Add(usList[i].Semester);
                }
                /////////////////////////////////

                bool hfirstsemester = false, hsecondsemester = false, hthirdsemester = false, hforthsemester = false;
                bool lfirstsemester = false, lsecondsemester = false, lthirdsemester = false, lforthsemester = false;

                for (int i = 0; i < harr.Count(); i++)
                {
                    switch (harr[i])
                    {
                        case 1:
                            for (int j = 0; j < 5; j++)
                                if (tempsList[j].Highpt == true)
                                    hfirstsemester = true;
                            break;
                        case 2:
                            for (int j = 5; j < 10; j++)
                                if (tempsList[j].Highpt == true)
                                    hsecondsemester = true;
                            break;
                        case 3:
                            for (int j = 10; j < 15; j++)
                                if (tempsList[j].Highpt == true)
                                    hthirdsemester = true;
                            break;
                        case 4:
                            for (int j = 15; j < 20; j++)
                                if (tempsList[j].Highpt == true)
                                    hforthsemester = true;
                            break;
                    }
                }

                for (int i = 0; i < larr.Count(); i++)
                {
                    switch (larr[i])
                    {
                        case 1:
                            for (int j = 0; j < 5; j++)
                                if (tempsList[j].Lowpt == true)
                                    lfirstsemester = true;
                            break;
                        case 2:
                            for (int j = 5; j < 10; j++)
                                if (tempsList[j].Lowpt == true)
                                    lsecondsemester = true;
                            break;
                        case 3:
                            for (int j = 10; j < 15; j++)
                                if (tempsList[j].Lowpt == true)
                                    lthirdsemester = true;
                            break;
                        case 4:
                            for (int j = 15; j < 20; j++)
                                if (tempsList[j].Lowpt == true)
                                    lforthsemester = true;
                            break;
                    }
                }

                int hmatchcount = 0;
                for (int i = 0; i < harr.Count(); i++)
                {
                    switch (harr[i])
                    {
                        case 1:
                            if (hfirstsemester == true)
                                hmatchcount++;
                            break;
                        case 2:
                            if (hsecondsemester == true)
                                hmatchcount++;
                            break;
                        case 3:
                            if (hthirdsemester == true)
                                hmatchcount++;
                            break;
                        case 4:
                            if (hforthsemester == true)
                                hmatchcount++;
                            break;
                    }
                }

                int lmatchcount = 0;
                for (int i = 0; i < larr.Count(); i++)
                {
                    switch (larr[i])
                    {
                        case 1:
                            if (lfirstsemester == true)
                                lmatchcount++;
                            break;
                        case 2:
                            if (lsecondsemester == true)
                                lmatchcount++;
                            break;
                        case 3:
                            if (lthirdsemester == true)
                                lmatchcount++;
                            break;
                        case 4:
                            if (lforthsemester == true)
                                lmatchcount++;
                            break;
                    }
                }
            #endregion

                if (harr.Count() == hmatchcount && larr.Count() == lmatchcount)
                    matchList.Add(allScan);
                
                for (int i = 0; i < tempsList.Count(); i++)
                {
                    tempsList[i].Highpt = false;
                    tempsList[i].Lowpt = false;
                }
                tempsList.Clear();
            }
        }

        void GetMatchRate_Algorithm(string code)
        {
            double highlow_MatchRate = 0, deviation_MatchRate = 0, slope_MatchRate = 0;

            int firstSemesterIdx = 0, secondSemesterIdx = 0, thirdSemesterIdx = 0, forthSemesterIdx = 0;

            #region Prepare_For_Match

            for (int i = 0; i < usList.Count() - 1; i++)
            {
                if (usList[i].Price > usList[i + 1].Price)
                    usList[i + 1].Up_down = "down";
                else if (usList[i].Price < usList[i + 1].Price)
                    usList[i + 1].Up_down = "up";
                else
                    usList[i + 1].Up_down = "same";
            }

            for (int i = 0; i < sList.Count() - 1; i++)
            {
                if (sList[i].StPrice > sList[i + 1].StPrice)
                    sList[i + 1].Up_down = "down";
                else if (sList[i].StPrice < sList[i + 1].StPrice)
                    sList[i + 1].Up_down = "up";
                else
                    sList[i + 1].Up_down = "same";
            }


            for (int kill = 0; kill < matchList.Count(); kill++)
            {
                int n = matchList[kill];
                for (int i = n; i < n+20; i++)
                    tempsList.Add(sList[i]);

                ////////////////////////////////////////////////////////////////////////

                int headValue = 0;
                headValue = (usList[0].Price + usList[1].Price + usList[2].Price + usList[3].Price + usList[4].Price) / usList.Count();

                for (int i = 0; i < usList.Count(); i++)
                    usList[i].Deviation = usList[i].Price - headValue;

                double tempHeadValue = 0;
                for (int i = 0; i < tempsList.Count(); i++)
                {
                    tempsList[i].PricePercent = (int)(tempsList[i].StPrice / tempsList[0].StPrice * 100);
                    tempHeadValue += tempsList[i].PricePercent;
                }
                tempHeadValue = tempHeadValue / tempsList.Count();

                for (int i = 0; i < tempsList.Count(); i++)
                {
                    tempsList[i].Deviation = (int)(tempsList[i].PricePercent - tempHeadValue);
                    tempsList[i].Day = i;
                }

                List<UserStock> semesterList = new List<UserStock>();
                for (int i = 0; i < usList.Count(); i++)
                {
                    if (usList[i].Highpt == true || usList[i].Lowpt == true)
                    {
                        semesterList.Add(usList[i]);
                    }
                }

                for (int i = 0; i < semesterList.Count(); i++)
                {
                    if (semesterList[i].Highpt == true)
                    {
                        int semester = semesterList[i].Semester;
                        switch (semester)
                        {
                            case 1:
                                for (int j = 0; j < 4; j++)
                                    if (tempsList[j].StPrice > tempsList[j + 1].StPrice)
                                    {
                                        if (tempsList[j].StPrice >= tempsList[firstSemesterIdx].StPrice)
                                            firstSemesterIdx = j;
                                    }
                                    else if (tempsList[j].StPrice < tempsList[j + 1].StPrice)
                                    {
                                        if (tempsList[firstSemesterIdx].StPrice <= tempsList[j + 1].StPrice)
                                            firstSemesterIdx = j + 1;
                                    }
                                tempsList[firstSemesterIdx].Highpt = true;
                                break;
                            case 2:
                                for (int j = 5; j < 9; j++)
                                    if (tempsList[j].StPrice > tempsList[j + 1].StPrice)
                                    {
                                        if (tempsList[j].StPrice >= tempsList[secondSemesterIdx].StPrice)
                                            secondSemesterIdx = j;
                                    }
                                    else if (tempsList[j].StPrice < tempsList[j + 1].StPrice)
                                    {
                                        if (tempsList[secondSemesterIdx].StPrice <= tempsList[j + 1].StPrice)
                                            secondSemesterIdx = j + 1;
                                    }
                                tempsList[secondSemesterIdx].Highpt = true;
                                break;
                            case 3:
                                for (int j = 10; j < 14; j++)
                                    if (tempsList[j].StPrice > tempsList[j + 1].StPrice)
                                    {
                                        if (tempsList[j].StPrice >= tempsList[thirdSemesterIdx].StPrice)
                                            thirdSemesterIdx = j;
                                    }
                                    else if (tempsList[j].StPrice < tempsList[j + 1].StPrice)
                                    {
                                        if (tempsList[thirdSemesterIdx].StPrice <= tempsList[j + 1].StPrice)
                                            thirdSemesterIdx = j + 1;
                                    }
                                tempsList[thirdSemesterIdx].Highpt = true;
                                break;
                            case 4:
                                for (int j = 15; j < 19; j++)
                                    if (tempsList[j].StPrice > tempsList[j + 1].StPrice)
                                    {
                                        if (tempsList[j].StPrice >= tempsList[forthSemesterIdx].StPrice)
                                            forthSemesterIdx = j;
                                    }
                                    else if (tempsList[j].StPrice < tempsList[j + 1].StPrice)
                                    {
                                        if (tempsList[forthSemesterIdx].StPrice <= tempsList[j + 1].StPrice)
                                            forthSemesterIdx = j + 1;
                                    }
                                tempsList[forthSemesterIdx].Highpt = true;
                                break;
                        }
                    }
                    if (semesterList[i].Lowpt == true)
                    {
                        int semester = semesterList[i].Semester;
                        switch (semester)
                        {
                            case 1:
                                for (int j = 0; j < 4; j++)
                                    if (tempsList[j].StPrice < tempsList[j + 1].StPrice)
                                    {
                                        if (tempsList[j].StPrice <= tempsList[firstSemesterIdx].StPrice)
                                            firstSemesterIdx = j;
                                    }
                                    else if (tempsList[j].StPrice > tempsList[j + 1].StPrice)
                                    {
                                        if (tempsList[firstSemesterIdx].StPrice >= tempsList[j + 1].StPrice)
                                            firstSemesterIdx = j + 1;
                                    }
                                tempsList[firstSemesterIdx].Lowpt = true;
                                break;
                            case 2:
                                for (int j = 5; j < 9; j++)
                                    if (tempsList[j].StPrice < tempsList[j + 1].StPrice)
                                    {
                                        if (tempsList[j].StPrice <= tempsList[secondSemesterIdx].StPrice)
                                            secondSemesterIdx = j;
                                    }
                                    else if (tempsList[j].StPrice > tempsList[j + 1].StPrice)
                                    {
                                        if (tempsList[secondSemesterIdx].StPrice >= tempsList[j + 1].StPrice)
                                            secondSemesterIdx = j + 1;
                                    }
                                tempsList[secondSemesterIdx].Lowpt = true;
                                break;
                            case 3:
                                for (int j = 10; j < 14; j++)
                                    if (tempsList[j].StPrice < tempsList[j + 1].StPrice)
                                    {
                                        if (tempsList[j].StPrice <= tempsList[thirdSemesterIdx].StPrice)
                                            thirdSemesterIdx = j;
                                    }
                                    else if (tempsList[j].StPrice > tempsList[j + 1].StPrice)
                                    {
                                        if (tempsList[thirdSemesterIdx].StPrice >= tempsList[j + 1].StPrice)
                                            thirdSemesterIdx = j + 1;
                                    }
                                tempsList[thirdSemesterIdx].Lowpt = true;
                                break;
                            case 4:
                                for (int j = 15; j < 19; j++)
                                    if (tempsList[j].StPrice < tempsList[j + 1].StPrice)
                                    {
                                        if (tempsList[j].StPrice <= tempsList[forthSemesterIdx].StPrice)
                                            forthSemesterIdx = j;
                                    }
                                    else if (tempsList[j].StPrice > tempsList[j + 1].StPrice)
                                    {
                                        if (tempsList[forthSemesterIdx].StPrice >= tempsList[j + 1].StPrice)
                                            forthSemesterIdx = j + 1;
                                    }
                                tempsList[forthSemesterIdx].Lowpt = true;
                                break;
                        }
                    }
                }

                for (int i = 0; i < tempsList.Count(); i++)
                {
                    if (tempsList[i].Highpt == true)
                    {
                        int maxIdx = 0;
                        if (0 <= i && i < 5)
                        {
                            for (int j = 0; j < 4; j++)
                            {
                                if (tempsList[j].StPrice > tempsList[j + 1].StPrice)
                                {
                                    if (tempsList[j].StPrice >= tempsList[maxIdx].StPrice)
                                        maxIdx = j;
                                }
                                else if (tempsList[j].StPrice < tempsList[j + 1].StPrice)
                                {
                                    if (tempsList[maxIdx].StPrice <= tempsList[j + 1].StPrice)
                                        maxIdx = j + 1;
                                }
                            }
                            tempsList[maxIdx].Charactor = true;
                        }
                        else if (5 <= i && i < 10)
                        {
                            for (int j = 5; j < 9; j++)
                            {
                                if (tempsList[j].StPrice > tempsList[j + 1].StPrice)
                                {
                                    if (tempsList[j].StPrice >= tempsList[maxIdx].StPrice)
                                        maxIdx = j;
                                }
                                else if (tempsList[j].StPrice < tempsList[j + 1].StPrice)
                                {
                                    if (tempsList[maxIdx].StPrice <= tempsList[j + 1].StPrice)
                                        maxIdx = j + 1;
                                }
                            }
                            tempsList[maxIdx].Charactor = true;
                        }
                        else if (10 <= i && i < 15)
                        {
                            for (int j = 10; j < 14; j++)
                            {
                                if (tempsList[j].StPrice > tempsList[j + 1].StPrice)
                                {
                                    if (tempsList[j].StPrice >= tempsList[maxIdx].StPrice)
                                        maxIdx = j;
                                }
                                else if (tempsList[j].StPrice < tempsList[j + 1].StPrice)
                                {
                                    if (tempsList[maxIdx].StPrice <= tempsList[j + 1].StPrice)
                                        maxIdx = j + 1;
                                }
                            }
                            tempsList[maxIdx].Charactor = true;
                        }
                        else if (15 <= i && i < 20)
                        {
                            for (int j = 15; j < 19; j++)
                            {
                                if (tempsList[j].StPrice > tempsList[j + 1].StPrice)
                                {
                                    if (tempsList[j].StPrice >= tempsList[maxIdx].StPrice)
                                        maxIdx = j;
                                }
                                else if (tempsList[j].StPrice < tempsList[j + 1].StPrice)
                                {
                                    if (tempsList[maxIdx].StPrice <= tempsList[j + 1].StPrice)
                                        maxIdx = j + 1;
                                }
                            }
                            tempsList[maxIdx].Charactor = true;
                        }
                    }

                    else if (tempsList[i].Lowpt == true)
                    {
                        int lowIdx = 0;
                        if (0 <= i && i < 5)
                        {
                            for (int j = 0; j < 4; j++)
                            {
                                if (tempsList[j].StPrice < tempsList[j + 1].StPrice)
                                {
                                    if (tempsList[j].StPrice <= tempsList[lowIdx].StPrice)
                                        lowIdx = j;
                                }
                                else if (tempsList[j].StPrice > tempsList[j + 1].StPrice)
                                {
                                    if (tempsList[lowIdx].StPrice >= tempsList[j + 1].StPrice)
                                        lowIdx = j + 1;
                                }
                            }
                            tempsList[lowIdx].Charactor = true;
                        }
                        else if (5 <= i && i < 10)
                        {
                            for (int j = 5; j < 9; j++)
                            {
                                if (tempsList[j].StPrice < tempsList[j + 1].StPrice)
                                {
                                    if (tempsList[j].StPrice <= tempsList[lowIdx].StPrice)
                                        lowIdx = j;
                                }
                                else if (tempsList[j].StPrice > tempsList[j + 1].StPrice)
                                {
                                    if (tempsList[lowIdx].StPrice >= tempsList[j + 1].StPrice)
                                        lowIdx = j + 1;
                                }
                            }
                            tempsList[lowIdx].Charactor = true;
                        }
                        else if (10 <= i && i < 15)
                        {
                            for (int j = 10; j < 14; j++)
                            {
                                if (tempsList[j].StPrice < tempsList[j + 1].StPrice)
                                {
                                    if (tempsList[j].StPrice <= tempsList[lowIdx].StPrice)
                                        lowIdx = j;
                                }
                                else if (tempsList[j].StPrice > tempsList[j + 1].StPrice)
                                {
                                    if (tempsList[lowIdx].StPrice >= tempsList[j + 1].StPrice)
                                        lowIdx = j + 1;
                                }
                            }
                            tempsList[lowIdx].Charactor = true;
                        }
                        else if (15 <= i && i < 20)
                        {
                            for (int j = 15; j < 19; j++)
                            {
                                if (tempsList[j].StPrice < tempsList[j + 1].StPrice)
                                {
                                    if (tempsList[j].StPrice <= tempsList[lowIdx].StPrice)
                                        lowIdx = j;
                                }
                                else if (tempsList[j].StPrice > tempsList[j + 1].StPrice)
                                {
                                    if (tempsList[lowIdx].StPrice >= tempsList[j + 1].StPrice)
                                        lowIdx = j + 1;
                                }
                            }
                            tempsList[lowIdx].Charactor = true;
                        }
                    }
                }

                /////////////////////////////// Deviation Prepare

                
            #endregion


                /////////////////////////////////////////////////////////////////




           

                ////////////////////////////////////////////////////////////////


                #region MatchingRate
                int highlow_Matchcount = 0;

                for (int j = 0; j < tempsList.Count(); j++)
                {
                    if (j >= 0 && j < 5)
                        if (tempsList[j].Up_down == usList[1].Up_down)
                            highlow_Matchcount++;
                    if (j >= 5 && j < 10)
                        if (tempsList[j].Up_down == usList[2].Up_down)
                            highlow_Matchcount++;
                    if (j >= 10 && j < 15)
                        if (tempsList[j].Up_down == usList[3].Up_down)
                            highlow_Matchcount++;
                    if (j >= 15 && j < 20)
                        if (tempsList[j].Up_down == usList[4].Up_down)
                            highlow_Matchcount++;
                }

                int highlow_DivCount = tempsList.Count();
                highlow_MatchRate = highlow_Matchcount * 35 / highlow_DivCount;



                
                               
                double userDeviation = 0;
                double stockDeviation = 0;
            
                for (int i = 0; i < usList.Count(); i++)
                    userDeviation += Math.Abs(usList[i].Deviation);

                for (int i = 0; i < tempsList.Count(); i++)
                    if (tempsList[i].Charactor == true)
                        stockDeviation += Math.Abs(tempsList[i].Deviation);

                deviation_MatchRate = 30 - (Math.Abs(stockDeviation - userDeviation) * 35 / userDeviation);

                /////////////////////////////////////////////////////////////////


                double slope = 0, hypotenuse = 0;
                for (int i = 0; i < usList.Count() - 1; i++)
                {
                    slope = (usList[i + 1].Price - usList[i].Price) / (usList[i + 1].Day - usList[i].Day);
                    usList[i].Slope = slope;

                    hypotenuse = Math.Sqrt(Math.Pow(usList[i + 1].Day - usList[i].Day, 2) + Math.Pow(usList[i + 1].Price - usList[i].Price, 2));
                    usList[i].Hypotenuse = hypotenuse;
                }



                int slope_MatchIdx = 0, slope_MatchCount = 0;
                
                for (int i = 0; i < usList.Count() - 1; i++)
                {
                    for (int j = slope_MatchIdx; j < tempsList.Count(); j++)
                    {
                        if (tempsList[j].Charactor == true)
                        {
                            if(tempsList[j].Day - tempsList[slope_MatchIdx].Day != 0)
                                slope = (double)(tempsList[j].PricePercent - tempsList[slope_MatchIdx].PricePercent) / (double)(tempsList[j].Day - tempsList[slope_MatchIdx].Day);
                            tempsList[slope_MatchIdx].Slope = slope;

                            hypotenuse = Math.Sqrt(Math.Pow(tempsList[j].Day - tempsList[slope_MatchIdx].Day, 2) + Math.Pow(tempsList[j].PricePercent - tempsList[slope_MatchIdx].PricePercent, 2));
                            tempsList[slope_MatchIdx].Hypotenuse = hypotenuse;

                            if (usList[i].Slope + 0.5 >= tempsList[slope_MatchIdx].Slope && usList[i].Slope - 0.5 <= tempsList[slope_MatchIdx].Slope)
                                if (usList[i].Hypotenuse + 2 >= tempsList[slope_MatchIdx].Hypotenuse && usList[i].Hypotenuse - 2 <= tempsList[slope_MatchIdx].Hypotenuse)
                                {
                                    slope_MatchCount++;
                                }
                        }
                    }
                }

                int SlopeDivCount = usList.Count() - 1;
                slope_MatchRate = slope_MatchCount * 40 / SlopeDivCount;

                if (highlow_MatchRate + deviation_MatchRate + slope_MatchRate > 30)
                {
                    string resultS = string.Format("{0}  {1}      종목코드 : {0} , 날짜 : {1},   고저점 매칭율 : {2},    편차 매칭율 : {3},     기울기&빗변 매칭율 : {4},     총합 매칭율 : {5}%",
                        code, matchList[kill], highlow_MatchRate, deviation_MatchRate, slope_MatchRate, highlow_MatchRate + deviation_MatchRate + slope_MatchRate);
                    listBox1.Items.Add(resultS);
                }

                tempsList.Clear();
                #endregion

            }
        }

        public void LoadDataFromDB(string code)
        {
            string connectionString =
                "server = 220.68.231.208,21433; uid = sa; pwd = sj12sj12; database = StockInformation1;";
            SqlConnection scon = null;
            SqlCommand scom = null;
            SqlDataReader sdr = null;

            sList.Clear();
            matchList.Clear();

            try
            {
                scon = new SqlConnection(connectionString);
                scom = new SqlCommand();
                scom.Connection = scon;
                string s = string.Format("select 일자, 시가 from Day_C_Data1 where 종목코드 in ({0}) order by 일자 asc", code);
                scom.CommandText = s;
                scon.Open();

                sdr = scom.ExecuteReader();

                while (sdr.Read())
                {
                    string d = sdr["일자"].ToString();
                    int sp = int.Parse(sdr["시가"].ToString());

                    sList.Add(new Stock(d, sp));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.Source);
                Console.WriteLine(e.StackTrace);
            }
            finally
            {
                if (scon != null)
                {
                    scon.Close();
                }
            }
        }
        public void LoadSubjectCode()
        {
            string connectionString =
                "server = 220.68.231.208,21433; uid = sa; pwd = sj12sj12; database = StockInformation1;";
            SqlConnection scon = null;
            SqlCommand scom = null;
            SqlDataReader sdr = null;

            try
            {
                scon = new SqlConnection(connectionString);
                scom = new SqlCommand();
                scom.Connection = scon;
                scom.CommandText = "select 종목코드 from Stock_Type_Name";
                scon.Open();

                sdr = scom.ExecuteReader();

                while (sdr.Read())
                {
                    string sc = sdr["종목코드"].ToString();

                    subcodeList.Add(sc);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.Source);
                Console.WriteLine(e.StackTrace);
            }
            finally
            {
                if (scon != null)
                {
                    scon.Close();
                }
            }
        }

        class Stock
        {
            private string code;
            private string date;
            int day;
            private double edPrice;
            private double stprice;
            private int stack;
            private int highprice;
            private int lowprice;
            private string up_down;
            bool highpt;
            bool lowpt;
            bool charactor;
            int deviation;
            int pricePercent;
            double slope;
            double hypotenuse;
            public Stock() { }
            public Stock(string d, int sp) { date = d; stprice = sp; }
            public Stock(string d, int edp, int s, int stp, int hp, int lp)
            { date = d; StPrice = edp; stack = s; stprice = stp; highprice = hp; lowprice = lp; }
            public string Date { get { return date; } set { date = value; } }
            public int Day { get { return day; } set { day = value; } }

            public string Code { get { return code; } set { code = value; } }
            public double EdPrice { get { return edPrice; } set { edPrice = value; } }
            public double StPrice { get { return stprice; } set { stprice = value; } }
            public int Stack { get { return stack; } set { stack = value; } }
            public int HighPrice { get { return highprice; } set { highprice = value; } }
            public int LowPrice { get { return lowprice; } set { lowprice = value; } }
            public string Up_down { get { return up_down; } set { up_down = value; } }
            public bool Highpt { get { return highpt; } set { highpt = value; } }
            public bool Lowpt { get { return lowpt; } set { lowpt = value; } }
            public bool Charactor { get { return charactor; } set { charactor = value; } }
            public int Deviation { get { return deviation; } set { deviation = value; } }
            public int PricePercent { get { return pricePercent; } set { pricePercent = value; } }
            public double Slope { get { return slope; } set { slope = value; } }
            public double Hypotenuse { get { return hypotenuse; } set { hypotenuse = value; } }
        }
        class UserStock
        {
            private int price;
            int day;
            private string up_down;
            bool highpt;
            bool lowpt;
            int semester;
            int deviation;
            double slope;
            double hypotenuse;
            public UserStock() { }
            public UserStock(int p)
            { price = p; }
            public int Price { get { return price; } set { price = value; } }
            public int Day { get { return day; } set { day = value; } }
            public string Up_down { get { return up_down; } set { up_down = value; } }
            public bool Highpt { get { return highpt; } set { highpt = value; } }
            public bool Lowpt { get { return lowpt; } set { lowpt = value; } }
            public int Semester { get { return semester; } set { semester = value; } }
            public int Deviation { get { return deviation; } set { deviation = value; } }
            public double Slope { get { return slope; } set { slope = value; } }
            public double Hypotenuse { get { return hypotenuse; } set { hypotenuse = value; } }
        };

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string itemString = (string)listBox1.SelectedItem;
            string[] itemStringArray = itemString.Split(new char[] { ' ' });

            string code = itemStringArray[0];
            string idx = itemStringArray[2];

            chart1.Series.Clear();

            sList.Clear();
            matchList.Clear();

            string connectionString =
               "server = 220.68.231.208,21433; uid = sa; pwd = sj12sj12; database = StockInformation1;";
            SqlConnection scon = null;
            SqlCommand scom = null;
            SqlDataReader sdr = null;

            try
            {
                scon = new SqlConnection(connectionString);
                scon.Open();

                scom = new SqlCommand();
                scom.Connection = scon;
                string findDB = string.Format("select 일자, 시가 from Day_C_Data1 where 종목코드 in ({0}) order by 일자 asc", code);
                scom.CommandText = findDB;
                

                sdr = scom.ExecuteReader();

                while (sdr.Read())
                {
                    string d = sdr["일자"].ToString();
                    int sp = int.Parse(sdr["시가"].ToString());

                    sList.Add(new Stock(d, sp));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.Source);
                Console.WriteLine(ex.StackTrace);
            }
            finally
            {
                if (scon != null)
                {
                    scon.Close();
                }
            }

            Series data = new Series();

            data.ChartType = SeriesChartType.Line;
            data.Name = "Price";

            int num = int.Parse(idx);
            for (int i = num; i < num + 20; i++)
                data.Points.Add(sList[i].StPrice);

            chart1.Series.Add(data);

            chart1.ChartAreas[0].AxisY.Maximum = sList[num].StPrice * 1.08;
            chart1.ChartAreas[0].AxisY.Minimum = sList[num].StPrice * 0.92;
        }

        private void findbtn_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 50; i++)
            {
                LoadDataFromDB(subcodeList[i]);

                UserPatternMatch();

                GetMatchRate_Algorithm(subcodeList[i]);
            }
            //MessageBox.Show("Complete");

            //25초 // 24초
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            textBox1.Text = string.Format("{0}", trackBar1.Value);
            textBox2.Text = string.Format("{0}", trackBar2.Value);
            textBox3.Text = string.Format("{0}", trackBar3.Value);
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            textBox1.Text = string.Format("{0}", trackBar1.Value);
            textBox2.Text = string.Format("{0}", trackBar2.Value);
            textBox3.Text = string.Format("{0}", trackBar3.Value);
        }

        private void trackBar3_Scroll(object sender, EventArgs e)
        {
            textBox1.Text = string.Format("{0}", trackBar1.Value);
            textBox2.Text = string.Format("{0}", trackBar2.Value);
            textBox3.Text = string.Format("{0}", trackBar3.Value);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            
        }

        private void pictureBox1_LoadCompleted(object sender, AsyncCompletedEventArgs e)
        {
            // Graphics g;
            Point pt = new Point();
            pt.X = pictureBox1.Width/4;
            pt.Y = pictureBox1.Location.Y;
        }
    }
}

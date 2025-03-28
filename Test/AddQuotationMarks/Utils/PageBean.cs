using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddQuotationMarks.Utils
{
    public class PageBean
    {
        //条目总数
        private int ItemsTotalNum;

        //每页显示
        private int EveryPageNum;

        //当前页号
        private int CurrentPageNum;

        public PageBean(int itemsTotalNum, int everyPageNum, int currentPageNum) //构造函数，可以直接初始化
        {
            ItemsTotalNum = itemsTotalNum;
            EveryPageNum = everyPageNum;
            CurrentPageNum = currentPageNum;
        }
        public int ItemsTotalNum1 //条目总数
        {
            get => ItemsTotalNum;
            set
            {
                if (value < 0)
                {
                    return;
                }
                ItemsTotalNum = value;
            }
        }
        //get { return name; }
        //set { name = value; }

        public int EveryPageNum1 //每页显示数量
        {
            get => EveryPageNum;   //get => everyPageNum; 等同于 get { return everyPageNum; }
            set
            {
                if (value < 0 || value > ItemsTotalNum)
                {
                    return;
                }
                EveryPageNum = value;
            }
        }

        public int CurrentPageNum1 //当前页数
        {
            get => CurrentPageNum;
            set
            {
                if (value < 0 || value > MaxPageNum)
                {
                    return;
                }
                else
                {
                    CurrentPageNum = value;
                }
            }
        }

        public int MaxPageNum  //最大页数数量
        {
            get
            {
                float temp = (float)ItemsTotalNum / (float)EveryPageNum;
                return (int)Math.Ceiling(temp);
                //Math.Ceiling 是C#中的一个数学方法，用于返回大于或等于指定数字的最小整数（即向上取整）。
                //    例如：Math.Ceiling(3.2) 返回 4。
            }
        }

        public int PrePageNum   //页数
        {
            get
            {
                if (CurrentPageNum >= 2)
                {
                    return CurrentPageNum - 1;
                }
                else
                {
                    return 1;
                }
            }

        }
        public int NextPageNum  //下一页
        {
            get
            {
                if (CurrentPageNum < MaxPageNum)
                {
                    return CurrentPageNum + 1;
                }
                else
                {
                    return MaxPageNum;
                }


            }
        }


    }
}

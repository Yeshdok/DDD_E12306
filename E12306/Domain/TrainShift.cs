﻿using E12306.Common;
using E12306.Common.Enum;
using E12306.Util;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace E12306.Domain
{
    /// <summary>
    ///    当天车次 聚合根
    /// </summary>
    [Table("TrainShift")]
    public class TrainShift : AggregateRoot
    {
        protected TrainShift()
        {

        }
        public TrainShift(TrainNumber TrainNumber, Train Train, DateTimeOffset Date)
        {
            Id = Guid.NewGuid();
            this.TrainNumber = TrainNumber ?? throw new ArgumentNullException("TrainNumber");
            this.Train = Train ?? throw new ArgumentNullException("TrainNumber");
            this.Date = Date;


            AddTime = DateTimeOffset.Now;
            UpdateTime = DateTimeOffset.Now;
            AddUserId = UserHelper.User.Id;
            UpdateUserId = UserHelper.User.Id;

            this.ExtraSeatInfos = new List<ExtraSeatInfo>();
            foreach (var carriage in Train.TrainCarriages)
            {
                foreach (var seat in carriage.Seats)
                {
                    this.ExtraSeatInfos.Add(new ExtraSeatInfo(this, seat));
                }
            }
            this.SaleSeatInfos = new List<SaleSeatInfo>();
            this.FreezeSeatInfos = new List<FreezeSeatInfo>();
        }




        /// <summary>
        /// 车次
        /// </summary>
        public TrainNumber TrainNumber { get; private set; }

        /// <summary>
        /// 当天运行的火车
        /// </summary>  
        public Train Train { get; set; }

        /// <summary>
        /// 出发日期
        /// </summary>
        [Required]
        public DateTimeOffset Date { get; private set; }


        /// <summary>
        /// 剩余的座位信息
        /// </summary>  
        public IList<ExtraSeatInfo> ExtraSeatInfos { get; private set; }


        /// <summary>
        /// 已销售的座位信息
        /// </summary> 
        public IList<SaleSeatInfo> SaleSeatInfos { get; private set; }

        /// <summary>
        /// 冻结的座位信息
        /// </summary>     
        public IList<FreezeSeatInfo> FreezeSeatInfos { get; private set; }




    }

}

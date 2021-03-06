﻿using PCME.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Text;

namespace PCME.Domain.AggregatesModel.BookAggregates
{
    public class Book:Entity,IAggregateRoot
    {
        public string Num { get; private set; }
        public string Name { get; private set; }
        public string PublishingHouse { get; private set; }
        public int ExamSubjectId { get; private set; }
        public decimal Pirce { get; private set; }
        /// <summary>
        /// 折扣
        /// </summary>
        public float Discount { get; private set; }
        public Book()
        {

        }

        public Book(string num, string name, string publishingHouse, int examSubjectId, decimal pirce=0, float discount=0)
        {
            Num = num ?? throw new ArgumentNullException(nameof(num));
            Name = name ?? throw new ArgumentNullException(nameof(name));
            PublishingHouse = publishingHouse ?? throw new ArgumentNullException(nameof(publishingHouse));
            ExamSubjectId = examSubjectId;
            Pirce = pirce;
            Discount = discount;
        }
        public void Update(string num, string name, string publishingHouse, int examSubjectId, decimal pirce = 0, float discount = 0)
        {
            Num = num ?? throw new ArgumentNullException(nameof(num));
            Name = name ?? throw new ArgumentNullException(nameof(name));
            PublishingHouse = publishingHouse ?? throw new ArgumentNullException(nameof(publishingHouse));
            ExamSubjectId = examSubjectId;
            Pirce = pirce;
            Discount = discount;
        }
    }
}

﻿using System;
namespace DotNet2020.Domain._4.Model
{
    public class Illness : AbstractCalendarEntry
    {
        public int Id { get; }
        public new DateTime From { get; private set; }
        public new DateTime To { get; private set; }
        public bool IsApproved { get; private set; }

        public void Approve()
        {
            IsApproved = true;
        }
    }
}

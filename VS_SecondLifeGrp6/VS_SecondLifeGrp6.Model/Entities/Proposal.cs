﻿using System;
using System.Collections.Generic;
using System.Text;

namespace VS_SLG6.Model.Entities
{
    public enum State
    {
        ACTIVE,
        ACCEPTED,
        REFUSED,
        CLOSED
    }

    public class Proposal
    {
        public int Id { get; set; }
        public double Price { get; set; }
        public TimeSpan Period { get; set; }
        public State State { get; set; }
        public Product Product { get; set; }
        public User Target { get; set; }
        public User Origin { get; set; }
    }
}

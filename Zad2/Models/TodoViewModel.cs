using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis.Diagnostics;
using Zad1;

namespace Zad2.Models
{
    public class TodoViewModel
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public bool IsCompleted => DateCompleted.HasValue;
        public DateTime? DateCompleted { get; set; }
        public DateTime DateCreated { get; set; }
        public Guid UserId { get; set; }
        public List<TodoItemLabel> Labels { get; set; }
        public DateTime? DateDue { get; set; }

        public TodoViewModel(string text)
        {
            Id = Guid.NewGuid();
            DateCreated = DateTime.UtcNow;
            Text = text;
        }

        public TodoViewModel(string text, Guid userId)
        {
            Id = Guid.NewGuid();
            Text = text;
            DateCreated = DateTime.UtcNow;
            UserId = userId;
            Labels = new List<TodoItemLabel>();
        }

        public TodoViewModel()
        {
            // entity framework needs this one
            // not for use :)
        }

        public bool MarkAsCompleted()
        {
            if (!IsCompleted)
            {
                DateCompleted = DateTime.Now;
                return true;
            }
            return false;
        }

        public string TimeLeft()
        {
            if (DateDue.HasValue && !IsCompleted)
            {
                int days = ((DateTime) DateDue - DateTime.Now).Days;
                if (days >= 0) return "(za " + days.ToString() +( days.ToString() == "1" ? " dan!)" : " dana!)");
                return "(Deadline has passed!)";
            }
            return "";
        }

        public override bool Equals(object obj)
        {
            if (!(obj is TodoItem objAItem)) return false;
            return Id.Equals(objAItem.Id);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }


        public class TodoItemLabel
        {
            public Guid Id { get; set; }
            public string Value { get; set; }
            public List<TodoItem> LabelTodoItems { get; set; }

            public TodoItemLabel(string value)
            {
                Id = Guid.NewGuid();
                Value = value;
                LabelTodoItems = new List<TodoItem>();

            }
        }

        public object GetDate()
        {
            if (DateDue.HasValue)
                if (!IsCompleted) return ((DateTime) DateDue).ToShortDateString();
            return DateCompleted.HasValue == true ? ((DateTime) DateCompleted).ToShortDateString() : "";
        }
    }
}
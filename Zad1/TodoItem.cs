using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zad1
{

    public class TodoItem
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public bool IsCompleted
        {
            get
            {
                return DateCompleted.HasValue;
            }
            set { }
        }
        public DateTime? DateCompleted { get; set; }
        public DateTime DateCreated { get; set; }
        public Guid UserId { get; set; }
        public List<TodoItemLabel> Labels { get; set; }
        public DateTime? DateDue { get; set; }

        public TodoItem(string text)
        {
            Id = Guid.NewGuid();
            DateCreated = DateTime.UtcNow;
            Text = text;
        }

        public TodoItem(string text, Guid userId)
        {
            Id = Guid.NewGuid();
            Text = text;
            DateCreated = DateTime.UtcNow;
            UserId = userId;
            Labels = new List<TodoItemLabel>();
        }

        public TodoItem()
        {
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

            public TodoItemLabel()
            {
            }

            public override bool Equals(object obj)
            {
                if (!(obj is TodoItemLabel objAItem)) return false;
                return Value.Equals(objAItem.Value);
            }
        }

    }

}
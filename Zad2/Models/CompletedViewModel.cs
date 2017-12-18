using System.Collections.Generic;

namespace Zad2.Models
{
    public class CompletedViewModel
    {
        public List<TodoViewModel> TodoViewModels { get; set; }

        public CompletedViewModel(List<TodoViewModel> completedTodoList)
        {
            TodoViewModels = completedTodoList;
        }
    }
}
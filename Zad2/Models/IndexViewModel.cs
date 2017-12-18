using System.Collections.Generic;

namespace Zad2.Models
{
    public class IndexViewModel
    {
        public List<TodoViewModel> TodoViewModels { get; set; }

        public IndexViewModel(List<TodoViewModel> todoViewModels)
        {
            TodoViewModels = todoViewModels;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Zad1;
using Zad2.Data;
using Zad2.Models;
using AutoMapper;
using static Zad1.TodoItem;

namespace Zad2.Controllers
{
    [Authorize]
    public class TodoController : Controller
    {

        private readonly ITodoRepository _repository;
        private readonly UserManager<ApplicationUser> _userManager;

        public TodoController(ITodoRepository repository, UserManager<ApplicationUser> userManager)
        {
            _repository = repository;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            if (currentUser == null) return View();
            var activeItems = await _repository.GetActiveAsync(new Guid(currentUser.Id));
            var activeTodoList = Mapper.Map<List<TodoItem>, List<TodoViewModel>>(activeItems);
            return View(new IndexViewModel(activeTodoList));
        }

        public async Task<IActionResult> Completed()
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            var completeditems = await _repository.GetCompletedAsync(new Guid(currentUser.Id));
            var completedTodoList = Mapper.Map<List<TodoItem>, List<TodoViewModel>>(completeditems);
            return View(new CompletedViewModel(completedTodoList));
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddTodoViewModel item)
        {
            if (!ModelState.IsValid) return View();
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            var _item = new TodoItem(item.Text, new Guid((currentUser.Id))) {DateDue = item.DateDue};

            if (item.Labels != null)
            {
                string[] labels = item.Labels.Split(',');

                foreach (var i in labels)
                {
                    var todoItemLabel = new TodoItemLabel(i.Trim());
                    _repository.AddLabel(todoItemLabel);
                    var state = false;
                    foreach (var j in _item.Labels)
                    {
                        if (j.Equals(todoItemLabel))
                            state = true;
                            break;
                    }
                    if(!state) _item.Labels.Add(todoItemLabel);
                }
            }
            await _repository.AddAsync(_item);
            return RedirectToAction("Index");
        }

        [HttpGet("RemoveFromCompleted/{Id}")]
        public async Task<IActionResult> RemoveFromCompleted(Guid id)
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            await _repository.RemoveAsync(id, new Guid(currentUser.Id));
            return RedirectToAction("Index");
        }

        [HttpGet("MarkAsCompleted/{Id}")]
        public async Task<IActionResult> MarkAsCompleted(Guid id)
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            await _repository.MarkAsCompletedAsync(id, new Guid(currentUser.Id));
            return RedirectToAction("Index");
        }


    }
}
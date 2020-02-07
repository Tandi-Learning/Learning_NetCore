﻿using CleanArch.Application.TodoLists.Queries.ExportTodos;
using System.Collections.Generic;

namespace CleanArch.Application.Common.Interfaces
{
    public interface ICsvFileBuilder
    {
        byte[] BuildTodoItemsFile(IEnumerable<TodoItemRecord> records);
    }
}

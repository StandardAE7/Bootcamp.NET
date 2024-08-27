﻿using System;
using System.Collections.Generic;

namespace Core.Features.Queries.GetTodoAll;

public class GetTodoAllResponse
{
    public List<TodoResponse> Todo { get; set; }
    public int TotalCount { get; set; }
}

public class TodoResponse
{
    public Guid TodoId { get; set; }
    public string Day { get; set; }
    public DateTime TodayDate { get; set; }
    public string Note { get; set; }
    public int DetailCount { get; set; }
    public List<TodoDetailResponse> TodoDetails { get; set; }
}

public class TodoDetailResponse
{
    public Guid TodoDetailId { get; set; }
    public string Activity { get; set; }
    public string Category { get; set; }
    public string DetailNote { get; set; }
}

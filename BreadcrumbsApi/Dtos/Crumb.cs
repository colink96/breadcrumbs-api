using System;
using System.Collections.Generic;
using BreadcrumbsApi;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.EntityFrameworkCore;

namespace BreadcrumbsApi.Dtos;

public partial class Crumb
{
    public int CrumbId { get; set; }

    public string Title { get; set; } = null!;

    public string Content { get; set; } = null!;

    public double X { get; set; }

    public double Y { get; set; }

    public double? Radius { get; set; }

    public int UserId { get; set; }

    public virtual User User { get; set; } = null!;
}
﻿namespace BeautySaloon.API.Exceptions;

public class CategoryNotFoundException : NotFoundException
{
    public CategoryNotFoundException(string message) : base(message)
    {
    }
}

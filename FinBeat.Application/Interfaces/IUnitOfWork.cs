﻿namespace FinBeat.Application.Interfaces;

public interface IUnitOfWork
{
	Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
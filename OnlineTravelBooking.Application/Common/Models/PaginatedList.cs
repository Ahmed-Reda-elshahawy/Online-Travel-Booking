
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace OnlineTravelBooking.Application.Common.Models;


public class PaginatedList<T>
{
	public List<T> Items { get; init; } = [];
	public int PageNumber { get; init; }
	public int PageSize { get; init; }
	public int TotalCount { get; init; }
	public int TotalPages { get; init; }
	public bool HasPreviousPage => PageNumber > 1;
	public bool HasNextPage => PageNumber < TotalPages;

	public PaginatedList(List<T> items, int count, int pageNumber, int pageSize)
	{
		Items = items;
		TotalCount = count;
		PageNumber = pageNumber;
		PageSize = pageSize;
		TotalPages = (int)Math.Ceiling(count / (double)pageSize);
	}

	public static async Task<PaginatedList<T>> Create(IQueryable<T> source, int pageNumber, int pageSize)
	{
		var count = source.Count();
		var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
		return new PaginatedList<T>(items, count, pageNumber, pageSize);
	}
}




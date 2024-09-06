using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
public class ExpensesController : ControllerBase
{
    private readonly ExpenseContext _context;

    public ExpensesController(ExpenseContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Expense>>> GetExpenses()
    {
        return await _context.Expenses.ToListAsync();
    }

    [HttpPost]
    public async Task<ActionResult<Expense>> PostExpense(Expense expense)
    {
        _context.Expenses.Add(expense);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetExpenses), new { id = expense.Id }, expense);
    }

    [HttpPut("{id}")]
public async Task<IActionResult> PutExpense(int id, Expense expense)
{
    if (id != expense.Id)
    {
        return BadRequest();
    }

    _context.Entry(expense).State = EntityState.Modified;

    try
    {
        await _context.SaveChangesAsync();
    }
    catch (DbUpdateConcurrencyException)
    {
        if (!_context.Expenses.Any(e => e.Id == id))
        {
            return NotFound();
        }
        else
        {
            throw;
        }
    }

    return NoContent();
}

}

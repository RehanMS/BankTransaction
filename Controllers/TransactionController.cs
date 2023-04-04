using BankTransaction.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BankTransaction.Controllers
{
    public class TransactionController : Controller
    {
        private readonly TransactionDbContext context;
        public TransactionController(TransactionDbContext context)
        {
            this.context = context;
        }
        public async Task<IActionResult> Index()
        {
            return View( await context.Transactions.ToListAsync());
        }

        public IActionResult AddOrEdit(int id = 0)
        {
            if (id == 0)
                return View(new Transaction());
            else
                return View(context.Transactions.Find(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit([Bind("TransactionId,AccountNumber,BeneficiaryName,BankName,SWIFTCode,Amount,Date")] Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                if (transaction.TransactionId == 0)
                {
                    transaction.Date = DateTime.Now;
                    context.Add(transaction);
                }
                else
                    context.Update(transaction);
                await context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(transaction);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var transaction = await context.Transactions.FindAsync(id);
            context.Transactions.Remove(transaction);
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    } 
}

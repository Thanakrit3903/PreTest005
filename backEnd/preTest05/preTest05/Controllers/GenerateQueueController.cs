using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using preTest05.Data;
using preTest05.Model;

namespace preTest05.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GenerateQueueController : Controller
    {
        private readonly AppDbContext _context;
        public GenerateQueueController(AppDbContext context) => _context = context;

        [HttpPost("generate")]
        public async Task<IActionResult> GenerateQueues()
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                var lastQueue = await _context.queues.FromSqlRaw("SELECT * FROM Queues ORDER BY Id DESC LIMIT 1 FOR UPDATE").FirstOrDefaultAsync();

                char prefix = lastQueue.prefix;
                int number = lastQueue.number;

                if (number < 9)
                {
                    number++;
                }
                else
                {
                    prefix = (char)(prefix + 1); // เลื่อน A -> B 
                    number = 0;
                    if (prefix > 'Z') prefix = 'A'; // วนกลับถ้าเกิน Z
                }

                var newQueue = new Queues { prefix = prefix, number = number };
                newQueue.createdAt = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                _context.queues.Add(newQueue);
                await _context.SaveChangesAsync();
                transaction.Commit();

                return Ok(new { queueNumber = $"{prefix}{number}", date = DateTime.Now });
            }
        }

        [HttpPost("reset")]
        public async Task<IActionResult> ResetQueue()
        {
            try
            {
                // 1. ลบข้อมูลและรีเซ็ต Identity (ใส่ ; เพื่อแยกคำสั่ง)
                // 2. Insert ข้อมูลเริ่มต้น (A, -1) เพื่อให้ตัวถัดไปที่รันคือ A0
                string sql = @"
            TRUNCATE TABLE ""queues"" RESTART IDENTITY; 
            INSERT INTO ""queues"" (""prefix"", ""number"") VALUES ('A', -1);";

                await _context.Database.ExecuteSqlRawAsync(sql);

                return Ok(new
                {
                    message = "Reset and Initialized successful",
                    resetAt = DateTime.Now,
                    status = "Ready for A0"
                });
            }
            catch (Exception ex)
            {
                // ถ้าเกิด Error เช่น ชื่อตารางผิด หรือชื่อ Column ไม่ตรงจะมาตกที่นี่
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}

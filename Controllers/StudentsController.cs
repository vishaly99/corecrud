using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using curdoperation.Models;

namespace curdoperation.Controllers
{
    public class StudentsController : Controller
    {
        private readonly DICTDBContext _context;

        public StudentsController(DICTDBContext context)
        {
            _context = context;
        }

        // GET: Students
        public async Task<IActionResult> Index(string searchtype, string parameter)
        {
            ViewData["Parameter"] = parameter;
            ViewData["searchtype"] = searchtype;
            switch (searchtype)
            {
                case "name":
                    var data = _context.Student.Where(e => e.Name.Contains(parameter)).Include(s => s.Course).ToList();
                    Tuple<List<Student>> datatuple = new Tuple<List<Student>>(data);
                    /*var data1 = await (from bv in _context.BookVehicle
                                       join v in _context.Vehicle on bv.Vehicleid equals v.Vid
                                       join c in _context.Customer on bv.Custid equals c.Cid
                                       where bv.Vehicle.Vname.Contains(parameter)
                                       select bv).Include(s => s.Vehicle).Include(s => s.Cust).ToListAsync();*/

                    return View(datatuple);
                    break;
                case "age":
                    var data1= await (from s in _context.Student
                                       join c in _context.Course on s.CourseId equals c.CourseId
                                       where s.Age.Contains(parameter)
                                       select s).ToListAsync();
                    return View(data1);
                    break;
                case "gender":
                    var data2 = await (from s in _context.Student
                                       join c in _context.Course on s.CourseId equals c.CourseId
                                       where s.Gender.Contains(parameter)
                                       select s).ToListAsync();
                    return View(data2);
                    break;
                case "course":
                    var data3 = await (from s in _context.Student
                                       join c in _context.Course on s.CourseId equals c.CourseId
                                       where c.CourseName.Contains(parameter)
                                       select s).ToListAsync();
                    return View(data3);
                    break;
                default:
                    var data4 = _context.Student.Include(s => s.Course).ToList();
                    Tuple<List<Student>> datatuple4 = new Tuple<List<Student>>(data4);
                    return View(datatuple4);
                    break;
            }

        }
        /*public async Task<ActionResult> Index(int? a1, int? a2, string uname)
        {
            List<Student> students = new List<Student>();
            HttpClient client = api.initial();
            HttpResponseMessage res = await client.GetAsync("api/Student");
            if (res.IsSuccessStatusCode)
            {
                var result = res.Content.ReadAsStringAsync().Result;
                students = JsonConvert.DeserializeObject<List<Student>>(result);
            }

            List<Student> name = students.Where(o => o.Age >= a1 && o.Age <= a2).ToList();

            if (a1 == null && a2 == null)
            {
                return View(students);
            }

            return View(name);
        }*/
        // GET: Students/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Student
                .Include(s => s.Course)
                .FirstOrDefaultAsync(m => m.StudentId == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // GET: Students/Create
        public IActionResult Create()
        {
            ViewData["CourseId"] = new SelectList(_context.Course, "CourseId", "CourseId");
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StudentId,Name,Age,Gender,CourseId")] Student student)
        {
            if (ModelState.IsValid)
            {
                _context.Add(student);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CourseId"] = new SelectList(_context.Course, "CourseId", "CourseId", student.CourseId);
            return View(student);
        }

        // GET: Students/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Student.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            ViewData["CourseId"] = new SelectList(_context.Course, "CourseId", "CourseId", student.CourseId);
            return View(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("StudentId,Name,Age,Gender,CourseId")] Student student)
        {
            if (id != student.StudentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(student);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentExists(student.StudentId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CourseId"] = new SelectList(_context.Course, "CourseId", "CourseId", student.CourseId);
            return View(student);
        }

        // GET: Students/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Student
                .Include(s => s.Course)
                .FirstOrDefaultAsync(m => m.StudentId == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var student = await _context.Student.FindAsync(id);
            _context.Student.Remove(student);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudentExists(int id)
        {
            return _context.Student.Any(e => e.StudentId == id);
        }
    }
}

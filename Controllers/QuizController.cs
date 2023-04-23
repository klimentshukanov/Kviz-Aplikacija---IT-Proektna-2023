using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Kviz_Aplikacija___IT_Proektna_2023.Models;
using Microsoft.Owin;
using FormCollection = System.Web.Mvc.FormCollection;

namespace Kviz_Aplikacija___IT_Proektna_2023.Controllers
{
    public static class ShuffleClass
    {
        private static Random rng = new Random();
        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }

    

    public class QuizController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [Authorize]
        [HttpPost]
        //presmetuvanje poeni
        public ActionResult Solve(FormCollection formResult)
        {
            int questionCount = 1;
            int correctQuestionCounter = 0;


            foreach (String key in formResult.AllKeys)
            {
                //AllKeys vrakja lista so stringovi koi vsushnost se QuestionIDs,
                //a formResult[key] vsushnost vrakja ID na odgovorot daden za toa prashanje 
                if(key.StartsWith("questionsCount_"))
                {
                    String changedKey = key.Replace("questionsCount_", "");
                    String testKey = changedKey;
                    questionCount = Int32.Parse(changedKey);
                    continue;
                }

                foreach (Choice c in db.Choices)
                {
                    if (c.ChoiceID == Int64.Parse(formResult[key]))
                    {
                        if(c.isCorrect)
                        {
                            correctQuestionCounter++;
                        }
                        break;
                    }
                }
            }

            Double result = correctQuestionCounter * 100.0 / questionCount;

            return RedirectToAction("QuizResult", new { quizResult = result });
        }

        [Authorize]
        public ActionResult QuizResult(Double quizResult)
        {
            return View(quizResult);
        }
        [Authorize]
        public ActionResult Solve(int? id)
        {
            Quiz selectedQuiz;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                selectedQuiz = db.Quizs.Find(id);
            }

            List<Question> quizQuestions = new List<Question>();
            List<Choice> quizChoices = new List<Choice>();

            foreach (Question q in db.Questions.ToList())
            {
                if (q.QuizID==selectedQuiz.QuizID)
                {
                    quizQuestions.Add(q);

                    foreach(Choice c in db.Choices.ToList())
                    {
                        if(c.QuestionID==q.QuestionID)
                        {
                            quizChoices.Add(c);
                        }
                    }

                }
            }

            ShuffleClass.Shuffle(quizQuestions);
            ShuffleClass.Shuffle(quizChoices);

            SolveQuizVM solveQuizVM = new SolveQuizVM(selectedQuiz, quizQuestions, quizChoices);
            return View(solveQuizVM);
        }

        // GET: Quiz
        [Authorize]
        public ActionResult Index()
        {
            return View(db.Quizs.ToList());
        }

        // GET: Quiz/Details/5
        /*public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Quiz quiz = db.Quizs.Find(id);
            if (quiz == null)
            {
                return HttpNotFound();
            }
            return View(quiz);
        }*/

        // GET: Quiz/Create
        [Authorize(Roles="Administrator")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Quiz/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        /*[HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "QuizID,QuizName")] Quiz quiz)
        {
            if (ModelState.IsValid)
            {
                db.Quizs.Add(quiz);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(quiz);
        }*/

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(EditAndCreateQuizVM model)
        {
            Quiz createdQuiz = model.Quiz;
            List<Question> createdQuestions = model.Questions;
            List<Choice> createdChoices = model.Choices;
            if (ModelState.IsValid)
            {
                db.Quizs.Add(createdQuiz);
                db.SaveChanges();

                int quizID = db.Quizs.ToList().Last().QuizID;

                for (int i = 0; i < 5; i++)
                {
                    createdQuestions[i].QuizID = quizID;
                    db.Questions.Add(createdQuestions[i]);
                    db.SaveChanges();

                    int k = i * 3 + 3;

                    int questionID = db.Questions.ToList().Last().QuestionID;

                    for (int j=i*3; j < k; j++)
                    {
                        createdChoices[j].QuestionID = questionID;
                        db.Choices.Add(createdChoices[j]);
                        db.SaveChanges();
                    }
                }

                // ushte da implementiram selected choices da se stavet

                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(createdQuiz);
        }

        // GET: Quiz/Edit/5
        [Authorize(Roles = "Administrator")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Quiz quiz = db.Quizs.Find(id);
            if (quiz == null)
            {
                return HttpNotFound();
            }

            EditAndCreateQuizVM editObj = new EditAndCreateQuizVM();
            editObj.Quiz = quiz;
            List<Question> quizQuestions = new List<Question>();
            List<Choice> quizChoices = new List<Choice>();

            List<Question> allQuestions = db.Questions.ToList();
            List<Choice> allChoices = db.Choices.ToList();

            foreach (Question q in allQuestions)
            {
                if(q.QuizID==quiz.QuizID)
                {
                    foreach(Choice c in allChoices)
                    {
                        if(c.QuestionID==q.QuestionID)
                        {
                            quizChoices.Add(c);
                        }
                    }
                    quizQuestions.Add(q);
                }
            }
            editObj.Choices = quizChoices;
            editObj.Questions = quizQuestions;
            return View(editObj);
        }

        // POST: Quiz/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        /*[HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "QuizID,QuizName")] Quiz quiz)
        {
            if (ModelState.IsValid)
            {
                db.Entry(quiz).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(quiz);
        }*/

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( EditAndCreateQuizVM model)
        {

            Quiz editedQuiz =model.Quiz;
            List<Choice> editedChoices = model.Choices;
            List<Question> editedQuestions = model.Questions;

            if (ModelState.IsValid)
            {
                db.Entry(editedQuiz).State = EntityState.Modified;

                for (int i=0;i<editedChoices.Count; i++)
                {
                    db.Entry(editedChoices[i]).State = EntityState.Modified;
                }

                for (int i = 0; i < editedQuestions.Count; i++)
                {
                    db.Entry(editedQuestions[i]).State = EntityState.Modified;
                }


                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        // GET: Quiz/Delete/5
        [Authorize(Roles = "Administrator")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Quiz quiz = db.Quizs.Find(id);
            if (quiz == null)
            {
                return HttpNotFound();
            }

            return View(quiz);
        }

        [Authorize(Roles = "Administrator")]
        // POST: Quiz/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Quiz quiz = db.Quizs.Find(id);
            List<Question> quizQuestions = db.Questions.ToList();
            foreach (Question q in quizQuestions)
            {
                if(q.QuizID==quiz.QuizID)
                {
                    List<Choice> quizChoices = db.Choices.ToList();
                    foreach(Choice c in quizChoices)
                    {
                        if(c.QuestionID==q.QuestionID)
                        {
                            db.Choices.Remove(c);
                        }
                    }
                    db.Questions.Remove(q);
                }
            }

            db.Quizs.Remove(quiz);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
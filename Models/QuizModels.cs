using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kviz_Aplikacija___IT_Proektna_2023.Models
{
    [Table("Quiz")]
    public class Quiz
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int QuizID { get; set; }
        [Required]
        [Display (Name ="Quiz Name")]
        public string QuizName { get; set; }
        public ICollection<Question> Questions { get; set; }

    }

    [Table("Question")]
    public class Question
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int QuestionID { get; set; }
        [Required]
        [Display(Name = "Question Text")]
        public string QuestionText { get; set; }

        [Required]
        [ForeignKey("Quiz")]
        public int QuizID { get; set; }
        public Quiz Quiz { get; set; }

        public ICollection<Choice> Choices { get; set; }
        /*public ICollection<Answer> Answers { get; set; }*/

        
    }

    [Table("Choice")]
    public class Choice
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ChoiceID { get; set; }
        [Required]
        [Display(Name = "Choice Text")]
        public string ChoiceText { get; set; }

        [Required]
        [ForeignKey("Question")]
        public int QuestionID { get; set; }
        public Question Question { get; set; }

        [Required]
        public bool isCorrect { get; set; }


    }

    /*[Table("Answer")]
    public class Answer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AnswerID { get; set; }

        [Required]
        [Display(Name = "Answer Text")]
        public string AnswerText { get; set; }

        [Required]
        [ForeignKey("Question")]
        public int QuestionID { get; set; }
        public Question Question { get; set; }

    }*/

    /*    public class QuizSolvingVM
        {
            private Quiz quiz { get; set; }

            private List<Question> questions { get; set; }

            private List<Choice> choices { get; set; }

            public QuizSolvingVM(Quiz quiz, List<Question> questions, List<Choice> choices)
            {
                this.quiz = quiz;
                this.questions = questions;
                this.choices = choices;
            }

            public QuizSolvingVM() { }
        }*/

}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



//Класс Student, описывающий учащегося в школе. Про каждого школьника известно ФИО (FIO), класс (grade), средняя успеваемость (performance)
//и уровень образования (stage: младшая 1-4 класс - elementary , средняя 5-8 класс - secondary,  старшая 9-11 класс - higher).
//Уровень образования  является перечислением.
//Класс Student должен реализовывать методы: конструктор по умолчанию,
//генерирующий учащихся с именами соответствующими последовательным буквам английского алфавита,
//классом равным случайному числу, со средней успеваемостью равной случайному числу c точностью до первого знака после запятой.
//Т.е.после вызова этого конструктора первый раз, генерируется студент  с именем A, после второго вызова с именем B и т.д.
//конструктор с параметрами имя, класс, средняя успеваемость
//методом Pass - перевод ученика в следующий класс, при этом изменяется класс и возможно уровень образования.
//ToString - вывод информации об учащемся в формате
//перечисление для уровня образования


namespace Кoлоквиум
{
    // Перечисление для уровня образования
    public enum EducationStage
    {
        Elementary, // младшая школа (1-4 класс)
        Secondary,  // средняя школа (5-8 класс)
        Higher      // старшая школа (9-11 класс)
    }

    //описание ученика
    class Student
    {
        //генерация имен
        private static int count = 0;
        private static Random rnd = new Random();

        //хранение информации про учеников
        public string FIO { get; set; }
        public int Grade { get; set; }
        public double Performance { get; set; }
        public EducationStage Stage { get; private set; }


        public Student()
        {
            //генерация имени русскими буквами
            char currentChar = (char)('А' + count % 32);
            FIO = currentChar.ToString();
            count++;

            //генерация класса от 1 до 11
            Grade = rnd.Next(1, 12);

            //генерация успеваемости от 1.0 до 5.0
            Performance = Math.Round(rnd.NextDouble() * 4 + 1, 1);

            //узнаем класс ученика
            UpdateStage();
        }

        //конструктор с параметрами
        public Student(string fio, int grade, double performance)
        {
            FIO = fio;
            Grade = grade;
            Performance = Math.Round(performance, 1);
            UpdateStage();
        }

        //метод для определения клпсааа ученика
        private void UpdateStage()
        {
            //если класс 1-4 то младшая школа
            if (Grade >= 1 && Grade <= 4)
                Stage = EducationStage.Elementary;

            //если класс 5-8 то средняя школа
            else if (Grade >= 5 && Grade <= 8)
                Stage = EducationStage.Secondary;

            //если класс 9-11 то старшая школа
            else if (Grade >= 9 && Grade <= 11)
                Stage = EducationStage.Higher;

            //ксли что-то другое то ошибка
            else
                throw new Exception("Класс должен быть от 1 до 11.");
        }

        //перевод в следующий класс усеника
        public void Pass()
        {
            if (Grade < 11)
            {
                Grade++;
                UpdateStage(); //обновляем уровень школы
            }
        }

        //в каком классе ученик
        private string GetStageInRussian()
        {
            switch (Stage)
            {
                case EducationStage.Elementary: return "младшая школа";
                case EducationStage.Secondary: return "средняя школа";
                case EducationStage.Higher: return "старшая школа";
                default: return "не определено";
            }
        }

        //вывод информации об ученике
        public override string ToString()
        {
            return $"{FIO}, {GetStageInRussian()}, {Grade} класс, {Performance} балла";
        }
    }

    //описание школы
    class School
    {
        public string Name { get; set; }
        private List<Student> listStudents = new List<Student>();

        public School(string name)
        {
            Name = name;
        }

        //добавление ученика
        public void Add(Student student)
        {
            listStudents.Add(student);
        }

        //вывод информации о школе
        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            result.AppendLine($"Школа: {Name}");
            result.AppendLine("Список учащихся:");

            for (int i = 0; i < listStudents.Count; i++)
            {
                result.AppendLine($"{i + 1}. {listStudents[i]}");
            }

            return result.ToString();
        }

        //фильтрация учащихся
        //Коликество учеников заданного класса
        public int Count(Func<int, bool> gradeFilter)
        {
            return listStudents.Count(student => gradeFilter(student.Grade));
        }

        //количество учеников в каждом классе
        public int Count(Func<EducationStage, bool> stageFilter)
        {
            return listStudents.Count(student => stageFilter(student.Stage));
        }

        //количество учеников заданного уровня успеваемости для указанных классов
        public int Count(Func<double, int, bool> performanceAndGradeFilter)
        {
            return listStudents.Count(student => performanceAndGradeFilter(student.Performance, student.Grade));
        }

        // 4. Количество учеников c указанными свойствами фамилии и имени
        public int Count(Func<string, bool> nameFilter)
        {
            return listStudents.Count(student => nameFilter(student.FIO));
        }
    }


    class Program
    {
        static void Main(string[] args)
        {
            //создаем учеников
            Student studA = new Student();
            Student studB = new Student();
            Student studAbaev = new Student("Абаев Георгий", 7, 3.4);
            Student studBagaev = new Student("Багаев Аслан", 4, 4);

            Console.WriteLine(studA);
            Console.WriteLine(studB);
            Console.WriteLine(studAbaev);
            Console.WriteLine(studBagaev);

            //перевод Багаева в следующий класс
            studBagaev.Pass();
            Console.WriteLine(studBagaev);

            //создание школы и добавляе учеников
            School school = new School("ФизМат");
            school.Add(studA);
            school.Add(studB);
            school.Add(studAbaev);
            school.Add(studBagaev);

            //вывод школы
            Console.WriteLine(school);

            //использование метода Count
            Console.WriteLine(school.Count(x => x > 1));
            Console.WriteLine(school.Count(x => x == EducationStage.Elementary));
            Console.WriteLine(school.Count((x, y) => (x >= 3.0 && x <= 5 && y == 1)));
            Console.WriteLine(school.Count(x => x.Contains("Багаев")));
        }
    }
}